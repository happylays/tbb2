using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class GenerateMaterials
{
    public static string MaterialsSrcDir = "Assets/Add On Resource/Characters/Models/";
    public static string MaterialsAssetbundlePath
    {
        get { return GenerateResource.ResAssetbundleDir + "Materials/"; }
    }

    private static string IOS_TAG = "_ios";

    [MenuItem("Resource Generator/GenerateResource/All Materials")]
    static void GenerateAllMaterials()
    {
        if (!Directory.Exists(MaterialsAssetbundlePath))
        {
            Directory.CreateDirectory(MaterialsAssetbundlePath);
        }

        BuildPipeline.PushAssetDependencies();
        List<string> shdList = BuildShader.BuildAllShader("all");

        BuildPipeline.PushAssetDependencies();

        BuilderDefualtTexture();
        ProcMaterials_Dir(MaterialsSrcDir);

        BuildPipeline.PopAssetDependencies();

        BuildPipeline.PopAssetDependencies();
    }

    [MenuItem("Resource Generator/GenerateResource/Select Materials")]
    static void GenerateSelectMaterials()
    {
        string tagpath = EditorUtility.OpenFolderPanel("", Application.dataPath + "/Add On Resource/Characters/Models/", "");
        if (string.IsNullOrEmpty(tagpath) && !tagpath.Contains(MaterialsSrcDir))
            return;
        tagpath = "Assets" + tagpath.Replace(Application.dataPath, "");
        BuildPipeline.PushAssetDependencies();
        List<string> shdList = BuildShader.BuildAllShader("all");

        BuildPipeline.PushAssetDependencies();

        BuilderDefualtTexture();


        ProcMaterials_Dir(tagpath);

        BuildPipeline.PopAssetDependencies();
        BuildPipeline.PopAssetDependencies();
    }
    static void BuilderDefualtTexture()
    {
        string skinpath = MaterialsAssetbundlePath + "Skin/";
        if (!Directory.Exists(skinpath))
        {
            Directory.CreateDirectory(skinpath);
        }

        string[] filetga = Directory.GetFiles(MaterialsSrcDir + "Skin", "*.mat");
        string path = skinpath + "default.skn";
        List<Object> assetlist = new List<Object>();
        foreach (string f in filetga)
        {
            //FileInfo i= new FileInfo(f);
            Object o = AssetDatabase.LoadMainAssetAtPath(f);
            assetlist.Add(o);
        }

		BuildAssetBundle.Build(null, assetlist.ToArray(), path, true);

        assetlist.Clear();

    }

    static void ProcMaterials_Dir(string dirPath)
    {
		try
		{
			bool bRes = false;
			if (Directory.Exists(dirPath))
			{
				string[] fileArr = Directory.GetFiles(dirPath, "*.FBX");
				foreach (string filePath in fileArr)
				{
					ProcMaterials_File(filePath, dirPath);
					bRes = true;
				}
				string[] prefabFileArr = Directory.GetFiles(dirPath, "*.prefab");
				foreach (string filePath in prefabFileArr)
				{
					ProcMaterials_File(filePath, dirPath);
					bRes = true;
				}

				if (!bRes && dirPath.Contains("socks") && Directory.Exists(dirPath + "/Materials"))
				{
					string[] matFiles = Directory.GetFiles(dirPath + "/Materials", "socks_*.mat");

					foreach (string mfs in matFiles)
					{
						Material mat = (Material)AssetDatabase.LoadAssetAtPath(mfs, typeof(Material));
						//if( !matList.Contains(mat.name) )
						{
							ExportNoMeshSocksMaterial(mat);
						}
						//Object.DestroyImmediate( mat );
					}
				}

				string[] dirArr = Directory.GetDirectories(dirPath);
				foreach (string dir in dirArr)
				{
					if (!dir.Contains("/."))
					{
						ProcMaterials_Dir(dir);
					}
				}
			}
			else
			{
				Debug.Log("path is not exist: " + dirPath);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError("Generate material faile." + dirPath + "." + e);
		}
    }

    static bool IsIOS()
    {
#if UNITY_IPHONE
		return true;
#else
        return false;
#endif
    }

    static void ExportNoMeshSocksMaterial(Material mat)
    {
        bool needBuild = true;
        string matPath = AssetDatabase.GetAssetPath(mat);
        string matName = mat.name;

        if (IsIOS())
        {
            //在IOS环境下，假如不是ios资源，查找到存在ios mat资源，则不编译该文件
            if (!IsIOSFile(matPath))
            {
                string matIOSPath = GetExtendPath(matPath, "_ios");

                if (File.Exists(matIOSPath))
                {
                    needBuild = false;
                }
            }
            else
            {
                matName = CheckIOSMat(mat.name);
            }
        }
        else
        {
            //非ios环境下，假如它是ios mat文件 ，则不编译该文件
            if (IsIOSFile(matPath))
            {
                needBuild = false;
            }
        }

        if (needBuild)
        {
            GameObject o = new GameObject(matName);

            SkinnedMeshRenderer smr = o.AddComponent<SkinnedMeshRenderer>();
            smr.sharedMaterial = mat;
            Object rendererPrefab = GenerateResource.ReplacePrefab(o, o.name);
            string desPath = MaterialsAssetbundlePath + rendererPrefab.name + ".clh";
            if (File.Exists(desPath))
            {
                File.Delete(desPath);
            }

            BuildPipeline.PushAssetDependencies();

			BuildAssetBundle.Build(null, new Object[] { rendererPrefab }, desPath);

            BuildPipeline.PopAssetDependencies();
            Debug.Log("Saved " + rendererPrefab.name);

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
        }
    }
    static void ProcMaterials_File(string filePath, string dirPath)
    {
        //BuildPipeline.PushAssetDependencies();
        //BuilderDefualtTexture();
        List<string> matList = new List<string>();
        Object o = AssetDatabase.LoadMainAssetAtPath(filePath);
        ProcMaterials_File(o, matList);

        string[] matFiles = Directory.GetFiles(dirPath + "/Materials", "socks_*.mat");

        foreach (string mfs in matFiles)
        {
            Material mat = (Material)AssetDatabase.LoadAssetAtPath(mfs, typeof(Material));
            if (!matList.Contains(mat.name))
            {
                ExportNoMeshSocksMaterial(mat);
            }
            Object.DestroyImmediate(mat);
        }
        //BuildPipeline.PopAssetDependencies();
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
    static void ProcMaterials_File(Object o, List<string> matList)
    {
        if (o is GameObject)
        {
            GameObject characterFBX = (GameObject)o;
            string bundleName = characterFBX.name;
            Debug.Log("******* Creating Material assetbundles for: " + bundleName + " *******");
#if false			
			string desPath = MaterialsAssetbundlePath + bundleName + ".clh";
			if( File.Exists( desPath ) )
			{
				File.Delete( desPath );
			}
		
			BuildPipeline.PushAssetDependencies();				
			BuildPipeline.BuildAssetBundle( characterFBX , null , desPath,
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, 
				EditorUserBuildSettings.activeBuildTarget );
			BuildPipeline.PopAssetDependencies();
#else
            if (!IsNeedBone(bundleName))
            {
                ProcMaterials_FileForWingHip(characterFBX);
                return;
            }
            //remove bones;
            GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(characterFBX);
            //GameObject Bonese = null;

            //获取骨骼下的所有特效
            List<GameObject> particallist = new List<GameObject>();
            for (int i = rendererClone.transform.childCount - 1; i >= 0; --i)
            {
                Transform childTran = rendererClone.transform.GetChild(i);
                if (childTran.name.Contains("Bip01"))
                {
                    FindParticalFromBonse(particallist, childTran);
                    break;
                }
            }

            for (int i = rendererClone.transform.childCount - 1; i >= 0; --i)
            {
                List<Object> includeList = new List<Object>();
                List<string> holderList = new List<string>();

                Transform childTran = rendererClone.transform.GetChild(i);
                if (childTran.name.Contains("Bip01"))
                {
                    //FindParticalFromBonse( particallist ,childTran);
                    continue;
                }
                else
                {
                    childTran.parent = null;
                }

                string desPath = MaterialsAssetbundlePath + childTran.name + ".clh";
                if (File.Exists(desPath))
                {
                    File.Delete(desPath);
                }

                // we don't need to Save bones and animations . Any 
                // As we can not edit assets we instantiate
                // the fbx and remove what we dont need. As only assets can be
                // added to assetbundles we save the result as a prefab and delete
                // it as soon as the assetbundle is created.
                GameObject partClone = childTran.gameObject;
                foreach (SkinnedMeshRenderer smr in partClone.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    //Replace Mat for IOS
                    if (IsIOS())
                    {
                        string matPath = string.Empty;
                        string matIOSPath = string.Empty;
                        List<Material> matList1 = new List<Material>();

                        foreach (Material mat in smr.sharedMaterials)
                        {
                            matPath = AssetDatabase.GetAssetPath(mat);
                            matIOSPath = GetExtendPath(matPath, "_ios");
                            if (File.Exists(matIOSPath))
                            {
                                Material newMat = AssetDatabase.LoadAssetAtPath(FixToUnityPath(matIOSPath), typeof(Material)) as Material;
                                if (newMat != null)
                                {
                                    matList1.Add(newMat);
                                }
                            }
                            else
                            {
                                matList1.Add(mat);
                            }
                        }

                        smr.sharedMaterials = matList1.ToArray();
                    }

                    //判断蒙皮的骨骼是否是共用骨骼
                    if (IsCommonBone(smr))
                    {
                        //Bonesname
                        List<string> boneNames = new List<string>();
                        foreach (Transform t in smr.bones)
                        {
                            boneNames.Add(GetTransfromPathName(t));
                        }

                        StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
                        holder.content = boneNames.ToArray();

                        string holderPath = "Assets/" + smr.name + "bonenames.prefab";
                        AssetDatabase.CreateAsset(holder, holderPath);

                        includeList.Add(AssetDatabase.LoadAssetAtPath(holderPath, typeof(StringHolder)));
                        holderList.Add(holderPath);

                        foreach (Material mat in smr.sharedMaterials)
                        {
                            if (mat != null && mat.name.Contains("socks_") && !matList.Contains(mat.name))
                            {
                                matList.Add(mat.name);
                            }
                        }
                    }
                    else
                    {
                        smr.gameObject.name = smr.gameObject.name + "_nobone";
                    }
                }

                if (particallist.Count != 0)
                {
                    foreach (GameObject p in particallist)
                    {
                        if (p.name.Contains(partClone.name))
                        {
                            GameObject gParent = new GameObject(GetTransfromPathName(p.transform.parent));
                            gParent.transform.position = p.transform.parent.position;
                            gParent.transform.rotation = p.transform.parent.rotation;
                            gParent.transform.localScale = Vector3.one;
                            p.transform.parent = gParent.transform;
                            gParent.transform.parent = partClone.transform;
                        }
                    }
                    foreach (Transform t in partClone.transform)
                    {
                        if (t.childCount > 0)
                        {
                            particallist.Remove(t.GetChild(0).gameObject);
                        }
                    }
                }

                Object rendererPrefab = GenerateResource.ReplacePrefab(partClone, partClone.name);
                includeList.Add(rendererPrefab);

                BuildPipeline.PushAssetDependencies();

				BuildAssetBundle.Build(null, includeList.ToArray(), desPath);

                BuildPipeline.PopAssetDependencies();
                Debug.Log("Saved " + rendererPrefab.name);

                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));

                foreach (string s in holderList)
                {
                    AssetDatabase.DeleteAsset(s);
                }
            }

            GameObject.DestroyImmediate(rendererClone);
#endif
        }
    }

    static void ProcMaterials_FileForWingHip(GameObject characterFBX)
    {
        if (!IsNeedBone(characterFBX.name))
        {
            GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(characterFBX);
            string desPath = MaterialsAssetbundlePath + characterFBX.name + ".clh";
            if (File.Exists(desPath))
            {
                File.Delete(desPath);
            }
            List<Object> includeList = new List<Object>();
            Object rendererPrefab = GenerateResource.ReplacePrefab(rendererClone, rendererClone.name);
            includeList.Add(rendererPrefab);

            BuildPipeline.PushAssetDependencies();

			BuildAssetBundle.Build(null, includeList.ToArray(), desPath);

            BuildPipeline.PopAssetDependencies();
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
        }
    }

    public static string GetTransfromPathName(Transform t)
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

    /// <summary>
    /// 是否需要裸模骨骼
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private static bool IsNeedBone(string bundleName)
    {
        if (bundleName.StartsWith("wing_")
         || bundleName.StartsWith("hip_")
         || bundleName.StartsWith("lefthand_")
         || bundleName.StartsWith("righthand_")
         || bundleName.StartsWith("shoulders_"))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 是否是共用骨骼的蒙皮
    /// </summary>
    /// <param name="smr"></param>
    /// <returns></returns>
    private static bool IsCommonBone(SkinnedMeshRenderer smr)
    {
        Transform[] bones = smr.bones;
        if (bones.Length > 0 && bones[0].name.Contains("Bip01"))
        {
            return true;
        }

        return false;
    }

    private static string GetExtendPath(string path, string a)
    {
        string newPath = "";

		FileInfo fi = new FileInfo(path);
		newPath = fi.FullName.Replace(fi.Extension, string.Empty) + a + fi.Extension;

		return newPath;
    }

    /// <summary>
    /// 获取unity路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string FixToUnityPath(string path)
    {
        string s = path;
        int index = path.IndexOf("Assets");
        if (index != -1)
        {
            s = path.Substring(index, path.Length - index);
        }

        return s;
    }

    private static string CheckIOSMat(string matPath)
    {
        if (IsIOSFile(matPath))
        {
            int iosIndex = matPath.LastIndexOf(IOS_TAG);
            matPath = matPath.Substring(0, iosIndex);
        }

        return matPath;
    }

    private static bool IsIOSFile(string path)
    {
        int iosIndex = path.LastIndexOf(IOS_TAG);
        int pointIndex = path.LastIndexOf('.');
        if (pointIndex != -1 && iosIndex != -1
            && (iosIndex + IOS_TAG.Length == pointIndex))
        {
            return true;
        }

        return false;
    }
}