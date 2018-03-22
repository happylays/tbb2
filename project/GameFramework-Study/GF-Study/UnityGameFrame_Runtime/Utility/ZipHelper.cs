using GameFramework;
using IOSharpCode.SharpZipLib.GZip;
using System.IO;


namespace UnityGameFramework.Runtime
{
    internal class ZipHelper : Utility.Zip.IZipHelper
    {
        public byte[] Compress(byte[] bytes)
        {
            return null;
        }
        public byte[] Decompress(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return bytes;
            }

            MemoryStream decompressedStream = null;
            MemoryStream memoryStream = null;
            try
            {
                decompressedStream = new MemoryStream();
                memoryStream = new MemoryStream(bytes);
                using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
                {
                    memoryStream = null;
                    int bytesRead = 0;
                    byte[] clip = new byte[0x1000];
                    while ((bytesRead = gZipInputStream.Read(clip, 0, clip.Length)))
                    {
                        decompressedStream.Write(clip, 0, bytesRead);
                    }
                }

                return decompressedStream.ToArray();
            }
            finally
            {
                if (decompressedStream != null)
                {
                    decompressedStream.Dispose();
                    decompressedStream = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
    }
}