using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using LoveDance.Client.Common;
using System.Collections.Generic;

public class GenerateAndroidAsset
{
	public static string AssetSrcDir =  "Assets/Plugins/Android/assets/res_unity/";

	[MenuItem("Resource Generator/GenerateResource/GenerateAndroidAsset")]
	public static void _GenerateAndroidAsset()
	{
		string resPath = "Assets/Plugins/Android/assets/res_unity/";
		string filePath = resPath + "androidasset.bytes";

		if(!Directory.Exists(resPath))
		{
			Directory.CreateDirectory(resPath);
		}

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}

		List<string> listAsset = new List<string>();
		if (Directory.Exists(AssetSrcDir))
		{
			BuildAssetList(AssetSrcDir, listAsset);
		}

		XQFileStream fs = new XQFileStream();
		fs.OpenOverWrite(filePath);

		fs.WriteInt(listAsset.Count);
		foreach (var v in listAsset)
		{
			FileInfo fi = new FileInfo(v);
			fs.WriteString2(fi.Name);
		}

		fs.Close();
	}

	static void BuildAssetList(string dirPath, List<string> fileList)
	{
		string[] fileArr = Directory.GetFiles(dirPath);
		foreach (string filePath in fileArr)
		{
			if (IsValidFile(filePath))
			{
				fileList.Add(filePath);
			}
		}

		string[] dirArr = Directory.GetDirectories(dirPath);
		foreach (string dir in dirArr)
		{
			if (!dir.Contains("/."))
			{
				BuildAssetList(dir, fileList);
			}
		}
	}

	/// <summary>
	/// 是否是合法的文件
	/// </summary>
	/// <param name="assetName"></param>
	/// <returns></returns>
	static bool IsValidFile(string assetName)
	{
		if(assetName.Contains(".meta"))
		{
			return false;
		}

		return true;
	}
}
