using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildShaderExtend
{
	[MenuItem("Resource Generator/GenerateResource/All ShaderExtend")]
	static void GenerateAllShaderExtend()
	{
		if (!Directory.Exists(AssetBundlePath.ShaderExtendAssetbundlePath))
		{
			Directory.CreateDirectory(AssetBundlePath.ShaderExtendAssetbundlePath);
		}

		string[] strShader = Directory.GetFiles(AssetBundlePath.ShaderExtentdSrcDir, "*.shader", SearchOption.AllDirectories);

		foreach (string s in strShader)
		{
			GenerateShaderExtend(s,true);
		}
	}

	public static void GenerateShaderExtend(string shaderPath,bool isLeaf)
	{
		if (!Directory.Exists(AssetBundlePath.ShaderExtendAssetbundlePath))
		{
			Directory.CreateDirectory(AssetBundlePath.ShaderExtendAssetbundlePath);
		}

		List<Object> listShaderO = new List<Object>();
		listShaderO.Add(AssetDatabase.LoadAssetAtPath(shaderPath, typeof(Shader)));

		string sTemp = shaderPath.Replace('\\', '/');
		string shdName = sTemp.Substring(sTemp.LastIndexOf('/') + 1);
		shdName = shdName.Replace(".shader", ".shd");

		if (!BuildAssetBundle.IsLegalAsset(shdName))
		{
			Debug.LogError("Build shader extend error, asset name is not all lower," + shaderPath);
			EditorUtility.DisplayDialog("Error", "Build shader extend error, asset name is not all lower, Please try again!" + shaderPath, "OK");
			return;
		}
        
		Object[] allShader = listShaderO.ToArray();
		string path = AssetBundlePath.ShaderExtendAssetbundlePath + shdName;

		BuildAssetBundle.Build(null, allShader, path, isLeaf);
	}
}