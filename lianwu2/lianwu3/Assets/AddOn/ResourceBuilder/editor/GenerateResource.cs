using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class GenerateResource
{
	static string ShaderSrcDir = "Assets/shaders/";

	public static string ResAssetbundleDir = "Res/";

	public static string ShaderAssetbundlePath
	{
		get { return ResAssetbundleDir; }
	}

	static bool IsShader(string filePath)
	{
		string extension = filePath.Substring( filePath.Length - 7 ).ToLower();
		if( extension == ".shader" )
		{
			return true;
		}

		return false;
	}
	
	static bool IsPrefabFile(string filePath)
	{
		string extension = filePath.Substring( filePath.Length - 7 ).ToLower();
		if( extension == ".prefab" )
		{
			return true;
		}

		return false;
	}


	public static Object ReplacePrefab(GameObject oldPrefab, string prefabName)
	{
		Object newPrefab = PrefabUtility.CreateEmptyPrefab( "Assets/" + prefabName + ".prefab" );
		newPrefab = PrefabUtility.ReplacePrefab( oldPrefab, newPrefab );
		
		Object.DestroyImmediate( oldPrefab );

		return newPrefab;
	}
	public static void BuildAllShader(string name)
	{
		string[] strShader = Directory.GetFiles( ShaderSrcDir,"*.shader",SearchOption.AllDirectories);
		List<Object> listShaderO = new List<Object>();
		foreach( string s in strShader )
		{
			listShaderO.Add( AssetDatabase.LoadAssetAtPath(s,typeof(Shader)));
		}
		Object[] allShader = listShaderO.ToArray();
		string path = ShaderAssetbundlePath + name + ".shd";
		BuildPipeline.PushAssetDependencies();

		BuildAssetBundle.Build(null, allShader, path, true);
	}
	
	public static void PopAllShader()
	{
		BuildPipeline.PopAssetDependencies();
	}	
}

/// <summary>
/// Animation file
/// </summary>
public class StateInfo
{
	public string Id { get; set; }//unique ID in .controller file 
	public string Name { get; set; }//unique Tag name in animator Panel
	public string Speed { get; set; }//play speed
	public string Motion { get; set; }//asset name
}

/// <summary>
/// Relationship
/// </summary>
public class TransitionInfo
{
	public string Id { get; set; }//unique ID in .controller file 
	public string SrcStateId { get; set; }//src State
	public string DestStateId { get; set; }//dest State
	public string TransitionDuration { get; set; }//CrossFade over time (percentage)
	public string TransitionOffset { get; set; }//next animation play time(percentage)
	public List<Dictionary<string, string>> Conditions { get; set; }//trigger condition

}
