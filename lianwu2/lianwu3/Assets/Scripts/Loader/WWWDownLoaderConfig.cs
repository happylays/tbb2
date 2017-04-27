using UnityEngine;
using System.Collections.Generic;
using System;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
    public class DownLoadAssetInfo
    {
        public string m_strName = "";//资源名,作为唯一的key;
        public string m_strMD5 = "";
        public long m_nSize = 0;//资源大小;

        public string m_strFileUrl = "";//资源保存路径;
        public string m_strBelongConfig = "";//所属配置文件,便于保存;

        public Callback<object> m_callBack = null;//回调;
    }

	public enum DownLoadAssetType : byte
    {
        None,
        ConfigVersion,  //资源文件的配置;   
        AssetConfig,    //资源MD5配置;
        Texture,        //图;
        Text,           //文本;
        Audio,          //音乐;
        Movie,          //MovieTexture;
        AssetBundle,    //打包资源;
        MAX
    }

    public class WWWDownLoaderConfig
    {
        /// <summary>
        /// 客户端是否打开动态下载开关;
        /// </summary>
        private static bool m_isDownLoadOpen = false;

        /// <summary>
        /// 资源名 对应 详细信息;
        /// </summary>
        private static Dictionary<string, DownLoadAssetInfo> m_dicDownLoadInfo = new Dictionary<string, DownLoadAssetInfo>();

        /// <summary>
        /// 客户端json,保存在内存,有资源下载成功会重新保存json文本;
        /// </summary>
        private static Dictionary<string,KeyValuePairConfigHelper> m_dicLocalResVerReader = new Dictionary<string,KeyValuePairConfigHelper>();

        /// <summary>
        /// 需要更新的资源;只有在下载的资源引用全部结束的时候,移除此列表内容,下载如果还有资源使用,切换成本地加载;
        /// </summary>
        private static Dictionary<string, string> m_dicUpdateRes = new Dictionary<string, string>();


        /// <summary>
        /// 本地配置是否解析完成;
        /// </summary>
        private static bool m_hasParseClientConfig = false;
        /// <summary>
        /// 服务器资源配置是否解析完成;
        /// </summary>
        private static bool m_hasParseServerConfig = false;

		private static bool m_AndroidAssetConfigLoadEnd = false;
        
		public static bool AndroidAssetConfigLoadEnd
		{
			set
			{
				m_AndroidAssetConfigLoadEnd = value;
			}
		}

        public static bool IsDownLoadOpen
        {
            set
            {
                m_isDownLoadOpen = value;
            }
        }

        public static bool IsVersionConfigInfoReady
        {
            get
            {
                if (!m_isDownLoadOpen)
                {
                    return true;
                }
                return HasParseClientConfig && HasParseServerConfig;
            }
        }

        public static bool HasParseClientConfig
        {
            get
            {
				bool downEnd = false;
                if (!m_isDownLoadOpen)
                {
                    downEnd = true;
                }
				else
				{
					downEnd = m_hasParseClientConfig;
				}

				return downEnd && m_AndroidAssetConfigLoadEnd;
            }
            set
            {
                m_hasParseClientConfig = value;
            }
        }

        public static bool HasParseServerConfig
        {
            get
            {
                if (!m_isDownLoadOpen)
                {
                    return true;
                }
                return m_hasParseServerConfig;
            }
            set
            {
                m_hasParseServerConfig = value;
            }
        }

        /// <summary>
        /// 客户端json存储路径;
        /// </summary>
        private static string m_strClientJsonPath = "";

        /// <summary>
        /// 获取一个资源的信息;
        /// </summary>
        public static DownLoadAssetInfo GetAssetDownLoadInfo(string str)
        {
            if (m_dicDownLoadInfo.ContainsKey(str))
            {
                return m_dicDownLoadInfo[str];
            }
            return null;
        }

        /// <summary>
        /// 检查资源是否需要更新;
        /// </summary>
        /// <returns></returns>
        public static bool CheckResNeedUpdate(string strRes)
        {
            if (!m_isDownLoadOpen)
            {
                return false;
            }
            return m_dicUpdateRes.ContainsKey(strRes);
        }

        /// <summary>
        /// 从需要更新的队列移除;
        /// </summary>
        public static void RemoveNeedUpdateList(string strRes,string strMD5)
        {
            if (m_dicUpdateRes.ContainsKey(strRes))
            {
                if (m_dicUpdateRes[strRes] != null && m_dicUpdateRes[strRes].Equals(strMD5))
                {
                    m_dicUpdateRes.Remove(strRes);
                }
            }
        }

		/// <summary>
		/// 清楚所有需要更新的文件，重新检查备用
		/// </summary>
		public static void ClearResVersion()
		{
			if (m_dicUpdateRes != null)
			{
				m_dicUpdateRes.Clear();
			}
		}

        /// <summary>
        /// 检查是否有资源需要更新;
        /// </summary>
        public static void CheckResVersion()
        {
            if (!m_isDownLoadOpen)
            {
                return;
            }

            foreach (KeyValuePair<string, DownLoadAssetInfo> kv in m_dicDownLoadInfo)
            {
				if (kv.Value == null)
				{
					Debug.LogError("WWWDownLoaderConfig CheckResVersion, kv.Value can not be null. Key : " + kv.Key);
					continue;
				}

                bool hasContains = true;
                foreach (KeyValuePair<string, KeyValuePairConfigHelper> kvLocal in m_dicLocalResVerReader)
                {
					KeyValuePairConfigHelper gtHelper = kvLocal.Value;
					if (gtHelper != null)
					{
						if (gtHelper.CheckHasResVersion(kv.Key))
						{
							string strMD5 = gtHelper.GetNodeMD5(kv.Key);//本地没有配置,返回为空;
							if (kv.Value.m_strMD5.Equals(strMD5))//如果MD5不相同,就是需要更新替换的资源;
							{
								hasContains = false;
							}
						}
					}
					else
					{
						Debug.LogError("WWWDownLoaderConfig CheckResVersion, gtHelper can not be null. " + kv.Key);
					}
                }
                if (hasContains)
                {
					if (!m_dicUpdateRes.ContainsKey(kv.Key))
					{
						m_dicUpdateRes.Add(kv.Key, kv.Value.m_strMD5);
					}
					else
					{
						// 有一种可能触发的情况: CheckResVersion有2次 一次版本文件的Check，一次Res内容的Check.
						// 如果 第一次版本文件的流程结束后,kv.Key没有更新(从Dic中移除),就会出现这个Log.
						// 表示,这个版本文件 没有更新成功。 or 服务器的版本文件中存在这个 版本.json 但是客户端代码里的 json列表里面没有这个文件;
						Debug.LogError("WWWDownLoaderConfig CheckResVersion, m_dicUpdateRes ContainsKey " + kv.Key);
					}
                }
            }
        }

        /// <summary>
        /// 解析客户端资源MD5配置;
        /// </summary>
        static public void ParseServerResourcesConfig(string strBelongConfig, string strData)
        {
            if (string.IsNullOrEmpty(strData))
            {
                Debug.LogError("WWWDownLoaderManage ParseServerResourcesConfig,Config Data can not be null");
                return;
            }

            KeyValuePairConfigHelper kvpHelper = new KeyValuePairConfigHelper();
            kvpHelper.LoadConfig(strData, CommonValue.ResDir + strBelongConfig);

            if(kvpHelper.SelectHeadNode())
            {
                while(true)
                {
                    DownLoadAssetInfo downLoadInfo = new DownLoadAssetInfo();
                    downLoadInfo.m_strName = kvpHelper.GetNodeKey();
					downLoadInfo.m_strMD5 = kvpHelper.GetNodeMD5();
					downLoadInfo.m_nSize = Convert.ToInt64(kvpHelper.GetNodeSize());
                    downLoadInfo.m_strBelongConfig = strBelongConfig;

                    if (m_dicDownLoadInfo.ContainsKey(downLoadInfo.m_strName))
                    {
                        Debug.LogError("WWWDownLoaderManage ParseServerJson,Same Key : " + downLoadInfo.m_strName);
                    }
                    else
                    {
                        m_dicDownLoadInfo.Add(downLoadInfo.m_strName, downLoadInfo);
                    }
                    
                    if(!kvpHelper.SelectNextNode())
                    {
                        break;//结束读取循环;
                    }
                }
            }
        }

        /// <summary>
        /// 解析客户端资源MD5配置;
        /// </summary>
		static public void ParseClientResourcesConfig(string outParentPath, string fileName)
		{
			if (!m_isDownLoadOpen)
			{
				return;
			}

			m_strClientJsonPath = outParentPath + fileName;
            KeyValuePairConfigHelper kvpHelper = new KeyValuePairConfigHelper();
            kvpHelper.LoadConfigFromFile(m_strClientJsonPath);
			if (!m_dicLocalResVerReader.ContainsKey(fileName))
			{
				m_dicLocalResVerReader.Add(fileName, kvpHelper);
			}
			else
			{
				Debug.LogError("WWWDownLoaderConfig ParseClientResourcesConfig, Same key : " + fileName);
			}
		}
        
        /// <summary>
        /// 新下载文件完成,需要保存MD5配置;
        /// </summary>
        static public void SaveNewAssetMD5(string strName, string strMD5, string strBelongConfig)
        {
            KeyValuePairConfigHelper kvpHelper = null;
            if (m_dicLocalResVerReader.ContainsKey(strBelongConfig))
            {
                kvpHelper = m_dicLocalResVerReader[strBelongConfig];
            }
            else
            {
                kvpHelper = new KeyValuePairConfigHelper();
                kvpHelper.LoadConfigFromFile(CommonValue.ClientVerDir + strBelongConfig);
				m_dicLocalResVerReader.Add(strBelongConfig, kvpHelper);
            }
            if (kvpHelper == null)
            {
                Debug.LogError("WWWDownLoaderConfig SaveNewAssetMD5,kvpHelper must be new first!");
                return;
            }

			kvpHelper.SetNodeValue(strName, strMD5, null);
        }

        /// <summary>
        /// 检查下载文件的MD5和服务器配置的MD5是否匹配;
        /// </summary>
        static public bool EqualsFileMD5WithNetMD5(string strURL, string strMD5)
        {
            if (m_dicDownLoadInfo.ContainsKey(strURL))
            {
                if (string.Equals(m_dicDownLoadInfo[strURL].m_strMD5, strMD5))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }

}