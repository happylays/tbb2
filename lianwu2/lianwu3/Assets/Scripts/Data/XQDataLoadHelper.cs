using LoveDance.Client.Common;
using System.Collections.Generic;

namespace LoveDance.Client.Data
{
	public class XQDataLoadHelper
	{
		/// <summary>
		/// 用于从文件中读取“键”在“值”中无引用的字典
		/// </summary>
		/// <typeparam name="TKey">键</typeparam>
		/// <typeparam name="TValue">值</typeparam>
		/// <param name="outputDic">输出字典，可以为空</param>
		/// <param name="xqFile">二进制文件</param>
		/// <param name="keyLoader">键读取方法</param>
		/// <param name="valueLoader">值读取方法</param>
		/// <param name="needClear">true 需要清除原有Dictionary的内容</param>
		/// <returns>true 读取成功</returns>
		public static bool LoadToDic<TKey, TValue>(ref Dictionary<TKey, TValue> outputDic, XQFileStream xqFile, CallbackReturn<TKey, XQFileStream> keyLoader, CallbackReturn<TValue, XQFileStream> valueLoader, bool needClear)
		{
			bool res = false;

			do
			{
				if (outputDic == null)
				{
					outputDic = new Dictionary<TKey, TValue>();
				}
				else if (needClear)
				{
					outputDic.Clear();
				}

				if (xqFile == null || !xqFile.IsOpen())
				{
					break;
				}

				if (keyLoader == null || valueLoader == null)
				{
					break;
				}

				TKey key = default(TKey);
				TValue value = default(TValue);
				ushort count = xqFile.ReadUShort();
				for (ushort i = 0; i < count; i++)
				{
					key = keyLoader(xqFile);
					value = valueLoader(xqFile);
					if (outputDic.ContainsKey(key))
					{
						continue;
					}

					outputDic.Add(key, value);
				}

				res = true;
			} while (false);

			return res;
		}

		/// <summary>
		/// 从文件中读取一个byte类型的值
		/// </summary>
		/// <param name="xqFile">二进制文件</param>
		/// <returns>值</returns>
		public static byte ReadByte(XQFileStream xqFile)
		{
			return xqFile == null && xqFile.IsOpen() ? byte.MinValue : xqFile.ReadByte();
		}

		/// <summary>
		/// 从文件中读取一个uint类型的值
		/// </summary>
		/// <param name="xqFile">二进制文件</param>
		/// <returns>值</returns>
		public static uint ReadUInt(XQFileStream xqFile)
		{
			return xqFile == null && xqFile.IsOpen() ? uint.MinValue : xqFile.ReadUInt();
		}
	}
}