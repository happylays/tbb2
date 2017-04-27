using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
    public class AssetLoader
    {                        
		class AssetPack
		{
            public DownLoadCoroutine m_loadCoroutine = null;//加载下载用Coroutine;
            public Callback<string> m_CallBack = null;
			public int m_nKeepLoadingRefer = 0;//外部KeepLoading的计数;

			AssetState m_AssetState = AssetState.Waitting;

			internal string m_AssetName = "";
			internal bool m_IsResident = false;
			internal int m_AssetRefer = 0;
			internal WWW m_AssetWWW = null;
			internal AssetBundle m_AssetBundle = null;
			internal Object m_AssetObject = null;

			internal AssetPack(string assetName, bool bResident,bool isKeepLoading)
			{
				m_AssetName = assetName;
				m_IsResident = bResident;
                m_AssetRefer = (bResident ? 0 : 1);
                
                if (!m_IsResident)
                {
                    if (isKeepLoading)//如果外部设置为KeepLoading,那么资源会切换为KeepLoading状态;
                    {
                        m_nKeepLoadingRefer++;
                    }
                }
			}

			internal bool AssetReady
			{
				get
				{
					if (m_AssetWWW != null && m_AssetWWW.isDone && m_AssetWWW.error == null)
					{
						m_AssetBundle = m_AssetWWW.assetBundle;

						m_AssetState = AssetState.Finish;
						m_AssetWWW.Dispose();
						m_AssetWWW = null;

						if (m_AssetState == AssetState.HasRelease)
						{
							Debug.Log("Asset set to be released before ready, check logic if not on purpose");
#if UNITY_4_0
							m_AssetObject = null;
							UnloadAssetBundle(true);
#else
							UnlaodAssetObject();
							UnloadAssetBundle(true);
#endif
							
							m_AssetRefer = (m_IsResident ? 0 : 1);
						}
					}

					return m_AssetState == AssetState.Finish;
				}
			}

			internal bool AssetRelease
			{
				set
				{
					if (value && m_AssetState == AssetState.Finish)
					{
						if (CommonValue.PhoneOS == Phone_OS.Ios)
						{
							UnlaodAssetObject();
							UnloadAssetBundle(true);
						}
						else
						{
							m_AssetObject = null;
							UnloadAssetBundle(true);
						}

						m_AssetState = AssetState.HasRelease;
					}
				}
			}

			internal AssetState State
			{
				get
				{
					return m_AssetState;
				}
				set
				{
					m_AssetState = value;
				}
			}

			internal IEnumerator LoadAsset(string assetWWWDir, string assetDir, string inAssetWWWDir, string assetExtension)
			{
				m_AssetState = AssetState.LocalLoading;

				string assetWWWPath = assetWWWDir + m_AssetName + assetExtension;
				string assetPath = assetDir + m_AssetName + assetExtension;
				if (!File.Exists(assetPath))
				{
					assetWWWPath = inAssetWWWDir + m_AssetName + assetExtension;
				}

				using (m_AssetWWW = new WWW(assetWWWPath))
				{
					//m_AssetWWW will be null when the www is done exactly in checking the attribute [AssetReady].
					while (m_AssetWWW != null && !m_AssetWWW.isDone)
					{
						yield return null;
					}

					if (m_AssetWWW != null)
					{
						if (m_AssetWWW.error == null)
						{
							m_AssetBundle = m_AssetWWW.assetBundle;
						}
						else
						{
							Debug.LogError("asset load error: " + assetWWWPath + " -- " + m_AssetWWW.error);
						}

						if (m_AssetState == AssetState.HasRelease)
						{
							Debug.Log("Asset set to be released before ready, check logic if not on purpose." + assetWWWPath);
#if UNITY_4_0
							m_AssetObject = null;
							UnloadAssetBundle(true);
#else
							UnlaodAssetObject();
							UnloadAssetBundle(true);
#endif
						}
						else
						{
							m_AssetState = AssetState.Finish;
						}

						m_AssetWWW.Dispose();
						m_AssetWWW = null;
					}
				}
			}

			internal void UnloadAssetBundle(bool unloadFlag)
			{
				if (m_AssetBundle != null)
				{
					m_AssetBundle.Unload(unloadFlag);
					m_AssetBundle = null;
				}
			}

			internal void UnlaodAssetObject()
			{
				if (m_AssetObject != null)
				{
					if (!(m_AssetObject is GameObject || m_AssetObject is Transform))
					{
						Resources.UnloadAsset(m_AssetObject);
					}

					m_AssetObject = null;
				}
			}
		}

        /// <summary>
        /// 加载信息节点;
        /// </summary>
        class AssetLoadInfo
        {
            public int m_nRefer = 1;//加载任务计数;
            public List<Callback<string>> m_listCallBack = new List<Callback<string>>();//各个界面回调;
            public DownLoadCoroutine m_loadCoroutine = null;//加载下载用Coroutine;
        }

		string m_strInAssetWWWDir = "";
		string m_strAssetDir = "";
		string m_strAssetWWWDir = "";
		string m_strAssetExtension = "";
        string m_strAssetNetDir = "";
        
		Dictionary<string, AssetPack> m_AllAssets = new Dictionary<string, AssetPack>();

        private GameObject m_objLoaderCoroutine = null;//Coroutine节点;
        private Queue<DownLoadCoroutine> m_qCoroutineInFree = new Queue<DownLoadCoroutine>();//Coroutine管理,保存空闲的Coroutine;

        /// <summary>
        /// 获取一个空闲Coroutine;
        /// </summary>
        private DownLoadCoroutine GetAssetLoadCoroutine(string strKey)
        {
            if (!m_AllAssets.ContainsKey(strKey))
            {
                Debug.LogWarning("DownLoadControl GetDownLoadCoroutine, Don't Has Key: " + strKey+" ??");
                return null;
            }
            if (m_AllAssets[strKey].m_loadCoroutine != null)
            {
                Debug.LogWarning("DownLoadControl GetDownLoadCoroutine, Already has Coroutine : " + strKey);
                return null;
            }

            DownLoadCoroutine gtFreeCoroutine = null;
            if (m_qCoroutineInFree.Count != 0)
            {
                gtFreeCoroutine = m_qCoroutineInFree.Dequeue();//踢出队列;
            }
            if (gtFreeCoroutine == null)
            {
                gtFreeCoroutine = m_objLoaderCoroutine.AddComponent<DownLoadCoroutine>();
            }
            m_AllAssets[strKey].m_loadCoroutine = gtFreeCoroutine;

            return gtFreeCoroutine;
        }
        
        /// <summary>
        /// 释放一个在用的Coroutine;
        /// </summary>
        private void ReleaseDownLoadCoroutine(string strKey)
        {
            if (m_AllAssets.ContainsKey(strKey))
            {
                DownLoadCoroutine downLoadCoroutine = m_AllAssets[strKey].m_loadCoroutine;
                m_AllAssets[strKey].m_loadCoroutine = null;

                if (downLoadCoroutine != null)
                {
                    downLoadCoroutine.StopAllCoroutines();//必须立即结束;
					m_qCoroutineInFree.Enqueue(downLoadCoroutine);//放入空闲队列里;
                }
            }
        }
    
		public void InitLoader(string strLoaderName,string assetWWWDir, string assetDir, string inAssetWWWDir, string inAssetDir, string netAssetDir,string assetExtension)
		{
			//m_strInAssetDir = inAssetDir;
			m_strInAssetWWWDir = inAssetWWWDir;
			m_strAssetDir = assetDir;
			m_strAssetWWWDir = assetWWWDir;
            m_strAssetNetDir = netAssetDir;
			m_strAssetExtension = assetExtension;

            //创建Coroutine节点;
            GameObject gtGroupObj = GameObject.Find("LoaderCoroutineGroup");
            if (gtGroupObj == null)
            {
                gtGroupObj = new GameObject("LoaderCoroutineGroup");
                GameObject.DontDestroyOnLoad(gtGroupObj);
            }
            m_objLoaderCoroutine = new GameObject(strLoaderName);//Coroutine节点;
            m_objLoaderCoroutine.transform.parent = gtGroupObj.transform;
            GameObject.DontDestroyOnLoad(m_objLoaderCoroutine);
		}
        		
        /// <summary>
        /// 对外接口异步下载;
        /// </summary>
        public void LoadAssetAsync(string assetName, bool bResident, Callback<string> callBack, bool isOnlyDownLoad ,DownLoadOrderType downLoadOrderType, bool isKeepLoading)
        {
			LoadAsset(assetName, bResident, callBack, isOnlyDownLoad, downLoadOrderType, isKeepLoading);
        }

        /// <summary>
        /// 对外接口同步下载;
        /// </summary>
		public IEnumerator LoadAssetSync(string assetName, bool bResident, bool isOnlyDownLoad, DownLoadOrderType downLoadOrderType, bool isKeepLoading)
        {
			LoadAsset(assetName, bResident, null, isOnlyDownLoad, downLoadOrderType, isKeepLoading);

            while (!ChechLoadReady(assetName))
            {
                yield return null;
            }

            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }

        private bool ChechLoadReady(string assetName)
        {
            if (m_AllAssets.ContainsKey(assetName))
            {
                AssetPack assetPack = m_AllAssets[assetName];
                return assetPack.AssetReady;
            }
            return true;
        }

        /// <summary>
        /// 内部下载接口;
        /// </summary>
        private void LoadAsset(string assetName, bool bResident, Callback<string> callBack, bool isOnlyDownLoad, DownLoadOrderType downLoadOrderType,bool isKeepLoading)
        {
            AssetPack assetPack = null;
            if (m_AllAssets.ContainsKey(assetName))
            {
                assetPack = m_AllAssets[assetName];
                if (!assetPack.m_IsResident)
                {
                    if (bResident)
                    {
                        assetPack.m_IsResident = true;
                        assetPack.m_AssetRefer = 0;
                    }
                    else
                    {
						if (isKeepLoading)//如果外部设置为KeepLoading,那么资源会切换为KeepLoading状态;
	                    {
	                        assetPack.m_nKeepLoadingRefer++;
	                    }
                        assetPack.m_AssetRefer++;
                    }
                }
                assetPack.m_CallBack += callBack;
                
                if(assetPack.AssetReady)
                {
                    if (callBack != null)
                    {
                        callBack(assetName);
                    }
                }
            }
            else
            {
                assetPack = new AssetPack(assetName, bResident, isKeepLoading);
                m_AllAssets.Add(assetName, assetPack);
                assetPack.m_CallBack += callBack;

                DownLoadCoroutine gtDLC = GetAssetLoadCoroutine(assetName);
                if (gtDLC != null)
                {
                    gtDLC.StartCoroutine(LoadingAsset(assetName, isOnlyDownLoad, downLoadOrderType));
                }
                else
                {
                    Debug.LogError("AssetLoader LoadAssetAsync,Coroutine Obj can not be null.");
                }
            }
        }

		private IEnumerator LoadingAsset(string assetName, bool isOnlyDownLoad, DownLoadOrderType downLoadOrderType)
		{
			if (m_AllAssets.ContainsKey(assetName))
			{
				AssetPack assetPack = m_AllAssets[assetName];

				string strNetLoadName = assetName + m_strAssetExtension;
				if (WWWDownLoaderConfig.CheckResNeedUpdate(strNetLoadName))//检查版本,区分是加载or下载;
				{
					DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strNetLoadName, m_strAssetNetDir + strNetLoadName, DownLoadAssetType.AssetBundle, null, null, downLoadOrderType);
					if (downloadPack != null)
					{
						while (!downloadPack.AssetReady)
						{
							assetPack.State = downloadPack.State;
							yield return null;
						}

						//资源交给AssetLoader;结束后删除wwwdownloader的任务;
						assetPack.m_AssetBundle = downloadPack.Bundle;
						assetPack.State = downloadPack.State;
					}
					else
					{
						assetPack.State = AssetState.Finish;	//临时处理，若不下载直接标记该资源已经被释放
					}
					
					WWWDownLoader.RemoveDownLoad(strNetLoadName, null);

					if (isOnlyDownLoad)//如果预下载流程;
					{
						assetPack.UnloadAssetBundle(true);//only下载,下载完卸载资源,结束后删除wwwdownloader的任务;
					}
				}
				else
				{
					if (!isOnlyDownLoad)
					{
						IEnumerator itor = assetPack.LoadAsset(m_strAssetWWWDir, m_strAssetDir, m_strInAssetWWWDir, m_strAssetExtension);
						while (itor.MoveNext())
						{
							yield return null;
						}
					}
					else
					{
						assetPack.State = AssetState.Finish;
					}
				}

				if (assetPack.m_CallBack != null)
				{
					assetPack.m_CallBack(assetName);
				}

				//下载完后判断是不是需要自主卸载,keepLoading的情况;
				assetPack.m_nKeepLoadingRefer = 0;
				if (!assetPack.m_IsResident && assetPack.m_AssetRefer <= 0 && assetPack.m_nKeepLoadingRefer <= 0)//非常驻资源,没有外部使用,并且下载完成的情况下;
				{
					ReleaseAsset(assetName, null, false);
				}
				else
				{
					ReleaseDownLoadCoroutine(assetName);
				}
			}
			else
			{
				Debug.LogError("AssetLoader LoadingAsset, m_AllAssets dont has key : " + assetName);
			}
		}
		
        /// <summary>
        /// 卸载资源;
        /// </summary>
		public void ReleaseAsset(string assetName, Callback<string> callBack, bool isKeepLoading)
		{
			if (m_AllAssets.ContainsKey(assetName))
			{
				AssetPack assetPack = m_AllAssets[assetName];
				if (assetPack != null)
				{
					if (assetPack.m_CallBack != null)
					{
						assetPack.m_CallBack -= callBack;
					}
					if (!assetPack.m_IsResident)
					{
						assetPack.m_AssetRefer--;

						if (isKeepLoading)
						{
							if (assetPack.State != AssetState.DownLoading && assetPack.State != AssetState.LocalLoading)//KeepLoading的任务, 只有在加载完成后,才能进行卸载;
							{
								assetPack.m_nKeepLoadingRefer--;
							}
						}
						if (assetPack.m_AssetRefer <= 0 && assetPack.m_nKeepLoadingRefer <= 0)//非常驻资源,没有外部使用,并且下载完成的情况下;
						{
							assetPack.m_CallBack = null;
							WWWDownLoader.StopLoad(assetName + m_strAssetExtension);
							WWWDownLoader.RemoveDownLoad(assetName + m_strAssetExtension, null);//AssetLoader内部的 www调用逻辑上来说没有回调.函数内部有非空判断;     

							ReleaseDownLoadCoroutine(assetName);//立即停止Coroutine;
							assetPack.AssetRelease = true;
							m_AllAssets.Remove(assetName);
						}
					}
				}
				else
				{
					Debug.LogError("AssetLoader ReleaseAsset,AssetPack can not be null. " + assetName);
				}
			}
		}

        /// <summary>
        /// 卸载所有资源;
        /// </summary>
		public void ReleaseAllAsset()
		{
			List<string> delAsset = new List<string>();
			foreach (KeyValuePair<string, AssetPack> kvp in m_AllAssets)
			{
				if (!kvp.Value.m_IsResident)//fufeng ni lao na yang? 
				{
					delAsset.Add(kvp.Key);
				}
			}

			foreach (string assetName in delAsset)
			{
				ReleaseDownLoadCoroutine(assetName);//立即停止Coroutine;
				WWWDownLoader.StopLoad(assetName + m_strAssetExtension);
				WWWDownLoader.RemoveDownLoad(assetName + m_strAssetExtension, null);//AssetLoader内部的 www调用逻辑上来说没有回调.函数内部有非空判断;

				AssetPack assetPack = m_AllAssets[assetName];
				m_AllAssets.Remove(assetName);

				assetPack.m_AssetRefer--;
				assetPack.AssetRelease = true;

				if (assetPack.m_AssetRefer > 0)
				{
					Debug.LogError("release asset still used: " + assetPack.m_AssetName);
				}

				assetPack = null;
			}

			delAsset.Clear();
		}
        
        /// <summary>
        /// 获取下载内容的资源;
        /// </summary>
		public Object GetMainAsset(string assetName)
		{
			if (m_AllAssets.ContainsKey(assetName))
			{
				AssetPack assetPack = m_AllAssets[assetName];
				if (assetPack.AssetReady)
				{
					if (assetPack.m_AssetObject == null && assetPack.m_AssetBundle != null)
					{
						assetPack.m_AssetObject = assetPack.m_AssetBundle.mainAsset;
					}

					return assetPack.m_AssetObject;
				}
			}

			return null;
		}

        /// <summary>
        /// 获取下载内容的资源;
        /// </summary>
        public AssetBundle GetMainAssetBundle(string assetName)
        {
            if (m_AllAssets.ContainsKey(assetName))
            {
                AssetPack assetPack = m_AllAssets[assetName];
                if (assetPack.AssetReady)
                {
                    return assetPack.m_AssetBundle;
                }
            }

            return null;
        }

		public void UnloadAssetBundle(string assetName)
		{
			if (m_AllAssets.ContainsKey(assetName))
			{
				AssetPack assetPack = m_AllAssets[assetName];
				if (assetPack.AssetReady)
				{
					assetPack.UnloadAssetBundle(false);
				}
			}
		}
	}
}