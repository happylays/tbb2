// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Helper functions related to shader generation and file saving

public static class TCP2_ShaderGeneratorUtils
{
	private const string TCP2_PATH = "/JMO Assets/Toony Colors Pro/";
	public const string OUTPUT_PATH = TCP2_PATH + "Shaders 2.0 Generated/";
	private const string INCLUDE_REL_PATH = "../Shaders 2.0/Include/";

	//--------------------------------------------------------------------------------------------------

	static public bool SelectGeneratedShader;
	static public bool CustomOutputDir;
	public static string _OutputPath;
	public static string OutputPath
	{
		get
		{
			if(!CustomOutputDir)
				return OUTPUT_PATH;

			if (_OutputPath == null)
				_OutputPath = EditorPrefs.GetString("TCP2_OutputPath", OUTPUT_PATH);

			return _OutputPath;
		}
		set
		{
			if (_OutputPath != value)
			{
				if(!value.EndsWith("/"))
					value = value + "/";
				if(!value.StartsWith("/"))
					value = "/" + value;

				//try to get safe path name
				foreach (char c in Path.GetInvalidPathChars())
				{
					value = value.Replace(c.ToString(), "");
				}
				foreach (char c in Path.GetInvalidFileNameChars())
				{
					if (c == '/' || c == '\\')
						continue;
					value = value.Replace(c.ToString(), "");
				}

				_OutputPath = value;
				EditorPrefs.SetString("TCP2_OutputPath", _OutputPath);
			}
		}
	}

	private static string MakeRelativePath( string fromPath, string toPath )
	{
		System.Uri fromUri = new System.Uri(fromPath);
		System.Uri toUri = new System.Uri(toPath);

		// Path can't be made relative (shouldn't happen though!)
		if (fromUri.Scheme != toUri.Scheme)
			return INCLUDE_REL_PATH;

		System.Uri relativeUri = fromUri.MakeRelativeUri(toUri);
		string relativePath = System.Uri.UnescapeDataString(relativeUri.ToString());

		return relativePath;
	}

	//--------------------------------------------------------------------------------------------------
	// GENERATION

	static public Shader Compile(TCP2_Config config, Shader existingShader, TCP2_ShaderGenerator.ShaderGeneratorTemplate template, bool showProgressBar = true, bool overwritePrompt = true, bool modifiedPrompt = false)
	{
		return Compile(config, existingShader, template, showProgressBar ? 0f : -1f, overwritePrompt, modifiedPrompt);
	}
	static public Shader Compile(TCP2_Config config, Shader existingShader, TCP2_ShaderGenerator.ShaderGeneratorTemplate template, float progress, bool overwritePrompt, bool modifiedPrompt)
	{
		//UI
		if(progress >= 0f)
			EditorUtility.DisplayProgressBar("Hold On", "Generating Shader: " + config.ShaderName, progress);
		
		//Generate source
		string source = config.GenerateShaderSource(template, existingShader);
		if(string.IsNullOrEmpty(source))
		{
			Debug.LogError("[TCP2 Shader Generator] Can't save Shader: source is null or empty!");
			return null;
		}

		//Save to disk
		Shader shader = SaveShader(config, existingShader, source, overwritePrompt, modifiedPrompt);

		if(config.configType == "terrain")
		{
			//Generate Base shader
			TCP2_Config baseConfig = config.Copy();
			baseConfig.Filename = baseConfig.Filename + "_Base";
			baseConfig.ShaderName = "Hidden/" + baseConfig.ShaderName + "-Base";
			baseConfig.Features.Add("TERRAIN_BASE");

			source = baseConfig.GenerateShaderSource(template, existingShader);
			if (string.IsNullOrEmpty(source))
				Debug.LogError("[TCP2 Shader Generator] Can't save Terrain Base Shader: source is null or empty!");
			else
				SaveShader(baseConfig, existingShader, source, false, false);

			//Generate AddPass shader
			TCP2_Config addPassConfig = config.Copy();
			addPassConfig.Filename = addPassConfig.Filename + "_AddPass";
			addPassConfig.ShaderName = "Hidden/" + addPassConfig.ShaderName + "-AddPass";
			addPassConfig.Features.Add("TERRAIN_ADDPASS");
			addPassConfig.Flags.Add("decal:add");

			source = addPassConfig.GenerateShaderSource(template, existingShader);
			if (string.IsNullOrEmpty(source))
				Debug.LogError("[TCP2 Shader Generator] Can't save Terrain AddPass Shader: source is null or empty!");
			else
				SaveShader(addPassConfig, existingShader, source, false, false);
		}

		//UI
		if(progress >= 0f)
			EditorUtility.ClearProgressBar();

		return shader;
	}
	
	//Generate the source code for the shader as a string
	static private string GenerateShaderSource(this TCP2_Config config, TCP2_ShaderGenerator.ShaderGeneratorTemplate template, Shader existingShader = null)
	{
		if(config == null)
		{
			string error = "[TCP2 Shader Generator] Config file is null";
			Debug.LogError(error);
			return error;
		}

		if(template == null)
		{
			string error = "[TCP2 Shader Generator] Template is null";
			Debug.LogError(error);
			return error;
		}

		if (template.textAsset == null || string.IsNullOrEmpty(template.textAsset.text))
		{
			string error = "[TCP2 Shader Generator] Template string is null or empty";
			Debug.LogError(error);
			return error;
		}

		//------------------------------------------------
		// SHADER PARAMETERS

		//Old hard-coded dependencies
		if (!template.newSystem)
		{

			//Custom Lighting
			bool customLighting = NeedCustomLighting(config) || HasFeatures(config, "CUSTOM_LIGHTING_FORCE");
			if (customLighting && !config.Features.Contains("CUSTOM_LIGHTING"))
				config.Features.Add("CUSTOM_LIGHTING");
			else if (!customLighting && config.Features.Contains("CUSTOM_LIGHTING"))
				config.Features.Remove("CUSTOM_LIGHTING");

			//Custom Ambient
			bool customAmbient = NeedCustomAmbient(config) || HasFeatures(config, "CUSTOM_AMBIENT_FORCE");
			if (customAmbient && !config.Features.Contains("CUSTOM_AMBIENT"))
				config.Features.Add("CUSTOM_AMBIENT");
			else if (!customAmbient && config.Features.Contains("CUSTOM_AMBIENT"))
				config.Features.Remove("CUSTOM_AMBIENT");

			//Specific dependencies
			if (HasFeatures(config, "MATCAP_ADD", "MATCAP_MULT"))
			{
				if (!config.Features.Contains("MATCAP"))
					config.Features.Add("MATCAP");
			}
			else
			{
				if (config.Features.Contains("MATCAP"))
					config.Features.Remove("MATCAP");
			}
		}

		//Masks
		bool mask1 = false, mask2 = false, mask3 = false, vcolors_mask = false;
		string mask1features = "";
		string mask2features = "";
		string mask3features = "";

		if (template.newSystem)
		{
			//Enable Masks according to their dependencies (new system using Template)
			foreach (KeyValuePair<string, string> kvp in config.Keywords)
			{
				if (kvp.Value == "mask1")
				{
					bool maskEnabled = template.GetMaskDependency(kvp.Key, config);
					mask1 |= maskEnabled;
					if (maskEnabled)
						mask1features += template.GetMaskDisplayName(kvp.Key) + ",";
				}
				else if (kvp.Value == "mask2")
				{
					bool maskEnabled = template.GetMaskDependency(kvp.Key, config);
					mask2 |= maskEnabled;
					if (maskEnabled)
						mask2features += template.GetMaskDisplayName(kvp.Key) + ",";
				}
				else if (kvp.Value == "mask3")
				{
					bool maskEnabled = template.GetMaskDependency(kvp.Key, config);
					mask3 |= maskEnabled;
					if (maskEnabled)
						mask3features += template.GetMaskDisplayName(kvp.Key) + ",";
				}
				else if (kvp.Value == "IN.color")
				{
					vcolors_mask |= template.GetMaskDependency(kvp.Key, config);
				}
			}
		}
		else
		{
			//Enable Masks according to their dependencies (old hard-coded system)
			foreach (KeyValuePair<string, string> kvp in config.Keywords)
			{
				if (kvp.Value == "mask1") { mask1 |= GetMaskDependency(config, kvp.Key); if (mask1) mask1features += GetDisplayNameForMask(kvp.Key) + ","; }
				else if (kvp.Value == "mask2") { mask2 |= GetMaskDependency(config, kvp.Key); if (mask2) mask2features += GetDisplayNameForMask(kvp.Key) + ","; }
				else if (kvp.Value == "mask3") { mask3 |= GetMaskDependency(config, kvp.Key); if (mask3) mask3features += GetDisplayNameForMask(kvp.Key) + ","; }
				else if (kvp.Value == "IN.color") { vcolors_mask |= GetMaskDependency(config, kvp.Key); }
			}
		}

		//Only enable Independent UVs if relevant Mask is actually enabled
		foreach (KeyValuePair<string, string> kvp in config.Keywords)
		{
			if (kvp.Key == "UV_mask1") ToggleSingleFeature(config.Features, "UVMASK1", kvp.Value == "Independent UV" && mask1);
			else if (kvp.Key == "UV_mask2") ToggleSingleFeature(config.Features, "UVMASK2", kvp.Value == "Independent UV" && mask2);
			else if (kvp.Key == "UV_mask3") ToggleSingleFeature(config.Features, "UVMASK3", kvp.Value == "Independent UV" && mask3);
		}
		mask1features = mask1features.TrimEnd(',');
		mask2features = mask2features.TrimEnd(',');
		mask3features = mask3features.TrimEnd(',');

		ToggleSingleFeature(config.Features, "MASK1", mask1);
		ToggleSingleFeature(config.Features, "MASK2", mask2);
		ToggleSingleFeature(config.Features, "MASK3", mask3);
		ToggleSingleFeature(config.Features, "VCOLORS_MASK", vcolors_mask);

		//---

		Dictionary<string, string> keywords = new Dictionary<string, string>(config.Keywords);
		List<string> flags = new List<string>(config.Flags);
		List<string> features = new List<string>(config.Features);

		//Unity version
#if UNITY_5_4_OR_NEWER
		TCP2_Utils.AddIfMissing(features, "UNITY_5_4");
#endif

		//Masks
		keywords.Add("MASK1", mask1features);
		keywords.Add("MASK2", mask2features);
		keywords.Add("MASK3", mask3features);

		//Shader name
		keywords.Add("SHADER_NAME", config.ShaderName);
		
		//Include path
		string include = GetIncludePrefix(config) + GetIncludeRelativePath(config, existingShader) + GetIncludeFile(config);
		keywords.Add("INCLUDE_PATH", include);

		//Lighting Model
		if (!template.newSystem)
		{
			string lightingModel = GetLightingFunction(config);
			keywords.Add("LIGHTING_MODEL", lightingModel);
		}

		//SurfaceOutput struct
		string surfOut = GetSurfaceOutput(config);
		keywords.Add("SURFACE_OUTPUT", surfOut);

		//Shader Model target
		string target = GetShaderTarget(config);
		keywords.Add("SHADER_TARGET", target);
		if(config.shaderTarget == 20)
		{
			TCP2_Utils.AddIfMissing(features, "FORCE_SM2");
		}

		if (!template.newSystem)
		{
			//Vertex Function
			bool vertexFunction = NeedVertexFunction(config);
			if (vertexFunction)
			{
				TCP2_Utils.AddIfMissing(flags, "vertex:vert");
				features.Add("VERTEX_FUNC");
			}

			//Final Colors Function
			bool finalColorFunction = NeedFinalColorFunction(config);
			if (finalColorFunction)
			{
				TCP2_Utils.AddIfMissing(flags, "finalcolor:fcolor");
				features.Add("FINAL_COLOR");
			}

			//Alpha Testing (Cutout)
			if (HasFeatures(config, "CUTOUT"))
			{
				TCP2_Utils.AddIfMissing(flags, "alphatest:_Cutoff");
			}
		}

#if UNITY_5
		//Alpha
		if(HasFeatures(config, "ALPHA"))
		{
			TCP2_Utils.AddIfMissing(flags, "keepalpha");
		}
#endif

		//Shadows
		if(HasFeatures(config, "CUTOUT"))
		{
			TCP2_Utils.AddIfMissing(flags, "addshadow");
		}

		//No/Custom Ambient
		if(HasFeatures(config, "CUSTOM_AMBIENT"))
		{
			TCP2_Utils.AddIfMissing(flags, "noambient");
		}
		
		//Generate Surface parameters
		string strFlags = ArrayToString(flags.ToArray(), " ");
		keywords.Add("SURF_PARAMS", strFlags);

		//------------------------------------------------
		// PARSING & GENERATION
		
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		string[] templateLines = template.textAsset.text.Split(new string[]{"\r\n","\n"}, System.StringSplitOptions.None);

		int depth = -1;
		List<bool> stack = new List<bool>();
		List<bool> done = new List<bool>();

		//Parse template file
		string line = null;
		for(int i = 0; i < templateLines.Length; i++)
		{
			line = templateLines[i];

			//Comment
			if(line.StartsWith("#"))
			{
				//Meta
				if (line.StartsWith("#CONFIG="))
				{
					config.configType = line.Substring(8).TrimEnd().ToLower();
				}

				//Features UI
				if (line.StartsWith("#FEATURES"))
				{
					while (i < templateLines.Length)
					{
						i++;
						if (templateLines[i] == "#END")
							break;
					}
					continue;
				}

				//Keywords
				if (line.StartsWith("#KEYWORDS"))
				{
					while (i < templateLines.Length)
					{
						i++;
						if(templateLines[i] == "#END")
							break;

						string error = ProcessKeywords(templateLines[i], ref features, ref flags, ref keywords, ref i, ref depth, ref stack, ref done, template.newSystem);
						if(!string.IsNullOrEmpty(error))
						{
							return error;
						}
					}

					//Update Surface parameters
					strFlags = ArrayToString(flags.ToArray(), " ");
					if(keywords.ContainsKey("SURF_PARAMS"))
						keywords["SURF_PARAMS"] = strFlags;
					else
						keywords.Add("SURF_PARAMS", strFlags);
				}

				//Debugging
				if (line.StartsWith("#break"))
				{
					Debug.Log("[TCP2] Parse Break @ " + i);
				}

				continue;
			}

			//Line break
			if(string.IsNullOrEmpty(line) && ((depth >= 0 && stack[depth]) || depth < 0))
			{
				sb.AppendLine(line);
				continue;
			}
			
			//Conditions
			if(line.Contains("///"))
			{
				string error = ProcessCondition(line, ref features, ref i, ref depth, ref stack, ref done, template.newSystem);
				if(!string.IsNullOrEmpty(error))
				{
					return error;
				}
			}
			//Regular line
			else
			{
				//Replace keywords
				line = ReplaceKeywords(line, keywords);

				//Append line if inside valid condition block
				if((depth >= 0 && stack[depth]) || depth < 0)
				{
					sb.AppendLine(line);
				}
			}
		}
		
		if(depth >= 0)
		{
			Debug.LogWarning("[TCP2 Shader Generator] Missing " + (depth+1) + " ending '///' tags");
		}
		
		string sourceCode = sb.ToString();

		//Normalize line endings
		sourceCode = sourceCode.Replace("\r\n", "\n");

		return sourceCode;
	}

	//Process #KEYWORDS section from Template
	static private string ProcessKeywords(string line, ref List<string> features, ref List<string> flags, ref Dictionary<string,string> keywords, ref int i, ref int depth, ref List<bool> stack, ref List<bool> done, bool useNewEvaluationSystem)
	{
		if (line.Contains("///"))
		{
			ProcessCondition(line, ref features, ref i, ref depth, ref stack, ref done, useNewEvaluationSystem);
		}
		//Regular line
		else
		{
			if (string.IsNullOrEmpty(line))
				return null;

			//Inside valid block
			if ((depth >= 0 && stack[depth]) || depth < 0)
			{
				string[] parts = line.Split(new string[] { "\t" }, System.StringSplitOptions.RemoveEmptyEntries);
				switch (parts[0])
				{
					case "set":
						if (keywords.ContainsKey(parts[1]))
							keywords[parts[1]] = parts[2];
						else
							keywords.Add(parts[1], parts[2]);
						break;

					case "enable_kw":		TCP2_Utils.AddIfMissing(features, parts[1]); break;
					case "disable_kw":		TCP2_Utils.RemoveIfExists(features, parts[1]); break;
					case "enable_flag":		TCP2_Utils.AddIfMissing(flags, parts[1]); break;
					case "disable_flag":	TCP2_Utils.RemoveIfExists(flags, parts[1]); break;
				}
			}
		}

		return null;
	}

	static private string ProcessCondition(string line, ref List<string> features, ref int i, ref int depth, ref List<bool> stack, ref List<bool> done, bool useNewEvaluationSystem)
	{
		//Remove leading white spaces
		line = line.TrimStart();

		string[] parts = line.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 1)  //END TAG
		{
			if (depth < 0)
			{
				string error = "[TCP2 Shader Generator] Found end tag /// without any beginning! Aborting shader generation.\n@ line: " + i;
				Debug.LogError(error);
				return error;
			}

			stack.RemoveAt(depth);
			done.RemoveAt(depth);
			depth--;
		}
		else if (parts.Length >= 2)
		{
			if (parts[1] == "IF")
			{
				bool cond = useNewEvaluationSystem ? EvaluateExpression(i, features, parts) : EvaluateExpressionOld(i, features, parts);

				depth++;
				stack.Add(cond && ((depth <= 0) ? true : stack[depth - 1]));
				done.Add(cond);
			}
			else if (parts[1] == "ELIF")
			{
				if (done[depth])
				{
					stack[depth] = false;
					return null;
				}

				bool cond = useNewEvaluationSystem ? EvaluateExpression(i, features, parts) : EvaluateExpressionOld(i, features, parts);

				stack[depth] = cond && ((depth <= 0) ? true : stack[depth - 1]);
				done[depth] = cond;
			}
			else if (parts[1] == "ELSE")
			{
				if (done[depth])
				{
					stack[depth] = false;
					return null;
				}
				else
				{
					stack[depth] = ((depth <= 0) ? true : stack[depth - 1]);
					done[depth] = true;
				}
			}
		}

		return null;
	}

	//Old simple expression evaluation
	static private bool EvaluateExpressionOld(int lineNumber, List<string> features, params string[] conditions)
	{
		if(conditions.Length <= 2)
		{
			Debug.LogWarning("[TCP2 Shader Generator] Invalid condition block\n@ line " + lineNumber);
			return false;
		}

		string firstCondition = conditions[2];
		if(firstCondition.StartsWith("!"))
			firstCondition = firstCondition.Substring(1);

		bool condition = HasFeatures(features, firstCondition);
		if(conditions[2].StartsWith("!"))
			condition = !condition;
		
		int cond = 0;	//0: OR, 1: !OR, 2: AND, 3: !AND
		int i = 3;

		while(i < conditions.Length)
		{
			//And/Or
			if(conditions[i] == "&&")
				cond = 2;
			else if(conditions[i] == "||")
				cond = 0;
			else
				Debug.LogWarning("[TCP2 Shader Generator] Unrecognized condition: " + conditions[i] + "\n@ line " + lineNumber);

			i++;

			if(conditions[i].StartsWith("!"))
			{
				conditions[i] = conditions[i].Substring(1);
				cond++;
			}

			//Condition
			if(cond == 0)
				condition |= HasFeatures(features, conditions[i]);
			else if(cond == 1)
				condition |= !HasFeatures(features, conditions[i]);
			else if(cond == 2)
				condition &= HasFeatures(features, conditions[i]);
			else if(cond == 3)
				condition &= !HasFeatures(features, conditions[i]);

			i++;
		}

		return condition;
	}

	//New evaluation system with parenthesis and complex expressions support
	static private bool EvaluateExpression( int lineNumber, List<string> features, params string[] conditions )
	{
		if (conditions.Length <= 2)
		{
			Debug.LogError("[TCP2 Shader Generator] Invalid condition block\n@ line " + lineNumber);
			return false;
		}

		string expression = "";
		for (int n = 2; n < conditions.Length; n++)
		{
			expression += conditions[n];
		}

		bool result = false;
		try
		{
			TCP2_ExpressionParser.ExpressionLeaf.EvaluateFunction evalFunc = ( string s ) => HasFeatures(features, s);
			result = TCP2_ExpressionParser.EvaluateExpression(expression, evalFunc);
		}
		catch (System.Exception e)
		{
			Debug.LogError("[TCP2 Shader Generator] Incorrect condition in template file\n@ line " + lineNumber + "\n\nError returned:\n" + e.Message + "\n");
		}

		return result;
	}

	static private bool CheckFeature(string part, List<string> features)
	{
		if(part.Contains("&"))
		{
			string[] andParts = part.Split('&');
			
			bool b = true;
			foreach(string ap in andParts)
				b &= CheckFeature(ap, features);
			return b;
		}
		else if(part.StartsWith("!"))
		{
			return !features.Contains(part.Substring(1));
		}
		else
		{
			return features.Contains(part);
		}
	}
	
	static private string ArrayToString(string[] array, string separator)
	{
		string str = "";
		foreach(string s in array)
		{
			str += s + separator;
		}
		
		if(str.Length > 0)
		{
			str = str.Substring(0, str.Length - separator.Length);
		}
		
		return str;
	}
	
	static private string ReplaceKeywords(string line, Dictionary<string,string> searchAndReplace)
	{
		if(line.IndexOf("@%") < 0)
		{
			return line;
		}
		
		foreach(KeyValuePair<string,string> kv in searchAndReplace)
		{
			line = line.Replace("@%" + kv.Key + "%@", kv.Value);
		}
		
		return line;
	}
	
	//--------------------------------------------------------------------------------------------------
	// IO
	
	//Save .shader file
	static private Shader SaveShader(TCP2_Config config, Shader existingShader, string sourceCode, bool overwritePrompt, bool modifiedPrompt)
	{
		if(string.IsNullOrEmpty(config.Filename))
		{
			Debug.LogError("[TCP2 Shader Generator] Can't save Shader: filename is null or empty!");
			return null;
		}
		
		//Save file
		string outputPath = OutputPath;
		if (existingShader != null)
		{
			outputPath = GetExistingShaderPath(config, existingShader);
		}

		string systemPath = Application.dataPath + outputPath;
		if(!Directory.Exists(systemPath))
		{
			Directory.CreateDirectory(systemPath);
		}
		
		string fullPath = systemPath + config.Filename + ".shader";
		bool overwrite = true;
		if(overwritePrompt && File.Exists(fullPath))
		{
			overwrite = EditorUtility.DisplayDialog("TCP2 : Shader Generation", "The following shader already exists:\n\n" + fullPath + "\n\nOverwrite?", "Yes", "No");
		}

		if(modifiedPrompt)
		{
			overwrite = EditorUtility.DisplayDialog("TCP2 : Shader Generation", "The following shader seems to have been modified externally or manually:\n\n" + fullPath + "\n\nOverwrite anyway?", "Yes", "No");
		}
		
		if(overwrite)
		{
			string directory = Path.GetDirectoryName(systemPath + config.Filename);
			if(!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			//Write file to disk
			File.WriteAllText(systemPath + config.Filename + ".shader", sourceCode, System.Text.Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			
			//Import (to compile shader)
			string assetPath = "Assets" + outputPath + config.Filename + ".shader";

			Shader shader = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Shader)) as Shader;
			if(SelectGeneratedShader)
			{
				Selection.objects = new Object[]{ shader };
			}
			
			//Set ShaderImporter userData
			ShaderImporter shaderImporter = ShaderImporter.GetAtPath(assetPath) as ShaderImporter;
			if(shaderImporter != null)
			{
				//Get file hash to verify if it has been manually altered afterwards
				string shaderHash = GetShaderContentHash(shaderImporter);

				//Use hash if available, else use timestamp
				List<string> customDataList = new List<string>();
				customDataList.Add(!string.IsNullOrEmpty(shaderHash) ? shaderHash : shaderImporter.assetTimeStamp.ToString());
				customDataList.Add(config.GetShaderTargetCustomData());
				string configTypeCustomData = config.GetConfigTypeCustomData();
				if(configTypeCustomData!= null)
					customDataList.Add(configTypeCustomData);
				customDataList.Add(config.GetConfigFileCustomData());

				string userData = config.ToUserData(customDataList.ToArray());
				shaderImporter.userData = userData;

				//Needed to save userData in .meta file
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.Default);
			}
			else
			{
				Debug.LogWarning("[TCP2 Shader Generator] Couldn't find ShaderImporter.\nMetadatas will be missing from the shader file.");
			}

			return shader;
		}

		return null;
	}

	//Returns hash of file content to check for manual modifications (with 'h' prefix)
	static public string GetShaderContentHash(ShaderImporter importer)
	{
		string shaderHash = null;
		string shaderFilePath = Application.dataPath.Replace("Assets", "") + importer.assetPath;
		if(System.IO.File.Exists( shaderFilePath ))
		{
			string shaderContent = System.IO.File.ReadAllText( shaderFilePath );
			shaderHash = (shaderContent != null) ? string.Format("h{0}", shaderContent.GetHashCode().ToString("X")) : "";
		}

		return shaderHash;
	}
	
	//--------------------------------------------------------------------------------------------------
	// UTILS
	
	static public bool HasFeatures(TCP2_Config config, params string[] features)
	{
		return HasFeatures(config, true, features);
	}
	static public bool HasFeatures(TCP2_Config config, bool anyFeature, params string[] features)
	{
		return HasFeatures(config.Features, anyFeature, features);
	}
	static public bool HasFeatures(List<string> configFeature, params string[] features)
	{
		return HasFeatures(configFeature, true, features);
	}
	static public bool HasFeatures(List<string> configFeature, bool anyFeature, params string[] features)
	{
		bool hasAllFeatures = true;
		foreach(string f in features)
		{
			if(configFeature.Contains(f))
			{
				if(anyFeature)
					return true;
				else
					hasAllFeatures &= configFeature.Contains(f);
			}
		}
		return anyFeature ? false : hasAllFeatures;
	}

	static public void ToggleSingleFeature(List<string> featuresList, string feature, int value)
	{
		ToggleSingleFeature(featuresList, feature, value > 0);
	}
	static public void ToggleSingleFeature(List<string> featuresList, string feature, bool enable)
	{
		if(enable && !featuresList.Contains(feature))
		{
			featuresList.Add(feature);
		}
		else if(!enable && featuresList.Contains(feature))
		{
			featuresList.Remove(feature);
		}
	}

	static public void ToggleFlag(List<string> flagsList, string flag, bool enable)
	{
		if(enable && !flagsList.Contains(flag))
		{
			flagsList.Add(flag);
		}
		else if(!enable && flagsList.Contains(flag))
		{
			flagsList.Remove(flag);
		}
	}

	static public void ToggleMultipleFeatures(List<string> featuresList, int value, params string[] features)
	{
		ToggleMultipleFeatures(featuresList, value, true, features);
	}
	static public void ToggleMultipleFeatures(List<string> featuresList, int value, bool firstIsVoid, params string[] features)
	{
		if(value < 0 || value >= features.Length)
		{
			Debug.LogWarning("[TCP2 Shader Generator] Invalid value for supplied params. Clamping.");
			value = Mathf.Clamp(value, 0, features.Length-1);
		}
		
		for(int i = 0; i < features.Length; i++)
		{
			if(firstIsVoid && i == 0)
				continue;
			
			bool enable = (i == value);
			
			if(enable && !featuresList.Contains(features[i]))
			{
				featuresList.Add(features[i]);
			}
			else if(!enable && featuresList.Contains(features[i]))
			{
				featuresList.Remove(features[i]);
			}
		}
	}

	static public string GetKeyword(TCP2_Config config, string key)
	{
		return GetKeyword(config.Keywords, key);
	}
	static public string GetKeyword(Dictionary<string,string> keywordsDict, string key)
	{
		if(key == null)
			return null;

		if(!keywordsDict.ContainsKey(key))
			return null;

		return keywordsDict[key];
	}

	static public void SetKeyword(Dictionary<string,string> keywordsDict, string key, string value)
	{
		if(string.IsNullOrEmpty(value))
		{
			if(keywordsDict.ContainsKey(key))
				keywordsDict.Remove(key);
		}
		else
		{
			if(keywordsDict.ContainsKey(key))
				keywordsDict[key] = value;
			else
				keywordsDict.Add(key, value);
		}
	}

	static private string GetDisplayNameForMask(string maskType)
	{
		switch(maskType)
		{
		case "SPEC_MASK": return "Specular";
		case "REFL_MASK": return "Reflection";
		case "MASK_MC": return "MatCap";
		case "SPEC_SHIN_MASK": return "Shininess";
		case "DETAIL_MASK": return "Detail";
		case "RIM_MASK": return "Rim";
		case "EMISSION_MASK": return "Emission";
		case "COLORMASK": return "Color";
		case "SS_MASK": return "Subsurface Scattering";
		default : Debug.LogWarning("[TCP2 Shader Generator] Unknown mask: " + maskType); return "";
		}
	}

	static private bool GetMaskDependency(TCP2_Config config, string maskType)
	{
		switch(maskType)
		{
		case "SPEC_MASK": return HasFeatures(config, "SPECULAR", "SPECULAR_ANISOTROPIC");
		case "REFL_MASK": return HasFeatures(config, "REFLECTION");
		case "MASK_MC": return HasFeatures(config, "MATCAP");
		case "SPEC_SHIN_MASK": return HasFeatures(config, "SPECULAR", "SPECULAR_ANISOTROPIC");
		case "DETAIL_MASK": return HasFeatures(config, "DETAIL_TEX");
		case "RIM_MASK": return HasFeatures(config, "RIM","RIM_OUTLINE");
		case "EMISSION_MASK": return HasFeatures(config, "EMISSION");
		case "SS_MASK": return HasFeatures(config, "SUBSURFACE_SCATTERING");
		case "COLORMASK": return true;
		}
		return false;
	}

	//-------------------------------------------------
	
	//Convert Config to ShaderImporter UserData
	static public string ToUserData(this TCP2_Config config, string[] customData)
	{
		string userData = "";
		if(!config.Features.Contains("USER"))
			userData = "USER,";

		foreach(string feature in config.Features)
			if(feature.Contains("USER"))
				userData += string.Format("{0},", feature);
			else
				userData += string.Format("F{0},", feature);
		foreach(string flag in config.Flags)
			userData += string.Format("f{0},", flag);
		foreach(KeyValuePair<string,string> kvp in config.Keywords)
			userData += string.Format("K{0}:{1},", kvp.Key, kvp.Value);
		foreach(string custom in customData)
			userData += string.Format("c{0},", custom);
		userData = userData.TrimEnd(',');

		return userData;
	}

	//Get Features array from ShaderImporter
	static public void ParseUserData(ShaderImporter importer, out List<string> Features)
	{
		string[] array;
		string[] dummy;
		Dictionary<string,string> dummyDict;
		ParseUserData(importer, out array, out dummy, out dummyDict, out dummy);
		Features = new List<string>(array);
	}
	static public void ParseUserData(ShaderImporter importer, out string[] Features, out string[] Flags, out Dictionary<string,string> Keywords, out string[] CustomData)
	{
		List<string> featuresList = new List<string>();
		List<string> flagsList = new List<string>();
		List<string> customDataList = new List<string>();
		Dictionary<string,string> keywordsDict = new Dictionary<string,string>();

		string[] data = importer.userData.Split(new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
		foreach(string d in data)
		{
			if(string.IsNullOrEmpty(d)) continue;

			switch(d[0])
			{
			//Features
			case 'F': featuresList.Add(d.Substring(1)); break;
			//Flags
			case 'f': flagsList.Add(d.Substring(1)); break;
			//Keywords
			case 'K':
				string[] kw = d.Substring(1).Split(':');
				if(kw.Length != 2)
				{
					Debug.LogError("[TCP2 Shader Generator] Error while parsing userData: invalid Keywords format.");
					Features = null; Flags = null; Keywords = null; CustomData = null;
					return;
				}
				else
				{
					keywordsDict.Add(kw[0], kw[1]);
				}
				break;
			//Custom Data
			case 'c': customDataList.Add(d.Substring(1)); break;
			//old format
			default: featuresList.Add(d); break;
			}
		}

		Features = featuresList.ToArray();
		Flags = flagsList.ToArray();
		Keywords = keywordsDict;
		CustomData = customDataList.ToArray();
	}
	static public string[] GetUserDataFeatures(ShaderImporter importer)
	{
		//Contains Features & Flags
		if(importer.userData.Contains("|"))
		{
			string[] data = importer.userData.Split('|');
			if(data.Length < 2)
			{
				Debug.LogError("[TCP2 Shader Generator] Invalid userData in ShaderImporter.\n" + importer.userData);
				return null;
			}
			else
			{
				string[] features = data[0].Split(new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
				return features;
			}
		}
		//No Flags data
		else
		{
			return importer.userData.Split(',');
		}
	}

	//Get Flags array from ShaderImporter
	static public string[] GetUserDataFlags(ShaderImporter importer)
	{
		//Contains Flags
		if(importer.userData.Contains("|"))
		{
			string[] data = importer.userData.Split('|');
			if(data.Length < 2)
			{
				Debug.LogError("[TCP2 Shader Generator] Invalid userData in ShaderImporter.\n" + importer.userData);
				return null;
			}
			else
			{
				string[] flags = data[1].Split(new string[]{","}, System.StringSplitOptions.RemoveEmptyEntries);
				return flags;
			}
		}
		//No Flags data
		else
		{
			return null;
		}
	}

	//--------------------------------------------------------------------------------------------------
	// PRIVATE - SHADER GENERATION

	static private string GetIncludePrefix(TCP2_Config config)
	{
		//Folder
		if(!config.Filename.Contains("/"))
			return "";

		string prefix = "";
		foreach(char c in config.Filename) if(c == '/') prefix += "../";
		return prefix;
	}

	static private string GetExistingShaderPath(TCP2_Config config, Shader existingShader)
	{
		//Override OutputPath if Shader already exists, to make sure we replace the original shader file
		string unityPath = AssetDatabase.GetAssetPath(existingShader);
		unityPath = unityPath.Replace(".shader", "");       //remove extension
		unityPath = Path.GetDirectoryName(unityPath);
		if (config.Filename.Contains("/"))
		{
			string filenamePath = Path.GetDirectoryName(config.Filename);
			unityPath = unityPath.Replace(filenamePath, "");       //remove subdirectories
		}
		unityPath = unityPath.Substring(6);                 //get only directory without leading "Assets"
		if (!unityPath.EndsWith("/"))
			unityPath = unityPath + "/";
		return unityPath;
	}

	static private string GetIncludeRelativePath(TCP2_Config config, Shader existingShader)
	{
		string outputPath = OutputPath;
		if (existingShader != null)
		{
			outputPath = GetExistingShaderPath(config, existingShader);
		}

		if (outputPath == OUTPUT_PATH)
			return INCLUDE_REL_PATH;
		else
		{
			string[] tcp2includeFile = Directory.GetFiles(Application.dataPath, "TCP2_Include.cginc", SearchOption.AllDirectories);
			if (tcp2includeFile.Length == 1)
			{
				string absoluteTcp2IncludeDir = Path.GetDirectoryName(tcp2includeFile[0]) + "/";
				string absoluteShaderPath = Application.dataPath + outputPath;
				string relativePath = MakeRelativePath(absoluteShaderPath, absoluteTcp2IncludeDir);
				if (outputPath != "/")
					relativePath = "../" + relativePath;
				return relativePath;
			}
			else
			{
				EditorApplication.Beep();
				Debug.LogError("Can't find file 'TCP2_Include.cginc' in project!\nCan't figure out the relative include path to the generated shader.");
				return INCLUDE_REL_PATH;
			}
		}
	}

	static private string GetIncludeFile(TCP2_Config config)
	{
		return "TCP2_Include.cginc";
	}

	static private string GetLightingFunction(TCP2_Config config)
	{
		bool customLighting = HasFeatures(config, "CUSTOM_LIGHTING");
		if(customLighting)
			return "ToonyColorsCustom";

		bool specular = HasFeatures(config, "SPECULAR", "SPECULAR_ANISOTROPIC");
		
		if(specular)
			return "ToonyColorsSpec";
		else
			return "ToonyColors";
	}
	
	static private string GetSurfaceOutput(TCP2_Config config)
	{
		return "SurfaceOutput";
	}

	static private string GetShaderTarget(TCP2_Config config)
	{
		return (config.shaderTarget/10f).ToString("0.0");

		/*
		bool tessellate = HasFeatures(config, "DX11_TESSELLATION");
		bool forcesm2 = HasFeatures(config, "FORCE_SM2");

		if(forcesm2)
			return "2.0";
		else if(tessellate)
			return "5.0";
		else
			return "3.0";
		*/
	}
	
	static private bool NeedVertexFunction(TCP2_Config config)
	{
		bool vFunc = HasFeatures(config, "VERTEX_FUNC");
		bool anisotropic = HasFeatures(config, "SPECULAR_ANISOTROPIC");
		bool matcap = HasFeatures(config, "MATCAP");
		bool sketch = HasFeatures(config, "SKETCH", "SKETCH_GRADIENT");
		bool rimdir = HasFeatures(config, "RIMDIR");
		bool rim = HasFeatures(config, "RIM", "RIM_OUTLINE");
		bool rimvertex = HasFeatures(config, "RIM_VERTEX");
		bool bump = HasFeatures(config, "BUMP");
		bool cstamb = HasFeatures(config, "CUSTOM_AMBIENT");
		
		return vFunc || matcap || sketch || (rimvertex && rim) || (bump && rimdir && rim) || anisotropic || cstamb;
	}
	
	static private bool NeedFinalColorFunction(TCP2_Config config)
	{
		bool rim_lightmask = HasFeatures(config, "RIM_LIGHTMASK");

		return rim_lightmask;
	}

	static private bool NeedCustomLighting(TCP2_Config config)
	{
		bool specmask = HasFeatures(config, "SPECULAR_MASK");
		bool anisotropic = HasFeatures(config, "SPECULAR_ANISOTROPIC");
		bool texthreshold = HasFeatures(config, "TEXTURED_THRESHOLD");
		bool occlusion = HasFeatures(config, "OCCLUSION");
		bool indshadows = HasFeatures(config, "INDEPENDENT_SHADOWS");
		bool sketch = HasFeatures(config, "SKETCH", "SKETCH_GRADIENT");
		bool lightmap = HasFeatures(config, "LIGHTMAP");
		bool dsbWrapLight = HasFeatures(config, "DISABLE_WRAPPED_LIGHTING");
		bool rim_lightmask = HasFeatures(config, "RIM_LIGHTMASK");
		bool color_mult = HasFeatures(config, "COLOR_MULTIPLIERS");
		bool subsurface = HasFeatures(config, "SUBSURFACE_SCATTERING");
		bool diffTint = HasFeatures(config, "DIFFUSE_TINT");

		return dsbWrapLight || specmask || anisotropic || occlusion || texthreshold || indshadows || sketch || lightmap || rim_lightmask || color_mult || subsurface || diffTint;
	}

	static private bool NeedCustomAmbient(TCP2_Config config)
	{
		bool occlusion = HasFeatures(config, "OCCLUSION");
		bool cubeambient = HasFeatures(config, "CUBE_AMBIENT");
		bool dirambient = HasFeatures(config, "DIRAMBIENT");
		
		return cubeambient || occlusion || dirambient;
	}
}
