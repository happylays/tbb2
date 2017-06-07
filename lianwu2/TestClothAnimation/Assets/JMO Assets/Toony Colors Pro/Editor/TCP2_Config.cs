// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Represents a Toony Colors Pro 2 configuration to generate the corresponding shader

public class TCP2_Config
{
	//--------------------------------------------------------------------------------------------------

	public string Filename = "TCP2 Custom";
	public string ShaderName = "Toony Colors Pro 2/User/My TCP2 Shader";
	public string configType = "Normal";
	public string templateFile = "TCP2_User_Unity5";
	public int shaderTarget = 30;
	public List<string> Features = new List<string>();
	public List<string> Flags = new List<string>();
	public Dictionary<string, string> Keywords = new Dictionary<string, string>();

	//--------------------------------------------------------------------------------------------------

	private enum ParseBlock
	{
		None,
		Features,
		Flags
	}

	static public TCP2_Config CreateFromFile(TextAsset asset)
	{
		return CreateFromFile(asset.text);
	}
	static public TCP2_Config CreateFromFile(string text)
	{
		string[] lines = text.Split(new string[]{"\n","\r\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		TCP2_Config config = new TCP2_Config();

		//Flags
		ParseBlock currentBlock = ParseBlock.None;
		for(int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			
			if(line.StartsWith("//")) continue;
			
			string[] data = line.Split(new string[]{"\t"}, System.StringSplitOptions.RemoveEmptyEntries);
			if(line.StartsWith("#"))
			{
				currentBlock = ParseBlock.None;
				
				switch(data[0])
				{
					case "#filename":	config.Filename = data[1]; break;
					case "#shadername":	config.ShaderName = data[1]; break;
					case "#features":	currentBlock = ParseBlock.Features; break;
					case "#flags":		currentBlock = ParseBlock.Flags; break;
					
					default: Debug.LogWarning("[TCP2 Shader Config] Unrecognized tag: " + data[0] + "\nline " + (i+1)); break;
				}
			}
			else
			{
				if(data.Length > 1)
				{
					bool enabled = false;
					bool.TryParse(data[1], out enabled);
					
					if(enabled)
					{
						if(currentBlock == ParseBlock.Features)
							config.Features.Add(data[0]);
						else if(currentBlock == ParseBlock.Flags)
							config.Flags.Add(data[0]);
						else
							Debug.LogWarning("[TCP2 Shader Config] Unrecognized line while parsing : " + line + "\nline " + (i+1));
					}
				}
			}
		}
		
		return config;
	}

	public TCP2_Config Copy()
	{
		TCP2_Config config = new TCP2_Config();

		config.Filename = this.Filename;
		config.ShaderName = this.ShaderName;

		foreach (string feature in this.Features)
			config.Features.Add(feature);

		foreach (string flag in this.Flags)
			config.Flags.Add(flag);

		foreach (KeyValuePair<string, string> kvp in this.Keywords)
			config.Keywords.Add(kvp.Key, kvp.Value);

		config.shaderTarget = this.shaderTarget;
		config.configType = this.configType;
		config.templateFile = this.templateFile;

		return config;
	}

	public string GetShaderTargetCustomData()
	{
		return string.Format("SM:{0}", this.shaderTarget);
	}

	public string GetConfigTypeCustomData()
	{
		if (configType != "Normal")
		{
			return string.Format("CT:{0}", this.configType);
		}

		return null;
	}

	public string GetConfigFileCustomData()
	{
		return string.Format("CF:{0}", this.templateFile);
	}

	public int ToHash()
	{

		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append(this.Filename);
		sb.Append(this.ShaderName);
		List<string> orderedFeatures = new List<string>(this.Features);
		orderedFeatures.Sort();
		List<string> orderedFlags = new List<string>(this.Flags);
		orderedFlags.Sort();
		List<string> sortedKeywordsKeys = new List<string>(this.Keywords.Keys);
		sortedKeywordsKeys.Sort();
		List<string> sortedKeywordsValues = new List<string>(this.Keywords.Values);
		sortedKeywordsValues.Sort();

		foreach(string f in orderedFeatures)
			sb.Append(f);
		foreach(string f in orderedFlags)
			sb.Append(f);
		foreach(string f in sortedKeywordsKeys)
			sb.Append(f);
		foreach(string f in sortedKeywordsValues)
			sb.Append(f);

		sb.Append(shaderTarget.ToString());

		return sb.ToString().GetHashCode();
	}
}
