using System.IO;
using LoveDance.Client.Common;
using System.Collections.Generic;

namespace LoveDance.Client.Data.Tips
{
	public class SystemTips
	{
		static Dictionary<string, string> s_SysTipDic = new Dictionary<string, string>();

		public static string GetTipsContent(string id)
		{
			if (s_SysTipDic.ContainsKey(id))
			{
				return s_SysTipDic[id];
			}
			return id;
		}

		public static string GetTipsContent(string id, string[] arParam)
		{
			string tipsContent = GetTipsContent(id);

			if (arParam != null)
			{
				for (int i = 1; i < arParam.Length; ++i)
				{
					if (arParam[i] != null)
					{
						string flag = "{#" + i.ToString() + "}";
						tipsContent = tipsContent.Replace(flag, arParam[i]);
					}
				}
			}
			return tipsContent;
		}

		/// <summary>
		/// ½âÎö SystemTips;
		/// </summary>
		public static void ParseSysTip(Stream sysTipStream)
		{
			using (StreamReader sr = new StreamReader(sysTipStream, CommonFunc.GetCharsetEncoding()))
			{
				char[] trimStart = { ' ', '\t' };
				char[] trimEnd = { ' ', '\r', '\n', '\t' };

				string strLine = null;
				while ((strLine = sr.ReadLine()) != null)
				{
					strLine = strLine.TrimEnd(trimEnd);
					strLine = strLine.TrimStart(trimStart);

					if (strLine.Length > 0)
					{
						int equalIndex = strLine.IndexOf("=");
						if (equalIndex != -1)
						{
							string strKey = strLine.Substring(0, equalIndex);
							strKey = strKey.TrimEnd(trimEnd);
							strKey = strKey.TrimStart(trimStart);

							string strValue = strLine.Substring(equalIndex + 1);
							strValue = strValue.TrimEnd(trimEnd);
							strValue = strValue.TrimStart(trimStart);

							if (!s_SysTipDic.ContainsKey(strKey))
							{
								strValue = strValue.Replace("{#n}", "\n");
								s_SysTipDic.Add(strKey, strValue);
							}
						}
					}
				}
				sr.Close();
			}
		}
	}
}
