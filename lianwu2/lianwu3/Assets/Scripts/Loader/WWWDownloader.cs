using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using LoveDance.Client.Common;
using System.IO;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
	public class DownLoadAudioExData
	{
		public bool m_threeD = false;
		public AudioType m_audioType = AudioType.UNKNOWN;

		public DownLoadAudioExData(bool threeD, AudioType audioType)
		{
			m_threeD = threeD;
			m_audioType = audioType;
		}
	}

	public class DownLoadPack
	{
		public delegate void DownLoadCallBack(DownLoadPack assetPack);
		public int m_AssetRefer = 0;//引用计数;

		private DownLoadCoroutine m_DownLoadCoroutine = null;
		private string m_strUrl = "";
		private DownLoadCallBack m_CallBack = null;
		private DownLoadAssetInfo m_DownLoadAssetInfo = null;
		private DownLoadAssetType m_AssetType = DownLoadAssetType.MAX;
		private object m_exData = null;

		private AssetState m_DownLoadState = AssetState.Waitting;	//资源下载状态

		private byte[] m_dataBytes = null;
		private AssetBundle m_AssetBundle = null;
		private AudioClip m_AudioClip = null;
		private string m_AssetText = null;
		private float m_nProgress = 0;
		private WWW m_AssetWWW = null;
		private bool m_DebugError = false;				// true 错误时输出Log

		public string WWWError { get; private set; }	// WWW异常

		public AssetState State
		{
			get
			{
				return m_DownLoadState;
			}
			set
			{
				m_DownLoadState = value;
			}
		}

		public DownLoadAssetInfo AssetInfo
		{
			get
			{
				return m_DownLoadAssetInfo;
			}
		}

		public DownLoadAssetType AssetType
		{
			get
			{
				return m_AssetType;
			}
		}

		public AssetBundle Bundle
		{
			get
			{
				return m_AssetBundle;
			}
		}

		public byte[] DataBytes
		{
			get
			{
				return m_dataBytes;
			}
		}

		public string AssetText
		{
			get
			{
				return m_AssetText;
			}
		}

		public AudioClip Audio
		{
			get
			{
				return m_AudioClip;
			}
		}

		public float Progress
		{
			get
			{
				return m_nProgress;
			}
		}

		/// <summary>
		/// 创建一个 下载节点;
		/// </summary>
		/// <param name="exData">追加内容,对应类型解;</param>
		public DownLoadPack(string strUrl, DownLoadAssetInfo downLoadInfo, DownLoadAssetType assetType, DownLoadCallBack funCallBack, object exData, bool debugError)
		{
			m_DownLoadAssetInfo = downLoadInfo;
			m_AssetType = assetType;
			m_CallBack = funCallBack;
			m_exData = exData;
			m_strUrl = strUrl;
			m_AssetRefer = 1;
			m_DebugError = debugError;
		}

		public bool AssetReady
		{
			get
			{
				return m_DownLoadState == AssetState.Finish;
			}
		}

		public bool AssetRelease
		{
			set
			{
				if (value)
				{
					if (m_DownLoadState == AssetState.Finish)
					{
						if (m_AssetBundle != null)
						{
							m_AssetBundle.Unload(true);
							m_AssetBundle = null;
						}
						if (m_AudioClip != null)
						{
							Object.Destroy(m_AudioClip);
							m_AudioClip = null;
						}
						m_AssetText = "";
						m_dataBytes = null;
					}
					m_DownLoadState = AssetState.HasRelease;
				}
			}
		}

		public void LoadAsset(DownLoadCoroutine coroutine)
		{
			if (coroutine != null)
			{
				m_DownLoadCoroutine = coroutine;

				coroutine.StartCoroutine(LoadAsset());//开始一个新任务;
			}
		}

		IEnumerator LoadAsset()
		{
			m_DownLoadState = AssetState.DownLoading;

			yield return null;

			using (m_AssetWWW = new WWW(m_strUrl + m_DownLoadAssetInfo.m_strFileUrl))
			{
				while (m_AssetWWW != null && !m_AssetWWW.isDone && string.IsNullOrEmpty(m_AssetWWW.error))
				{
					m_nProgress = m_AssetWWW.progress;
					//没有下载完成;
					yield return null;
				}

				if (m_AssetWWW != null)
				{
					m_nProgress = m_AssetWWW.progress;

					if (m_AssetWWW.error == null)
					{
						m_dataBytes = m_AssetWWW.bytes;
						switch (m_AssetType)
						{
							case DownLoadAssetType.AssetBundle:
								m_AssetBundle = m_AssetWWW.assetBundle;
								break;
							case DownLoadAssetType.ConfigVersion:
								m_AssetText = m_AssetWWW.text;
								break;
							case DownLoadAssetType.AssetConfig:
								m_AssetText = m_AssetWWW.text;
								break;
							case DownLoadAssetType.Text:
								m_AssetText = m_AssetWWW.text;
								break;
							case DownLoadAssetType.Audio:
								DownLoadAudioExData gtAudioData = (DownLoadAudioExData)m_exData;
								m_AudioClip = m_AssetWWW.GetAudioClip(gtAudioData.m_threeD, false, gtAudioData.m_audioType);
								break;
							default:
								break;
						}
					}
					else
					{
						WWWError = "WWWDownLoader Error: " + "" + (m_strUrl + m_DownLoadAssetInfo.m_strFileUrl) + " , " + m_AssetWWW.error;
						if (m_DebugError)
						{
							Debug.LogError(WWWError);
						}
					}

					if (m_DownLoadState == AssetState.HasRelease)
					{
						if (m_AssetBundle != null)
						{
							m_AssetBundle.Unload(true);
							m_AssetBundle = null;
						}
						if (m_AudioClip != null)
						{
							Object.Destroy(m_AudioClip);
							m_AudioClip = null;
						}
						m_AssetText = null;
						m_dataBytes = null;
					}
					else
					{
						m_DownLoadState = AssetState.Finish;
						if (m_CallBack != null)
						{
							m_CallBack(this);//回调;
						}
					}

					m_nProgress = 1;

					m_AssetWWW.Dispose();
					m_AssetWWW = null;
				}
				else
				{
					Debug.LogError("WWWDownLoadere LoadAsset ,assetWWW can not be null " + (m_strUrl + m_DownLoadAssetInfo.m_strFileUrl));
				}
			}
		}

		public void Stop()
		{
			if (m_AssetWWW != null)
			{
				//m_AssetWWW.Dispose();	// 不能用Dispose()，否则会宕机
				m_AssetWWW = null;
			}

			AssetRelease = true;
		}

		/// <summary>
		/// 释放一个在用的Coroutine;
		/// </summary>
		public DownLoadCoroutine ReleaseDownLoadCoroutine()
		{
			if (m_DownLoadCoroutine != null)
			{
				DownLoadCoroutine coroutine = m_DownLoadCoroutine;

				m_DownLoadCoroutine.StopAllCoroutines();//必须立即结束;
				m_DownLoadCoroutine = null;

				return coroutine;
			}

			return null;
		}

	}

	public class WWWDownLoader
	{
		static public void DebugGUI()
		{
			GUI.Label(new Rect(20, 20, 200, 20), "AssetPackMap Count: " + m_AssetPackMap.Count.ToString());
			GUI.Label(new Rect(20, 50, 200, 20), "DownLoadCoroutine Count: " + m_qCoroutineInFree.Count.ToString());

			float y = 80;
			for (int i = 0; i < m_AssetPackNameList.Count; i++)
			{
				GUI.Label(new Rect(20, y, 200, 20), m_AssetPackMap[m_AssetPackNameList[i]].AssetInfo.m_strName + "  " + m_AssetPackMap[m_AssetPackNameList[i]].Progress);
				y += 20;
			}
			
			y = 80;
			for (int i = 0; i < m_StopingAssetPackList.Count; i++)
			{
				GUI.Label(new Rect(220, y, 200, 20), m_StopingAssetPackList[i]);
				y += 20;
			}
		}

		static private List<string> m_AssetPackNameList = new List<string>();//用于记录下载资源顺序;
		static private Dictionary<string, DownLoadPack> m_AssetPackMap = new Dictionary<string, DownLoadPack>();// 下载链接对象;

		static private List<string> m_StopingAssetPackList = new List<string>();//跳过过程中,暂停下载的任务名;
		static private List<string> m_WaittingAssetPackList = new List<string>();//等待下载的内容项(for弹框确认是否下载);

		/// <summary>
		/// 已经下载了的内容量;
		/// </summary>
		static private long m_nDownLoadSize = 0;
		/// <summary>
		/// 下载的服务器url;
		/// </summary>
		static private string m_strWWWDownLoadUrl = "";

		static private long m_MomorySize = 0;	//当前存储空间剩余容量;

		static private bool m_IsShowTipsBox = true;//下载前是否弹框;true=下载前需要弹框确认;
		static private bool m_IgnoreDownLoad = false;//是否可以开始下载;true=跳过下载;

		private static GameObject m_objLoaderCoroutine = null;//Coroutine节点;
		private static Queue<DownLoadCoroutine> m_qCoroutineInFree = new Queue<DownLoadCoroutine>();//Coroutine管理,保存空闲的Coroutine;

		public static bool IsShowTipsBox
		{
			get
			{
				return m_IsShowTipsBox;
			}
			set
			{
				m_IsShowTipsBox = value;
			}
		}

		public static bool IgnoreDownLoad
		{
			get
			{
				return m_IgnoreDownLoad;
			}
			set
			{
				m_IgnoreDownLoad = value;
			}
		}

		/// <summary>
		/// 初始化下载URL;
		/// </summary>
		public static void InitWWWDownLoader(string strUrl, long memorySize)
		{
			m_strWWWDownLoadUrl = strUrl;

			m_MomorySize = memorySize;

			//创建Coroutine节点;
			GameObject gtGroupObj = GameObject.Find("LoaderCoroutineGroup");
			if (gtGroupObj == null)
			{
				gtGroupObj = new GameObject("LoaderCoroutineGroup");
				GameObject.DontDestroyOnLoad(gtGroupObj);
			}
			m_objLoaderCoroutine = new GameObject("WWWDownLoader");//Coroutine节点;
			m_objLoaderCoroutine.transform.parent = gtGroupObj.transform;
			GameObject.DontDestroyOnLoad(m_objLoaderCoroutine);
		}

		/// <summary>
		/// 获取一个空闲Coroutine;
		/// </summary>
		private static DownLoadCoroutine GetDownLoadCoroutine()
		{
			if (m_objLoaderCoroutine == null)
			{
				Debug.LogError("WWWDownLoader GetAssetLoadCoroutine,m_objLoaderCoroutine can not be null.");
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
			
			return gtFreeCoroutine;
		}

		/// <summary>
		/// 释放一个在用的Coroutine;
		/// </summary>
		private static void RecycleDownLoadCoroutine(DownLoadCoroutine downLoadCoroutine)
		{
			if (downLoadCoroutine != null)
			{
				downLoadCoroutine.StopAllCoroutines();//必须立即结束;
				m_qCoroutineInFree.Enqueue(downLoadCoroutine);//放入空闲队列里;
			}
		}

		/// <summary>
		/// 移除一个 Dic资源 中的一个元素;
		/// </summary>
		static public void RemoveDownLoad(string strName, Callback<object> callBack)
		{
			RemoveDownLoad(strName, callBack, false);
		}

		static public void RemoveDownLoad(string strName, Callback<object> callBack, bool isKeepDownLoading)
		{
			if (m_AssetPackMap.ContainsKey(strName))
			{
				DownLoadPack pack = m_AssetPackMap[strName];
				if (pack != null)
				{
					pack.m_AssetRefer--;
					pack.AssetInfo.m_callBack -= callBack;

					if (pack.m_AssetRefer <= 0)//可能会出现小于0的情况,下载到一半停止,资源没有被引用,计数为0可以被删.
					{
						pack.AssetInfo.m_callBack = null;//卸载完成callback一定会为null.之前的 CallBack -= 并不一定能确保CallBack被清空了.

						if (!isKeepDownLoading)
						{
							//不保持下载,正常卸载流程;
							if (pack.State != AssetState.Finish)//为下载完,被卸载,本地没有保存,已下载size不需要增加;
							{
								m_nDownLoadSize -= pack.AssetInfo.m_nSize;
							}

							DownLoadCoroutine freeCoroutine = pack.ReleaseDownLoadCoroutine();
							RecycleDownLoadCoroutine(freeCoroutine);
							m_AssetPackMap.Remove(strName);

							if (m_AssetPackNameList.Contains(strName))
							{
								m_AssetPackNameList.Remove(strName);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 移除一个 list下载队列 中的一个元素;
		/// </summary>
		static private void RemoveAssetPackNameList(string strKey)
		{
			//移出排序队列;
			if (m_AssetPackNameList.Contains(strKey))
			{
				m_AssetPackNameList.Remove(strKey);//移出 下载排序list;
			}
		}

		/// <summary>
		/// 检查是否可以开始下载当前任务;
		/// </summary>
		static private bool CheckCanBeginDownLoad(DownLoadPack dlPack)
		{
			int nRunCount = 0;
			long nRunSize = 0;
			foreach (KeyValuePair<string, DownLoadPack> kv in m_AssetPackMap)
			{
				if (kv.Value.State == AssetState.DownLoading)
				{
					nRunCount++;
					nRunSize += kv.Value.AssetInfo.m_nSize;
				}
			}

			if (nRunCount < CommonDef.DOWNLOAD_MAX_LINK)
			{
				if (dlPack.AssetInfo.m_nSize + m_nDownLoadSize < m_MomorySize)//本机硬盘空间不足;
				{
					if (dlPack.AssetInfo.m_nSize + nRunSize < CommonDef.DOWNLOAD_MAX_SIZE)
					{
						return true;
					}
					else
					{
						Debug.LogError("WWWDownLoader InsertDownLoad, Max Running Size: " + dlPack.AssetInfo.m_strName + ",AssetSize:" + dlPack.AssetInfo.m_nSize + ",RunSize:" + nRunSize);
					}
				}
				else
				{
					Debug.LogError("WWWDownLoader InsertDownLoad, No More SDCard Memory: " + dlPack.AssetInfo.m_strName + ",AssetSize:" + dlPack.AssetInfo.m_nSize + ",DownLoadSize:" + m_nDownLoadSize);
				}
			}
			else
			{
				//在等待中的队列;
				//Debug.Log("WWWDownLoader InsertDownLoad, Max Running Link: " + dlPack.downLoadInfo.m_strName);
			}
			return false;
		}

		static private DownLoadPack AddDownLoadAssetPack(string strName, string strURL, DownLoadAssetType assetType, Callback<object> callBack, object exData, bool debugError)
		{
			DownLoadPack gtAssetPack = null;

			if (m_AssetPackMap.ContainsKey(strName))//如果存在上一个if的情况,此步等同于下载不销毁,重置回调 or 正常流程重复请求;
			{
				gtAssetPack = m_AssetPackMap[strName];
				if (gtAssetPack != null)
				{
					gtAssetPack.AssetInfo.m_callBack += callBack;
					gtAssetPack.m_AssetRefer++;
				}
			}
			else
			{
				DownLoadAssetInfo gtInfo = null;
				if (assetType == DownLoadAssetType.ConfigVersion)//配置文件 特殊处理;
				{
					gtInfo = new DownLoadAssetInfo();
					gtInfo.m_strName = strName;
				}
				else
				{
					gtInfo = WWWDownLoaderConfig.GetAssetDownLoadInfo(strName);
				}

				if (gtInfo == null)
				{
					Debug.LogError("WWWLoader LoadAsset, DownLoadInfo can not be null. \n" + strName);
				}
				else
				{
					gtInfo.m_callBack = callBack;
					gtInfo.m_strFileUrl = strURL;//for组装下载路径 and 保存路径;

					gtAssetPack = new DownLoadPack(m_strWWWDownLoadUrl, gtInfo, assetType, DownLoadFinish, exData, debugError);

					m_AssetPackMap.Add(strName, gtAssetPack);
				}
			}

			return gtAssetPack;
		}

		static public void InsertDownLoadImmediate(string strName, string strURL, DownLoadAssetType assetType, Callback<object> callBack, object exData, bool debugError)
		{
			AddDownLoadAssetPack(strName, strURL, assetType, callBack, exData, debugError);
			if (m_AssetPackNameList.Contains(strName))//重复情况,重新插入队首;
			{
				m_AssetPackNameList.Remove(strName);
				Debug.LogError("WWWDownLoader InsertDownLoadImmediate, Something Error, Has Same :" + strName);
			}
			m_AssetPackNameList.Insert(0, strName);
			StartAll();
		}

		static public DownLoadPack InsertDownLoad(string strName, string strURL, DownLoadAssetType assetType, Callback<object> callBack, object exData, DownLoadOrderType downLoadOrderType)
		{
			return InsertDownLoad(strName, strURL, assetType, callBack, exData, downLoadOrderType, false);
		}

		/// <summary>
		/// 下载任务;
		/// </summary>		
		/// <param name="strName">资源唯一key;</param>
		/// <param name="strURL">资源下载完整URL(包含资源名,由外部组合. 不包含服务器IP地址)</param>
		/// <param name="assetType">资源类型;</param>
		/// <param name="callBack">外部的回调;</param>
		/// <param name="exData">附加信息(音效提取有附加参数);</param>
		static public DownLoadPack InsertDownLoad(string strName, string strURL, DownLoadAssetType assetType, Callback<object> callBack, object exData, DownLoadOrderType downLoadOrderType, bool debugError)
		{
			DownLoadPack pack = null;

			if (!m_IgnoreDownLoad)
			{
				pack = AddDownLoadAssetPack(strName, strURL, assetType, callBack, exData, debugError);

				switch (downLoadOrderType)
				{
					case DownLoadOrderType.Head:
						{
							if (m_AssetPackNameList.Contains(strName))//重复情况,重新插入队首;
							{
								m_AssetPackNameList.Remove(strName);
							}
							m_AssetPackNameList.Insert(0, strName);
						}
						break;
					case DownLoadOrderType.AfterRunning:
						{
							int nIndex = 0; //第一个等待下载的资源索引;
							DownLoadPack curResPack = null;

							for (nIndex = 0; nIndex < m_AssetPackNameList.Count; nIndex++)
							{
								if (m_AssetPackMap.ContainsKey(strName))
								{
									DownLoadPack gtPack = m_AssetPackMap[m_AssetPackNameList[nIndex]];
									if (gtPack == null)
									{
										Debug.LogWarning("WWWDownLoader InsertDownLoadToWaittingBegin, gtPack can not be null.");
										continue;
									}
									else
									{
										//获取第一个等待下载的资源的索引;
										if (gtPack.State == AssetState.Waitting || gtPack.State == AssetState.HasRelease)
										{
											break;
										}

										//从下载队列里面获取目标资源;
										if (gtPack.AssetInfo.m_strName.Equals(strName))
										{
											curResPack = gtPack;
										}
									}
								}
							}

							if (curResPack == null || curResPack.State != AssetState.DownLoading)
							{
								if (m_AssetPackNameList.Contains(strName))
								{
									m_AssetPackNameList.Remove(strName);
								}
								m_AssetPackNameList.Insert(nIndex, strName);
							}
						}
						break;
					default:
						Debug.LogError("WWWDownLoader InsertDownLoad, error name : " + strName + " ,downLoadOrderType: " + downLoadOrderType);
						break;
				}

				if (m_IsShowTipsBox)
				{
					m_WaittingAssetPackList.Add(strName);
					Messenger<Callback, Callback>.Broadcast(MessangerEventDef.DOWNLOAD_MOBILE_TIPSBOX, TipBoxCallBackYes, TipBoxCallBackNo);
				}
				else
				{
					StartAll();
				}
			}

			return pack;
		}

		/// <summary>
		/// 弹出框点击确定的回调;
		/// </summary>
		static private void TipBoxCallBackYes()
		{
			m_WaittingAssetPackList.Clear();

			StartAll();
			m_IgnoreDownLoad = false;
			m_IsShowTipsBox = false;
		}

		static private void TipBoxCallBackNo()
		{
			for (int i = 0; i < m_WaittingAssetPackList.Count; i++)
			{
				string assetName = m_WaittingAssetPackList[i];
				if (m_AssetPackMap.ContainsKey(assetName))
				{
					m_AssetPackMap[assetName].State = AssetState.Finish;
				}
			}
			m_WaittingAssetPackList.Clear();

			m_IgnoreDownLoad = true;
			m_IsShowTipsBox = false;
		}

		/// <summary>
		/// 停止某个链接下载;
		/// </summary>
		static public void StopLoad(string strName)
		{
			if (m_AssetPackMap.ContainsKey(strName))
			{
				DownLoadPack pack = m_AssetPackMap[strName];
				if(pack != null)
				{
					DownLoadCoroutine freeCoroutine = pack.ReleaseDownLoadCoroutine();
					RecycleDownLoadCoroutine(freeCoroutine);
					pack.Stop();
				}
			}
		}

		/// <summary>
		/// 开始未下载的任务;
		/// </summary>
		static private void StartAll()
		{
			int nRunCount = 0;
			long nRunSize = 0;
			for (int i = 0; i < m_AssetPackNameList.Count; i++)
			{
				string assetName = m_AssetPackNameList[i];
				if (m_AssetPackMap.ContainsKey(assetName))
				{
					DownLoadPack gtPack = m_AssetPackMap[assetName];
					if (gtPack == null)
					{
						Debug.LogWarning("WWWDownLoader StartAll, gtPack can not be null.");
					}
					else
					{
						nRunCount++;
						nRunSize += gtPack.AssetInfo.m_nSize;

						if (nRunCount > CommonDef.DOWNLOAD_MAX_LINK)//正常的链接限制控制;
						{
							StopLoad(assetName);
							//Debug.LogError("WWWDownLoader StartAll, Max Link: " + gtPack.AssetInfo.m_strName + ",nRunCount:" + nRunCount);
						}
						else if (gtPack.AssetInfo.m_nSize + nRunSize >= CommonDef.DOWNLOAD_MAX_SIZE)//正常的链接限制控制;
						{
							StopLoad(assetName);
							//Debug.LogError("WWWDownLoader InsertDownLoad, Max Running Size: " + gtPack.AssetInfo.m_strName + ",AssetSize:" + gtPack.AssetInfo.m_nSize + ",RunSize:" + nRunSize);
						}
						else if (gtPack.AssetInfo.m_nSize + m_nDownLoadSize >= m_MomorySize)//本机硬盘空间不足,属于异常;
						{
							StopLoad(assetName);
							Debug.LogError("WWWDownLoader InsertDownLoad, No More SDCard Memory: " + gtPack.AssetInfo.m_strName + ",AssetSize:" + gtPack.AssetInfo.m_nSize + ",DownLoadSize:" + m_nDownLoadSize);
						}
						else
						{
							if (gtPack.State == AssetState.Waitting || gtPack.State == AssetState.HasRelease)//取停止状态下的任务,重新下载;
							{
								gtPack.LoadAsset(GetDownLoadCoroutine());
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 开始所有未完成任务;
		/// </summary>
		static public void ReStartAll()
		{
			//加回等待队列;
			m_AssetPackNameList.AddRange(m_StopingAssetPackList);
			m_StopingAssetPackList.Clear();
			StartAll();
		}

		static public void StopAll()
		{
			//任务加入停止队列;
			for (int i = 0; i < m_AssetPackNameList.Count; i++)
			{
				string assetName = m_AssetPackNameList[i];
				if (m_AssetPackMap.ContainsKey(assetName))
				{
					DownLoadPack gtPack = m_AssetPackMap[assetName];
					if (gtPack != null)
					{
						if (gtPack.State == AssetState.Waitting || gtPack.State == AssetState.HasRelease)//取停止状态下的任务;
						{
							m_StopingAssetPackList.Add(assetName);
						}
					}
				}
			}
			//移除等待队列;
			for ( int i = 0 ; i < m_StopingAssetPackList.Count ; i++ )
			{
				m_AssetPackNameList.Remove(m_StopingAssetPackList[i]);
			}
		}

		/// <summary>
		/// 下载某个资源完成,回调;
		/// </summary>
		static private void DownLoadFinish(DownLoadPack assetPack)
		{
			if (!assetPack.AssetReady)
			{
				Debug.LogError("WWWDownLoader DownLoadFinish,Asset is not Ready.");
				return;
			}

			string strName = assetPack.AssetInfo.m_strName;
			string strMD5 = XQMD5.GetByteMd5String(assetPack.DataBytes);
			if (assetPack.AssetType != DownLoadAssetType.ConfigVersion)
			{
				if (assetPack.DataBytes != null)
				{
					//检查服务器MD5和资源是否匹配;
					if (WWWDownLoaderConfig.EqualsFileMD5WithNetMD5(strName, strMD5))
					{
						string strSavePath = CommonValue.ResDir + assetPack.AssetInfo.m_strFileUrl;
						SaveFile(strSavePath, assetPack.DataBytes);

						WWWDownLoaderConfig.SaveNewAssetMD5(strName, strMD5, assetPack.AssetInfo.m_strBelongConfig);
					}
					else
					{
						//配置文件 不存在MD5码;
					}
				}
				else
				{
					Debug.LogError("WWWDownLoader DownLoadFinish,DownLoad fail: " + strName);
				}
			}

			//当前下载的总量;
			m_nDownLoadSize += assetPack.AssetInfo.m_nSize;

			WWWDownLoaderConfig.RemoveNeedUpdateList(strName, strMD5);//从未下载的资源列表 移除;
			RemoveAssetPackNameList(strName);//从下载队列移除;

			if (m_AssetPackMap[strName].m_AssetRefer > 0)//外部在等待这个资源;
			{
				//回调通知;
				if (assetPack.AssetInfo.m_callBack != null)
				{
					assetPack.AssetInfo.m_callBack((object)assetPack);
					assetPack.AssetInfo.m_callBack = null;
				}
			}
			else
			{
				//是keepLoading的资源,在下载完成之后,还没有外部引用,是无用的资源,释放;
				RemoveDownLoad(strName, assetPack.AssetInfo.m_callBack);
				assetPack.Stop();
			}


			//结束下载后 继续队列的下一个元素下载;
			StartAll();
		}

		/// <summary>
		/// 生成本地文件;
		/// </summary>
		static private void SaveFile(string strFileName, byte[] bytes)
		{
			int nLastPos = strFileName.LastIndexOf('/');
			string strDirectory = strFileName.Substring(0, nLastPos);

			if (!Directory.Exists(strDirectory))
			{
				Directory.CreateDirectory(strDirectory);
			}

			using (FileStream fileStream = new FileStream(strFileName, FileMode.Create))
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}
}