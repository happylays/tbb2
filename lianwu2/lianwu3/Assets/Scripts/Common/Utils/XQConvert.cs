using System;
using System.Text;
using System.Security.Cryptography;

namespace LoveDance.Client.Common
{
	public class XQConvert
	{
		static string s_Key = "ddianl1234567890";

		public static string Encrypt(string strContent)
		{
			byte[] arContent = UnicodeEncoding.Unicode.GetBytes(strContent);
			byte[] arKey = ASCIIEncoding.ASCII.GetBytes(s_Key);

			TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
			provider.Key = arKey;
			provider.Mode = CipherMode.ECB;
			provider.Padding = PaddingMode.PKCS7;

			ICryptoTransform encryptor = provider.CreateEncryptor();
			byte[] arData = encryptor.TransformFinalBlock(arContent, 0, arContent.Length);

			return Convert.ToBase64String(arData);
		}

		public static string Decrypt(string strContent)
		{
			byte[] arContent = Convert.FromBase64String(strContent);
			byte[] arKey = ASCIIEncoding.ASCII.GetBytes(s_Key);

			TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
			provider.Key = arKey;
			provider.Mode = CipherMode.ECB;
			provider.Padding = PaddingMode.PKCS7;

			ICryptoTransform decryptor = provider.CreateDecryptor();
			byte[] arData = decryptor.TransformFinalBlock(arContent, 0, arContent.Length);

			return UnicodeEncoding.Unicode.GetString(arData);
		}
	}
}