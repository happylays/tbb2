using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace LoveDance.Client.Common
{
	public class XQIni
	{
		bool m_bCaseSensitive = false;
		bool m_bXQConvert = false;
		string m_strFlag = "=";
		string m_strIniPath = "";

		Dictionary<string, Dictionary<string, string>> m_AllSetting = new Dictionary<string, Dictionary<string, string>>();
		List<string> m_OriginContent = new List<string>();
		List<string> m_AssistContent = new List<string>();

		public void LoadIni(string strPath, bool bCaseSensitive, bool bXQConvert)
		{
			m_bCaseSensitive = bCaseSensitive;
			m_bXQConvert = bXQConvert;
			m_strIniPath = strPath;

			m_AllSetting.Clear();
			m_OriginContent.Clear();
			m_AssistContent.Clear();
			if (strPath != null && File.Exists(strPath))
			{
				try
				{
					using (StreamReader sr = new StreamReader(strPath, CommonFunc.GetCharsetEncoding()))
					{
						char[] trimStart = { ' ', '\t' };
						char[] trimEnd = { ' ', '\r', '\n', '\t' };

						string strSection = "";
						Dictionary<string, string> sectionSetting = null;

						string strLine = null;
						while ((strLine = sr.ReadLine()) != null)
						{
							if (m_bXQConvert)
							{
								strLine = XQConvert.Decrypt(strLine);
							}

							m_OriginContent.Add(strLine);

							string strAssist = strLine;
							strAssist = strAssist.TrimStart(trimStart);
							strAssist = strAssist.TrimEnd(trimEnd);

							if (_ParseSection(strAssist, ref strSection))
							{
								if (m_AllSetting.ContainsKey(strSection))
								{
									sectionSetting = m_AllSetting[strSection];
								}
								else
								{
									sectionSetting = new Dictionary<string, string>();
									m_AllSetting.Add(strSection, sectionSetting);
								}

								m_AssistContent.Add(_BuildAssistSection(strSection));
							}
							else
							{
								string key = "";
								string value = "";

								if (_ParseSetting(strAssist, ref key, ref value) && sectionSetting != null)
								{
									if (sectionSetting.ContainsKey(key))
									{
										Debug.LogError("key already exist, will be replaced: " + strSection + " -- " + key);
										sectionSetting[key] = value;
									}
									else
									{
										sectionSetting.Add(key, value);
									}

									m_AssistContent.Add(_BuildAssistSetting(key, value));
								}
								else
								{
									m_AssistContent.Add(strAssist);
								}
							}
						}

						sr.Close();
					}
				}
				catch (System.IO.FileNotFoundException e)
				{
					Debug.LogException(e);
					//Debug.LogError( e.ToString() );
				}
				catch (System.IO.DirectoryNotFoundException e)
				{
					Debug.LogException(e);
					//Debug.LogError( e.ToString() );
				}
			}
			else
			{
				StreamWriter sw = File.CreateText(strPath);
				sw.Dispose();
			}
		}

		public int GetInt(string strSection, string strKey, int nDefault)
		{
			int temp = 0;
			if (int.TryParse(GetString(strSection, strKey, nDefault.ToString()), out temp))
				return temp;
			else
			{
				Debug.LogError("TryParse nDefault fail");
				return nDefault;
			}
		}

		public bool SetInt(string strSection, string strKey, int nNewValue)
		{
			return SetString(strSection, strKey, nNewValue.ToString());
		}

		public string GetString(string strSection, string strKey, string strDefault)
		{
			string strResult = strDefault;

			if (!m_bCaseSensitive)
			{
				strSection = strSection.ToUpper();
				strKey = strKey.ToUpper();
			}

			if (m_AllSetting.ContainsKey(strSection))
			{
				Dictionary<string, string> sectionSetting = m_AllSetting[strSection];
				if (sectionSetting.ContainsKey(strKey))
				{
					strResult = sectionSetting[strKey];
				}
				else
				{
					sectionSetting.Add(strKey, strResult);
					string sectionStr = _BuildAssistSection(strSection);
					string keyStr = _BuildAssistSetting(strKey, strResult);
					int index = m_OriginContent.IndexOf(sectionStr);
					if (index < m_OriginContent.Count - 1)
					{
						m_OriginContent.Insert(index + 1, keyStr);
					}
					else
					{
						m_OriginContent.Add(keyStr);
					}

					index = m_AssistContent.IndexOf(sectionStr);
					if (index < m_AssistContent.Count - 1)
					{
						m_AssistContent.Insert(index + 1, keyStr);
					}
					else
					{
						m_AssistContent.Add(keyStr);
					}
				}
			}
			else
			{
				Dictionary<string, string> sectionSetting = new Dictionary<string, string>();
				sectionSetting.Add(strKey, strResult);
				m_AllSetting.Add(strSection, sectionSetting);
				m_OriginContent.Add(_BuildAssistSection(strSection));
				m_OriginContent.Add(_BuildAssistSetting(strKey, strResult));
				m_AssistContent.Add(_BuildAssistSection(strSection));
				m_AssistContent.Add(_BuildAssistSetting(strKey, strResult));

			}

			return strResult;
		}

		public bool SetString(string strSection, string strKey, string strNewValue)
		{
			if (!m_bCaseSensitive)
			{
				strSection = strSection.ToUpper();
				strKey = strKey.ToUpper();
			}

			if (m_AllSetting.ContainsKey(strSection))
			{
				Dictionary<string, string> sectionSetting = m_AllSetting[strSection];
				if (sectionSetting.ContainsKey(strKey))
				{
					string strOldValue = sectionSetting[strKey];
					sectionSetting[strKey] = strNewValue;

					string strAssist = _BuildAssistSetting(strKey, strOldValue);
					int nIndex = m_AssistContent.LastIndexOf(strAssist);
					string strOrigin = m_OriginContent[nIndex];

                    if (string.IsNullOrEmpty(strOldValue))//settingValue 可能为空.写入时候做追加.
                    {
                        m_OriginContent[nIndex] = strOrigin + strNewValue;
                        m_AssistContent[nIndex] = strAssist + strNewValue;
                    }
                    else
                    {
                        m_OriginContent[nIndex] = strOrigin.Replace(strOldValue, strNewValue);
                        m_AssistContent[nIndex] = strAssist.Replace(strOldValue, strNewValue);
                    }

					_RebuildIni();
					return true;
				}
			}

			return false;
		}

		bool _ParseSection(string parseContent, ref string sectionTitle)
		{
			int tmpIndex = parseContent.Length - 1;
			if (parseContent.Length > 0 && parseContent[0] == '[' && parseContent[tmpIndex] == ']')
			{
				sectionTitle = parseContent.Substring(1, parseContent.Length - 2);

				if (!m_bCaseSensitive)
				{
					sectionTitle = sectionTitle.ToUpper();
				}

				return true;
			}

			return false;
		}

		string _BuildAssistSection(string sectionTitle)
		{
			string assistContent = "[" + sectionTitle + "]";
			return assistContent;
		}

		bool _ParseSetting(string parseContent, ref string settingKey, ref string settingValue)
		{
			if (parseContent.Length > 0)
			{
				string[] result = parseContent.Split(m_strFlag.ToCharArray());
				if (result.Length == 2)
				{
					settingKey = result[0];
					settingValue = result[1];

					char[] trimStart = { ' ', '\t' };
					char[] trimEnd = { ' ', '\r', '\n', '\t' };
					settingKey = settingKey.TrimStart(trimStart);
					settingKey = settingKey.TrimEnd(trimEnd);
					settingValue = settingValue.TrimStart(trimStart);
					settingValue = settingValue.TrimEnd(trimEnd);

					if (!m_bCaseSensitive)
					{
						settingKey = settingKey.ToUpper();
					}

					return true;
				}
			}

			return false;
		}

		string _BuildAssistSetting(string settingKey, string settingValue)
		{
			string assistContent = settingKey + m_strFlag + settingValue;
			return assistContent;
		}

		void _RebuildIni()
		{
			if (File.Exists(m_strIniPath))
			{
				using (StreamWriter sw = new StreamWriter(m_strIniPath, false, CommonFunc.GetCharsetEncoding()))
				{
					for (int i = 0; i < m_OriginContent.Count; ++i)
					{
						string temp = (m_bXQConvert ? XQConvert.Encrypt(m_OriginContent[i]) : m_OriginContent[i]);

						if (i != m_OriginContent.Count - 1)
						{
							sw.WriteLine(temp);
						}
						else
						{
							sw.Write(temp);
						}
					}

					sw.Close();
				}
			}
			else
			{
				Debug.LogError("ini file is not exist " + m_strIniPath);
			}
		}
	}
}