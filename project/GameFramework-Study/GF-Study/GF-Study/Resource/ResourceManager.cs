
using GameFramework.Download;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private static readonly char[] PackageListHeader = new char[] { 'E', 'L', 'P' };
        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
        private static readonly char[] ReadOnlyListHeader = new char[] { 'E', 'L', 'R' };
        private static readonly char[] ReadWriteListHeader = new char[] { 'e', 'l', 'w' };
        private const string VersionListFileName = "version";
        private const string ResourceListFileName = "list";
        private const string BackupFileSuffixName = ".bak";
        private const byte ReadWriteListVersionHeader  0;

        private readonly Dictionary<string, AssetInfo> m_AssetInfos;
        private readonly Dictionary<string, AssetDependencyInfo> m_AssetDependencyInfos;
        private readonly Dictionary<ResourceName, ResourceInfo> m_ResourceInfos;
        private readonly Dictionary<string, ResourceGroup> m_ResourceGroups;
        private readonly SortedDictionary<ResourceName, ReadWriteResourceInfo> m_ReadWriteResourceInfos;
        private ResourceIniter m_ResourceIniter;
        private VersionListProcessor m_VersionListProcessor;
        private ResourceChecker m_ResouceChecker;
        private ResourceUpdater m_ResourceUpdater;
        private ResourceLoader m_ResourceLoader;
        private IResourceHelper m_ResourceHelper;
        private string m_ReadOnlyPath;
        private string m_ReadWriePath;
        private ResourceMode m_ResourceMode;
        private string m_CurrentVariant;
        private string m_UpdateVariant;
        private string m_ApplicableGameVersion;
        private int m_InternalResourceVersion;
        private DecryResourceCallback m_DecryptResourceCallback;
        private EventHandler<ResourceInitCompleteEventArgs> m_ResourceInitCompleteEventHandler;
        private EventHandler<ResourceUpdateSuccessEventArgs> m_ResourceUpdateSuccessEventHandler;

        public ResourceManager()
        {
            ResourceNameComparer resourceNameComparer = new ResourceNameComparer();
            m_AssetInfos = new Dictionary<string, AssetInfo>();
            m_AssetDependencyInfos = new Dictionary<string, AssetDependencyInfo>();
            m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resourceNameComparer;
            m_ResourceGroup = new Dictionary<string, ResourceGroup>();

            m_ResourceIniter = null;
            m_VersionListProcesor = null;
            m_ResourceChecker = null;
            m_ResourceUpdater  null;
            m_ResourceLoader = new ResourceLoader(this);

            m_ResourceHelper = null;
            m_ReadOnlyPath = null;
            m_CurrentVariant = null;
            m_ApplicableGameVersion = null;

            m_ResourceInitCompleteEventHandler = null;
            m_ResourceUpdateSuccessEventHandler = null;
        }

        internal override int Priority
        {
            get
            {
                return 70;
            }
        }
        public string ReadOnlyPath
        {
            get
            {
                return m_ReadOnlyPath;
            }
        }
        public int AssetCount
        {
            get
            {
                return m_AssetInfos.Count;
            }
        }
        public int UpdateRetryCount
        {
            get
            {
                return m_ResourceUpdater != null ? m_ResourceUpdater.RetryCount : 0;
            }
            set
            {
                if (m_ResourceUpdater == null)
                {
                    throw;
                }
                m_ResourceUpdater.RetryCount = value;
            }
        }

        public int LoadTotalAgentCount
        {
            get
            {
                return m_ResourceLoader.TotalAgentCount
            }
        }
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_ResourceLoader.AssetAutoReleaseInterval;
            }
            set
            {
                m_ResourceLoader.AssetAutoReleaseInterval = value;
            }
        }

        public event EventHandler<ResourceInitCompleteEventArgs> ResourceInitComplete
        {
            add
            {
                m_ResourceInitCompleteEventHandler += value;
            }
            remove
            {
                m_ResourceInitCompleteEventHandler -= value;
            }
        }

        internal override void Update()
        {
            if (m_ResourceUpdater)
            {
                m_ResourceUpdater.Update();
            }
            m_ResourceUpdater.Update();
        }
        internal override void Shutdown()
        {
            if (m_ResourceIniter != null)
            {
                m_ResourceIniter.Shutdown();
                m_ResourceIniter = null;
            }

            m_VersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
            m_VersionListProcessor.Shutdown();
            m_VersionListProcessor = null;

            m_ResourceChecker.ResourceNeedUpdate -= OnCheckerResourceNeedUpdate;
            m_resourceChecker.Shutdown();
            m_ResourceChecker = null;

            m_ResourceUpdater.ResourceUpdateStart -= OnUpdateResourceUpdateStart;
            m_ResourceUpdater.ResourceUpdateAllComplete -= OnUpdateResourceUpdateAllComplete;
            m_ResourceUpdater.Shutdown();
            m_ResourceUpdater = null;

            m_ResourceLoader.Shutdown();
            m_ResourceLoader = null;

            m_AssetInfos.Clear();
            m_AssetDependencyInfos.Clear();
            m_ResourceInfos.Clear();
            m_ReadWriteResourceInfos.Clear();
        }

        public void SetResourceMode(ResourceMode mode)
        {
            if (m_ResourceMode == m_ResourceMode.Unspecified)
            {
                m_ResourceMode = m_ResourceMode;

                if (m_ResourceMode == ResourceMode.Package)
                {
                    m_ResourceIniter = new ResourceIniter(this);
                    m_ResourceIniter.ResourceInitComplete += OnInitResourceInitComplete;
                }
                else if (m_ResourceMode == ResourceMode.Updatable)
                {
                    m_VersionListProcessor = new VersionListProcessor(this);
                    m_VersionListProcessor.VersionListUpdateSuccess += OnVersionListProcessorUpdateSuccess;

                    m_ResourceChecker = new ResourceChecker(this);
                    m_ResourceChecker.ResourceNeedUpdate += OnCheckerResourceNeedUpdate;
                    m_ResourceChecker.ResourceCheckComplete += OnCheckResourceCheckComplete;

                    m_ResourceUpdater = new ResourceUpdater(this);
                    m_ResourceUpdater.ResourceUpdateStart += OnUpdateResourceUpdateStart;
                    m_ResourceUpdater.ResourceUpdateSuccess += OnUpdateResourceUpdateSuccess;
                }
            }
        }

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            m_ResourceLoader.SetObjectPoolManager(objectPoolManager);
        }

        public void SetDownloadManager(IDownloadManager downloadManager)
        {
            if (downloadManager == null)
            {

            }

            if (m_VersionListProcessor != null)
            {
                m_VersionListProcessor.SetDownloadManager(downloadManager);
            }

            m_ResourceUpdater.SetDownloadManager(downloadManager);

        }

        public void SetResourceHelper(IResourceHelper resourceHelper)
        {
            m_ResourceHelper = resourceHelper;
        }

        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            m_ResourceLoader.AddLoadResourceAgentHelper(loadResourceAgentHelper, m_ResourceHelper);
        }
        public void InitResources()
        {
            m_ResourceIniter.InitResources(m_CurrentVariant);
        }

        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            return m_VersionListProcessor.CheckVersionList(latestInternalResoureVersion);
        }
        public void CheckResources()
        {
            m_ResourceChecker.CheckResources(m_CurrentVariant);
        }

        public void UpdateResources()
        {
            m_ResourceUpdater.UpdateResources();
        }
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            m_ResourceLoader.LoadAsset(assetName, loadAssetCallbacks, null);
        }
        public void UnloadAsset(object asset)
        {
            m_ResourceLoader.UnloadAsset(asset);
        }
        private AssetInfo? GetAssetInfo(string assetName)
        {
            AssetInfo assetInfo = default(AssetInfo);
            if (m_AssetInfos.TryGetValue(assetName, out assetInf))
            {
                return assetInfo;
            }
            return null;
        }

        private void OnIniterResourceInitComplete()
        {
            m_ResourceIniter.ResouceInitComplete -= OnIniterResourceInitComplete;
            m_ResourceIniter.Shutdown();
            m_ResourceIniter = null;

            m_ResourceInitCompleteEventHandler(this, new ResourceInitCompleteEventArgs() ;
        }

        private void OnCheckerResourceNeedUpdate(ResourceName, resourceName, LoadType loadType, int length)
        {
            m_ResourceUpdater.AddResourceUpdate(resourceName, loadType, length);
        }

        private void OnCheckerResourceCheckComplete(int removedCount, int updateCount)
        {
            m_VersionListProcessor.VersionListUpdateSuccess -= OnVersionListProcessorUpdateSuccess;
            m_VersionListProcessor.Shutdown();
            m_VersionListProcessor = null;

            m_ResourceChecker.ResourceNeedComplete -= OnCheckerResourceNeedUpdate;
            m_ResourceChecker.ResourceCheckComplete -= OnCheckerResourceCheckComplete;
            m_ResourceChecker.Shutdown();
            m_ResourceChecker = null;

            m_ResourceUpdater.CheckResourceComplete(removedCount > 0);

            if (updateCount < 0)
            {
                m_ResourceUpdater.ResourceUpdateAllComplete -= OnUpdateResourceUpdateAllComplete;
                m_ResourceUpdater.Shutdown();
                m_ResourceUpdater = null;
            }

            m_ResourceCheckCompleteEventHandler(this, new ResourceCheckCompleteEventArgs(removedCount, updateCount));
        }

        private void OnUpdateResourceUpdateAllComplete()
        {
            m_ResourceUpdater.ResourceUpdateStart -= OnUpdateResourceUpdateStart;
            m_ResourceUpdateAllCompleteEventHandler(this, new ResourceUpdateAllCompleteEventArgs());
        }
    }

}