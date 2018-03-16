
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private sealed partial class ResourceChecker
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly Dictionary<ResourceName, CheckInfo> m_CheckInfos;
            private string m_CurrentVariant;
            private bool m_VersionListReady;
            private bool m_ReadOnlyListReady;

            public GameFrameworkAction<ResourceName, LoadType, int> ResourceNeedUpdate;
            public GameFrameworkAction<int, int> ResourceCheckComplete;

            public ResourceChecker(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CheckInfos = new Dictionary<ResourceName, CheckInfo>();
                m_CurrentVariant = null;

                ResourceNeedUpdate = null;
            }

            public void Shutdown()
            {
                m_CheckInfos.Clear();
            }

            public void CheckResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                TryRecoverReadWriteList();

                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadWriePath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParseVersionList);
                m_ResourceManager.m_ResourceHelper.LoadBytes(ResourceListFileName, ParseReadOnlyList);
                
            }

            private void SetVersionInfo(ResourceName resourceName, LoadType loadType, int length)
            {
                GetOrAddCheckInfo(resourceName).SetVersionInfo(loadType, length, hashCode);
            }

            private CheckInfo GetOrAddCheckInfo(ResourceName resourceName)
            {
                CheckInfo checkInfo = null;
                if (m_CheckInfos.TryGetValue(resourceName, out checkInfo))
                {
                    return checkInfo;
                }

                checkInfo = new CheckInfo(resourceName);
                m_CheckInfos.Add(checkInfo.ResourceName, checkInfo);

                return checkInfo;
            }

            private void RefreshCheckInfoStatus()
            {
                if (!m_VersionListReady)
                {
                    return;
                }

                int removedCount = 0;
                int updateCount = 0;
                int updateTotalLength = 0;
                foreach (KeyValuePair<resourceName, CheckInfo> checkInfo in m_CheckInfos)
                {
                    checkInfo ci = checkInfo.Value;
                    ci.RefreshStatus(m_CurrentVariant);

                    if (ci.Status == checkInfo.CheckStatus.StorageInReadOnly)
                    {
                        ProcessResourceInfo(ci.RsourceName, ci.LoadType, ci.Length, ci.HashCode, true);
                    }
                    else if (ci.Status == checkInfo.Checkstatus.StorageInReadWrite)
                    {
                        ProcessResourceInfo(ci.ResourceName, false);
                    }
                    else if (ci.Status == checkInfo.CheckStatus.NeedUpdate)
                    {
                        updateCount++;
                        updateTotalLength += ci.Length;
                        updateTotalZipLength += ci.ZipLength;

                        ResourceNeedUpdate(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode);
                    }

                    if (ci.NeedRemove)
                    {
                        removedCount++;

                        string path = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(ci.ResourceName.FullName));
                        File.Delete(path);

                        if (!m_ResourceManager.m_ReadWriteResourceInfos.ContainsKey(ci.ResourceName))
                        {
                            throw;
                        }
                        m_ResourceManager.m_ReadWriteResourceInfos.Remove(ci.ResourceName);
                    }
                }

                ResourceCheckComplete(removedCount, updateCount);
            }

            private bool TryRecoverReadWriteList()
            {
                string file = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWriePath, ResourceListFileName);
                strng backupFile = file + BackupFileSuffixName;

                try
                {
                    if (!File.Exists(backupFile))
                    {
                        return false;
                    }

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    File.Move(backupFile, file);
                }
                catch
                {
                    return false;
                }

                return true;
            }

            private CheckInfo GetOrAddCheckInfo(ResourceName resourceName)
            {
                CheckInfo checkInfo = null;
                if (m_CheckInfos.TryGetValue(resourceName, out checkInfo))
                {
                    return 
                }
            }

            private void ProcessAssetInfo(ResourceName resourceName, string[] assetNames)
            { }
            private void ProcessAssetDependencyInfo(Dictionary<string, string[]> dependencyAssetNamesCollection)
            { }
            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            { }
        }

    }

}