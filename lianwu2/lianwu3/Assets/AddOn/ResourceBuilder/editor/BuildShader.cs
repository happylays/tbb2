using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
/// <summary>
/// 该脚本为临时脚本，为了兼容旧的打包方式
/// </summary>
public class BuildShader
{
	public static List<string> BuildAllShader(string name)
	{
		List<string> allShdName = new List<string>();

		string[] strShader = Directory.GetFiles(AssetBundlePath.ShaderSrcDir, "*.shader", SearchOption.AllDirectories);
		List<Object> listShaderO = new List<Object>();
		foreach (string s in strShader)
		{
			string sTemp = s.Replace('\\', '/');
			string shdName = sTemp.Substring(sTemp.LastIndexOf('/') + 1);
			if (!allShdName.Contains(shdName))
			{
				allShdName.Add(shdName);
			}

			listShaderO.Add(AssetDatabase.LoadAssetAtPath(s, typeof(Shader)));
		}
		Object[] allShader = listShaderO.ToArray();
		string path = AssetBundlePath.ShaderAssetbundlePath + name + ".shd";

		BuildAssetBundle.Build(null, allShader, path, true);

		return allShdName;
	}
}
