using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
	public class SkinnLoader
	{
		// The class constructor is called when the class instance is created
		GameObject m_Prefb = null;
		private List<object> m_BoneNames = new List<object>();
		private bool m_bMainLoader = true;
		private bool m_bFailed = false;
		public uint m_RefNum = 1;
		private AssetBundle m_AssetBundle = null;
        private IEnumerator itor = null;

		private static AssetBundle s_DefaultSkins = null;
		private static Material[] s_DefaultLegSkinMat = null;

		static string mAssetWWWDir = "";
		static string mAssetDir = "";
		static string mInAssetWWWDir = "";
        static string m_assetNetDir = "";
        
        public static void InitSkinLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir, string assetNetDir)
		{
			mAssetDir = assetDir;
			mAssetWWWDir = assetWWWDir;
            mInAssetWWWDir = inAssetWWWDir;
            m_assetNetDir = assetNetDir;
		}

        private static string GetSkinResPath()
        {
            return m_assetNetDir + "Skin/";
        }

		public static IEnumerator LoaddefaultSkins()
		{
            string strName = "default.skn";
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
            {
				DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, GetSkinResPath() + strName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
				if(downloadPack != null)
				{
					while (!downloadPack.AssetReady)
					{
						yield return null;
					}
					s_DefaultSkins = downloadPack.Bundle;
				}
				
                WWWDownLoader.RemoveDownLoad(strName, null);
            }
            else
            {
                string strSkins = mAssetWWWDir + "Skin/default.skn";
                string assetPath = mAssetDir + "Skin/default.skn";
                if (!File.Exists(assetPath))
                {
                    strSkins = mInAssetWWWDir + "Skin/default.skn";
                }

                WWW www = null;
                using (www = new WWW(strSkins))
                {
                    while (!www.isDone)
                    {
                        yield return null;
                    }
                    if (www.error != null)
                    {
                        Debug.LogError("Skin Load Error!");
                        Debug.LogError(www.error);
                    }
                    else
                    {
                        s_DefaultSkins = www.assetBundle;
                    }

                    www.Dispose();
                    www = null;
                }
            }
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
		}

		public static Material GetDefaultLegSkinMat(bool bBoy)
		{
			if (s_DefaultSkins != null)
			{
				if (s_DefaultLegSkinMat == null)
				{
					s_DefaultLegSkinMat = new Material[2];
				}

				if (bBoy)
				{
					if (s_DefaultLegSkinMat[0] == null)
					{
						s_DefaultLegSkinMat[0] = (Material)s_DefaultSkins.Load("boy_leg");
					}
					return s_DefaultLegSkinMat[0];
				}
				else
				{
					if (s_DefaultLegSkinMat[1] == null)
					{
						s_DefaultLegSkinMat[1] = (Material)s_DefaultSkins.Load("girl_leg");
					}
					return s_DefaultLegSkinMat[1];
				}
			}

			return null;
		}
		
		public static void releasedefaultSkins()
		{
			if (s_DefaultSkins != null)
			{
				s_DefaultSkins.Unload(true);
				s_DefaultSkins = null;
			}
		}

		public bool IsFailed
		{
			get
			{
				return m_bFailed;
			}
			set
			{
				m_bFailed = value;
			}
		}
		public bool IsMainLoader
		{
			get
			{
				return m_bMainLoader;
			}
			set
			{
				m_bMainLoader = value;
			}
		}
		private string m_ResuceName;
		private string m_ChildPath;		//服饰子目录
		private static Dictionary<string, SkinnLoader> m_mapRoleBodyLoader = new Dictionary<string, SkinnLoader>();
		public string ResuceName
		{
			get
			{
				return m_ResuceName;
			}
		}

		public SkinnLoader(string strFileName, string childPath)
		{
			m_ResuceName = strFileName;
			m_ChildPath = childPath;
		}

		public static SkinnLoader GetkinnedMeshRendererLoader(string strFileName, string childPath)
		{
			SkinnLoader Loader = null;
			if (m_mapRoleBodyLoader.ContainsKey(strFileName))
			{
				Loader = m_mapRoleBodyLoader[strFileName];
				Loader.m_RefNum += 1;
			}
			else
			{
				Loader = new SkinnLoader(strFileName, childPath);
				m_mapRoleBodyLoader.Add(strFileName, Loader);
			}
			return Loader;
		}

		public bool IsLoadSuc
		{
			get
			{
				return m_Prefb != null && m_BoneNames.Count != 0;
			}
		}

        public IEnumerator LoadMeshRenderer()
        {
            if (itor == null)
            {
                itor = LoadRenderer();
            }
            while(itor.MoveNext())
            {
                yield return null;
            }
        }

        private IEnumerator LoadRenderer()
        {
            if (!ResuceName.StartsWith("wing_") && !ResuceName.StartsWith("hip_") &&
                !ResuceName.StartsWith("lefthand_") && !ResuceName.StartsWith("righthand_") &&
                !ResuceName.StartsWith("skin_") &&
                !ResuceName.StartsWith("shoulders_") &&
                !ResuceName.StartsWith("leftear_") && !ResuceName.StartsWith("rightear_"))
            {
                if (WWWDownLoaderConfig.CheckResNeedUpdate(ResuceName))
                {
					DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(ResuceName, m_assetNetDir + m_ChildPath + ResuceName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
					if (downloadPack != null)
					{
						while (!downloadPack.AssetReady)
						{
							yield return null;
						}
						m_AssetBundle = downloadPack.Bundle;
					}
                    WWWDownLoader.RemoveDownLoad(ResuceName, null);
                }
                else
                {
                    string assetWWWPath = mAssetWWWDir + m_ChildPath + ResuceName;
                    string assetPath = mAssetDir + m_ChildPath + ResuceName;
                    if (!File.Exists(assetPath))
                    {
                        assetWWWPath = mInAssetWWWDir + m_ChildPath + ResuceName;
                    }

                    WWW www = null;
                    using (www = new WWW(assetWWWPath))
                    {
                        while (!www.isDone)
                        {
                            yield return null;
                        }

						if (string.IsNullOrEmpty(www.error))
						{
							m_AssetBundle = www.assetBundle;
						}
						else
						{
							Debug.LogError(www.error + "." + ResuceName);
						}

                        www.Dispose();
                        www = null;
                    }
                }

                if (m_AssetBundle != null)
                {
                    AssetBundleRequest gameObjectRequest = null;
                    AssetBundleRequest boneNameRequest = null;

                    gameObjectRequest = m_AssetBundle.LoadAsync(ResuceName.Replace(".clh", ""), typeof(GameObject));
                    while (!gameObjectRequest.isDone)
                    {
                        yield return null;
                    }

                    if (m_AssetBundle != null)
                    {
                        List<AssetBundleRequest> las = new List<AssetBundleRequest>();
                        m_Prefb = (GameObject)gameObjectRequest.asset;

                        if (m_Prefb != null)
                        {
                            foreach (SkinnedMeshRenderer smr in m_Prefb.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                            {
                                boneNameRequest = m_AssetBundle.LoadAsync(smr.name.Replace("(Clone)", "") + "bonenames", typeof(object));
                                if (boneNameRequest != null)
                                {
                                    las.Insert(0, boneNameRequest);
                                }
                            }

                            while (las.Count != 0)
                            {
                                AssetBundleRequest gtABR = las[las.Count - 1];
                                if (gtABR == null)
                                {
                                    break;
                                }
                                if (gtABR.isDone)
                                {
                                    m_BoneNames.Add((object)gtABR.asset);
                                    las.RemoveAt(las.Count - 1);
                                }
                                yield return null;
                            }
                        }

						if (CommonValue.PhoneOS == Phone_OS.Ios)
						{
							m_AssetBundle.Unload(false);
						}
                    }
                }
            }
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }
        
		public System.Object[] GetMainGameObject
		{
			get
			{
				System.Object[] resArr = new System.Object[2];

				GameObject go = null;
				List<GameObject> SMRs = new List<GameObject>();
				Dictionary<ParticleSystem, float> clothParticleEffect = new Dictionary<ParticleSystem, float>();
				if (m_Prefb != null)
				{
					go = (GameObject)GameObject.Instantiate(m_Prefb);
				}

				if (go != null)
				{
					foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
					{
						if (!smr.gameObject.name.Contains("_nobone"))
						{
							SMRs.Add(smr.gameObject);
							smr.transform.parent = null;
						}
					}

					foreach (ParticleSystem cpe in go.GetComponentsInChildren<ParticleSystem>(true))
					{
						clothParticleEffect.Add(cpe, cpe.emissionRate);
					}

					for (int i = go.transform.childCount - 1; i >= 0; --i)
					{
						Transform childTran = go.transform.GetChild(i);
                        if (childTran.gameObject.name.Contains("Bip01"))
                        {
                            childTran.parent = null;
                            SMRs.Add(childTran.gameObject);
                        }
                        else
                        {
                            if (childTran.childCount > 0 && childTran.GetComponent<SkinnedMeshRenderer>() == null)
                            {
                                SMRs.Add(childTran.gameObject);
                            }
                        }
					}
					if (go.GetComponent<SkinnedMeshRenderer>() == null)
					{
						GameObject.Destroy(go);
					}
				}

				resArr[0] = SMRs;
				resArr[1] = clothParticleEffect;

				return resArr;
			}
		}
		public List<object> BoneNames
		{
			get
			{
				return m_BoneNames;
			}
		}

		void _Release()
		{
			m_Prefb = null;
			if (m_BoneNames != null)
			{
				m_BoneNames.Clear();
			}
			if (m_AssetBundle != null)
			{
				m_AssetBundle.Unload(true);
				m_AssetBundle = null;
			}
		}
		public void Release()
		{
			m_RefNum -= 1;
			if (m_RefNum == 0)
			{
                m_mapRoleBodyLoader.Remove(ResuceName);

                WWWDownLoader.StopLoad(ResuceName);
                WWWDownLoader.RemoveDownLoad(ResuceName, null);

				_Release();
			}
		}
	}
}