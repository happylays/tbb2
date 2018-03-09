using System;

namespace GameFramework
{
    public static partial class Utility
    {
        internal static class Encryption
        {
            private const int QuickEncryptLength = 220;

            public static byte[] GetQuickorBytes(byte[] bytes, byte[] code)
            {
                return GetXorBytes(bytes, code, QuickEncryptLength);
            }

            public static byte[] GetXorBytes(byte[] bytes, byte[] code, int length = 0)
            {
                if (bytes == null)
                {
                    return null;
                }
                if (code == null)
                {
                    throw;
                }
                int codeLength = code.Length;
                if (codeLength <= 0)
                {

                }

                int codeIndex = 0;
                int bytesLength = bytes.Length;
                if (length <= 0 || length > bytesLength)
                {
                    length = bytesLength;
                }

                byte[] result = new byte[bytesLength];
                Buffer.BlockCopy(bytes, 0, result, 0, bytesLength);

                for (int i = 0; i < length; i ++)
                {
                    result[i] ^= code[codeIndex++];
                    codexIndex = codeIndex % codeLength;
                }

                return result;
            }
           
        }
    }
}