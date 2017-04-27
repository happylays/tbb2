using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Loader;
/// <summary>
/// 新的打包方式
/// 打包UI，需要手动将DLL文件绑定到一个Prefab上并存入AssetBundlePath.DLLPrefabAssetDir目录下。
/// </summary>
public class BuildUI
{
	static List<string> IgnoreAssetList = new List<string>() { "t_nomall.png", "pr_ui-font.prefab", "m_ui_font.mat", };

	[MenuItem("Resource Generator/GenerateResource/UI")]
	static void GenerateAllUIScene()
	{
		List<string> uiWndSrcDirList = new List<string>();
		List<string> uiTxtSrcDirList = new List<string>();
		List<string> uiPrefabSrcDirList = new List<string>();

		uiWndSrcDirList.Add(AssetBundlePath.UIPrefabBasicAssetDir);
		uiWndSrcDirList.Add(AssetBundlePath.UIPrefabNewAssetDir);

		uiTxtSrcDirList.Add(AssetBundlePath.SpecialTexBasicAssetDir);
		uiPrefabSrcDirList.Add(AssetBundlePath.PrefabBasicAssetDir);
#if !PACKAGE_BASIC
		uiTxtSrcDirList.Add(AssetBundlePath.SpecialTexAssetDir);
		uiPrefabSrcDirList.Add(AssetBundlePath.PrefabAssetDir);
#endif

		ProcUIScene_All(uiTxtSrcDirList, uiWndSrcDirList, uiPrefabSrcDirList);
	}

	static void ProcUIScene_All(List<string> specialDirList, List<string> uisceneDirList, List<string> uiPrefabSrcDirList)
	{
		List<string> specialList = new List<string>();
		List<string> uiScenePrefabList = new List<string>();
		List<string> prefabList = new List<string>();

		foreach (string specialDir in specialDirList)
		{
			if (Directory.Exists(specialDir))
			{
				BuildAssetList(specialDir, specialList, AssetBundleType.Texture);
			}
			else
			{
				Debug.Log("path is not exist: " + specialDir);
			}
		}

		foreach (string uisceneDir in uisceneDirList)
		{
			if (Directory.Exists(uisceneDir))
			{
				BuildUISceneList(uisceneDir, uiScenePrefabList);
			}
			else
			{
				Debug.Log("path is not exist: " + uisceneDir);
			}
		}

		foreach (string uiPrefabDir in uiPrefabSrcDirList)
		{
			if (Directory.Exists(uiPrefabDir))
			{
				BuildAssetList(uiPrefabDir, prefabList, AssetBundleType.Pre);
			}
			else
			{
				Debug.Log("path is not exist: " + uiPrefabDir);
			}
		}

		ProcUIScene(specialList, uiScenePrefabList, prefabList);
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

	static void BuildUISceneList(string dirPath, List<string> fileList)
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

	private static Dictionary<string,string> GetUIPrefabList()
	{
		List<string> prefabList = new List<string>();
		BuildAssetList(AssetBundlePath.UIPrefabAssetDir, prefabList, AssetBundleType.Pre);

		Dictionary<string, string> dic = new Dictionary<string, string>();
	
		foreach (var v in prefabList)
		{
			string fileName = BuildAssetBundle.GetAssetName(v);
			dic[fileName] = v;
		}

		return dic;
	}

	private static List<string> FixedUIPrefabList(List<string> sceneList)
	{
		List<string> list = new List<string>();

		Dictionary<string, string> dic = GetUIPrefabList();
		foreach (var v in sceneList)
		{
			string fileName = BuildAssetBundle.GetAssetName(v);
			if (dic.ContainsKey(fileName))
			{
				list.Add(dic[fileName]);
			}
			else
			{
				Debug.LogError("UIPrefab File is no Exist,filename is : " + v);
			}
		}

		return list;
	}

	public static void ProcUIScene(List<string> specialList, List<string> uiScenePrefabList, List<string> prefabList)
	{
		AssetBundleConfig uiConfig = new AssetBundleConfig();		//UI配置文件
		AssetBundleConfig prefabConfig = new AssetBundleConfig();	//动态Prefab配置文件

		Dictionary<string, AssetBundleType> allAssetBundleMap = new Dictionary<string, AssetBundleType>();	//记录所有打包的Asset
		List<string> comAssetList = new List<string>();		//记录所有公共资源
		List<string> ignoreBundleList = new List<string>();	//需要删除的资源
#if PACKAGE_BASIC
		//添加小包过滤操作
		Dictionary<string, string> extendAssetMap = new Dictionary<string, string>();
		List<string> basicAssetList = new List<string>();
#endif

		//开始打包资源
		BuildPipeline.PushAssetDependencies();

		//打包部分指定Shader
		List<string> shdList = BuildShader.BuildAllShader("all");

		//打包Special Texture
		foreach (string texturePath in specialList)
		{
			string sTemp = texturePath.Replace('\\', '/');
			int startIndex = sTemp.LastIndexOf('/') + 1;
			int endIndex = sTemp.LastIndexOf('.') - 1;
			string textureName = sTemp.Substring(startIndex, endIndex - startIndex + 1);

			if (!allAssetBundleMap.ContainsKey(textureName))
			{
                string parentPath = GetParentPath(texturePath);
				if (!Directory.Exists(parentPath))
				{
					Directory.CreateDirectory(parentPath);
				}

				if (!BuildAssetBundle.IsLegalAsset(textureName))
				{
					Debug.LogError("Build ui error, asset name is not all lower," + texturePath);
					EditorUtility.DisplayDialog("Error", "Build ui error, asset name is not all lower,Please try again!" + texturePath, "OK");
					return;
				}

				Texture2D tex2D = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
				string path = parentPath + textureName + "." + AssetBundleType.Texture.ToString().ToLower();

				BuildAssetBundle.Build(tex2D, null, path, true);

				allAssetBundleMap.Add(textureName, AssetBundleType.Texture);
			}
		}

		//获取依赖关系
		Dictionary<string, List<DependencyAsset>> prefabAssetRefMap = BuildAssetBundle.GetAssetDependencieRefs(prefabList.ToArray());//获取动态Prefab依赖关系
		Dictionary<string, List<DependencyAsset>> uiAssetRefMap = BuildAssetBundle.GetAssetDependencieRefs(uiScenePrefabList.ToArray());//获取UI依赖关系

		Dictionary<string, List<DependencyAsset>> allAssetRefMap = new Dictionary<string, List<DependencyAsset>>(uiAssetRefMap);
		if (prefabAssetRefMap != null && prefabAssetRefMap.Count > 0)
		{
			foreach (string prefabPath in prefabAssetRefMap.Keys)
			{
				if (!allAssetRefMap.ContainsKey(prefabPath))
				{
					allAssetRefMap.Add(prefabPath, prefabAssetRefMap[prefabPath]);
				}
			}
		}

		//排序
		List<string> allAssetRefKeyList = null;
		if (allAssetRefMap != null && allAssetRefMap.Count > 0)
		{
			allAssetRefKeyList = new List<string>(allAssetRefMap.Keys);
			allAssetRefKeyList.Sort(new UICompare());
		}

		//获取所有脚本
		if (allAssetRefKeyList != null && allAssetRefKeyList.Count > 0)
		{
			foreach (string uiscenePath in allAssetRefKeyList)
			{
				List<DependencyAsset> val = allAssetRefMap[uiscenePath];
				foreach (DependencyAsset dasset in val)
				{
					string sceneDepPath = dasset.AssetPath;
					AssetBundleType assetType = dasset.AssetType;

					if (assetType == AssetBundleType.Script)
					{
						if (!comAssetList.Contains(sceneDepPath))
						{
							comAssetList.Add(sceneDepPath);
						}
					}
					else if (assetType == AssetBundleType.ScriptDLL)
					{
						string dllPath = AssetBundlePath.DLLPrefabAssetDir + dasset.AssetName + ".prefab";
						if (File.Exists(dllPath))
						{
							if (!comAssetList.Contains(dllPath))
							{
								comAssetList.Add(dllPath);
							}
						}
						else
						{
							Debug.LogError("Build UI failed. Can not find dll file prefab, path=" + dllPath);
						}
					}
				}
			}
		}

		//打包公共资源
		if (comAssetList != null && comAssetList.Count > 0)
		{
			//创建界面目录
			string comParentPath = AssetBundlePath.UIAssetRootPath + AssetBundleType.Scene + "/";	//场景目录
			if (!Directory.Exists(comParentPath))
			{
				Directory.CreateDirectory(comParentPath);
			}
			
			List<Object> comObjectList = new List<Object>();
			for(int i = 0;i < comAssetList.Count; ++i)
			{
				comObjectList.Add(AssetDatabase.LoadAssetAtPath(comAssetList[i], typeof(Object)));
			}

			BuildAssetBundle.Build(null, comObjectList.ToArray(), comParentPath + "ui_common" + "." + AssetBundleType.Scene.ToString().ToLower(), true);
			
			comObjectList.Clear();
		}

		//打包其他资源
		if (allAssetRefKeyList != null && allAssetRefKeyList.Count > 0)
		{
			foreach (string rootAssetPath in allAssetRefKeyList)
			{
				AssetBundleConfig bundleConfig = null;
				string rootAssetName = BuildAssetBundle.GetAssetName(rootAssetPath);
				AssetBundleType rootAssetType = BuildAssetBundle.GetAssetType(rootAssetPath);

				//确定配置文件
				if (uiScenePrefabList.Contains(rootAssetPath))
				{
					bundleConfig = uiConfig;
				}
				else if (prefabList.Contains(rootAssetPath))
				{
					bundleConfig = prefabConfig;
				}
				else
				{
					Debug.LogError("Build ui error, can not find current assetType," + rootAssetType + "," + rootAssetPath);
				}

				List<DependencyAsset> depList = allAssetRefMap[rootAssetPath];
				for (int i = 0; i < depList.Count; ++i)
				{
					DependencyAsset dasset = depList[i];
					AssetBundleType arType = dasset.AssetType;
					string assetName = dasset.AssetName;
					string assetFullName = dasset.AssetName + "." + dasset.AssetSuffix;
					string assetPath = dasset.AssetPath;
					string bundleParentPath = AssetBundlePath.UIAssetRootPath + arType + "/";
					string bundlePath = bundleParentPath + assetName + "." + arType.ToString().ToLower();
					bool popDependence = false;

					if (arType == AssetBundleType.Max)
					{
						Debug.LogError("BuildUI error, unSupport assetBundle," + rootAssetPath);
					}

					if (IgnoreAssetList.Contains(assetFullName))
					{
						if (!ignoreBundleList.Contains(bundlePath))
						{
							ignoreBundleList.Add(bundlePath);
						}
					}
					else
					{
						if (rootAssetName.Equals(assetName))
						{
							popDependence = true;
						}
						else if (dasset.IsLeaf || assetPath.Replace('\\', '/').Contains(AssetBundlePath.ArtUIDir))
						{
							if (arType == AssetBundleType.Script || arType == AssetBundleType.ScriptDLL)
							{
								continue;
							}
							else if (arType == AssetBundleType.Shd && shdList.Contains(assetFullName))
							{
								//忽略已经打包的Shader，此操作为了兼容旧版本
								continue;
							}
						}
						else
						{
							continue;
						}

						bundleConfig.AddConfig(rootAssetName, arType, assetName, i);
					}

					if (!allAssetBundleMap.ContainsKey(assetName))
					{
						if (!BuildAssetBundle.IsLegalAsset(assetName))
						{
							Debug.LogError("Build ui error, asset name is not all lower," + assetPath);
							EditorUtility.DisplayDialog("Error", "Build ui error, asset name is not all lower,Please try again!" + assetPath, "OK");
							return;
						}

						BuildBundle(assetPath, bundleParentPath, bundlePath, dasset.IsLeaf, popDependence);

						allAssetBundleMap.Add(assetName, arType);
#if PACKAGE_BASIC
						//记录扩展包资源
						if (IsExtendPackageScene(rootAssetPath) && !extendAssetMap.ContainsKey(assetPath))
						{
							if (specialList.Contains(assetPath))
							{//Check it again.
								Debug.LogError("Special texture dictionary has same texture,TexturePath=" + assetPath + ".UIScenePath=" + rootAssetPath);
							}
							else
							{
								extendAssetMap.Add(assetPath, bundlePath);
							}
						}
						//记录小包资源
						if (IsBasicPackageScene(rootAssetPath) && !basicAssetList.Contains(assetPath))
						{
							basicAssetList.Add(assetPath);
						}
#endif
					}
					else
					{
						AssetBundleType usedType = allAssetBundleMap[assetName];
						if (usedType != arType)
						{
							Debug.LogError("Build UI error, same asset name has been found.AssetName=" +
								assetName + "." + arType + "," + assetName + "." + usedType
								+ ".UIPath=" + rootAssetPath);
						}
					}
				}
			}
		}

		//保存UI界面配置文件
		if (uiScenePrefabList != null && uiScenePrefabList.Count > 0)
		{
			string uisceneParentPath = AssetBundlePath.UIAssetRootPath + AssetBundleType.Scene + "/";	//场景目录
			if (!Directory.Exists(uisceneParentPath))
			{
				Directory.CreateDirectory(uisceneParentPath);
			}

			uiConfig.SaveConfig(uisceneParentPath + "uiconfig.txt");
		}
		else
		{
			Debug.Log("Build UI tips:no scene found.");
		}

		//保存Prefab配置文件
		if (prefabList != null && prefabList.Count > 0)
		{
			string prefabconfigParentPath = AssetBundlePath.UIAssetRootPath + AssetBundleType.Scene + "/";
			if (!Directory.Exists(prefabconfigParentPath))
			{
				Directory.CreateDirectory(prefabconfigParentPath);
			}
			prefabConfig.SaveConfig(prefabconfigParentPath + "uiprefabconfig.txt");
		}
		else
		{
			Debug.Log("Build UI tips:no dynamic prefab found.");
		}

		//删除忽略的资源
		foreach (string delPath in ignoreBundleList)
		{
			if (!string.IsNullOrEmpty(delPath))
			{
				File.Delete(delPath);
			}
		}

#if PACKAGE_BASIC
		//过滤小包中用到的资源
		foreach (string basicAssetPath in basicAssetList)
		{
			if (extendAssetMap.ContainsKey(basicAssetPath))
			{
				extendAssetMap.Remove(basicAssetPath);
			}
		}

		//删除扩展包资源
		foreach (string path in extendAssetMap.Values)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogError("Build basic package error, can not delete unuse texture");
			}
			else
			{
				File.Delete(path);
			}
		}
#endif

		//结束打包资源
		BuildPipeline.PopAssetDependencies();

		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 打包资源
	/// </summary>
	/// <param name="assetPath">源路径</param>
	/// <param name="bundleParentPath">目标父路径</param>
	/// <param name="bundlePath">目标路径</param>
	/// <param name="isLeaf">是否是叶子节点</param>
	/// <param name="needPopDepencance"></param>
	static void BuildBundle(string assetPath, string bundleParentPath, string bundlePath, bool isLeaf, bool popDependence)
	{
		if (!Directory.Exists(bundleParentPath))
		{
			Directory.CreateDirectory(bundleParentPath);
		}

		if(popDependence)
		{
			BuildPipeline.PushAssetDependencies();
		}

		Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));

		BuildAssetBundle.Build(obj, null, bundlePath, isLeaf);

		if(popDependence)
		{
			BuildPipeline.PopAssetDependencies();
		}

		obj = null;
	}

	/// <summary>
	/// 判断是否是小包UI界面
	/// </summary>
	/// <returns>ture-小包资源，false-非小包资源</returns>
	static bool IsBasicPackageScene(string uiscenePath)
	{
		string uiscenePathTemp = uiscenePath.Replace('\\', '/');

		return uiscenePathTemp.Contains(AssetBundlePath.UIPrefabBasicAssetDir) || uiscenePathTemp.Contains(AssetBundlePath.PrefabBasicAssetDir);
	}

	/// <summary>
	/// 判断是否是扩展包UI界面
	/// </summary>
	static bool IsExtendPackageScene(string uiscenePath)
	{
		string uiscenePathTemp = uiscenePath.Replace('\\', '/');

		return uiscenePathTemp.Contains(AssetBundlePath.UIPrefabNewAssetDir) || uiscenePathTemp.Contains(AssetBundlePath.PrefabAssetDir);
	}


    static string GetParentPath(string texturePath)
    {
        string parentPath = AssetBundlePath.UIAssetRootPath + AssetBundleType.Texture + "/";

#if PACKAGE_DYNAMICDOWNLOAD
        if (texturePath.Replace('\\', '/').Contains(AssetBundlePath.SpecialTexBasicPrimitiveAssetDir)
			|| texturePath.Replace('\\', '/').Contains(AssetBundlePath.SpecialTexPrimitiveAssetDir))
        {
            parentPath = AssetBundlePath.UIExtendAssetRootPath + AssetBundleType.Texture + "/";
        }
#endif

        return parentPath;
    }
	
	/// <summary>
	/// 排序算法
	/// </summary>
	class UICompare : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			string xTemp = x.Replace('\\', '/');
			string yTemp = y.Replace('\\', '/');

			if (xTemp.Contains(AssetBundlePath.UIPrefabBasicAssetDir) || xTemp.Contains(AssetBundlePath.PrefabBasicAssetDir))
			{
				if (yTemp.Contains(AssetBundlePath.UIPrefabBasicAssetDir) || yTemp.Contains(AssetBundlePath.PrefabBasicAssetDir))
				{
					return xTemp.CompareTo(yTemp);
				}
				else
				{
					return -1;
				}
			}
			else
			{
				if (yTemp.Contains(AssetBundlePath.UIPrefabBasicAssetDir) || yTemp.Contains(AssetBundlePath.PrefabBasicAssetDir))
				{
					return 1;
				}
				else
				{
					return xTemp.CompareTo(yTemp);
				}
			}
		}
	}
}
