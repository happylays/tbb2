
using GameFramework.Download;
using System;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private sealed class VersionListProcessor
        {
            private readonly ResourceManager m_ResourceManager;
            private IDownloadManager m_DownloadManager;
            private int m_VersionListLength;
            private int m_VersionListHashCode;
            private int m_VersionListZipLength;

            public GameFrameworkAction<string, string> VersionListUpdateSuccess;
            public GameFrameworkAction<string, string> VersionListUpdateFailure;

            public VersionListProcessor(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_DownloadManager = null;
                m_VersionListLength = 0;

                VersionListUpdateSuccess = null;
            }

            public void Shutdown()
            {
                m_DownloadManager.DownloadSuccess -= OnDownloadSuccess;
            }

            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                m_DownloadManager = downloadManager;
                m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
            }

            public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
            {
                string applicableGameVersion = null;
                int internalResourceVerison = 0;

                string versionListFileName = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath);
                if (!File.Exists(versionListFileName))
                {
                    Log.Debug();
                    return CheckVersionListResult.NeedUpdate;
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(versionListFileName, FileMode.Open, FileAccess.Read);
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        fileStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != VersionListHeader)
                        {
                            Log.Debug();
                            return CheckVersionListResult.NeedUpdate;
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            applicableGameVersion = Utility.Converter.GetString();
                            internalResourceVerison = binaryReader.ReadInt32();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                catch
                {
                    Log.Debug();
                    return CheckVersionListResult.NeedUpdate;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }

                if (internalResourceVerison != latestInternalResourceVersion)
                {
                    return CheckVersionListResult.NeedUpdate;
                }

                return CheckVersionListResult.Updated;
            }

            public void UpdateVersionList(int versionListLength, int versionListHashCode)
            {
                m_VersionListLength = versionListLength;
                m_VersionListHashCode = versionListHashCode;

                string versionListFileName = Utility.Path.GetResourceNameWithSuffix(VersionListName);
                m_DownloadManager.AddDownload(localVersionListFilePath);
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                VersionListProcessor versionListProcessor = e.UserData as VersionListProcessor;

                byte[] bytes = File.ReadAllBytes(e.DownloadPath);
                if (m_VersionListZipLength != bytes.Length)
                {

                }

                int hashCode = Utility.Converter.GetInt32(Utility.Verifier.GetCrc32(bytes));
                if (m_VersionListZipLength != hashCode)
                {

                }

                try
                {
                    bytes = Utility.Zip.Decompress(bytes);
                }
                catch (Exception e)
                {

                }

                if (bytes == null) {

                }

                if (m_VersionListLength != bytes.Length)
                {

                }

                File.WriteAllBytes(e.DownloadPath, bytes);
                VersionListUpdateSuccess(e.DownloadPath, e.DownloadUri);
            }
        }
    }

}
       