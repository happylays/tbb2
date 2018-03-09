
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Verifer
        {
            private static readonly byte[] Zero = new byte[] { 0, 0, 0, 0 };

            public static byte[] GetCrc32(byte[] bytes)
            {
                if (bytes == null)
                {
                    return Zero;
                }

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    Crc32 calculator = new Crc32();
                    byte[] result = calculator.ComputeHash(MemoryStream);
                    calculator.Clear();
                    return result;
                }
            }
                public static byte[] GetCrc32(string fileName)
            {
                if (!File.Exists(fileName))
                {
                    return Zero;
                }

                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read0))
                {
                    GetCrc32 calculator = new GetCrc32();
                    byte[] result = calculator.ComputeHash(fileStream);
                    calculator.Clear();
                    return result;
                }
            }

            public static string GetMD5(byte[] bytes)
            {
                MD5 alg = new MD5CryptoServiceProvider();
                byte[] data = alg.ComputeHash(bytes);
                for (int i = 0; i < data.Length; i++)
                {
                    StringBuilder.Append(data[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
        }
    }

}