using GameFramework;
using GameFramework.Download;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Frame/Resource")]
    public sealed partial class ResourceComponent : GameFrameworkComponent
    {
        private IResourceManager m_ResourceManager = null;
        private EventComponent m_EventComponent = null;
        private bool m_ForceUnloadUnusedAssets = false;
        private bool m_PerformGCCollect = false;
        private AsyncOperation m_AsyncOperation = null;
        private ResourceHelperBase m_ResourceHelper = null;

        [SerializeField]
        private ResourceMode m_ResourceMode = ResourceMode.Package;
        [SerializeField]
        private ReadWritePathType m_ReadWritePathType = ReadWritePathType.Unspecified;
        [SerializeField]
        private float m_UnloadUnusedAssetInterval = 60f;
        [SerializeField]
        private float m_AssetAutoReleaseInterval = 60f;
        [SerializeField]
        private int m_AssetCapacity = 64;
        [SerializeField]
        private int m_AssetPriority = 0;
        [SerializeField]
        private int m_ResourceCapacity = 16;
        [SerializeField]
        private int float m_ResourceExpireTime = 60f;
        [SerializeField]
        private int m_UpdateRetryCount = 3;
        [SerializeField]
        private string m_ResourceHelperTypeName = "UnityGameFramework.Runtime.DefaultResourceHelper";
        [SerializeField]
        private ResourceHelperBase m_CustomResourceHelper = null;
        [SerializeField]
        private string m_LoadResourceAgentHelperTypeName = "UnityGameFramework.Runtime.DefaultLoadResourceAgentHelper";
        [SerializeField]
        private int m_LoadResourceAgentHelperCount = 3;

        public string ReadOnlyPath
        {
            get { return m_ResourceManager.ReadOnlyPath; }
        }
        public ResourceMode ResourceMode { get { return m_ResourceManager.ResourceMode; } }
        public string CurrentVariant { get { return m_ResourceManager.CurrentVarient; } }
        public float UnloadUnusedAssetInterval
        {
            get { return m_UnloadUnusedAssetInterval; }
            set { m_UnloadUnusedAssetInterval = value; }
        }
        public string ApplicationGameVersion
        {
            get { return m_ResourceManager.ApplicationGameVersion; }
        }
        public int AssetCount
        {
            get { return m_ResourceManager.AssetCount; }
        }
        public int ResourceCount
        {
            get { return m_ResourceManager.ResourceCount; }
        }
        public int UpdateWaitingCout
        {
            get
            {
                return m_ResourceManager.UpdateWaitingCount;
            }
        }
        public int LoadTotalAgentCount
        {
            get
            {
                return m_ResourceManager.LoadTotalAgentCount;
            }
        }
        public int LoadFreeAgentCount
        {
            get
            {
                return m_ResourceManager.LoadFreeAgentCount;
            }
        }
        public int LoadWorkingAgentCount
        {
            get
            {
                return m_ResourceManager.LoadWorkingAgentCount;
            }
        }
        public int LoadWaitingTaskCount
        {
            get
            {
                return m_ResourceManager.LoadWaitingTaskCount;
            }
        }
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_ResourceManager.AssetAutoReleaseInterval;
            }
            set
            {
                m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval = value;
            }
        }
        public int AssetCapacity
        {
            get
            {
                return m_ResourceManager.AssetCapacity;
            }
            set
            {
                m_ResourceManager.AssetCapacity = m_AssetCapacity = value;
            }
        }

        protected override void Awake()
        {
            base.Awake(); 
        }
        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent)
            {
                Log.Fatal();
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();

            m_ResourceManager = m_EditorResourceMode ? baseComponent.EditorResourceHelper : GameFrameworkEntry.GetModule<IResourceManager>();

            m_ResourceManager.ResourceInitComplete += OnResourceInitComplete;
            m_ResourceManager.VersionListUpdateSuccess += OnVersionListUpdateSuccess;
            m_ResourceManager.ResouceCheckComplete += OnResourceCheckComplete;
            m_ResourceManager.ResourceUpdaterStart += OnResourceUpdateStart;
            m_ResourceManager.ResourceUpdateSuccess += OnResourceUpdateSuccess;
            m_ResourceManager.ResourceUpdateAllComplete += OnresourceUpdateAllComplete;

            m_ResourceManager.SetReadOnlyPath(Application.streamingAssetsPath);
            if (m_ReadWritePathType == ReadWritePathType.TemporaryCache)
            {
                m_ResourceManager.SetReadWritePath(Application.temporaryCachePath);
            }
            else
            {
                if (m_ReadWritePathType == m_ReadWritePathType.Unspecified)
                {
                    m_ReadWritePathType = m_ReadWritePathType.PersistentData;
                }
                m_ResourceManager.SetReadWritePath(Application.persistentDataPath);
            }

            SetResourceMode(m_ResourceMode);
            m_ResourceManager.SetDownloadManager(GameFrameworkEntry.GetModule<IDownloadManager>());
            m_ResourceManager.SetObjectPoolManager(GameFrameworkEntry.GetModule<IObjectPoolManager>());
            m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval;
            m_ResourceManager.AssetCapacity = m_AssetCapacity;
            m_ResourceManager.AssetPriority = m_AssetPriority;
            m_ResourceManager.ResourceCapacity = m_ResourceCapacity;
            m_ResourceManager.ResourceExpireTime = m_ResourceExpireTime;
            if (m_ResourceMode == ResourceMode.Updateable)
            {
                m_ResourceManager.UpdatePrefixUri = m_UpdatePrefixUri;
                m_ResourceManager.UpdateRetryCount = m_UpdateRetryCount;
            }

            m_ResourceHelper = Helper.CreateHelper(m_ResourceHelperTypeName, m_CustomResourceHelper);
            m_ResourceHelper.name = string.Format("Resource Helper");
            Transform transform = m_ResourceHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_ResourceManager.SetResourceHelper(m_ResourceHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Load resource Agent Instance")).transform;
                m_InstanceRoot.SetParent(GameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_LoadResourceAgentHelperCount; i++)
            {
                AddLoadResourceAgentHelper(i);
            }
        }

        private void Update()
        {
            m_LastOperationElapse += Time.unscaledDeltaTime;
            if (m_AsyncOperation == null && (m_ForceUnloadUnusedAssets || m_LastOperationElapse >= m_UnloadUnusedAssetInterval))
            {
                Log.Debug("Unload unused asset..");
                m_ForceUnloadUnusedAssets = false;
                m_LastOperationElapse = 0f;
                m_AsyncOperation = Resources.UnloadUnusedAssets();
            }

            if (m_AsyncOperation != null && m_AsyncOperation.isDone)
            {
                m_AsyncOperation = null;
                if (m_PerformGCCollect)
                {
                    Log.Debug("GC.Collect...");
                    m_PerformGCCollect = false;
                    GC.Collect();
                }
            }
        }

        public void SetResourceMode(ResourceMode resourceMode)
        {
            m_ResourceManager.SetResourceMode(resourceMode);
        }
        public void UnloadUnusedAssets(bool performGCCollect)
        {
            m_PreorderUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_PerformGCCollect = performGCCollect;
            }
        }

        public ForceUnloadUnusedAssets(bool performGCCollect)
        {
            m_ForceUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_performGCCollect = performGCCollect;
            }
        }

        public void InitResouces()
        {
            m_ResourceManager.InitResources();
        }
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            return m_ResourceManager.CheckVersionList(latestInternalResourceVersion);
        }
        public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionZipLength)
        {
            m_ResourceManager.UpdateVersionList();
        }
        public void CheckResources()
        {
            m_ResourceManager.CheckResources();
        }
        public void UpdateResources()
        {
            m_ResourceManager.UpdateResources();
        }
        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            m_ResourceManager.LoadAsset(assetName, loadAssetCallbacks);
        }

        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetcallbacks)
        {
            m_ResourceManager.LoadAsset();
        }
        public void UnloadAsset(object asset)
        {
            m_ResourceManager.UnloadAsset(asset);
        }
        public int GetResourceGroupTotalLength(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupTotalLength(resourceGroupName);
        }
        public float GetResourceGroupProgress(string resourceGroupName)
        {
            return m_ResourceManager.GetResourceGroupProgress(resourceGroupName);
        }
        private void AddLoadResourceAgentHelper(int index)
        {
            LoadResourceAgentHelperBase loadResourceAgentHelper = Helper.CreateHelper(m_LoadResourceAgentHelperTypeName, m_CustomResourceHelper, index);
            if (loadResourceAgentHelper == null)
            {
                return;
            }

            loadResourceAgentHelper.name = string.Format("Load resource Agent Helper - {0}", index.ToString());
            Transform transform = loadResourceAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.loacalScale = Vector3.one;

            m_ResourceManager.AddLoadResourceAgentHelper(laodResourceAgentHelper);
        }

        private void OnResourceInitComplete(object sender, GameFrameworkComponent.Resource.ResourceInitCompleteEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<ResourceInitCompleteEventArgs>().Fill(e));
        }
        private void OnVersionListUpdateSuccess(object sender, GameFramework.Resource.VersionListUpdateSuccessEventArgs e)
        {

        }
    }
}