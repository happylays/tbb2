// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno

//#define DEBUG_MODE

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Utility to generate custom Toony Colors Pro 2 shaders with specific features

public class TCP2_ShaderGenerator : EditorWindow
{
	//--------------------------------------------------------------------------------------------------

	[MenuItem(TCP2_Menu.MENU_PATH + "Shader Generator", false, 500)]
	static void OpenTool()
	{
		GetWindowTCP2();
	}

	static public void OpenWithShader(Shader shader)
	{
		TCP2_ShaderGenerator shaderGenerator = GetWindowTCP2();
		shaderGenerator.LoadCurrentConfigFromShader(shader);
	}

	static private TCP2_ShaderGenerator GetWindowTCP2()
	{
		TCP2_ShaderGenerator window = EditorWindow.GetWindow<TCP2_ShaderGenerator>(true, "TCP2 : Shader Generator", true);
		window.minSize = new Vector2(375f, 400f);
		window.maxSize = new Vector2(500f, 900f);
		return window;
	}

	//--------------------------------------------------------------------------------------------------
	// UI
	
	//Represents a template
	public class ShaderGeneratorTemplate
	{
		public TextAsset textAsset { get; private set; }
		public string templateInfo;
		public string templateWarning;
		public string templateType;
		public bool newSystem;			//if false, use the hard-coded GUI and dependencies/conditions
		public UIFeature[] uiFeatures;

		public ShaderGeneratorTemplate()
		{
			TryLoadTextAsset();
		}

		public void SetTextAsset( TextAsset templateAsset )
		{
			if (this.textAsset != templateAsset)
			{
				this.textAsset = templateAsset;
				UpdateTemplateMeta();
			}
		}

		public void FeaturesGUI(TCP2_Config config)
		{
			if (this.uiFeatures == null)
			{
				EditorGUILayout.HelpBox("Couldn't parse the features from the Template.", MessageType.Error);
				return;
			}

			int length = this.uiFeatures.Length;
			for (int i = 0; i < length; i++)
			{
				this.uiFeatures[i].DrawGUI(config);
			}
		}

		public string GetMaskDisplayName(string maskFeature)
		{
			foreach(var uiFeature in this.uiFeatures)
			{
				if(uiFeature is UIFeature_Mask && (uiFeature as UIFeature_Mask).maskKeyword == maskFeature)
				{
					return (uiFeature as UIFeature_Mask).displayName;
				}
			}

			return "Unknown Mask";
		}

		public bool GetMaskDependency(string maskFeature, TCP2_Config config)
		{
			foreach (var uiFeature in this.uiFeatures)
			{
				if (uiFeature is UIFeature_Mask && (uiFeature as UIFeature_Mask).keyword == maskFeature)
				{
					return uiFeature.Enabled(config);
				}
			}

			return true;
		}

		//Try to load a Template according to a config type and/or file
		public void TryLoadTextAsset( string configType = null, string configFile = null )
		{
			//Append file extension if necessary
			if(!string.IsNullOrEmpty(configFile) && !configFile.EndsWith(".txt"))
			{
				configFile = configFile + ".txt";
			}

			TextAsset loadedTextAsset = null;

			if (!string.IsNullOrEmpty(configType))
			{
				switch (configType)
				{
					case "terrain":
						loadedTextAsset = LoadTextAsset("TCP2_User_Unity5_Terrain.txt");
						if(loadedTextAsset != null)
						{
							SetTextAsset(loadedTextAsset);
							return;
						}
						break;
				}
			}

			if (!string.IsNullOrEmpty(configFile))
			{
				TextAsset conf = LoadTextAsset(configFile);
				if (conf != null)
				{
					loadedTextAsset = conf;
					if (loadedTextAsset != null)
					{
						SetTextAsset(loadedTextAsset);
						return;
					}
				}
			}

			loadedTextAsset = LoadTextAsset("TCP2_User_Unity5.txt");
			if (loadedTextAsset != null)
			{
				SetTextAsset(loadedTextAsset);
				return;
			}
		}

		//--------

		private TextAsset LoadTextAsset( string filename )
		{
			TextAsset asset = AssetDatabase.LoadAssetAtPath(string.Format("Assets/JMO Assets/Toony Colors Pro/Editor/Shader Templates/{0}", filename), typeof(TextAsset)) as TextAsset;
			if (asset == null)
			{
				string filenameNoExtension = Path.GetFileNameWithoutExtension(filename);
				string[] guids = AssetDatabase.FindAssets(string.Format("{0} t:TextAsset", filenameNoExtension));
				if (guids.Length >= 1)
				{
					string path = AssetDatabase.GUIDToAssetPath(guids[0]);
					asset = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset)) as TextAsset;
				}
			}

			return asset;
		}

		private void UpdateTemplateMeta()
		{
			uiFeatures = null;
			this.newSystem = false;
			this.templateInfo = null;
			this.templateWarning = null;
			this.templateType = null;

			if (this.textAsset != null && !string.IsNullOrEmpty(this.textAsset.text))
			{
				using (System.IO.StringReader reader = new StringReader(this.textAsset.text))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (line.StartsWith("#INFO="))
						{
							this.templateInfo = line.Substring(6).TrimEnd().Replace("  ", "\n");
						}

						else if (line.StartsWith("#WARNING="))
						{
							this.templateWarning = line.Substring(9).TrimEnd().Replace("  ", "\n");
						}

						else if (line.StartsWith("#CONFIG="))
						{
							this.templateType = line.Substring(8).TrimEnd().ToLower();
						}

						else if (line.StartsWith("#FEATURES"))
						{
							this.newSystem = true;
							this.uiFeatures = UIFeature.GetUIFeatures(reader);
						}

						//Config meta should appear before the Shader name line
						else if (line.StartsWith("Shader"))
						{
							return;
						}
					}
				}
			}
		}
	}

	private ShaderGeneratorTemplate _Template;
	private ShaderGeneratorTemplate Template
	{
		get
		{
			if(_Template == null)
				_Template = new ShaderGeneratorTemplate();
			return _Template;
		}
	}

	//--------------------------------------------------------------------------------------------------
	// UI from Template System

	#region UIFeatures

	public class UIFeature
	{
		public string label;
		public string tooltip;
		public string[] requires;	//features required for this feature to be enabled (AND)
		public string[] requiresOr;	//features required for this feature to be enabled (OR)
		public string[] excludes;   //features required to be OFF for this feature to be enabled
		public string[] excludesAll;   //features required to be OFF for this feature to be enabled
		public bool showHelp = true;
		public bool helpIndent = true;
		public bool increaseIndent;
		public string helpTopic;

		virtual public void DrawGUI(TCP2_Config config)
		{
			GUILayout.Label("Unknown feature type for: " + this.label);
		}

		public bool Enabled(TCP2_Config config)
		{
			bool enabled = true;
			if (this.requiresOr != null)
			{
				enabled = false;
				enabled |= HasFeatOr(config, this.requiresOr);
			}
			if (this.excludesAll != null)
				enabled &= !HasFeatAnd(config, this.excludesAll);
			if (this.requires != null)
				enabled &= HasFeatAnd(config, this.requires);
			if (this.excludes != null)
				enabled &= !HasFeatOr(config, this.excludes);
			return enabled;
		}

		//Parses a #FEATURES text block
		static public UIFeature[] GetUIFeatures( StringReader reader )
		{
			List<UIFeature> uiFeaturesList = new List<UIFeature>();
			string subline;
			int overflow = 0;
			while ((subline = reader.ReadLine()) != "#END")
			{
				//Just in case template file is badly written
				overflow++;
				if (overflow > 99999)
					break;

				string[] data = subline.Split(new char[] { '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

				//Skip empty or comment # lines
				if (data == null || data.Length == 0 || (data.Length > 0 && data[0].StartsWith("#")))
					continue;

				List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();
				for (int i = 1; i < data.Length; i++)
				{
					var sdata = data[i].Split('=');
					kvpList.Add(new KeyValuePair<string, string>(sdata[0], sdata[1]));
				}

				UIFeature feature = null;
				switch (data[0])
				{
					case "---":
						feature = new UIFeature_Separator();
						break;

					case "space":
						feature = new UIFeature_Space();
						if (data.Length > 1)
						{
							foreach (var kvp in kvpList)
							{
								switch (kvp.Key)
								{
									case "space": (feature as UIFeature_Space).space = float.Parse(kvp.Value); break;
								}
							}
						}
						break;

					case "flag":
						feature = new UIFeature_Flag();
						foreach (var kvp in kvpList)
						{
							switch (kvp.Key)
							{
								case "kw": (feature as UIFeature_Flag).keyword = kvp.Value; break;
							}
						}
						break;

					case "subh":
						feature = new UIFeature_SubHeader();
						break;

					case "header":
						feature = new UIFeature_Header();
						break;

					case "warning":
						feature = new UIFeature_Warning();
						foreach (var kvp in kvpList)
						{
							switch (kvp.Key)
							{
								case "msgType": (feature as UIFeature_Warning).msgType = (MessageType)System.Enum.Parse(typeof(MessageType), kvp.Value, true); break;
							}
						}
						break;

					case "sngl":
						feature = new UIFeature_Single();
						foreach (var kvp in kvpList)
						{
							switch (kvp.Key)
							{
								case "kw": (feature as UIFeature_Single).keyword = kvp.Value; break;
								case "toggles": (feature as UIFeature_Single).toggles = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
							}
						}
						break;

					case "mult":
						feature = new UIFeature_Multiple();
						foreach (var kvp in kvpList)
						{
							switch (kvp.Key)
							{
								case "kw": (feature as UIFeature_Multiple).labelsAndFeatures = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
							}
						}
						break;

					case "mask":
						feature = new UIFeature_Mask();
						foreach (var kvp in kvpList)
						{
							switch (kvp.Key)
							{
								case "kw": (feature as UIFeature_Mask).keyword = kvp.Value; break;
								case "ch": (feature as UIFeature_Mask).channelKeyword = kvp.Value; break;
								case "msk": (feature as UIFeature_Mask).maskKeyword = kvp.Value; break;
								case "dispName": (feature as UIFeature_Mask).displayName = kvp.Value; break;
							}
						}
						break;

					case "shader_target":
						feature = new UIFeature_ShaderTarget();
						break;

					default: feature = new UIFeature(); break;
				}

				foreach (var kvp in kvpList)
				{
					switch (kvp.Key)
					{
						case "lbl": feature.label = kvp.Value; break;
						case "tt": feature.tooltip = kvp.Value.Replace(@"\n", "\n"); break;
						case "help": feature.showHelp = bool.Parse(kvp.Value); break;
						case "helpIndent": feature.helpIndent = bool.Parse(kvp.Value); break;
						case "indent": feature.increaseIndent = bool.Parse(kvp.Value); break;
						case "needs": feature.requires = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
						case "needsOr": feature.requiresOr = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
						case "excl": feature.excludes = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
						case "exclAll": feature.excludesAll = kvp.Value.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); break;
						case "hlptop": feature.helpTopic = kvp.Value; break;
					}
				}

				uiFeaturesList.Add(feature);
			}
			return uiFeaturesList.ToArray();
		}
	}

	public class UIFeature_Single : UIFeature
	{
		public string keyword;
		public string[] toggles;    //features forced to be toggled when this feature is enabled

		public override void DrawGUI(TCP2_Config config)
		{
			bool featureOn = GUISingleFeature(config, this.keyword, this.label, this.tooltip, this.Enabled(config), this.increaseIndent, helpTopic: this.helpTopic, showHelp: this.showHelp, helpIndent:this.helpIndent);
			if(toggles != null && featureOn)
			{
				foreach(var t in toggles)
				{
					TCP2_ShaderGeneratorUtils.ToggleSingleFeature(config.Features, t, true);
				}
			}
		}
	}

	public class UIFeature_Multiple : UIFeature
	{
		public string[] labelsAndFeatures;

		public override void DrawGUI( TCP2_Config config )
		{
			GUIMultipleFeatures(config, this.label, this.tooltip, this.labelsAndFeatures, this.Enabled(config), this.increaseIndent, this.helpTopic, this.showHelp, this.helpIndent);
		}
	}

	public class UIFeature_Mask : UIFeature
	{
		public string maskKeyword;
		public string channelKeyword;
		public string keyword;
		public string displayName;

		public override void DrawGUI( TCP2_Config config )
		{
			GUIMask(config, this.label, this.tooltip, this.maskKeyword, this.channelKeyword, this.keyword, this.Enabled(config), this.increaseIndent, helpTopic: this.helpTopic, helpIndent: this.helpIndent);
		}
	}

	public class UIFeature_ShaderTarget : UIFeature
	{
		public override void DrawGUI( TCP2_Config config )
		{
			EditorGUILayout.BeginHorizontal();
			TCP2_GUI.HelpButton("Shader Target");
			TCP2_GUI.SubHeader("Shader Target", "Defines the shader target level to compile for", config.shaderTarget != 30, LABEL_WIDTH - 24f);
			int newTarget = EditorGUILayout.IntPopup(config.shaderTarget,
#if UNITY_5_4_OR_NEWER
				new string[] { "2.0", "2.5", "3.0", "3.5", "4.0", "5.0" },
				new int[] { 20, 25, 30, 35, 40, 50 });
#else
				new string[] { "2.0", "3.0", "4.0", "5.0" },
				new int[] { 20, 30, 40, 50 });
#endif
			if (newTarget != config.shaderTarget)
			{
				config.shaderTarget = newTarget;
			}
			EditorGUILayout.EndHorizontal();
		}
	}

	public class UIFeature_Flag : UIFeature
	{
		public string keyword;

		public override void DrawGUI( TCP2_Config config )
		{
			GUISingleFlag(config, this.keyword, this.label, this.tooltip, this.Enabled(config), this.increaseIndent, helpTopic:this.helpTopic);
		}
	}

	public class UIFeature_Separator : UIFeature
	{
		public override void DrawGUI( TCP2_Config config )
		{
			Space();
		}
	}

	public class UIFeature_Space : UIFeature
	{
		public float space = 8f;

		public override void DrawGUI( TCP2_Config config )
		{
			GUILayout.Space(space);
		}
	}

	public class UIFeature_SubHeader : UIFeature
	{
		public override void DrawGUI( TCP2_Config config )
		{
			TCP2_GUI.SubHeaderGray(this.label);
		}
	}

	public class UIFeature_Header : UIFeature
	{
		public override void DrawGUI( TCP2_Config config )
		{
			TCP2_GUI.Header(this.label);
		}
	}

	public class UIFeature_Warning : UIFeature
	{
		public MessageType msgType = MessageType.Warning;

		public override void DrawGUI( TCP2_Config config )
		{
			if(this.Enabled(config))
				EditorGUILayout.HelpBox(this.label, msgType);
		}
	}

	#endregion

	//--------------------------------------------------------------------------------------------------
	// INTERFACE

	private Shader mCurrentShader;
	private TCP2_Config mCurrentConfig;
	private int mCurrentHash;
	private Shader[] mUserShaders;
	private List<string> mUserShadersLabels;
	private Vector2 mScrollPosition;
	private int mConfigChoice;
	private bool mIsModified;
	private bool mDirtyConfig;

	//Static
	static private bool sHideDisabled;
	static private bool sAutoNames;
	static private bool sOverwriteConfigs;
	static private bool sLoadAllShaders;
	static private bool sSelectGeneratedShader;
	static private bool sGUIEnabled;

#if DEBUG_MODE
	private string mDebugText;
	private bool mDebugExpandUserData;
	private ShaderImporter mCurrentShaderImporter;
#endif

	void OnEnable()
	{
		LoadUserPrefs();
		ReloadUserShaders();
		if(mUserShaders != null && mUserShaders.Length > 0)
		{
			if((mConfigChoice-1) > 0 && (mConfigChoice-1) < mUserShaders.Length)
			{
				mCurrentShader = mUserShaders[mConfigChoice-1];
				LoadCurrentConfigFromShader(mCurrentShader);
			}
			else
				NewShader();
		}
	}

	void OnDisable()
	{
		SaveUserPrefs();
	}

	void OnGUI()
	{
		sGUIEnabled = GUI.enabled;

		EditorGUILayout.BeginHorizontal();
		TCP2_GUI.HeaderBig("TOONY COLORS PRO 2 - SHADER GENERATOR");
		TCP2_GUI.HelpButton("Shader Generator");
		EditorGUILayout.EndHorizontal();
		TCP2_GUI.Separator();

		float lW = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 105f;

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginHorizontal();
		mCurrentShader = EditorGUILayout.ObjectField("Current Shader:", mCurrentShader, typeof(Shader), false) as Shader;
		if(EditorGUI.EndChangeCheck())
		{
			if(mCurrentShader != null)
			{
				LoadCurrentConfigFromShader(mCurrentShader);
			}
		}
		if(GUILayout.Button("Copy Shader", EditorStyles.miniButton, GUILayout.Width(78f)))
		{
			CopyShader();
		}
		if(GUILayout.Button("New Shader", EditorStyles.miniButton, GUILayout.Width(76f)))
		{
			NewShader();
		}
		EditorGUILayout.EndHorizontal();

		if(mIsModified)
		{
			EditorGUILayout.HelpBox("It looks like this shader has been modified externally/manually. Updating it will overwrite the changes.", MessageType.Warning);
		}

		if(mUserShaders != null && mUserShaders.Length > 0)
		{
			EditorGUI.BeginChangeCheck();
			int prevChoice = mConfigChoice;
			Color gColor = GUI.color;
			GUI.color = mDirtyConfig ? gColor * Color.yellow : GUI.color;
			GUILayout.BeginHorizontal();
			mConfigChoice = EditorGUILayout.Popup("Load Shader:", mConfigChoice, mUserShadersLabels.ToArray());
			if(GUILayout.Button("◄", EditorStyles.miniButtonLeft, GUILayout.Width(22)))
			{
				mConfigChoice--;
				if(mConfigChoice < 1) mConfigChoice = mUserShaders.Length;
			}
			if(GUILayout.Button("►", EditorStyles.miniButtonRight,GUILayout.Width(22)))
			{
				mConfigChoice++;
				if(mConfigChoice > mUserShaders.Length) mConfigChoice = 1;
			}
			GUILayout.EndHorizontal();
			GUI.color = gColor;
			if(EditorGUI.EndChangeCheck() && prevChoice != mConfigChoice)
			{
				bool load = true;
				if(mDirtyConfig)
				{
					if(mCurrentShader != null)
						load = EditorUtility.DisplayDialog("TCP2 : Shader Generation", "You have unsaved changes for the following shader:\n\n" + mCurrentShader.name + "\n\nDiscard the changes and load a new shader?", "Yes", "No");
					else
						load = EditorUtility.DisplayDialog("TCP2 : Shader Generation", "You have unsaved changes.\n\nDiscard the changes and load a new shader?", "Yes", "No");
				}
				
				if(load)
				{
					//New Shader
					if(mConfigChoice == 0)
					{
						NewShader();
					}
					else
					{
						//Load selected Shader
						Shader selectedShader = mUserShaders[mConfigChoice-1];
						mCurrentShader = selectedShader;
						LoadCurrentConfigFromShader(mCurrentShader);
					}
				}
				else
				{
					//Revert choice
					mConfigChoice = prevChoice;
				}
			}
		}

		//Avoid refreshing Template meta at every Repaint
		TextAsset _tmpTemplate = EditorGUILayout.ObjectField("Template:", Template.textAsset, typeof(TextAsset), false) as TextAsset;
		if(_tmpTemplate != Template.textAsset)
		{
			Template.SetTextAsset(_tmpTemplate);
		}

		//Template not found
		if (Template == null || Template.textAsset == null)
		{
			EditorGUILayout.HelpBox("Couldn't find template file!\n\nVerify that the file TCP2_User_Unity5.txt is in your project.\nPlease reimport the pack if you can't find it!", MessageType.Error);
			return;
		}

		//Infobox for custom templates
		if (!string.IsNullOrEmpty(Template.templateInfo))
		{
			EditorGUILayout.HelpBox(Template.templateInfo, MessageType.Info);
		}
		if(!string.IsNullOrEmpty(Template.templateWarning))
		{
			EditorGUILayout.HelpBox(Template.templateWarning, MessageType.Warning);
		}

		EditorGUIUtility.labelWidth = lW;
		
		if(mCurrentConfig == null)
		{
			NewShader();
		}

		//Name & Filename
		TCP2_GUI.Header("NAME");
		GUI.enabled = (mCurrentShader == null);
		EditorGUI.BeginChangeCheck();
		mCurrentConfig.ShaderName = EditorGUILayout.TextField(new GUIContent("Shader Name", "Path will indicate how to find the Shader in Unity's drop-down list"), mCurrentConfig.ShaderName);
		mCurrentConfig.ShaderName = Regex.Replace(mCurrentConfig.ShaderName, @"[^a-zA-Z0-9 _!/]", "");
		if(EditorGUI.EndChangeCheck() && sAutoNames)
		{
			AutoNames();
		}
		GUI.enabled &= !sAutoNames;
		EditorGUILayout.BeginHorizontal();
		mCurrentConfig.Filename = EditorGUILayout.TextField("File Name", mCurrentConfig.Filename);
		mCurrentConfig.Filename = Regex.Replace(mCurrentConfig.Filename, @"[^a-zA-Z0-9 _!/]", "");
		GUILayout.Label(".shader", GUILayout.Width(50f));
		EditorGUILayout.EndHorizontal();
		GUI.enabled = sGUIEnabled;

		Space();

		//########################################################################################################
		// FEATURES

		TCP2_GUI.Header("FEATURES");

		//Scroll view
		mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
		EditorGUI.BeginChangeCheck();

		if (Template.newSystem)
		{
			//New UI embedded into Template
			Template.FeaturesGUI(mCurrentConfig);
		}
		else
		{
			//Old hard-coded UI
			switch (Template.templateType)
			{
				case "terrain": FeaturesGUI_Terrain(); break;
				default: FeaturesGUI(); break;
			}
		}

#if DEBUG_MODE
		TCP2_GUI.SeparatorBig();

		TCP2_GUI.SubHeaderGray("DEBUG MODE");
		//Custom Lighting
		GUISingleFeature(mCurrentConfig, "CUSTOM_LIGHTING_FORCE", "Custom Lighting", "Use an inline custom lighting model, allowing more flexibility per shader over lighting", showHelp:false);
		GUISingleFeature(mCurrentConfig, "VERTEX_FUNC", "Vertex Function", "Force custom vertex function in surface shader", showHelp:false);
		Space();

		GUILayout.BeginHorizontal();
		mDebugText = EditorGUILayout.TextField("Custom", mDebugText);
		if(GUILayout.Button("Add Feature", EditorStyles.miniButtonLeft, GUILayout.Width(80f)))
			mCurrentConfig.Features.Add(mDebugText);
		if(GUILayout.Button("Add Flag", EditorStyles.miniButtonRight, GUILayout.Width(80f)))
			mCurrentConfig.Flags.Add(mDebugText);

		GUILayout.EndHorizontal();
		GUILayout.Label("Features:");
		GUILayout.BeginHorizontal();
		int count = 0;
		for(int i = 0; i < mCurrentConfig.Features.Count; i++)
		{
			if(count >= 3)
			{
				count = 0;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			count++;
			if(GUILayout.Button(mCurrentConfig.Features[i], EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
			{
				mCurrentConfig.Features.RemoveAt(i);
				break;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Flags:");
		GUILayout.BeginHorizontal();
		count = 0;
		for(int i = 0; i < mCurrentConfig.Flags.Count; i++)
		{
			if(count >= 3)
			{
				count = 0;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			count++;
			if(GUILayout.Button(mCurrentConfig.Flags[i], EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
			{
				mCurrentConfig.Flags.RemoveAt(i);
				break;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Keywords:");
		GUILayout.BeginHorizontal();
		count = 0;
		foreach(KeyValuePair<string,string> kvp in mCurrentConfig.Keywords)
		{
			if(count >= 3)
			{
				count = 0;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
			count++;
			if(GUILayout.Button(kvp.Key + ":" + kvp.Value, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
			{
				mCurrentConfig.Keywords.Remove(kvp.Key);
				break;
			}
		}
		GUILayout.EndHorizontal();

		//----------------------------------------------------------------

		Space();
		if(mCurrentShader != null)
		{
			if(mCurrentShaderImporter != null && mCurrentShaderImporter.GetShader() == mCurrentShader)
			{
				mDebugExpandUserData = EditorGUILayout.Foldout(mDebugExpandUserData, "Shader UserData");
				if(mDebugExpandUserData)
				{
					string[] userData = mCurrentShaderImporter.userData.Split(',');
					foreach(var str in userData)
					{
						GUILayout.Label(str);
					}
				}
			}
			else
			{
				mCurrentShaderImporter = ShaderImporter.GetAtPath(AssetDatabase.GetAssetPath(mCurrentShader)) as ShaderImporter;
			}
		}
#endif

		//Update config
		if (EditorGUI.EndChangeCheck())
		{
			int newHash = mCurrentConfig.ToHash();
			if(newHash != mCurrentHash)
			{
				mDirtyConfig = true;
			}
			else
			{
				mDirtyConfig = false;
			}
		}

		//Scroll view
		EditorGUILayout.EndScrollView();

		Space();

		//GENERATE

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
#if DEBUG_MODE
		if(GUILayout.Button("Re-Generate All", GUILayout.Width(120f), GUILayout.Height(30f)))
		{
			float progress = 0;
			float total = mUserShaders.Length;
			foreach(Shader s in mUserShaders)
			{
				progress++;
				EditorUtility.DisplayProgressBar("Hold On", "Generating Shader: " + s.name, progress/total);

				mCurrentShader = null;
				LoadCurrentConfigFromShader(s);
				if(mCurrentShader != null && mCurrentConfig != null)
				{
					TCP2_ShaderGeneratorUtils.Compile(mCurrentConfig, s, Template.text, false, !sOverwriteConfigs, mIsModified);
				}
			}
			EditorUtility.ClearProgressBar();
		}
#endif
		if(GUILayout.Button(mCurrentShader == null ? "Generate Shader" : "Update Shader", GUILayout.Width(120f), GUILayout.Height(30f)))
		{
			if(Template == null)
			{
				EditorUtility.DisplayDialog("TCP2 : Shader Generation", "Can't generate shader: no Template file defined!\n\nYou most likely want to link the TCP2_User.txt file to the Template field in the Shader Generator.", "Ok");
				return;
			}

			//Set config type
			if (Template.templateType != null)
			{
				mCurrentConfig.configType = Template.templateType;
			}

			//Set config file
			mCurrentConfig.templateFile = Template.textAsset.name;

			Shader generatedShader = TCP2_ShaderGeneratorUtils.Compile(mCurrentConfig, mCurrentShader, Template, true, !sOverwriteConfigs, mIsModified);
			ReloadUserShaders();
			if(generatedShader != null)
			{
				mDirtyConfig = false;
				LoadCurrentConfigFromShader(generatedShader);
				mIsModified = false;
			}
		}
		EditorGUILayout.EndHorizontal();
		TCP2_GUI.Separator();

		// OPTIONS
		TCP2_GUI.Header("OPTIONS");

		GUILayout.BeginHorizontal();
		sSelectGeneratedShader = GUILayout.Toggle(sSelectGeneratedShader, new GUIContent("Select Generated Shader", "Will select the generated file in the Project view"), GUILayout.Width(180f));
		sAutoNames = GUILayout.Toggle(sAutoNames, new GUIContent("Automatic Name", "Will automatically generate the shader filename based on its UI name"), GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		sOverwriteConfigs = GUILayout.Toggle(sOverwriteConfigs, new GUIContent("Always overwrite shaders", "Overwrite shaders when generating/updating (no prompt)"), GUILayout.Width(180f));
		sHideDisabled = GUILayout.Toggle(sHideDisabled, new GUIContent("Hide disabled fields", "Hide properties settings when they cannot be accessed"), GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginChangeCheck();
		TCP2_ShaderGeneratorUtils.CustomOutputDir = GUILayout.Toggle(TCP2_ShaderGeneratorUtils.CustomOutputDir, new GUIContent("Custom Output Directory:", "Will save the generated shaders in a custom directory within the Project"), GUILayout.Width(165f));
		GUI.enabled &= TCP2_ShaderGeneratorUtils.CustomOutputDir;
		if (TCP2_ShaderGeneratorUtils.CustomOutputDir)
		{
			TCP2_ShaderGeneratorUtils.OutputPath = EditorGUILayout.TextField("", TCP2_ShaderGeneratorUtils.OutputPath);
			if(GUILayout.Button("Select...", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
			{
				string path = EditorUtility.OpenFolderPanel("Choose custom output directory for TCP2 generated shaders", "", "");
				if(!string.IsNullOrEmpty(path))
				{
					bool validPath = TCP2_Utils.SystemToUnityPath(ref path);
					if (validPath)
					{
						TCP2_ShaderGeneratorUtils.OutputPath = path.Substring("Assets/".Length);
					}
					else
					{
						EditorApplication.Beep();
						EditorUtility.DisplayDialog("Invalid Path", "The selected path is invalid.\n\nPlease select a folder inside the \"Assets\" folder of your project!", "Ok");
					}
				}
			}
		}
		else
			EditorGUILayout.TextField("", TCP2_ShaderGeneratorUtils.OUTPUT_PATH);
		if (EditorGUI.EndChangeCheck())
		{
			ReloadUserShaders();
		}

		GUI.enabled = sGUIEnabled;
		EditorGUILayout.EndHorizontal();

		EditorGUI.BeginChangeCheck();
		sLoadAllShaders = GUILayout.Toggle(sLoadAllShaders, new GUIContent("Reload Shaders from all Project", "Load shaders from all your Project folders instead of just Toony Colors Pro 2.\nEnable it if you move your generated shader files outside of the default TCP2 Generated folder."), GUILayout.ExpandWidth(false));
		if(EditorGUI.EndChangeCheck())
		{
			ReloadUserShaders();
		}

		TCP2_ShaderGeneratorUtils.SelectGeneratedShader = sSelectGeneratedShader;
	}

	void OnProjectChange()
	{
		ReloadUserShaders();
		if(mCurrentShader == null && mConfigChoice != 0)
		{
			NewShader();
		}
	}

	//--------------------------------------------------------------------------------------------------
	// FEATURES
	
	//Old hard-coded UI
	private void FeaturesGUI()
	{
		//Ramp
		GUIMultipleFeatures(mCurrentConfig, "Ramp Style", "Defines the transitioning between dark and lit areas of the model", new string[] { "Slider Ramp|", "Texture Ramp|TEXTURE_RAMP" });
		//Color Multipliers
		GUISingleFeature(mCurrentConfig, "COLOR_MULTIPLIERS", "Colors Multipliers", "Adds multiplier values for highlight and shadow colors to enhance contrast or colors", showHelp:false);
		//Diffuse Tint
		GUISingleFeature(mCurrentConfig, "DIFFUSE_TINT", "Diffuse Tint", "Adds a diffuse tint color, to add some subtle coloring to the diffuse lighting");
		//Textured Threshold
		GUISingleFeature(mCurrentConfig, "TEXTURED_THRESHOLD", "Textured Threshold", "Adds a textured variation to the highlight/shadow threshold, allowing handpainting like effects for example");
		//Disable Wrapped Lighting
		GUISingleFeature(mCurrentConfig, "DISABLE_WRAPPED_LIGHTING", "Disable Wrapped Lighting", "Disable wrapped diffuse lighting, making lights appear more focused");
		Space();
		//----------------------------------------------------------------
		//Detail
		GUISingleFeature(mCurrentConfig, "DETAIL_TEX", "Detail Texture");
		//Detail UV2
		GUISingleFeature(mCurrentConfig, "DETAIL_UV2", "Use UV2 coordinates", "Use second texture coordinates for the detail texture", HasFeat("DETAIL_TEX"), true);
		GUIMask(mCurrentConfig, "Detail Mask", null, "DETAIL_MASK", "DETAIL_MASK_CHANNEL", "DETAIL_MASK", HasFeatOr("DETAIL_TEX"), true);
		Space();
		//----------------------------------------------------------------
		//Color Mask
		GUIMask(mCurrentConfig, "Color Mask", "Adds a mask to the main Color", "COLORMASK", "COLORMASK_CHANNEL", "COLORMASK", true, helpTopic: "Color Mask");
		Space();
		//----------------------------------------------------------------
		//Vertex Colors
		GUISingleFeature(mCurrentConfig, "VCOLORS", "Vertex Colors", "Multiplies the color with vertex colors");
		//Texture Blend
		GUISingleFeature(mCurrentConfig, "VCOLORS_BLENDING", "Vertex Texture Blending", "Enables 2-way texture blending based on the mesh's vertex color alpha");
		//Normal Map Blend
		GUISingleFeature(mCurrentConfig, "VCOLORS_BUMP_BLENDING", "Normal Map Blending", "Enables 2-way texture blending for the normal map as well", HasFeat("VCOLORS_BLENDING"), true);
		Space();
		//----------------------------------------------------------------
		//Emission
		GUIMask(mCurrentConfig, "Emission Map", null, "EMISSION_MASK", "EMISSION_MASK_CHANNEL", "EMISSION", helpTopic:"Self-Illumination Map");
		//Emission Color
		GUISingleFeature(mCurrentConfig, "EMISSION_COLOR", "Emission Color", helpTopic:"Self-Illumination Map");
		GUISingleFeature(mCurrentConfig, "EMISSION_COLOR_HDR", "HDR Color", "Makes the Emission Color an HDR color that can go outside the [0:1] range (useful for effects like bloom)", HasFeat("EMISSION_COLOR"), true);
		Space();
		//----------------------------------------------------------------
		//Bump
		GUISingleFeature(mCurrentConfig, "BUMP", "Normal/Bump Map", helpTopic: "normal_bump_map_sg");
		//BumpScale
		GUISingleFeature(mCurrentConfig, "BUMP_SCALE", "Bump Scale", null, HasFeat("BUMP"), true);
		//Parallax
		GUISingleFeature(mCurrentConfig, "PARALLAX", "Parallax/Height Map", null, HasFeat("BUMP"), true);
		Space();
		//----------------------------------------------------------------
		//Occlusion
//		GUISingleFeature(mCurrentConfig, "OCCLUSION", "Occlusion Map", "Use an Occlusion Map that will be multiplied with the Ambient lighting");
		//Occlusion RGB
//		GUISingleFeature(mCurrentConfig, "OCCL_RGB", "Use RGB map", "Use the RGB channels for Occlusion Map if enabled, use Alpha channel is disabled", HasFeat("OCCLUSION"), true);
//		Space();
		//----------------------------------------------------------------
		//Specular
		GUIMultipleFeatures(mCurrentConfig, "Specular", null, new string[] { "Off|", "Regular|SPECULAR", "Anisotropic|SPECULAR_ANISOTROPIC" }, showHelp: true, helpTopic: "specular_sg");
		//Specular Mask
		GUIMask(mCurrentConfig, "Specular Mask", "Enables specular mask (gloss map)", "SPEC_MASK", "SPEC_MASK_CHANNEL", "SPECULAR_MASK", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		//Specular Shininess Mask
		GUIMask(mCurrentConfig, "Shininess Mask", null, "SPEC_SHIN_MASK", "SPEC_SHIN_MASK_CHANNEL", "SPEC_SHIN_MASK", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		//Cartoon Specular
		GUISingleFeature(mCurrentConfig, "SPECULAR_TOON", "Cartoon Specular", "Enables clear delimitation of specular color", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		Space();
		//----------------------------------------------------------------
		//Reflection
		GUISingleFeature(mCurrentConfig, "REFLECTION", "Reflection", "Enables cubemap reflection", helpTopic: "reflection_sg");
		//Reflection Mask
		GUIMask(mCurrentConfig, "Reflection Mask", null, "REFL_MASK", "REFL_MASK_CHANNEL", "REFL_MASK", HasFeatOr("REFLECTION"), true);
#if UNITY_5
		//Unity5 Reflection Probes
		GUISingleFeature(mCurrentConfig, "U5_REFLPROBE", "Reflection Probes (Unity5)", "Pick reflection from Unity 5 Reflection Probes", HasFeat("REFLECTION"), true, helpTopic:"Reflection Probes");
#endif
		//Reflection Color
		GUISingleFeature(mCurrentConfig, "REFL_COLOR", "Reflection Color", "Enables reflection color control", HasFeat("REFLECTION"), true);
		//Reflection Roughness
		GUISingleFeature(mCurrentConfig, "REFL_ROUGH", "Reflection Roughness", "Simulates reflection roughness using the Cubemap's LOD levels\n\nREQUIRES MipMaps ENABLED IN THE CUBEMAP TEXTURE!", HasFeat("REFLECTION") && !HasFeat("U5_REFLPROBE"), true);
		//Rim Reflection
		GUISingleFeature(mCurrentConfig, "RIM_REFL", "Rim Reflection/Fresnel", "Reflection will be multiplied by rim lighting, resulting in a fresnel-like effect", HasFeat("REFLECTION"), true);
		Space();
		//----------------------------------------------------------------
		//Subsurface Scattering
		GUISingleFeature(mCurrentConfig, "SUBSURFACE_SCATTERING", "Subsurface Scattering");
		GUIMultipleFeatures(mCurrentConfig, "Subsurface Lights", "Defines which lights will affect subsurface scattering",
									new string[] { "Point\\Spot Lights|", "Directional Lights|SS_DIR_LIGHTS", "All Lights|SS_ALL_LIGHTS" },
									HasFeat("SUBSURFACE_SCATTERING"), true, showHelp:false);
		GUIMask(mCurrentConfig, "Subsurface Mask", "Defines where the subsurface scattering effect should apply", "SS_MASK", "SS_MASK_CHANNEL", "SS_MASK", HasFeat("SUBSURFACE_SCATTERING"), true);
		GUISingleFeature(mCurrentConfig, "SUBSURFACE_COLOR", "Subsurface Color", "Control the color of the subsurface effect (only affecting back lighting)", HasFeat("SUBSURFACE_SCATTERING"), true, showHelp:false);
		GUISingleFeature(mCurrentConfig, "SUBSURFACE_AMB_COLOR", "Subsurface Ambient Color", "Adds an ambient subsurface color, affecting both front and back lighting", HasFeat("SUBSURFACE_SCATTERING"), true, showHelp:false);
		GUISingleFeature(mCurrentConfig, "SS_MULTIPLICATIVE", "Multiplicative", "Makes the subsurface scattering effect multiplied to the diffuse color instead of added with it", HasFeat("SUBSURFACE_SCATTERING"), true, showHelp:false);
		Space();
		//----------------------------------------------------------------
		//Cubemap Ambient
		GUIMultipleFeatures(mCurrentConfig, "Custom Ambient", "Custom ambient lighting", new string[]{"Off|", "Cubemap Ambient|CUBE_AMBIENT", "Directional Ambient|DIRAMBIENT"});
		Space();
		//----------------------------------------------------------------
		//Independent Shadows
		GUISingleFeature(mCurrentConfig, "INDEPENDENT_SHADOWS", "Independent Shadows", "Disable shadow color influence for cast shadows");
		Space();
		//----------------------------------------------------------------
		//Rim
		GUIMultipleFeatures(mCurrentConfig, "Rim", "Rim effects (fake light coming from behind the model)", new string[]{"Off|", "Rim Lighting|RIM", "Rim Outline|RIM_OUTLINE"}, !(HasFeatAnd("REFLECTION", "RIM_REFL")), false, "rim_sg");
		//Vertex Rim
		GUISingleFeature(mCurrentConfig, "RIM_VERTEX", "Vertex Rim", "Compute rim lighting per-vertex (faster but innacurate)", HasFeatOr("RIM","RIM_OUTLINE"), true);
		//Directional Rim
		GUISingleFeature(mCurrentConfig, "RIMDIR", "Directional Rim", null, HasFeatOr("RIM","RIM_OUTLINE"), true);
		//Rim Mask
		GUIMask(mCurrentConfig, "Rim Mask", null, "RIM_MASK", "RIM_MASK_CHANNEL", "RIM_MASK", HasFeatOr("RIM","RIM_OUTLINE"), true);
		//Rim Mask
		GUISingleFeature(mCurrentConfig, "RIM_LIGHTMASK", "Light-based Mask", "Will make rim be influenced by nearby lights", HasFeat("RIM"), true);
		Space();
		//----------------------------------------------------------------
		//MatCap
		GUIMultipleFeatures(mCurrentConfig, "MatCap", "MatCap effects (fast fake reflection using a spherical texture)", new string[] { "Off|", "MatCap Add|MATCAP_ADD", "MatCap Multiply|MATCAP_MULT" }, showHelp: true, helpTopic: "matcap_sg");

		//MatCap Mask
		GUIMask(mCurrentConfig, "MatCap Mask", null, "MASK_MC", "MASK_MC_CHANNEL", "MASK_MC", HasFeatOr("MATCAP_ADD","MATCAP_MULT"), true);
		//MatCap Pixel
		GUISingleFeature(mCurrentConfig, "MATCAP_PIXEL", "Pixel MatCap", "If enabled, will calculate MatCap per-pixel\nRequires normal map", HasFeat("BUMP") && HasFeatOr("MATCAP_ADD","MATCAP_MULT"), true);
		//MatCap Color
		GUISingleFeature(mCurrentConfig, "MC_COLOR", "MatCap Color", null, HasFeatOr("MATCAP_ADD","MATCAP_MULT"), true);
		Space();
		//----------------------------------------------------------------
		//Sketch
		GUIMultipleFeatures(mCurrentConfig, "Sketch", "Sketch texture overlay on the shadowed areas\nOverlay: regular texture overlay\nGradient: used for halftone-like effects", new string[] { "Off|", "Sketch Overlay|SKETCH", "Sketch Gradient|SKETCH_GRADIENT" });
		//Sketch Blending
		GUIMultipleFeatures(mCurrentConfig, "Sketch Blending", "Defines how to blend the Sketch texture with the model",
		                            new string[]{"Regular|", "Color Burn|SKETCH_COLORBURN"},
		                            HasFeat("SKETCH") && !HasFeat("SKETCH_GRADIENT"), true, showHelp:false);
		//Sketch Anim
		GUISingleFeature(mCurrentConfig, "SKETCH_ANIM", "Animated Sketch", "Animates the sketch overlay texture, simulating a hand-drawn animation style",
		                 HasFeatOr("SKETCH","SKETCH_GRADIENT"), true);
		//Sketch Vertex
		GUISingleFeature(mCurrentConfig, "SKETCH_VERTEX", "Vertex Coords", "Compute screen coordinates in vertex shader (faster but can cause distortions)\nIf disabled will compute in pixel shader (slower)",
		                 HasFeatOr("SKETCH","SKETCH_GRADIENT"), true);
		//Sketch Scale
		//GUISingleFeature(mCurrentConfig, "SKETCH_SCALE", "Scale with model", "If enabled, overlay texture scale will depend on model's distance from view",
		//                 HasFeatOr("SKETCH","SKETCH_GRADIENT"), true);
		//No Obj Offset
		GUISingleFeature(mCurrentConfig, "NO_SKETCH_OFFSET", "Disable Obj-Space Offset", "Prevent the screen-space UVs from being offset based on the object's position",
						 HasFeatOr("SKETCH", "SKETCH_GRADIENT"), true);
		Space();
		//----------------------------------------------------------------
		//Outline
		GUIMultipleFeatures(mCurrentConfig, "Outline", "Outline around the model", new string[] { "Off|", "Opaque Outline|OUTLINE", "Blended Outline|OUTLINE_BLENDING" });
		GUISingleFeature(mCurrentConfig, "OUTLINE_BEHIND", "Outline behind model", "If enabled, outline will only show behind model",
		                 HasFeatOr("OUTLINE","OUTLINE_BLENDING"), true);
		Space();
		//----------------------------------------------------------------
		//Lightmaps
#if UNITY_4_5
		GUISingleFeature(mCurrentConfig, "LIGHTMAP", "TCP2 Lightmap", "Will use TCP2's lightmap decoding, affecting it with ramp and color settings", helpTopic:"Lightmap");
		Space();
#endif
		//----------------------------------------------------------------
		//GPU Instancing
		//GUISingleFeature(mCurrentConfig, "GPU_INSTANCING", "GPU Instancing Support", "Will enable instancing support if enabled", showHelp: false);
		//----------------------------------------------------------------
		TCP2_GUI.SubHeaderGray("Transparency");
		//Alpha Blending
		GUISingleFeature(mCurrentConfig, "ALPHA", "Alpha Blending");
		GUISingleFeature(mCurrentConfig, "DITHERED_SHADOWS", "Dithered Shadows", "Enables dithered shadows for transparent materials", HasFeat("ALPHA"), true);
		//Alpha Testing
		GUISingleFeature(mCurrentConfig, "CUTOUT", "Alpha Testing (Cutout)");
		Space();
		//----------------------------------------------------------------
		//Culling
		int cull = TCP2_ShaderGeneratorUtils.HasFeatures(mCurrentConfig, "CULL_FRONT") ? 1 : TCP2_ShaderGeneratorUtils.HasFeatures(mCurrentConfig, "CULL_OFF") ? 2 : 0;
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(24f);
		TCP2_GUI.SubHeader("Culling", "Defines how to cull faces", cull > 0, 170-24f);
		cull = EditorGUILayout.Popup(cull, new string[]{"Back (default)", "Front", "Off (double-sided)"});
		TCP2_ShaderGeneratorUtils.ToggleSingleFeature(mCurrentConfig.Features, "CULL_FRONT", cull == 1);
		TCP2_ShaderGeneratorUtils.ToggleSingleFeature(mCurrentConfig.Features, "CULL_OFF", cull == 2);
		EditorGUILayout.EndHorizontal();
		Space();
		//----------------------------------------------------------------
		//Shader target
		EditorGUILayout.BeginHorizontal();
		TCP2_GUI.HelpButton("Shader Target");
		TCP2_GUI.SubHeader("Shader Target", "Defines the shader target level to compile for", mCurrentConfig.shaderTarget != 30, 170 - 24f);
		int newTarget = EditorGUILayout.IntPopup(mCurrentConfig.shaderTarget,
#if UNITY_5_4_OR_NEWER
												 new string[] { "2.0", "2.5", "3.0", "3.5", "4.0", "5.0" },
												 new int[] { 20, 25, 30, 35, 40, 50 });
#else
												 new string[] { "2.0", "3.0", "4.0", "5.0" },
												 new int[] { 20, 30, 40, 50 });
#endif
		if (newTarget != mCurrentConfig.shaderTarget)
		{
			mCurrentConfig.shaderTarget = newTarget;
		}
		EditorGUILayout.EndHorizontal();
		Space();

		//----------------------------------------------------------------
		// FLAGS
		GUILayout.Space(6f);
		TCP2_GUI.Header("FLAGS");
		GUILayout.Space(2f);
		GUISingleFlag(mCurrentConfig, "addshadow", "Add Shadow Passes", "Force the shader to have the Shadow Caster and Collector passes.\nCan help if shadows don't work properly with the shader");
		GUISingleFlag(mCurrentConfig, "fullforwardshadows", "Full Forward Shadows", "Enable support for all shadow types in Forward rendering path");
#if UNITY_5
		GUISingleFlag(mCurrentConfig, "noshadow", "Disable Shadows", "Disables all shadow receiving support in this shader");
		GUISingleFlag(mCurrentConfig, "nofog", "Disable Fog", "Disables Unity Fog support.\nCan help if you run out of vertex interpolators and don't need fog.");
#endif
		GUISingleFlag(mCurrentConfig, "nolightmap", "Disable Lightmaps", "Disables all lightmapping support in this shader.\nCan help if you run out of vertex interpolators and don't need lightmaps.");
		GUISingleFlag(mCurrentConfig, "noambient", "Disable Ambient Lighting", "Disable ambient lighting", !HasFeatOr("DIRAMBIENT", "CUBE_AMBIENT", "OCCLUSION"));
		GUISingleFlag(mCurrentConfig, "novertexlights", "Disable Vertex Lighting", "Disable vertex lights and spherical harmonics (light probes)");
		//GUISingleFeature("FORCE_SM2", "Force Shader Model 2", "Compile with Shader Model 2 target. Useful for (very) old GPU compatibility, but some features may not work with it.", showHelp:false);

		GUILayout.Space(6f);
		TCP2_GUI.Header("FLAGS (Mobile-friendly)", null, true);
		GUILayout.Space(2f);
		GUISingleFlag(mCurrentConfig, "noforwardadd", "One Directional Light", "Use additive lights as vertex lights.\nRecommended for Mobile");
#if UNITY_5
		GUISingleFlag(mCurrentConfig, "interpolateview", "Vertex View Dir", "Calculate view direction per-vertex instead of per-pixel.\nRecommended for Mobile");
#else
		GUISingleFlag(mCurrentConfig, "approxview", "Vertex View Dir", "Calculate view direction per-vertex instead of per-pixel.\nRecommended for Mobile");
#endif
		GUISingleFlag(mCurrentConfig, "halfasview", "Half as View", "Pass half-direction vector into the lighting function instead of view-direction.\nFaster but inaccurate.\nRecommended for Specular, but use Vertex Rim to optimize Rim Effects instead");
	}

	//Old hard-coded UI for experimental Terrain
	private void FeaturesGUI_Terrain()
	{
		//Ramp
		GUIMultipleFeatures(mCurrentConfig, "Ramp Style", "Defines the transitioning between dark and lit areas of the model", new string[] { "Slider Ramp|", "Texture Ramp|TEXTURE_RAMP" });
		//Color Multipliers
		GUISingleFeature(mCurrentConfig, "COLOR_MULTIPLIERS", "Colors Multipliers", "Adds multiplier values for highlight and shadow colors to enhance contrast or colors", showHelp: false);
		//Textured Threshold
		// Can't work with TERRAIN: Too many texture interpolators currently in SM3
		//GUISingleFeature(mCurrentConfig, "TEXTURED_THRESHOLD", "Textured Threshold", "Adds a textured variation to the highlight/shadow threshold, allowing handpainting like effects for example");
		//Disable Wrapped Lighting
		GUISingleFeature(mCurrentConfig, "DISABLE_WRAPPED_LIGHTING", "Disable Wrapped Lighting", "Disable wrapped diffuse lighting, making lights appear more focused");
		Space();
		//----------------------------------------------------------------
		//Specular
		GUISingleFeature(mCurrentConfig, "SPECULAR", "Specular", null);
		//GUIMultipleFeaturesHelp("Specular", null, "specular_sg", "Off|", "Regular|SPECULAR", "Anisotropic|SPECULAR_ANISOTROPIC");
		//if (HasFeatAnd("FORCE_SM2", "SPECULAR_ANISOTROPIC"))
		//{
		//	EditorGUILayout.HelpBox("Anisotropic Specular will not compile with Shader Model 2!\n(too many instructions used)", MessageType.Warning);
		//}
		//Specular Mask
		//GUIMask(mCurrentConfig, "Specular Mask", "Enables specular mask (gloss map)", "SPEC_MASK", "SPEC_MASK_CHANNEL", "SPECULAR_MASK", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		//Specular Shininess Mask
		//GUIMask(mCurrentConfig, "Shininess Mask", null, "SPEC_SHIN_MASK", "SPEC_SHIN_MASK_CHANNEL", "SPEC_SHIN_MASK", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		//Cartoon Specular
		GUISingleFeature(mCurrentConfig, "SPECULAR_TOON", "Cartoon Specular", "Enables clear delimitation of specular color", HasFeatOr("SPECULAR", "SPECULAR_ANISOTROPIC"), true);
		Space();
		//----------------------------------------------------------------
		//Independent Shadows
		GUISingleFeature(mCurrentConfig, "INDEPENDENT_SHADOWS", "Independent Shadows", "Disable shadow color influence for cast shadows");
		Space();
		//----------------------------------------------------------------
		//Rim
		GUIMultipleFeatures(mCurrentConfig, "Rim", "Rim effects (fake light coming from behind the model)", new string[] { "Off|", "Rim Lighting|RIM", "Rim Outline|RIM_OUTLINE" }, !(HasFeatAnd("REFLECTION", "RIM_REFL")), false, "rim_sg");
		if (HasFeat("REFLECTION") && HasFeat("RIM_REFL"))
			TCP2_ShaderGeneratorUtils.ToggleSingleFeature(mCurrentConfig.Features, "RIM", true);
		//Vertex Rim
		//GUISingleFeature(mCurrentConfig, "RIM_VERTEX", "Vertex Rim", "Compute rim lighting per-vertex (faster but innacurate)", HasFeatOr("RIM", "RIM_OUTLINE"), true);
		//Directional Rim
		//GUISingleFeature(mCurrentConfig, "RIMDIR", "Directional Rim", null, HasFeatOr("RIM", "RIM_OUTLINE"), true);
		//Rim Mask
		//GUIMask(mCurrentConfig, "Rim Mask", null, "RIM_MASK", "RIM_MASK_CHANNEL", "RIM_MASK", HasFeatOr("RIM", "RIM_OUTLINE"), true);
		//Rim Mask
		GUISingleFeature(mCurrentConfig, "RIM_LIGHTMASK", "Light-based Mask", "Will make rim be influenced by nearby lights", HasFeat("RIM"), true);
		Space();
		//----------------------------------------------------------------
		//Sketch
		// Can't work with TERRAIN: Too many texture interpolators currently in SM3
		GUIMultipleFeatures(mCurrentConfig, "Sketch", "Sketch texture overlay on the shadowed areas\nOverlay: regular texture overlay\nGradient: used for halftone-like effects", new string[] { "Off|", "Sketch Overlay|SKETCH", "Sketch Gradient|SKETCH_GRADIENT" });
		//Sketch Blending
		GUIMultipleFeatures(mCurrentConfig, "Sketch Blending", "Defines how to blend the Sketch texture with the model",
									new string[] { "Regular|", "Color Burn|SKETCH_COLORBURN" },
									HasFeat("SKETCH") && !HasFeat("SKETCH_GRADIENT"), true, showHelp:false);
		//Sketch Anim
		GUISingleFeature(mCurrentConfig, "SKETCH_ANIM", "Animated Sketch", "Animates the sketch overlay texture, simulating a hand-drawn animation style",
						 HasFeatOr("SKETCH", "SKETCH_GRADIENT"), true);
		//Sketch Vertex
		GUISingleFeature(mCurrentConfig, "SKETCH_VERTEX", "Vertex Coords", "Compute screen coordinates in vertex shader (faster but can cause distortions)\nIf disabled will compute in pixel shader (slower)",
						 HasFeatOr("SKETCH", "SKETCH_GRADIENT"), true);
		//Sketch Scale
		GUISingleFeature(mCurrentConfig, "SKETCH_SCALE", "Scale with model", "If enabled, overlay texture scale will depend on model's distance from view",
						 HasFeatOr("SKETCH", "SKETCH_GRADIENT"), true);
		if(HasFeatOr("SKETCH", "SKETCH_GRADIENT"))
		{
			EditorGUILayout.HelpBox("Sketch feature requires at least Shader Model 4 to compile properly with Terrain template!", MessageType.Warning);
		}
		Space();
		//----------------------------------------------------------------
		//Outline
		GUIMultipleFeatures(mCurrentConfig, "Outline", "Outline around the model", new string[] { "Off|", "Opaque Outline|OUTLINE", "Blended Outline|OUTLINE_BLENDING" });
		GUISingleFeature(mCurrentConfig, "OUTLINE_BEHIND", "Outline behind model", "If enabled, outline will only show behind model",
						 HasFeatOr("OUTLINE", "OUTLINE_BLENDING"), true);
		Space();
		//----------------------------------------------------------------
		//Shader target
		EditorGUILayout.BeginHorizontal();
		TCP2_GUI.HelpButton("Shader Target");
		TCP2_GUI.SubHeader("Shader Target", "Defines the shader target level to compile for", mCurrentConfig.shaderTarget != 30, 170 - 24f);
		int newTarget = EditorGUILayout.IntPopup(mCurrentConfig.shaderTarget,
												 new string[] { "2.0", "2.5", "3.0", "3.5", "4.0", "5.0" },
												 new int[] { 20, 25, 30, 35, 40, 50 });
		if (newTarget != mCurrentConfig.shaderTarget)
		{
			mCurrentConfig.shaderTarget = newTarget;
		}
		EditorGUILayout.EndHorizontal();
		Space();

		//----------------------------------------------------------------
		// FLAGS
		GUILayout.Space(6f);
		TCP2_GUI.Header("FLAGS");
		GUILayout.Space(2f);
		GUISingleFlag(mCurrentConfig, "addshadow", "Add Shadow Passes", "Force the shader to have the Shadow Caster and Collector passes.\nCan help if shadows don't work properly with the shader");
		GUISingleFlag(mCurrentConfig, "fullforwardshadows", "Full Forward Shadows", "Enable support for all shadow types in Forward rendering path");
#if UNITY_5
		GUISingleFlag(mCurrentConfig, "noshadow", "Disable Shadows", "Disables all shadow receiving support in this shader");
		GUISingleFlag(mCurrentConfig, "nofog", "Disable Fog", "Disables Unity Fog support.\nCan help if you run out of vertex interpolators and don't need fog.");
#endif
		GUISingleFlag(mCurrentConfig, "nolightmap", "Disable Lightmaps", "Disables all lightmapping support in this shader.\nCan help if you run out of vertex interpolators and don't need lightmaps.");
		GUISingleFlag(mCurrentConfig, "noambient", "Disable Ambient Lighting", "Disable ambient lighting", !HasFeatOr("DIRAMBIENT", "CUBE_AMBIENT", "OCCLUSION"));
		GUISingleFlag(mCurrentConfig, "novertexlights", "Disable Vertex Lighting", "Disable vertex lights and spherical harmonics (light probes)");
		//GUISingleFeature("FORCE_SM2", "Force Shader Model 2", "Compile with Shader Model 2 target. Useful for (very) old GPU compatibility, but some features may not work with it.", showHelp:false);

		GUILayout.Space(6f);
		TCP2_GUI.Header("FLAGS (Mobile-friendly)", null, true);
		GUILayout.Space(2f);
		GUISingleFlag(mCurrentConfig, "noforwardadd", "One Directional Light", "Use additive lights as vertex lights.\nRecommended for Mobile");
#if UNITY_5
		GUISingleFlag(mCurrentConfig, "interpolateview", "Vertex View Dir", "Calculate view direction per-vertex instead of per-pixel.\nRecommended for Mobile");
#else
		GUISingleFlag(mCurrentConfig, "approxview", "Vertex View Dir", "Calculate view direction per-vertex instead of per-pixel.\nRecommended for Mobile");
#endif
		GUISingleFlag(mCurrentConfig, "halfasview", "Half as View", "Pass half-direction vector into the lighting function instead of view-direction.\nFaster but inaccurate.\nRecommended for Specular, but use Vertex Rim to optimize Rim Effects instead");
	}

	//--------------------------------------------------------------------------------------------------
	// Shader Generator GUI Elements

	private const float LABEL_WIDTH = 210f;

	static private void GUILabel(string label, string tooltip, bool highlight, bool showHelp, string helpTopic, bool indent, bool prefix)
	{
		if(showHelp && !indent)
			TCP2_GUI.HelpButton(label, string.IsNullOrEmpty(helpTopic) ? label : helpTopic);

		if (prefix)
			label = "  ▪ " + label;
		float space = indent ? 24f : 0f;
		float totalWidth = LABEL_WIDTH - space - ((showHelp && !indent) ? 24f : 0f);

		GUILayout.Space(space);
		TCP2_GUI.SubHeader(label, tooltip, highlight, totalWidth);
	}

	static private bool GUISingleFeature(TCP2_Config config, string featureName, string label, string tooltip = null,
	                              bool enabled = true, bool increaseIndentLevel = false, bool visible = true,
	                              string helpTopic = null, bool showHelp = true, bool helpIndent = true)
	{
		if(!enabled)
			GUI.enabled = false;

		bool feature = TCP2_ShaderGeneratorUtils.HasFeatures(config, featureName);

		if(sHideDisabled && increaseIndentLevel)
			visible = enabled;

		EditorGUI.BeginChangeCheck();
		if(visible)
		{
			EditorGUILayout.BeginHorizontal();
			GUILabel(label, tooltip, feature && enabled, showHelp, helpTopic, increaseIndentLevel || (!showHelp && helpIndent), increaseIndentLevel);
			feature = EditorGUILayout.Toggle(feature);
			EditorGUILayout.EndHorizontal();
		}

		if (EditorGUI.EndChangeCheck())
			TCP2_ShaderGeneratorUtils.ToggleSingleFeature(config.Features, featureName, feature);

		if(!enabled)
			GUI.enabled = sGUIEnabled;

		return feature;
	}

	static private bool GUIMultipleFeatures(TCP2_Config config, string label, string tooltip, string[] labelsAndFeatures, bool enabled = true, bool increaseIndentLevel = false, string helpTopic = null, bool showHelp = true, bool helpIndent = true)
	{
		return GUIMultipleFeaturesInternal(config, label, tooltip, labelsAndFeatures, enabled, increaseIndentLevel, helpTopic, showHelp, helpIndent);
	}
	static private bool GUIMultipleFeaturesInternal(TCP2_Config config, string label, string tooltip,  string[] labelsAndFeatures, bool enabled = true, bool increaseIndentLevel = false, string helpTopic = null, bool showHelp = true, bool helpIndent = true, bool visible = true)
	{
		if(!enabled)
			GUI.enabled = false;

		string[] labels = new string[labelsAndFeatures.Length];
		string[] features = new string[labelsAndFeatures.Length];

		int feature = 0;
		for(int i = 0; i < labelsAndFeatures.Length; i++)
		{
			string[] data = labelsAndFeatures[i].Split('|');
			labels[i] = data[0];
			features[i] = data[1];

			if(data.Length > 1 && !string.IsNullOrEmpty(features[i]))
			{
				if(TCP2_ShaderGeneratorUtils.HasFeatures(config, features[i]))
				{
					feature = i;
				}
			}
		}

		visible = (sHideDisabled && increaseIndentLevel) ? enabled : visible;

		if(visible)
		{
			EditorGUILayout.BeginHorizontal();
			GUILabel(label, tooltip, (feature > 0) && enabled, showHelp, helpTopic, increaseIndentLevel || (!showHelp && helpIndent), increaseIndentLevel);
			feature = EditorGUILayout.Popup(feature, labels);
			EditorGUILayout.EndHorizontal();
		}

		TCP2_ShaderGeneratorUtils.ToggleMultipleFeatures(config.Features, feature, features);

		if(!enabled)
			GUI.enabled = sGUIEnabled;

		return feature > 0;
	}
	
	static private bool GUIMask(TCP2_Config config, string label, string tooltip, string maskKeyword, string channelKeyword, string feature = null, bool enabled = true, bool increaseIndentLevel = false, bool visible = true, string helpTopic = null, bool helpIndent = true)
	{
		string[] labelsAndKeywords = new string[]{
			"Off|",
			"Main Texture|mainTex",
			"Mask 1|mask1","Mask 2|mask2","Mask 3|mask3",
			"Vertex Colors|IN.color"};	//small hack to make vertex colors mask working

		if(!enabled)
			GUI.enabled = false;
		
		string[] labels = new string[labelsAndKeywords.Length];
		string[] masks = new string[labelsAndKeywords.Length];
		string[] uvs = new string[]{"Main Tex UV","Independent UV"};

		for(int i = 0; i < labelsAndKeywords.Length; i++)
		{
			string[] data = labelsAndKeywords[i].Split('|');
			labels[i] = data[0];
			masks[i] = data[1];
		}

		int curMask = System.Array.IndexOf(masks, TCP2_ShaderGeneratorUtils.GetKeyword(config, maskKeyword));
		if(curMask < 0) curMask = 0;
		TCP2_Utils.TextureChannel curChannel = TCP2_Utils.FromShader( TCP2_ShaderGeneratorUtils.GetKeyword(config, channelKeyword) );
		if(curMask <= 1)
			curChannel = TCP2_Utils.TextureChannel.Alpha;
		string uvKey = (curMask > 1 && curMask < 5) ? "UV_" + masks[curMask] : null;
		int curUv = System.Array.IndexOf(uvs, TCP2_ShaderGeneratorUtils.GetKeyword(config, uvKey));
		if(curUv < 0) curUv = 0;

		if(sHideDisabled && increaseIndentLevel)
			visible = enabled;

		if(visible)
		{
			EditorGUILayout.BeginHorizontal();
			GUILabel(label, tooltip, (curMask > 0) && enabled, !string.IsNullOrEmpty(helpTopic), helpTopic, increaseIndentLevel || (string.IsNullOrEmpty(helpTopic) && helpIndent), increaseIndentLevel);
			curMask = EditorGUILayout.Popup(curMask, labels);
			GUI.enabled &= curMask > 1;
			curChannel = (TCP2_Utils.TextureChannel)EditorGUILayout.EnumPopup(curChannel);
			GUI.enabled &= curMask < 5;
			curUv = EditorGUILayout.Popup(curUv, uvs);
			GUI.enabled = sGUIEnabled;
			TCP2_GUI.HelpButton("Masks");
			EditorGUILayout.EndHorizontal();
		}

		TCP2_ShaderGeneratorUtils.SetKeyword(config.Keywords, maskKeyword, masks[curMask]);
		if(curMask > 0)
		{
			TCP2_ShaderGeneratorUtils.SetKeyword(config.Keywords, channelKeyword, curChannel.ToShader());
		}
		if(curMask > 1 && !string.IsNullOrEmpty(uvKey))
		{
			TCP2_ShaderGeneratorUtils.SetKeyword(config.Keywords, uvKey, uvs[curUv]);
		}
		TCP2_ShaderGeneratorUtils.ToggleSingleFeature(config.Features, "VCOLORS_MASK", (curMask == 5));
		TCP2_ShaderGeneratorUtils.ToggleSingleFeature(config.Features, feature, (curMask > 0));

		if(!enabled)
			GUI.enabled = sGUIEnabled;
		
		return curMask > 0;
	}

	static private void GUISingleFlag(TCP2_Config config, string flagName, string label, string tooltip = null, bool enabled = true, bool increaseIndentLevel = false, bool visible = true, string helpTopic = null)
	{
		if(!enabled)
			GUI.enabled = false;
		if(increaseIndentLevel)
			label = "▪ " + label;
		
		bool flag = TCP2_ShaderGeneratorUtils.HasFeatures(config.Flags, flagName);
		
		if(visible)
		{
			EditorGUILayout.BeginHorizontal();
			GUILabel(label, tooltip, flag && enabled, !string.IsNullOrEmpty(helpTopic), helpTopic, increaseIndentLevel || string.IsNullOrEmpty(helpTopic), increaseIndentLevel);
			flag = EditorGUILayout.Toggle(flag);
			EditorGUILayout.EndHorizontal();
		}
		
		TCP2_ShaderGeneratorUtils.ToggleFlag(config.Flags, flagName, flag);
		
		if(!enabled)
			GUI.enabled = sGUIEnabled;
	}

	//Instance
	private bool HasFeat( string feature ) { return HasFeat(mCurrentConfig, feature); }
	private bool HasFeatOr( params string[] features ) { return HasFeatOr(mCurrentConfig, features); }
	private bool HasFeatAnd( params string[] features ) { return HasFeatAnd(mCurrentConfig, features); }

	//Static
	static private bool HasFeat( TCP2_Config config, string feature ) { return config != null && TCP2_ShaderGeneratorUtils.HasFeatures(config, feature); }
	static private bool HasFeatOr( TCP2_Config config, params string[] features )
	{
		bool ret = false;
		if (config != null)
		{
			foreach (string f in features)
				ret |= TCP2_ShaderGeneratorUtils.HasFeatures(config, f);
		}
		return ret;
	}
	static private bool HasFeatAnd( TCP2_Config config, params string[] features )
	{
		bool ret = true;
		if (config != null)
		{
			foreach (string f in features)
				ret &= TCP2_ShaderGeneratorUtils.HasFeatures(config, f);
		}
		else
			ret = false;

		return ret;
	}

	//--------------------------------------------------------------------------------------------------
	// MISC

	private void LoadUserPrefs()
	{
		sAutoNames = EditorPrefs.GetBool("TCP2_mAutoNames", true);
		sOverwriteConfigs = EditorPrefs.GetBool("TCP2_mOverwriteConfigs", false);
		sHideDisabled = EditorPrefs.GetBool("TCP2_mHideDisabled", false);
		sSelectGeneratedShader = EditorPrefs.GetBool("TCP2_mSelectGeneratedShader", true);
		sLoadAllShaders = EditorPrefs.GetBool("TCP2_mLoadAllShaders", false);
		mConfigChoice = EditorPrefs.GetInt("TCP2_mConfigChoice", 0);
		TCP2_ShaderGeneratorUtils.CustomOutputDir = EditorPrefs.GetBool("TCP2_TCP2_ShaderGeneratorUtils.CustomOutputDir", false);
	}

	private void SaveUserPrefs()
	{
		EditorPrefs.SetBool("TCP2_mAutoNames", sAutoNames);
		EditorPrefs.SetBool("TCP2_mOverwriteConfigs", sOverwriteConfigs);
		EditorPrefs.SetBool("TCP2_mHideDisabled", sHideDisabled);
		EditorPrefs.SetBool("TCP2_mSelectGeneratedShader", sSelectGeneratedShader);
		EditorPrefs.SetBool("TCP2_mLoadAllShaders", sLoadAllShaders);
		EditorPrefs.SetInt("TCP2_mConfigChoice", mConfigChoice);
		EditorPrefs.SetBool("TCP2_TCP2_ShaderGeneratorUtils.CustomOutputDir", TCP2_ShaderGeneratorUtils.CustomOutputDir);
	}

	private void LoadCurrentConfig(TCP2_Config config)
	{
		mCurrentConfig = config;
		mDirtyConfig = false;
		if(sAutoNames)
		{
			AutoNames();
		}
		mCurrentHash = mCurrentConfig.ToHash();
		Template.TryLoadTextAsset(mCurrentConfig.configType, mCurrentConfig.templateFile);
	}

	private void NewShader()
	{
		mCurrentShader = null;
		mConfigChoice = 0;
		mIsModified = false;
		LoadCurrentConfig(new TCP2_Config());
	}

	private void CopyShader()
	{
		mCurrentShader = null;
		mConfigChoice = 0;
		mIsModified = false;
		TCP2_Config newConfig = new TCP2_Config();
		newConfig.Features = mCurrentConfig.Features;
		newConfig.Flags = mCurrentConfig.Flags;
		newConfig.Keywords = mCurrentConfig.Keywords;
		newConfig.ShaderName = mCurrentConfig.ShaderName + " Copy";
		newConfig.Filename = mCurrentConfig.Filename + " Copy";
		LoadCurrentConfig(newConfig);
	}

	private void LoadCurrentConfigFromShader(Shader shader)
	{
		ShaderImporter shaderImporter = ShaderImporter.GetAtPath(AssetDatabase.GetAssetPath(shader)) as ShaderImporter;
		string[] features;
		string[] flags;
		string[] customData;
		Dictionary<string,string> keywords;
		TCP2_ShaderGeneratorUtils.ParseUserData(shaderImporter, out features, out flags, out keywords, out customData);
		if(features != null && features.Length > 0 && features[0] == "USER")
		{
			mCurrentConfig = new TCP2_Config();
			mCurrentConfig.ShaderName = shader.name;
			mCurrentConfig.Filename = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(shader));
			mCurrentConfig.Features = new List<string>(features);
			mCurrentConfig.Flags = (flags != null) ? new List<string>(flags) : new List<string>();
			mCurrentConfig.Keywords = (keywords != null) ? new Dictionary<string,string>(keywords) : new Dictionary<string,string>();
			mCurrentShader = shader;
			mConfigChoice = mUserShadersLabels.IndexOf(shader.name);
			mDirtyConfig = false;
			AutoNames();
			mCurrentHash = mCurrentConfig.ToHash();

			mIsModified = false;
			if(customData != null && customData.Length > 0)
			{
				foreach(string data in customData)
				{
					//Hash
					if(data.Length > 0 && data[0] == 'h')
					{
						string dataHash = data;
						string fileHash = TCP2_ShaderGeneratorUtils.GetShaderContentHash(shaderImporter);

						if(!string.IsNullOrEmpty(fileHash) && dataHash != fileHash)
						{
							mIsModified = true;
						}
					}
					//Timestamp
					else
					{
						ulong timestamp;
						if(ulong.TryParse(data, out timestamp))
						{
							if(shaderImporter.assetTimeStamp != timestamp)
							{
								mIsModified = true;
							}
						}
					}

					//Shader Model target
					if (data.StartsWith("SM:"))
					{
						mCurrentConfig.shaderTarget = int.Parse(data.Substring(3));
					}

					//Configuration Type
					if (data.StartsWith("CT:"))
					{
						mCurrentConfig.configType = data.Substring(3);
					}

					//Configuration File
					if (data.StartsWith("CF:"))
					{
						mCurrentConfig.templateFile = data.Substring(3);
					}
				}
			}

			//Load appropriate template
			Template.TryLoadTextAsset(mCurrentConfig.configType, mCurrentConfig.templateFile);
		}
		else
		{
			EditorApplication.Beep();
			this.ShowNotification(new GUIContent("Invalid shader loaded: it doesn't seem to have been generated by the TCP2 Shader Generator!"));
			mCurrentShader = null;
			NewShader();
		}
	}

	private void ReloadUserShaders()
	{
		mUserShaders = GetUserShaders();
		mUserShadersLabels = new List<string>(GetShaderLabels(mUserShaders));

		if(mCurrentShader != null)
		{
			mConfigChoice = mUserShadersLabels.IndexOf(mCurrentShader.name);
		}
	}

	private Shader[] GetUserShaders()
	{
		string rootPath = Application.dataPath + (sLoadAllShaders ? "" : TCP2_ShaderGeneratorUtils.OutputPath);

		if(System.IO.Directory.Exists(rootPath))
		{
			string[] paths = System.IO.Directory.GetFiles(rootPath, "*.shader", System.IO.SearchOption.AllDirectories);
			List<Shader> shaderList = new List<Shader>();

			foreach(string path in paths)
			{
#if UNITY_EDITOR_WIN
				string assetPath = "Assets" + path.Replace(@"\", @"/").Replace(Application.dataPath, "");
#else
				string assetPath = "Assets" + path.Replace(Application.dataPath, "");
#endif
				Shader shader = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Shader)) as Shader;
				ShaderImporter shaderImporter = ShaderImporter.GetAtPath(assetPath) as ShaderImporter;
				if(shaderImporter != null && shader != null && !shaderList.Contains(shader))
				{
					if(shaderImporter.userData.Contains("USER"))
					{
						shaderList.Add(shader);
					}
				}
			}

			return shaderList.ToArray();
		}

		return null;
	}

	private string[] GetShaderLabels(Shader[] array, string firstOption = "New Shader")
	{
		if(array == null)
		{
			return new string[0];
		}

		List<string> labelsList = new List<string>();
		if(!string.IsNullOrEmpty(firstOption))
			labelsList.Add(firstOption);
		foreach(Shader shader in array)
		{
			labelsList.Add(shader.name);
		}
		return labelsList.ToArray();
	}

	private void AutoNames()
	{
		string rawName = mCurrentConfig.ShaderName.Replace("Toony Colors Pro 2/", "");
		mCurrentConfig.Filename = rawName;
	}

	static private void Space()
	{
		TCP2_GUI.GUILine(new Color(0.65f,0.65f,0.65f), 1);
		GUILayout.Space(1);
	}
}
