
using GameFramework.Download;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private sealed partial class ResourceManager
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly List<UpdateInfo> m_UpdateWaitingInfo;
            private IDownloadManager m_DownloadManager;
            private bool m_CheckResourceComplete;
            private bool m_UpdateAllowed;
            private bool m_UpdateComplete;
            private int m_RetryCount;
            private int m_UpdatingCount;

            public GameFrameworkAction<ResourceName, string, int> ResourceUpdateStart;
            public GameFrameworkAction<ResourceName, striing> ResourceUpdateSuccess;

            public ResourceUpdater(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_UpdateWaitingInfo = new List<UpdateInfo>();
                m_DownloadManager = null;
                m_CheckResourceComplete = false;
                m_UpdateAllowed = false;
                m_UpdateComplete = false;
                m_RetryCount = 3;
                m_UpdatingCount = 0;

                ResourceUpdateStart = null;
            }

            public void Update()
            {
                if (m_UpdateAllowed && !m_UpdateComplete)
                {
                    if (m_UpdateWaitingInfo.Count > 0)
                    {
                        if (m_DownloadManager.FreeAgentCount > 0)
                        {
                            UpdateInfo updateInfo = m_UpdateWaitingInfo[0];
                            m_UpdateWaitingInfo.RemoveAt(0);
                            m_DownloadManager.AddDownload(updateInfo.DownloadPath, updateInfo.DownloadUri, updateInfo);
                            m_UpdatingCount++;
                        }
                    }
                    else if (m_UpdatingCount <= 0)
                    {
                        m_UpdateComplete = true;
                        Utility.Path.RemoveEmptyDirectory(m_ResourceManager.m_ReadWritePath);
                        ResourceUpdateAllComplete();
                    }
                }
            }

            public void Shutdown()
            {
                m_DownloadManager.DownloadStart -= OnDownloadStart;

                m_UpdateWaitingInfo.Clear();
            }

            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                m_DownloadManager = downloadManager;
                m_DownloadManager.DownloadStart += OnDownloadStart;
            }

            public void AddResourceUpdate(ResourceName resourceName, LoadType loadType)
            {
                m_UpdateWaitingInfo.Add(new UpdateInfo(resouceName));
            }
            public void CheckResourceComplete(bool needGenerateReadWriteList)
            {
                m_CheckResourceComplete = true;
                if (needGenerateReadWriteList)
                {
                    GenerateReadWriteList();
                }
            }

            public void UpdateResources()
            {
                m_UpdateAllowed = true;
            }
            private void GenerateReadWriteList()
            {
                string file = Utility.Path.GetCombinePath(ResourceListFileName);
                string backupFile = null;

                if (File.Exists(file))
                {
                    backupFile = file + backupFileSuffixName;
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    File.Move(file, backupFile);
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(file, FileMode.CreateNew, FileAccess.Write);
                    using (BinaryWriter binaryWriter = new BinaryWriter(filestream))
                    {
                        fileStream = null;
                        byte[] encryptCode = new byte[4];
                        Utility.Random.GetRandomBytes(encryptCode);

                        binaryWriter.Write(ReadWriteListHeader);
                        binaryWriter.Write(ReadWriteListVersionHeader);
                        binaryWriter.Write(encryptCode);
                        binaryWriter.Write(m_ResourceManager.m_ReadWriteResourceInfos.Count);
                        foreach (KeyValuePair<resourceName, ReadWriteResourceInfo> i in m_ReadWriteResourceInfos)
                        {
                            byte[] nameBytes = Utility.Encryption.GetXorBytes(Utility.Converter.GetBytes(i.Key.Name), encryptCode);
                            binaryWriter.Write((byte)nameBytes.Length);
                            binaryWriter.Write(nameBytes);

                            if (i.Key.Variant == null)
                            {
                                binaryWriter.Write((byte)0);
                            }
                            else
                            {
                                byte[] variantBytes = Utility.Encryption.GetXorBytes(Utility.Converter.GetBytes(i.Key.Variant), encrycode);

                            }

                            binaryWriter.Write((byte)i.Value.LoadType);
                            binaryWriter.Write(i.Value.Length);
                            binaryWriter.Write(i.Value.HashCode);
                            
                        }
                    }

                    if (!string.IsNullOrEmpty(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                }
                catch ()
            }
        }
    }

}