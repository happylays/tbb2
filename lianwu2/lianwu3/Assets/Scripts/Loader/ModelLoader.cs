using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
	public class ModelLoader
	{
		string mResName;
		string mAssetName;
		string mChildPath;	//服饰子目录
		GameObject modelPrefab;
		WWW m_www;
		AssetBundleRequest request;
		AssetBundle bundle;
        private IEnumerator itor = null;

		public uint m_RefNum = 1;

		static Dictionary<string, ModelLoader> cacheLoaders = new Dictionary<string, ModelLoader>();

		//路径
		static string mAssetWWWDir = "";
		static string mAssetDir = "";
		static string mInAssetWWWDir = "";
        static string m_assetNetDir = "";
        
		ModelLoader(string resourceName, string childPath)
		{
			mResName = resourceName;
			mAssetName = mResName;
			mChildPath = childPath;
		}

		public static void InitModelLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir,string assetNetDir)
		{
			mAssetDir = assetDir;
			mAssetWWWDir = assetWWWDir;
			mInAssetWWWDir = inAssetWWWDir;
            m_assetNetDir = assetNetDir;
		}

		public static ModelLoader GetModelLoader(string resourceName, string childPath)
		{
			ModelLoader loader = null;
			if (cacheLoaders.ContainsKey(resourceName))
			{
				loader = cacheLoaders[resourceName];
				loader.m_RefNum += 1;
			}
			else
			{
				loader = new ModelLoader(resourceName, childPath);
				cacheLoaders.Add(resourceName, loader);
			}
			return loader;
		}

		public bool IsLoadSuc
		{
			get
			{
				return modelPrefab != null;
			}
		}

        public IEnumerator LoadModel()
        {
            if (itor == null)
            {
                itor = Load();
            }
            while (itor.MoveNext())
            {
                yield return null;
            }
        }
        
		private IEnumerator Load()
		{
			if (mResName.StartsWith("wing_") || mResName.StartsWith("hip_") ||
				mResName.StartsWith("lefthand_") || mResName.StartsWith("righthand_") ||
				mResName.StartsWith("shoulders_") ||
				mResName.StartsWith("leftear_") || mResName.StartsWith("rightear_"))
            {
                if (WWWDownLoaderConfig.CheckResNeedUpdate(mAssetName))
                {
					DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(mAssetName, m_assetNetDir + mChildPath + mAssetName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
					if (downloadPack != null)
					{
						while (!downloadPack.AssetReady)
						{
							yield return null;
						}
						bundle = downloadPack.Bundle;
					}
                    WWWDownLoader.RemoveDownLoad(mAssetName, null);
                }
                else
                {
                    string assetWWWPath = mAssetWWWDir + mChildPath + mAssetName;
                    string assetPath = mAssetDir + mChildPath + mAssetName;
                    if (!File.Exists(assetPath))
                    {
                        assetWWWPath = mInAssetWWWDir + mChildPath + mAssetName;
                    }

                    using (m_www = new WWW(assetWWWPath))
                    {
                        while (m_www != null && !m_www.isDone)
                        {
                            yield return null;
                        }

						if (m_www != null)
                        {
                            if (m_www.error != null)
                            {
                                Debug.LogError(m_www.error);
                                Debug.LogError("Model Load Error! AssetName : " + mAssetName);
                            }
                            if (m_www != null)
                            {
                                bundle = m_www.assetBundle;
                            }

							m_www.Dispose();
							m_www = null;
                        }
                    }
                }
                if (bundle != null)
                {
                    request = bundle.LoadAsync(mAssetName.Replace(".clh", ""), typeof(GameObject));
					while (request != null && !request.isDone)
                    {
                        yield return null;
                    }
					if (request != null)
                    {
                        modelPrefab = request.asset as GameObject;
                    }

					if (CommonValue.PhoneOS == Phone_OS.Ios)
					{
						bundle.Unload(false);
					}
                }
			}
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
		}

		public System.Object[] TotalModel
		{
			get
			{
				System.Object[] resArr = new System.Object[2];
				GameObject modelInstance = null;
				Dictionary<ParticleSystem, float> extraParticleEffect = new Dictionary<ParticleSystem, float>();
				if (modelPrefab == null)
					modelInstance = new GameObject();
				else
				{
					modelInstance = (GameObject)GameObject.Instantiate(modelPrefab);

					foreach (ParticleSystem cpe in modelInstance.GetComponentsInChildren<ParticleSystem>(true))
					{
						extraParticleEffect.Add(cpe, cpe.emissionRate);
					}
				}

				resArr[0] = modelInstance;
				resArr[1] = extraParticleEffect;

				return resArr;
			}
		}

		public void Release()
		{
			m_RefNum -= 1;

			if (m_RefNum == 0)
			{
				cacheLoaders.Remove(mResName);

                WWWDownLoader.StopLoad(mResName);
                WWWDownLoader.RemoveDownLoad(mResName, null);

				modelPrefab = null;
				request = null;
				if (bundle != null)
				{
					bundle.Unload(true);
					bundle = null;
				}

				m_www = null;
			}
		}

		public static void ReleaseAllModel()
		{
			foreach (ModelLoader loader in cacheLoaders.Values)
			{
				WWWDownLoader.StopLoad(loader.mResName);
				WWWDownLoader.RemoveDownLoad(loader.mResName, null);

				loader.modelPrefab = null;
				loader.request = null;
				if (loader.bundle != null)
				{
					loader.bundle.Unload(true);
					loader.bundle = null;
				}
			}
			cacheLoaders.Clear();
		}
	}
}