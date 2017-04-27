using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using LoveDance.Client.Common;
/// <summary>
/// 新的打包方式
/// 该文件必须放在Editor目录下
/// </summary>
public class BuildAssetBundle
{
	public static void Build(Object mainAsset, Object[] assets, string bundlePath, bool isLeaf = false)
	{
		BuildAssetBundleOptions buildOption =
			BuildAssetBundleOptions.CollectDependencies
			| BuildAssetBundleOptions.UncompressedAssetBundle
			| BuildAssetBundleOptions.DeterministicAssetBundle;

		if (isLeaf)
		{
			buildOption |= BuildAssetBundleOptions.CompleteAssets;
		}

		BuildPipeline.BuildAssetBundle(mainAsset, assets, bundlePath,buildOption,EditorUserBuildSettings.activeBuildTarget);
	}

	/// <summary>
	///  判断是否是符合标准的资源
	/// </summary>
	/// <param name="assetName">资源名</param>
	public static bool IsLegalAsset(string assetName)
	{
		if (string.IsNullOrEmpty(assetName))
		{
			return false;
		}

		return assetName.Equals(assetName.ToLower());
	}

	/// <summary>
	/// 搜索目录，获取指定文件
	/// </summary>
	public static void GetFiles(string dirPath, string fileter, ref List<string> fileList)
	{
		if (Directory.Exists(dirPath))
		{
			string[] fileArr = Directory.GetFiles(dirPath, fileter);
			fileList.AddRange(fileArr);

			string[] dirArr = Directory.GetDirectories(dirPath);
			foreach (string dir in dirArr)
			{
				if (!dir.Contains("/."))
				{
					GetFiles(dir, fileter, ref fileList);
				}
			}
		}
		else
		{
			Debug.Log("path is not exist: " + dirPath);
		}
	}

	public static string GetAssetName(string path)
	{
		string sTemp = path.Replace('\\', '/');
		int startIndex = sTemp.LastIndexOf('/') + 1;
		int endIndex = sTemp.LastIndexOf('.') - 1;
		string assetName = sTemp.Substring(startIndex, endIndex - startIndex + 1);

		return assetName;
	}

	/// <summary>
	/// 获取文件类型
	/// </summary>
	public static AssetBundleType GetAssetType(string assetPath)
	{
		AssetBundleType arType = AssetBundleType.Max;

		if (IsImageFile(assetPath))
		{
			arType = AssetBundleType.Texture;
		}
		else if (IsSoundFile(assetPath))
		{
			arType = AssetBundleType.Sound;
		}
		else if (IsPrefabFile(assetPath))
		{
			arType = AssetBundleType.Pre;
		}
		else if (IsFBXFile(assetPath))
		{
			arType = AssetBundleType.Model;
		}
		else if (IsScriptFile(assetPath))
		{
			arType = AssetBundleType.Script;
		}
		else if (IsScriptDLLFile(assetPath))
		{
			arType = AssetBundleType.ScriptDLL;
		}
		else if (IsMatFile(assetPath))
		{
			arType = AssetBundleType.Material;
		}
		else if (IsAnimationFile(assetPath))
		{
			arType = AssetBundleType.Animation;
		}
		else if (IsShaderFile(assetPath))
		{
			arType = AssetBundleType.Shd;
		}
		else if (IsSceneFile(assetPath))
		{
			arType = AssetBundleType.Scene;
		}
		else if (IsMetaFile(assetPath))
		{
			//Ignore this type file
		}
		else if (IsAssetFile(assetPath))
		{
			arType = AssetBundleType.Asset;
		}
		else
		{
			Debug.LogError("UnKnown asset Type, assetPath=" + assetPath);
		}

		return arType;
	}

	/// <summary>
	/// 获取依赖资源的入度数
	/// </summary>
	public static Dictionary<string, List<DependencyAsset>> GetAssetDependencieRefs(string[] assetPaths)
	{
		Dictionary<string, List<DependencyAsset>> resMap = new Dictionary<string, List<DependencyAsset>>();

		//获取所有资源依赖关系
		Dictionary<string, string[]> allDepMap = new Dictionary<string, string[]>();
		foreach (string assetPath in assetPaths)
		{
			string[] depArr = AssetDatabase.GetDependencies(new string[] { assetPath });
			if (!allDepMap.ContainsKey(assetPath))
			{
				allDepMap.Add(assetPath, depArr);
			}

			foreach (string assetDepPath in depArr)
			{
				if (!allDepMap.ContainsKey(assetDepPath))
				{
					allDepMap.Add(assetDepPath, AssetDatabase.GetDependencies(new string[] { assetDepPath }));
				}
			}
		}

		//生成依赖关系图
		foreach (string path in assetPaths)
		{
			Dictionary<string, DependencyAsset> depMap = new Dictionary<string, DependencyAsset>();
			GetAssetDependencies_Penetration(path, path, 0, allDepMap, ref depMap);

			//排序
			List<DependencyAsset> list = new List<DependencyAsset>(depMap.Values);
			for (int i = 0; i < list.Count; ++i)
			{
				for (int j = i; j < list.Count; ++j)
				{
					DependencyAsset daTemp = null;
					if (list[i].Depth < list[j].Depth)
					{
						daTemp = list[i];
						list[i] = list[j];
						list[j] = daTemp;
					}
				}
			}

			if (!resMap.ContainsKey(path))
			{
				resMap.Add(path, list);
			}
		}

		return resMap;
	}

	static void GetAssetDependencies_Penetration(string assetPath, string parentNodePath, int depth, Dictionary<string, string[]> allDepMap,
		ref Dictionary<string, DependencyAsset> depMap)
	{
		string[] depArr = null;	//当前资源所有依赖关系
		List<string> childrenList = new List<string>();	//所有间接依赖关系
		List<string> directChildrenList = new List<string>();	//直接依赖关系

		//获取当前资源依赖关系
		if (allDepMap.ContainsKey(assetPath))
		{
			depArr = allDepMap[assetPath];

			if (depArr.Length > 1)
			{
				AddAssetDependeny(assetPath, parentNodePath, depth, false, ref depMap);
			}
		}
		else
		{
			Debug.LogError("Has not contain asset dependencies," + assetPath);
		}

		++depth;

		//获取当前资源间接依赖关系
		foreach (string depPath in depArr)
		{
			if (!depPath.Equals(assetPath))
			{
				if (allDepMap.ContainsKey(depPath))
				{
					string[] depArrChild = allDepMap[depPath];
					foreach (string depChild in depArrChild)
					{
						if (!depChild.Equals(depPath) && !childrenList.Contains(depChild))
						{
							childrenList.Add(depChild);
						}
					}
				}
				else
				{
					Debug.LogError("Has not contain asset dependencies," + assetPath);
				}
			}
		}

		//获取当前资源直接依赖关系
		foreach (string directDepChild in depArr)
		{
			if (!childrenList.Contains(directDepChild) && !directDepChild.Equals(assetPath))
			{
				directChildrenList.Add(directDepChild);
			}
		}

		//处理
		foreach (string directDepChild in directChildrenList)
		{
			bool isLeaf = false;
			if (allDepMap.ContainsKey(directDepChild))
			{
				isLeaf = (allDepMap[directDepChild].Length > 1) ? false : true;
			}
			else
			{
				Debug.LogError("Has not contain asset dependencies," + directDepChild);
			}

			AddAssetDependeny(directDepChild, assetPath, depth, isLeaf, ref depMap);

			GetAssetDependencies_Penetration(directDepChild, assetPath, depth, allDepMap, ref depMap);
		}
	}

	static void AddAssetDependeny(string assetPath, string parentNodePath, int depth, bool isLeaf, ref Dictionary<string, DependencyAsset> depMap)
	{
		string assetPathTemp = assetPath.Replace('\\', '/');
		string sectionName = assetPathTemp.Substring(assetPathTemp.LastIndexOf('/') + 1);

		DependencyAsset dasset = null;
		if (depMap.ContainsKey(sectionName))
		{
			dasset = depMap[sectionName];
		}
		else
		{
			dasset = new DependencyAsset();
			dasset.AssetName = sectionName.Substring(0, sectionName.LastIndexOf('.'));
			dasset.AssetSuffix = sectionName.Substring(sectionName.LastIndexOf('.') + 1);
			dasset.AssetPath = assetPath;
			dasset.AssetType = GetAssetType(assetPath);
			dasset.IsLeaf = isLeaf;
			dasset.Depth = depth;

			depMap.Add(sectionName, dasset);
		}

		dasset.ParentNodeSet.Add(parentNodePath);
	}

	static bool IsMetaFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".meta"))
		{
			return true;
		}

		return false;
	}

	static bool IsSceneFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".unity"))
		{
			return true;
		}

		return false;
	}

	static bool IsShaderFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".shader"))
		{
			return true;
		}

		return false;
	}

	static bool IsPrefabFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".prefab"))
		{
			return true;
		}

		return false;
	}

	static bool IsImageFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".png") || filePath.EndsWith(".bmp") || filePath.EndsWith(".tga")
			|| filePath.EndsWith(".psd") || filePath.EndsWith(".dds") || filePath.EndsWith(".jpg"))
		{
			return true;
		}

		return false;
	}

	static bool IsSoundFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".ogg") || filePath.EndsWith(".wav") || filePath.EndsWith(".mp3"))
		{
			return true;
		}

		return false;
	}

	static bool IsScriptFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".js") || filePath.EndsWith(".cs") || filePath.EndsWith(".boo"))
		{
			return true;
		}

		return false;
	}

	static bool IsScriptDLLFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".dll"))
		{
			return true;
		}

		return false;
	}

	static bool IsMatFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".mat"))
		{
			return true;
		}

		return false;
	}

	static bool IsFBXFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".fbx"))
		{
			return true;
		}

		return false;
	}

	static bool IsAnimationFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".anim"))
		{
			return true;
		}

		return false;
	}

	static bool IsAssetFile(string filePath)
	{
		filePath = filePath.ToLower();
		if (filePath.EndsWith(".asset"))
		{
			return true;
		}

		return false;
	}

	public static List<string> GetAllUIPrefab()
	{
		List<string> uiWndSrcDirList = new List<string>();
		uiWndSrcDirList.Add(AssetBundlePath.UIPrefabBasicAssetDir);
		uiWndSrcDirList.Add(AssetBundlePath.UIPrefabNewAssetDir);

		List<string> uisceneList = new List<string>();

		foreach (string uisceneDir in uiWndSrcDirList)
		{
			if (Directory.Exists(uisceneDir))
			{
				BuildUISceneList(uisceneDir, uisceneList);
			}
			else
			{
				Debug.Log("path is not exist: " + uisceneDir);
			}
		}

		return uisceneList;
	}

	private static void BuildUISceneList(string dirPath, List<string> fileList)
	{
		string[] fileArr = Directory.GetFiles(dirPath, "*.prefab");
		foreach (string filePath in fileArr)
		{
			fileList.Add(filePath);
		}

		string[] dirArr = Directory.GetDirectories(dirPath);
		foreach (string dir in dirArr)
		{
			if (!dir.Contains("/."))
			{
				BuildUISceneList(dir, fileList);
			}
		}
	}
}
