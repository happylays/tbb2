using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Text;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;

namespace LoveDance.Client.Loader
{
	/// <summary>
	/// 新的打包方式
	/// </summary>
	public class AssetBundleConfig
	{
		//<sectionName, <AssetIndex, Asset>[AssetBundleType]>
		Dictionary<string, SortedDictionary<int, List<DependencyAsset>>[]> m_ConfigData = new Dictionary<string, SortedDictionary<int, List<DependencyAsset>>[]>();

        /// <summary>
        /// 同步方式;
        /// </summary>
        public IEnumerator LoadConfigSync(string strOutFilePath, string strInFilePath, string strNetFilePath, string strName)
        {
            strOutFilePath = strOutFilePath + strName + ".txt";
            strInFilePath = strInFilePath + strName + ".txt";
            strName = strName + ".txt";
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
            {
				DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, strNetFilePath + strName, DownLoadAssetType.Text, null, null, DownLoadOrderType.AfterRunning);
				if (downloadPack != null)
				{
					while (!downloadPack.AssetReady)
					{
						yield return null;
					}
					using (StreamReader sr = new StreamReader(new MemoryStream(downloadPack.DataBytes), CommonFunc.GetCharsetEncoding()))
					{
						ParseConfig(sr);
						sr.Close();
					}
				}
				WWWDownLoader.RemoveDownLoad(strName, null);
            }
            else
            {
				IEnumerator itor = LoadConfigFromLocal(strOutFilePath, strInFilePath);
				while(itor.MoveNext())
				{
					yield return null;
				}
            }
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }

		/// <summary>
		/// 加载资源配置文件;
		/// </summary>
		private IEnumerator LoadConfigFromLocal(string strOutFilePath, string strInFilePath)
		{
			string assetWWWPath = CommonFunc.GetCorrectWWWDir(strOutFilePath,strInFilePath);

			WWW www = null;
			using ( www = new WWW(assetWWWPath))
			{
				while (!www.isDone)
				{
					yield return null;
				}

				if (www.error != null)
				{
					Debug.LogError(www.error);
					Debug.LogError("StaticData Load Error! AssetName : " + assetWWWPath);
				}
				else
				{
					using (StreamReader sr = new StreamReader(new MemoryStream(www.bytes), CommonFunc.GetCharsetEncoding()))
					{
						ParseConfig(sr);
						sr.Close();
					}
				}

				www.Dispose();
				www = null;
			}
		}

        /// <summary>
        /// 解析数据;
        /// </summary>
		private void ParseConfig(StreamReader sr)
		{
			string strLine = null;
			char[] trimEnd = { ' ', '\r', '\n', '\t' };

			SortedDictionary<int, List<DependencyAsset>>[] sectionData = null;
			SortedDictionary<int, List<DependencyAsset>> subData = null;

			while ((strLine = sr.ReadLine()) != null)
			{
				strLine = strLine.Trim(trimEnd);

				if (IsSection(ref strLine))
				{
					sectionData = GetSection(strLine);
				}
				else
				{
					string[] contentArr = strLine.Split(';');
					if (contentArr == null)
					{
						continue;
					}

					int contentLength = contentArr.Length;
					for (int i = 0; i < contentLength; ++i)
					{
						string content = contentArr[i];
						if (string.IsNullOrEmpty(content))
						{//去除空字符串
							continue;
						}

						//解析Texture:1=t_texture_1
						string[] allAssetArr = content.Split(':');
						if (allAssetArr == null || allAssetArr.Length != 2)
						{//长度必须为2
							continue;
						}

						AssetBundleType assetType = GetSubType(allAssetArr[0]);
						subData = GetSubData(sectionData, assetType);
						if (subData == null)
						{
							continue;
						}

						//解析1=t_texture_3,2=t_texture_2_a
						string[] assetArr = allAssetArr[1].Split(',');
						if (assetArr == null)
						{
							continue;
						}

						int assetLength = assetArr.Length;
						for (int j = 0; j < assetLength; ++j)
						{
							string asset = assetArr[j];
							if (string.IsNullOrEmpty(asset))
							{
								continue;
							}

							string[] itemArr = asset.Split('=');
							if (itemArr == null || itemArr.Length != 2)
							{
								Debug.LogError("Load assetBundle error," + content);
								continue;
							}

							int assetIndex = 0;
							bool indexFlag = int.TryParse(itemArr[0].Trim(trimEnd), out assetIndex);
							string assetName = itemArr[1].Trim(trimEnd); ;
							if (!indexFlag)
							{
								Debug.LogWarning("Load uiconfig filed warning, Invailde asset index," + assetName);
							}

							DependencyAsset dasset = new DependencyAsset();
							dasset.AssetName = assetName;
							dasset.AssetType = assetType;
							dasset.Depth = assetIndex;

							if (subData.ContainsKey(assetIndex))
							{
								subData[assetIndex].Add(dasset);
							}
							else
							{
								subData.Add(assetIndex, new List<DependencyAsset>() { dasset });
							}
						}
					}
				}
			}
		}

		public void SaveConfig(string strFilePath)
		{
			using (StreamWriter sw = new StreamWriter(strFilePath, false, CommonFunc.GetCharsetEncoding()))
			{
				foreach (KeyValuePair<string, SortedDictionary<int, List<DependencyAsset>>[]> curSection in m_ConfigData)
				{
					string strContent = "[" + curSection.Key + "]";
					sw.WriteLine(strContent);

					StringBuilder sb = new StringBuilder();
					sb.Append("\t");
					for (AssetBundleType s = AssetBundleType.Texture; s < AssetBundleType.Max; ++s)
					{
						SortedDictionary<int, List<DependencyAsset>> textureData = GetSubData(curSection.Value, s);
						if (textureData.Count > 0)
						{
							sb.Append(s + ":");
							foreach (KeyValuePair<int, List<DependencyAsset>> kvp in textureData)
							{
								foreach (DependencyAsset dasset in kvp.Value)
								{
									sb.Append(kvp.Key + "=" + dasset.AssetName);
									sb.Append(",");
								}
							}
							sb.Append(";");
						}
					}
					sw.WriteLine(sb.ToString());
				}

				sw.Close();
			}
		}

		public void ClearSection(string sectionName)
		{
			m_ConfigData.Remove(sectionName);
		}

		public void AddConfig(string sectionName, AssetBundleType subType, string assetName, int assetIndex)
		{
			SortedDictionary<int, List<DependencyAsset>>[] sectionData = GetSection(sectionName);
			if (sectionData != null)
			{
				SortedDictionary<int, List<DependencyAsset>> subData = GetSubData(sectionData, subType);
				if (subData != null)
				{
					DependencyAsset dasset = new DependencyAsset();
					dasset.AssetName = assetName;
					dasset.AssetType = subType;
					dasset.Depth = assetIndex;

					if (subData.ContainsKey(assetIndex))
					{
						subData[assetIndex].Add(dasset);
					}
					else
					{
						subData.Add(assetIndex, new List<DependencyAsset>() { dasset });
					}
				}
			}
		}

		public SortedDictionary<int, List<DependencyAsset>> GetConfig(string sectionName, AssetBundleType subType)
		{
			if (m_ConfigData.ContainsKey(sectionName))
			{
				SortedDictionary<int, List<DependencyAsset>>[] sectionData = m_ConfigData[sectionName];
				if (sectionData != null)
				{
					return GetSubData(sectionData, subType);
				}
			}

			return null;
		}

		public SortedDictionary<int, List<DependencyAsset>> GetConfig(string sectionName)
		{
			SortedDictionary<int, List<DependencyAsset>> map = new SortedDictionary<int, List<DependencyAsset>>();
			for (AssetBundleType arType = AssetBundleType.Texture; arType < AssetBundleType.Max; ++arType)
			{
				SortedDictionary<int, List<DependencyAsset>> subMap = GetConfig(sectionName, arType);
				if (subMap != null)
				{
					foreach (int assetIndex in subMap.Keys)
					{
						List<DependencyAsset> list = subMap[assetIndex];
						if (map.ContainsKey(assetIndex))
						{
							map[assetIndex].AddRange(list);
						}
						else
						{
							map.Add(assetIndex, list);
						}
					}
				}
			}

			return map;
		}

		bool IsSection(ref string curline)
		{
			if (curline.Length > 0 && curline[0] == '[' && curline[curline.Length - 1] == ']')
			{
				curline = curline.Substring(1, curline.Length - 2);
				return true;
			}

			return false;
		}

		AssetBundleType GetSubType(string curline)
		{
			for (AssetBundleType s = AssetBundleType.Texture; s < AssetBundleType.Max; ++s)
			{
				if (curline.StartsWith(s.ToString()))
				{
					return s;
				}
			}

			return AssetBundleType.Max;
		}

		SortedDictionary<int, List<DependencyAsset>>[] GetSection(string sectionName)
		{
			SortedDictionary<int, List<DependencyAsset>>[] sectionData = null;

			if (m_ConfigData.ContainsKey(sectionName))
			{
				sectionData = m_ConfigData[sectionName];
			}
			else
			{
				sectionData = new SortedDictionary<int, List<DependencyAsset>>[(int)AssetBundleType.Max];
				for (AssetBundleType s = AssetBundleType.Texture; s < AssetBundleType.Max; ++s)
				{
					sectionData[(int)s] = new SortedDictionary<int, List<DependencyAsset>>();
				}
				m_ConfigData.Add(sectionName, sectionData);
			}

			return sectionData;
		}

		SortedDictionary<int, List<DependencyAsset>> GetSubData(SortedDictionary<int, List<DependencyAsset>>[] sectionData, AssetBundleType subType)
		{
			if (sectionData != null)
			{
				if (subType >= 0 && (int)subType < sectionData.Length)
				{
					return sectionData[(int)subType];
				}
			}

			return null;
		}
	}
}