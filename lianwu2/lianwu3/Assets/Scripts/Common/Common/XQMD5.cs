using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace LoveDance.Client.Common
{
	public class XQMD5
	{
		public static byte[] getMd5Hash(string input)
		{
			MD5 md5Hasher = MD5.Create();
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
			return data;
		}

		public static string GetFileMd5String(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return "";
			}
			StringBuilder sb = new StringBuilder();
			using (MD5 md5 = MD5.Create())
			{
				using (FileStream fs = File.OpenRead(filePath))
				{
					byte[] arr = md5.ComputeHash(fs);
					foreach (byte b in arr)
					{
						sb.Append(b.ToString("x2"));
					}
                    fs.Close();
				}
			}
			return sb.ToString();
		}

		public static string GetByteMd5String(byte[] srcArr)
		{
			StringBuilder sb = new StringBuilder();
			if (srcArr != null)
			{
				using (MD5 md5 = MD5.Create())
				{
					byte[] arr = md5.ComputeHash(srcArr);
					foreach (byte b in arr)
					{
						sb.Append(b.ToString("x2"));
					}
				}
			}

			return sb.ToString();
		}
	}
}