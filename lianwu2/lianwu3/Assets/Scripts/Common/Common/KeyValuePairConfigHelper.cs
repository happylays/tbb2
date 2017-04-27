using System.Collections.Generic;
using System.IO;
using System.Text;
using LoveDance.Client.Common;
using UnityEngine;

/// <summary>
/// 自定义的文件读写类;
/// 文件格式：;
///     key:value|value|value,
///     key:value|value|value
///     ...
/// 支持文件读写.;
/// 修改某一个key的value,会直接进行文本写入.;
/// 写入逻辑是seek位置,覆盖的方式写入,必须保证同一个key对应的value区长度相同.;
/// key1:aa|bb|cc,  修改为 key1:cc|dd|ee,  (ok);
/// key1:aa|bb|cc,  修改为 key1:c|d|e,  (no);
/// 
/// 遍历读取使用和xml一样的指针逻辑,文件只有键值对,不存在层级深度概念;
/// 需要SelectHead后可以开始读取内容,每次需要先SelectNext,确保节点存在,才能读取.SelectNext会自行将指针指向下一个节点;
/// </summary>
public class KeyValuePairConfigHelper
{
    /// <summary>
    /// 每一条信息当作一个节点;
    /// </summary>
    public class NodeInfo
    {
        public NodeInfo(string strKey, List<string> listValue)
        {
            m_strKey = strKey;
            m_listData = listValue;
        }

        private string m_strKey = "";//key
        private List<string> m_listData = new List<string>();//value

        private NodeInfo m_lastNode = null;
        private NodeInfo m_nextNode = null;

        public string StrKey
        {
            get
            {
                return m_strKey;
            }
        }

		public string StrMD5
		{
			set
			{
				if (m_listData.Count == 2)
				{
					m_listData[0] = value;
				}
				else
				{
					Debug.LogError("KeyValuePairConfigHelper NodeInfo, Set StrMD5 m_listData.Count != 2");
				}
			}
			get
			{
				if (m_listData.Count == 2)
				{
					return m_listData[0];
				}
				else
				{
					Debug.LogError("KeyValuePairConfigHelper NodeInfo, Get StrMD5 m_listData.Count != 2");
					return null;
				}
			}
		}

		public string StrSize
		{
			set
			{
				if (m_listData.Count == 2)
				{
					m_listData[1] = value;
				}
				else
				{
					Debug.LogError("KeyValuePairConfigHelper NodeInfo, Set StrSize m_listData.Count != 2");
				}
			}
			get
			{
				if (m_listData.Count == 2)
				{
					return m_listData[1];
				}
				else
				{
					Debug.LogError("KeyValuePairConfigHelper NodeInfo, Get StrSize m_listData.Count != 2");
					return null;
				}
			}
		}

        public string StrTrueValue
        {
            get
            {
                string strData = "";
                for (int i = 0; i < m_listData.Count; i++)
                {
                    strData += m_listData[i] + "|";
                }
                if (!string.IsNullOrEmpty(strData))
                {
                    strData = strData.Substring(0, strData.Length - 1);
                }
                return m_strKey + ":" + strData + ",";
            }
        }
        
        public NodeInfo LastNode
        {
            get
            {
                return m_lastNode;
            }
            set
            {
                m_lastNode = value;
            }
        }

        public NodeInfo NextNode
        {
            get
            {
                return m_nextNode;
            }
            set
            {
                m_nextNode = value;
            }
        }
    }

    /// <summary>
    /// 配置文件键值对;
    /// </summary>
    private Dictionary<string, NodeInfo> m_dicNodeInfo = new Dictionary<string, NodeInfo>();

    /// <summary>
    /// 保存路径;
    /// </summary>
    private string m_strFilePath = "";

    //指针节点 队首;
    private NodeInfo m_pBeginNode = null;

    //指针节点 队尾;
    private NodeInfo m_pEndNode = null;

    //临时指针 for 读取;
    private NodeInfo m_pReadNode = null;

	private static readonly object m_objlock = new object();

	/// <summary>
	/// 键值对数量
	/// </summary>
	public int Count
	{
		get
		{
			return m_dicNodeInfo.Count;
		}
	}

    /// <summary>
    /// 指向头节点;
    /// </summary>
    public bool SelectHeadNode()
    {
        if (m_pBeginNode == null)
        {
            return false;
        }
        m_pReadNode = m_pBeginNode;
        return true;
    }

	/// <summary>
	/// 获取 指向 节点的 MD5;
	/// </summary>
	public string GetNodeMD5()
	{
		return GetNodeMD5(m_pReadNode);
	}

	/// <summary>
	/// 获取 指定 节点的 MD5;
	/// </summary>
	public string GetNodeMD5(string strName)
	{
		if (m_dicNodeInfo.ContainsKey(strName))
		{
			return GetNodeMD5(m_dicNodeInfo[strName]);
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeMD5 , m_dicNodeInfo not ContainsKey : " + strName);
		return null;
	}

	private string GetNodeMD5(NodeInfo nodeInfo)
	{
		if (nodeInfo != null)
		{
			return nodeInfo.StrMD5;
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeMD5 , nodeInfo can not be null.");
		return null;
	}


	/// <summary>
	/// 获取 指向 节点的 Size;
	/// </summary>
	public string GetNodeSize()
	{
		return GetNodeSize(m_pReadNode);
	}

	/// <summary>
	/// 获取 指定 节点的 Size;
	/// </summary>
	public string GetNodeSize(string strName)
	{
		if (m_dicNodeInfo.ContainsKey(strName))
		{
			return GetNodeSize(m_dicNodeInfo[strName]);
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeSize , m_dicNodeInfo not ContainsKey : " + strName);
		return null;
	}

	private string GetNodeSize(NodeInfo nodeInfo)
	{
		if (nodeInfo != null)
		{
			return nodeInfo.StrSize;
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeSize , nodeInfo can not be null.");
		return null;
	}

    /// <summary>
    /// 获取 指向 节点的 Key;
    /// </summary>
	public string GetNodeKey()
	{
		return GetNodeKey(m_pReadNode);
	}

	/// <summary>
	/// 获取 指定 节点的 Key;
	/// </summary>
    public string GetNodeKey(string strName)
    {
		if (m_dicNodeInfo.ContainsKey(strName))
        {
			return GetNodeKey(m_dicNodeInfo[strName]);
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeKey , m_dicNodeInfo not ContainsKey : " + strName);
        return null;
    }

	private string GetNodeKey(NodeInfo nodeInfo)
	{
		if (nodeInfo != null)
		{
			return nodeInfo.StrKey;
		}
		Debug.LogError("KeyValuePairConfigHelper GetNodeKey , nodeInfo can not be null.");
		return null;
	}



    /// <summary>
    /// 是否有下一个节点,并且指向;
    /// </summary>
    public bool SelectNextNode()
    {
        m_pReadNode = m_pReadNode.NextNode;//如果next为空,也进行赋值,返回指针是空,是正确逻辑;
        if (m_pReadNode == null)
        {
            return false;
        }
		
        return true;
    }


    /// <summary>
    /// 本地是否存在配置信息;
    /// </summary>
    public bool CheckHasResVersion(string strURL)
    {
        return m_dicNodeInfo.ContainsKey(strURL);
    }

    /// <summary>
    /// 通过唯一key获取对应数据,list长度内部不关心,交给使用者操作;
    /// </summary>
    public NodeInfo GetConfigData(string strName)
    {
		if (m_dicNodeInfo.ContainsKey(strName))
        {
            return m_dicNodeInfo[strName];
        }
        return null;
    }

	/// <summary>
	/// 设置一个资源的MD5码 默认写入文本;
	/// </summary>
	/// <param name="strValue1">支持NULL参数,是为了在修改的时候,可以区分有参数不修改的情况</param>
	/// <param name="strValue2"></param>
	public void SetNodeValue(string strName, string strValue1,string strValue2)
	{
		NodeInfo gtNodeInfo = GetConfigData(strName);	// 重新组合节点的信息;
		if (gtNodeInfo == null) // 如果为空,认为原来没有配置,是新增的配置;
		{
			List<string> listValue = new List<string>();
			if (!string.IsNullOrEmpty(strValue1))
			{
				listValue.Add(strValue1);
			}
			else
			{
				listValue.Add("0");
				Debug.Log("KeyValuePairConfigHelper SetNodeValue , This is a Warning, Sure for set first parameter == null?");
			}

			if (!string.IsNullOrEmpty(strValue2))
			{
				listValue.Add(strValue2);
			}
			else
			{
				listValue.Add("0");
			}

			AddNewNode(strName, listValue);	// 新增内容 组合数据;

			gtNodeInfo = GetConfigData(strName);//重新获取一份 gtNodeInfo for 保存;
		}
		else
		{
			// 修改内容需要保持和原来内容的长度一致;
			if (!string.IsNullOrEmpty(strValue1))
			{
				gtNodeInfo.StrMD5 = strValue1;
			}

			if (!string.IsNullOrEmpty(strValue2))
			{
				gtNodeInfo.StrSize = strValue2;
			}
		}

		if (gtNodeInfo != null)
		{
			SaveNode(gtNodeInfo);
		}
		else
		{
			Debug.LogError("KeyValuePairConfigHelpers SetMD5, gtNodeInfo can not be null.");
		}
	}

    /// <summary>
    /// 从字符串解析;
    /// </summary>
    public void LoadConfig(string strData,string strFilePath)
    {
        m_strFilePath = strFilePath;
        if (!string.IsNullOrEmpty(strData))
        {
            strData = strData.Substring(0, strData.Length - 1);//为了删除字符串结尾的逗号;

            string[] arrStrData = strData.Split(',');//拆分行;
            for (int i = 0; i < arrStrData.Length; i++)
            {
                string[] strKV = arrStrData[i].Split(':');//拆分为键值对;
                if (strKV.Length == 2)
                {
                    string[] gtValues = strKV[1].Split('|');
					if (!AddNewNode(strKV[0], new List<string>(gtValues)))
					{
						string errorLog = string.Format("File : {0},KeyValuePairConfigHelper LoadConfig, duplicate key is :{1}", strFilePath, strKV[0]);
						UnityEngine.Debug.Log(errorLog);
					}
                }
                else
                {
					string errorLog = string.Format("File : {0},KeyValuePairConfigHelper LoadConfig, cur data is error:{1}", strFilePath, arrStrData[i]);
					UnityEngine.Debug.LogException(new System.Exception(errorLog));
                }
            }
        }
        else
        {
            //Debug.LogError("ClientResVerReadHelper LoadConfig,read : " + strFilePath + ", data is null");
        }
    }

    /// <summary>
    /// 从文本读取;
    /// </summary>
    public void LoadConfigFromFile(string strFilePath)
    {
        ClearAllData();

        m_strFilePath = strFilePath;

        if (!File.Exists(strFilePath))
        {
            return;//不存在文件,下面步骤都不需要做;
        }

        string gtStrData = "";
		using (FileStream fs = new FileStream(m_strFilePath, FileMode.Open, FileAccess.Read))
        {
            byte[] gtBytes = new byte[fs.Length];
            fs.Read(gtBytes, 0, (int)fs.Length);

            gtStrData = CommonFunc.GetCharsetEncoding().GetString(gtBytes);

            fs.Close();
        }

        LoadConfig(gtStrData, strFilePath);
    }

    /// <summary>
    /// 添加一个新的信息to dic,内部封装使用;
    /// </summary>
    private bool AddNewNode(string strKey, List<string> listValue)
    {
        NodeInfo lastNode = m_pEndNode;
        m_pEndNode = new NodeInfo(strKey, listValue);
        m_pEndNode.LastNode = lastNode;
        if (lastNode != null)
        {
            lastNode.NextNode = m_pEndNode;//make双向链表关系;
        }
        if (m_pBeginNode == null)
        {
            m_pBeginNode = m_pEndNode;
        }

		if (!m_dicNodeInfo.ContainsKey(strKey))
		{
			m_dicNodeInfo.Add(strKey, m_pEndNode);
		}
		else
		{
			return false;
		}

		return true;
    }
	
    /// <summary>
    /// 删;
    /// </summary>
    public void DeleteNode(string strKey)
    {
        if (m_dicNodeInfo.ContainsKey(strKey))
        {
            NodeInfo lastInfo = m_dicNodeInfo[strKey].LastNode;
            NodeInfo nextInfo = m_dicNodeInfo[strKey].NextNode;
            if (lastInfo != null)
            {
                lastInfo.NextNode = nextInfo;
            }
            if (nextInfo != null)
            {
                nextInfo.LastNode = lastInfo;
            }

            if (m_pBeginNode == m_dicNodeInfo[strKey])
            {
                m_pBeginNode = m_dicNodeInfo[strKey].NextNode;
            }
            if (m_pEndNode == m_dicNodeInfo[strKey])
            {
                m_pEndNode = m_dicNodeInfo[strKey].LastNode;
            }

            m_dicNodeInfo[strKey] = null;
            m_dicNodeInfo.Remove(strKey);
        }

        SaveAll();
    }

    /// <summary>
    /// 清理所有数据;
    /// </summary>
    public void ClearAllData()
    {
        m_dicNodeInfo.Clear();
        m_strFilePath = "";
        m_pBeginNode = null;
        m_pEndNode = null;
    }

    /// <summary>
    /// 保存某个节点信息to文件;
    /// </summary>
	private void SaveNode(NodeInfo nodeInfo)
	{
		if (nodeInfo != null)
		{
			lock (m_objlock)
			{
				FileMode fileMode = FileMode.Open;
				if (!File.Exists(m_strFilePath))
				{
					fileMode = FileMode.Create;

					string strDir = "";
					int nEndPos = m_strFilePath.LastIndexOf("/");
					if (nEndPos != -1)
					{
						strDir = m_strFilePath.Substring(0, nEndPos);
					}

					if (!Directory.Exists(strDir))
					{
						Directory.CreateDirectory(strDir);
					}
				}

				using (FileStream fs = new FileStream(m_strFilePath, fileMode, FileAccess.ReadWrite))
				{
					int nBeginIndex = 0;
					NodeInfo gtLastNode = nodeInfo.LastNode;
					while (gtLastNode != null)
					{
						nBeginIndex += Encoding.UTF8.GetByteCount(gtLastNode.StrTrueValue);
						gtLastNode = gtLastNode.LastNode;
					}

					fs.Seek(nBeginIndex, SeekOrigin.Begin);

					byte[] gtBytes = CommonFunc.GetCharsetEncoding().GetBytes(nodeInfo.StrTrueValue);

					fs.Write(gtBytes, 0, gtBytes.Length);

					fs.Close();
				}
			}
		}
		else
		{
			Debug.LogError("KeyValuePairConfigHelpers ResetMD5ToFile, nodeInfo can not be null.");
		}
	}

    /// <summary>
    /// 保存全部内容;
    /// </summary>
    private void SaveAll()
    {
		lock (m_objlock)
		{
			using (FileStream fs = new FileStream(m_strFilePath, FileMode.Create))
			{
				NodeInfo pNode = m_pBeginNode;
				string strAllData = "";
				while (pNode != null)
				{
					strAllData += pNode.StrTrueValue;
					pNode = pNode.NextNode;
				}

				byte[] gtBytes = CommonFunc.GetCharsetEncoding().GetBytes(strAllData);
				fs.Write(gtBytes, 0, gtBytes.Length);

				fs.Close();
			}
		}
    }
}
