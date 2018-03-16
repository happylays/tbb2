
namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceChecker
        {
            private sealed partial class CheckInfo
            {
                private readonly ResourceName m_ResourceName;
                private CheckStatus m_Status;
                private bool m_NeedRemove;
                private RemoteVersionInfo m_VersionInfo;
                private LocalVersionInfo m_ReadOnlyInfo;
                private LocalVersionInfo m_ReadWriteInfo;

                public CheckInfo(ResourceName resourceName)
                {
                    m_ResourceName = resourceName;
                    m_Status = CheckStatus.Unknown;
                    m_NeedRemove = false;
                    m_VersionInfo = default(RemoteVersionInfo);
                    m_ReadOnlyInfo = default(LocalVersionInfo);
                    m_ReadWriteInfo = default(LoaclVersionInfo);
                }

                public void RefreshStatus(string currentVariant)
                {
                    if (!m_VersionInfo.Exist)
                    {
                        m_Status = CheckStatus.Disuse;
                        m_NeedRemove = m_ReadWriteInfo.Exist;
                    }

                    if (m_ResourceName.Variant == null || m_ResourceName.Variant == currentVariant)
                    {
                        if (m_ReadOnlyInfo.Exist && m_ReadOnlyInfo.LoadType == m_VersionInfo.LoadType)
                        {
                            m_Status = CheckStatus.StorageInReadOnly;
                            m_NeedRemove = m_ReadWriteInfo.Exist;
                        }
                    }
                }

               
            }
        }

    }