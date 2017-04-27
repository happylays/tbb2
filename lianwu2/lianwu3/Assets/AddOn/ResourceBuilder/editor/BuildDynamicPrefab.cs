using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using LoveDance.Client.Common;
using System.IO;

/// <summary>
/// 打包非UI类动态prefab
/// NOTES：不与UI共用脚本、贴图
/// </summary>
public class BuildDynamicPrefab 
{
    [MenuItem("Resource Generator/GenerateResource/DynamicPrefab")]
    public static void  GenerateDynamicPrefab()
    {
        string bundleParentPath = AssetBundlePath.PrefabAssetRootPath + "/";

        if (!Directory.Exists(bundleParentPath))
        {
            Directory.CreateDirectory(bundleParentPath);
        }

        BuildPipeline.PushAssetDependencies();
        BuildShader.BuildAllShader("all");

        List<string> prefabSrcDirList = new List<string>();

        prefabSrcDirList.Add(AssetBundlePath.DynamicPrefabBasicAssetDir);
#if !PACKAGE_BASIC
		prefabSrcDirList.Add(AssetBundlePath.DynamicPrefabAssetDir);
#endif

        ProcDynamicPrefab_All(prefabSrcDirList);

        BuildPipeline.PopAssetDependencies();
    }

    private static void ProcDynamicPrefab_All(List<string> prefabSrcDirList)
    {
        List<string> prefabList = new List<string>();

        foreach (string prefabDir in prefabSrcDirList)
		{
			if (Directory.Exists(prefabDir))
			{
				BuildAssetList(prefabDir, prefabList, AssetBundleType.Pre);
			}
			else
			{
				Debug.Log("path is not exist: " + prefabDir);
			}
		}

        ProcDynamicPrefab(prefabList);
    }

    private static void ProcDynamicPrefab(List<string> prefabList)
    {
        string bundleParentPath = AssetBundlePath.PrefabAssetRootPath  + "/";

        for (int i = 0; i < prefabList.Count; ++i)
        {
            BuildPipeline.PushAssetDependencies();
            Object o = AssetDatabase.LoadAssetAtPath(prefabList[i],typeof(Object));

			if (!BuildAssetBundle.IsLegalAsset(o.name))
			{
				Debug.LogError("Build dynamic prefab error, asset name is not all lower," + prefabList[i]);
				EditorUtility.DisplayDialog("Error", "Build dynamic prefab error, asset name is not all lower,Please try again!" + prefabList[i], "OK");
				return;
			}

            string bundlePath = bundleParentPath + o.name + ".pre";

			BuildAssetBundle.Build(o, null, bundlePath, true);

            BuildPipeline.PopAssetDependencies();
        }

    }

    /// <summary>
	/// 获取指定资源
	/// </summary>
	static void BuildAssetList(string dirPath, List<string> fileList, AssetBundleType assetType)
	{
		string[] fileArr = Directory.GetFiles(dirPath);
		foreach (string filePath in fileArr)
		{
			if (BuildAssetBundle.GetAssetType(filePath) == assetType)
			{
				fileList.Add(filePath);
			}
		}

		string[] dirArr = Directory.GetDirectories(dirPath);
		foreach (string dir in dirArr)
		{
			if (!dir.Contains("/."))
			{
				BuildAssetList(dir, fileList, assetType);
			}
		}
	}
}
