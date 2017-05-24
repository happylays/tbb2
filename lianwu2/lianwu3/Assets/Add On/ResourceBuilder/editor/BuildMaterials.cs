using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using LoveDance.Client.Common;
using LoveDance.Client.Loader;

public class BuildMaterials
{
	/// <summary>
	/// 服饰信息类
	/// </summary>
	class MaterialInfo
	{
		public string MatName = "";
		public string MatAssetPath = "";
		public Object MatObject = null;
		public DependencyAsset DepAsset = null;
		public Dictionary<string, Object> HolderMap = new Dictionary<string, Object>();
	}

	/// <summary>
	/// 打包所有贴图
	/// </summary>
	/// <param name="srcPath">源路径</param>
	/// <param name="destParentPath">生成文件父路径</param>
	static void ProcTextures(string srcPath, string destParentPath)
	{
		List<string> textureList = new List<string>();
		BuildAssetBundle.GetFiles(srcPath, "*.tga", ref textureList);

		if (!Directory.Exists(destParentPath + AssetBundleType.Texture.ToString()))
		{
			Directory.CreateDirectory(destParentPath + AssetBundleType.Texture.ToString());
		}
		foreach (string s in textureList)
		{
			Object obj = AssetDatabase.LoadAssetAtPath(s, typeof(Object));
			string bundlePath = destParentPath + AssetBundleType.Texture.ToString() + "/" + BuildAssetBundle.GetAssetName(s) + "." + AssetBundleType.Texture.ToString().ToLower();
			BuildAssetBundle.Build(obj, null, bundlePath, true);
		}
	}

	/// <summary>
	/// 打包手持、翅膀、尾巴、左右手持、肩膀
	/// </summary>
	/// <param name="characterFBX">模型</param>
	/// <param name="bundleParentPath">生成文件父路径</param>
	static void ProcMaterials_FileForWingHip(GameObject characterFBX, string bundleParentPath)
	{
		string bundleName = characterFBX.name;

		if (bundleName.StartsWith("wing_") || bundleName.StartsWith("hip_") || bundleName.StartsWith("lefthand_")
			|| bundleName.StartsWith("righthand_") || bundleName.StartsWith("shoulders_")
			|| bundleName.StartsWith("leftear_") || bundleName.StartsWith("rightear_"))
		{
			GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(characterFBX);
			string desPath = bundleParentPath + bundleName + ".clh";
			if (File.Exists(desPath))
			{
				File.Delete(desPath);
			}
			List<Object> includeList = new List<Object>();
			Object rendererPrefab = GenerateResource.ReplacePrefab(rendererClone, rendererClone.name);
			includeList.Add(rendererPrefab);

			if (!Directory.Exists(bundleParentPath))
			{
				Directory.CreateDirectory(bundleParentPath);
			}

			BuildAssetBundle.Build(null, includeList.ToArray(), desPath);

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
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

}
