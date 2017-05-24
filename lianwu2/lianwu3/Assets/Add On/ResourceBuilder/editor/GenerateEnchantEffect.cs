/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:GenerateEnchantEffect.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.08.03
					Modify: 
******************************************************************************/

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using LoveDance.Client.Common;
using LoveDance.Client.Loader;

public class GenerateEnchantEffect
{
	class EnchantEffectInfo
	{
		public string EffectName = "";
		public string EffectAssetPath = "";
	}

	/// <summary>
	/// 打包特效
	/// </summary>
	[MenuItem("Resource Generator/GenerateResource/All EnchantEffect(编译所有光效分离资源)")]
	public static void GenerateAllEnchantEffect()
	{
		CreateEnchantEffectAssetbundleFolder();

		BuildPipeline.PushAssetDependencies();

		List<string> shdList = BuildShader.BuildAllShader("all");

		ProcEnchantEffect_Dir(AssetBundlePath.EnchantEffectAssetPath, shdList);

		BuildPipeline.PopAssetDependencies();

		//ProcLightMaterial();
	}

	/// <summary>
	/// 选择指定的特效打包
	/// </summary>
	[MenuItem("Resource Generator/GenerateResource/Select EnchantEffect(编译指定文件夹的光效分离资源)")]
	static void GenerateSelectEnchantEffect()
	{
		CreateEnchantEffectAssetbundleFolder();

		string targetpath = EditorUtility.OpenFolderPanel("选择需要打包的光效", Application.dataPath + "/Add On Resource/EnchantEffect/", "");
		if (string.IsNullOrEmpty(targetpath))
			return;

		if (!targetpath.Contains("EnchantEffect"))
		{
			EditorUtility.DisplayDialog("文件夹路径错误", "无效的光效文件夹，光效路径必须在[Assets/Add On Resource/EnchantEffect/]文件夹下", "确定");
			return;
		}
		targetpath = "Assets" + targetpath.Replace(Application.dataPath, "");

		BuildPipeline.PushAssetDependencies();

		List<string> shdList = BuildShader.BuildAllShader("all");

		ProcEnchantEffect_Dir(targetpath, shdList);

		BuildPipeline.PopAssetDependencies();
	}

	/// <summary>
	/// 打包流光材质
	/// </summary>
	//[MenuItem("Resource Generator/GenerateResource/Enchant LightMaterial")]
	//static void GenerateEnchantLightMaterial()
	//{
	//    //ProcLightMaterial();
	//}

	static bool CreateEnchantEffectAssetbundleFolder()
	{
		if (!Directory.Exists(AssetBundlePath.EnchantEffectAssetPath))
		{
			Directory.CreateDirectory(AssetBundlePath.EnchantEffectAssetPath);
			return false;
		}

		return true;
	}

	static void ProcEnchantEffect_Dir(string dirPath, List<string> shdList)
	{
		Dictionary<string, string> assetMap = new Dictionary<string, string>();

		//获取所有模型
		List<string> prefabList = new List<string>();
		BuildAssetBundle.GetFiles(dirPath, "*.prefab", ref prefabList);

		foreach (string s in prefabList)
		{
			string sTemp = s.Replace('\\', '/');
			if (!sTemp.Contains(AssetBundlePath.EnchantEffectAssetPath))
			{
				continue;
			}
			string assetName = BuildAssetBundle.GetAssetName(s);
			if (assetMap.ContainsKey(assetName))
			{
				assetMap.Remove(assetName);
			}
			assetMap.Add(assetName, s);
		}

		//打包所有模型
		foreach (string assetName in assetMap.Keys)
		{
			string pathTemp = assetMap[assetName].Replace('\\', '/');
			string parentPath = pathTemp.Substring(0, pathTemp.LastIndexOf('/') + 1);

			ProcEnchantEffect_File(parentPath, pathTemp, AssetBundlePath.EnchantEffectBundlePath, shdList);
		}
	}

	static void ProcEnchantEffect_File(string filePath, string dirPath, string destParentPath, List<string> shdList)
	{
		GameObject effectPrefab = AssetDatabase.LoadMainAssetAtPath(dirPath) as GameObject;
		if(effectPrefab == null)
		{
			Debug.Log(dirPath);
			return;
		}

		string bundleName = effectPrefab.name;

		List<GameObject> particallist = new List<GameObject>();
		Dictionary<string, DependencyAsset> allAssetBundleMap = new Dictionary<string, DependencyAsset>();	//记录所有已经打包的Asset

		GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(effectPrefab);
		for (int i = rendererClone.transform.childCount - 1; i >= 0; --i)
		{
			Transform childTran = rendererClone.transform.GetChild(i);
			if (childTran.name.Contains("Bip01"))
			{
				FindParticalFromBonse(particallist, childTran);
				break;
			}
		}

		Object rendererPrefab = null;
		string effectAssetPath = string.Empty;
	
		if (bundleName.Contains("group"))
		{
			rendererPrefab = GenerateResource.ReplacePrefab(rendererClone, rendererClone.name);
		}
		else
		{
			GameObject effectRoot = new GameObject();
			effectRoot.name = bundleName;
			foreach (GameObject p in particallist)
			{
				GameObject gParent = new GameObject(GetTransfromPathName(p.transform.parent));
				gParent.transform.position = p.transform.parent.position;
				gParent.transform.rotation = p.transform.parent.rotation;
				gParent.transform.localScale = Vector3.one;
				p.transform.parent = gParent.transform;
				gParent.transform.parent = effectRoot.transform;
			}

			rendererPrefab = GenerateResource.ReplacePrefab(effectRoot, effectRoot.name);
		}
		effectAssetPath = AssetDatabase.GetAssetPath(rendererPrefab);

		AssetDatabase.Refresh();	//刷新
		AssetDatabase.SaveAssets();	//保存

		AssetBundleConfig modeConfig = new AssetBundleConfig();
		Dictionary<string, List<DependencyAsset>> allAssetRefs = BuildAssetBundle.GetAssetDependencieRefs((new List<string>() { effectAssetPath }).ToArray());

		string dirName = bundleName;
		//打包依赖资源
		foreach (string modePath in allAssetRefs.Keys)
		{
			string modeName = BuildAssetBundle.GetAssetName(modePath);
			List<DependencyAsset> depList = allAssetRefs[modePath];
			for (int i = 0; i < depList.Count; ++i)
			{
				List<Object> includeList = new List<Object>();

				DependencyAsset dasset = depList[i];
				AssetBundleType arType = dasset.AssetType;
				string assetName = dasset.AssetName;
				string assetFullName = dasset.AssetName + "." + dasset.AssetSuffix;
				string assetPath = dasset.AssetPath;
				string bundleParentPath = destParentPath + arType + "/";
				string bundlePath = bundleParentPath + assetName + "." + arType.ToString().ToLower();

				if (modePath.Equals(assetPath))
				{	
					//重新改变路径
					bundleParentPath = destParentPath +assetName + "/";
					bundlePath = bundleParentPath + assetName + ".enceff";
				}
				else
				{
					//忽略原始FBX或者Prefab文件，也忽略了最终需要生成的FBX或Prefab
					if (arType == AssetBundleType.Pre)
					{
						continue;
					}
					if (arType == AssetBundleType.Shd && shdList.Contains(assetFullName))
					{
						continue;
					}

					modeConfig.AddConfig(modeName, arType, assetName, i);
				}

				if (!allAssetBundleMap.ContainsKey(assetName))
				{
					if (!Directory.Exists(bundleParentPath))
					{
						Directory.CreateDirectory(bundleParentPath);
					}
					if (modePath.Equals(assetPath))
					{
						BuildPipeline.PushAssetDependencies();
					}

					if (arType == AssetBundleType.Shd)
					{
						BuildShaderExtend.GenerateShaderExtend(assetPath, dasset.IsLeaf);
					}
					else
					{
						if (!BuildAssetBundle.IsLegalAsset(assetName))
						{
							string errorTips = string.Format("Generate enchant effect warning, asset name is not all lower,FileName is {0},AssetName is {1}",
							dirName, assetPath);
							Debug.LogError(errorTips);
							EditorUtility.DisplayDialog("Error", errorTips+"Please try again!", "OK");
							return;
						}

						Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
						includeList.Add(obj);

						BuildAssetBundle.Build(obj, includeList.ToArray(), bundlePath, dasset.IsLeaf);

						if (modePath.Equals(assetPath))
						{
							BuildPipeline.PopAssetDependencies();
						}
						allAssetBundleMap.Add(assetName, dasset);
					}
				}
				else
				{
					DependencyAsset usedAsset = allAssetBundleMap[assetName];
					if (usedAsset.AssetType != arType)
					{
						Debug.LogError("Build EnchantEffect error, same asset name has been found.AssetName=" +
							assetPath + "," + usedAsset.AssetPath);
					}
				}
			}
		}
	
		if (!Directory.Exists(destParentPath + dirName))
		{
			Directory.CreateDirectory(destParentPath + dirName);
		}
		string configDirPath = destParentPath + dirName + "/" + bundleName + ".txt";
		modeConfig.SaveConfig(configDirPath);

		//删除临时资源		
		AssetDatabase.DeleteAsset(effectAssetPath);
		GameObject.DestroyImmediate(rendererClone);
	}

	/// <summary>
	/// 打包流光特效
	/// </summary>
	static void ProcLightMaterial()
	{
		if (!Directory.Exists(AssetBundlePath.EnchantLightMaterialBundlePath))
		{
			Directory.CreateDirectory(AssetBundlePath.EnchantLightMaterialBundlePath);
		}

		string lightMatDir = AssetBundlePath.EnchantLightMaterialAssetPath;

		string[] filetga = Directory.GetFiles(lightMatDir, "*.mat");
		string path = AssetBundlePath.EnchantLightMaterialBundlePath + "light.material";
		List<Object> assetlist = new List<Object>();
		foreach (string f in filetga)
		{
			Object o = AssetDatabase.LoadMainAssetAtPath(f);
			assetlist.Add(o);
		}

		BuildAssetBundle.Build(null, assetlist.ToArray(), path, true);
		assetlist.Clear();
	}

	static void FindParticalFromBonse(List<GameObject> particallist, Transform parent)
	{
		for (int i = parent.transform.childCount - 1; i >= 0; --i)
		{
			Transform childTran = parent.transform.GetChild(i);
			if (childTran.name.Contains("Bip01"))
			{
				FindParticalFromBonse(particallist, childTran);
			}
			else
			{
				particallist.Add(childTran.gameObject);
			}
		}
	}

	static string GetTransfromPathName(Transform t)
	{
		string name = t.name;

		Transform parent = t.parent;
		while (t.root != parent)
		{
			name = parent.name + "/" + name;
			parent = parent.parent;
		}

		return name;
	}
}