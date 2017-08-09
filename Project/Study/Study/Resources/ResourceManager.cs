using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Resource
{
    class ResourceManager {
        ResourceIniter m_ResourceIniter;
        ResourceChecker m_ResourceChecker;
        ResourceUpdater m_ResourceUpdater;
        ResourceLoader m_ResourceLoader;

        EventHandler<ResourceInitCompleteEventArgs> m_ResourceInitCompleteEventHandler;

        event EventHandler<ResourceInitCompleteEventArgs> ResourceInitComplete { }

        void Update();
        void Shutdown();

        void LoadAsset()
        {
            m_ResourceLoader.LoadAsset(assetName);
        }

        void OnIniterResourceInitComplete() { }
    }
    class ResourceChecker { 
        
    }
    class ResourceUpdater { }
    class ResourceLoader
    {
        ResourceManager m_ResourceManager;
        TaskPool<LoadResourceTaskBase> m_TaskPool;

        void Update() { 
            m_TaskPool.Update();
        }
        void Shutdown() { 
            m_TaskPool.Shutdown();
            m_SceneToAssetMap.Clear();
        }
        void SetObjectPoolManager() {
            m_AssetPool = objectPoolManger.CreateMultiSpawnObjectPool<AssetObject>("Asset pool");
            m_ResourcePool = objectPoolManager.CreateMultiSpawnObjectPool<ResourceObject>("");
        }
        void UnloadAsset(){
            m_AssetPool.UnSpawn();
        }
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {

            if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames))
            {
                string errorAsset = string.Format();
                if (laodAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady);
                    return;
                }

                throw new;
            }

            LoadAssetTask mainTask = new LoadAssetTask(assetName, resourceInfo.Value, dependencyAssetNames);
            foreach (string dependencyAssetName in dependencyAssetNames)
            {
                if (!LoadDependencyAsset(dependencyAssetName, mainTask))
                {
                    string errorMessage = ;
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null) {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName);
                        return;
                    }
                    throw new ;
                }
            }

            m_TaskPool.AddTask(mainTask);
        }
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper)
        {
            LoadResourceAgent agent = new LoadResourceAgent(loadResourceAgentHelper,)
            m_TaskPool.AddAgent(agent);
        }
        bool CheckAsset(string assetName, out ResourceInfo? resourceInfo)
        {
            if (!string.IsNullOrEmpty(assetName))
            {
                return false;
            }
            AssetInfo assetInfo = m_ResourceManager.GetAssetInfo(assetName);
            if (!assetInfo.HasValue)
            {
                return false;
            }

            resourceInfo = m_ResourceManager.GetResourceInfo(assetInfo.Value.ResourceName);

            resourceChildName = assetName.Substring(childNamePosition + 1);

            assetDependencyInfo = m_ResourceManager.GetAssetDependencyInfo();

        }
    }    
    class LoadResourceAgent {

        ILoadResourceAgentHelper m_Helper;

        private enum WaitingType {
            None = 0,
            WaitForAsset,
            WaitForDependencyAsset,
            WaitForResource,
        }

        WaitingType m_WaitingType;

        void Initialize()
        {
            m_Helper.LoadResourceAgentHelperReadFileComplete += OnLoadResourceAgentHelperReadFileComplete; ;
        }

        void Start(LoadResourceAgent task)
        {
            m_Task = task;

            TryLoadAsset();
        }

        void TryLoadAsset()
        {
            AssetObject assetObject = m_AssetPool.Spawn(m_Task.AssetName);
            if (assetObject != null)
            {
                OnAssetObjectReady(assetObject);
                return;
            }

            m_LoadingAsset = true;
            s_LoadingAssetName.Add(m_Task.AssetName);

            foreach (string depend in m_Task.GetDependencyAssetNames())
            {
                if (!m_AssetPool.CanSpawn(dependencyAssetName))
                {
                    if (!IsAssetLoading(dependencyAssetName))
                    {
                        OnError();
                        return;
                    }

                    m_LoadingDependencyAssetNames.AddLast(dependencyAssetName);
                }
            }

            OnDependencyAssetReady();
        }

        void OnAssetObjectReady(AssetObject assetObject)
        {
            object asset = assetObject.Target;
            if (m_Task.IsScene)
            {
                m_ResourceLoader.m_SceneToAssetMap.Add(m_Task.AssetName, asset);
            }

            m_Task.OnLoadAssetSuccess(this, asset);
            m_Task.Done = true;
        }

        void OnDependencyAssetReady()
        {
            ResourceObject resourceObject = m_ResourcePool.Spawn(m_Task.ResourceInfo.ResourceName.Name);
            if (resourceObject != null)
            {
                OnResourceObjectReady(resourceObject);
                return;
            }

            m_LoadingResource = true;
            s_LoadingResourceNames.Add(m_Task.ResourceInfo);

            string fullPath = Utility.Path.GetCombinePath(m_Task.ResourceInfo);
            if (m_Task.ResourceInfo.LoadType == LoadType.LoadFromFile)
            {
                m_Helper.ReadFile(fullPath);
            }
            else
            {
                m_Helper.ReadBytes(fullPath);
            }

        }

        void OnLoadResourceAgentHelperReadFileComplete(object sender, LoadResourceAgentHelperReadFileCompleteEventArgs e)
        {
            ResourceObject resourceObject = new ResourceObject(m_Task.ResourceInfo);
            m_ResourcePool.Register(resourceObject, true);
            m_LoadingResource = false;
            s_LoadingResourceName.Remove(m_Task.ResourceInfo);
            OnResourceObjectReady(resourceObject);
        }
        void Update()
        {
            if (m_WaitingType == WaitingType.None)
            {
                return;
            }

            if (m_WaitingType == WaitingType.WaitForAsset)
            {
                if (IsAssetLoading(m_Task.AssetName))
                {
                    return;
                }

                m_WaitingType = WaitingType.None;
                AssetObject assetObject = m_AssetPool.Spawn(m_Task.AssetName);
                if (assetObject == null)
                {
                    TryLoadAsset();
                    return;
                }

                OnAssetObjectReady();
                return;
            }

            if (m_WaitingType == WaitingType.WaitForDependencyAsset)
            {
                LinkedListNode<string> current = m_LoadingDependencyAssetName.First;
                while (current != null)
                {
                    if (!IsAssetLoading(current.Value))
                    {
                        LinkedListNode<string> next = current.Next;
                        if (!m_AssetPool.CanSpawn(current.Value))
                        {
                            OnError(LoadResourceStatus.DependencyError);
                            return; 
                        }

                        m_LoadingDependencyAssetNames.Remove(current);
                        current = next;
                        continue;
                    }
                }

                if (m_LoadingDependencyAssetNames.Count > 0)
                {
                    return;
                }

                m_WaitingType = WaitingType.None;
                OnDependencyAssetReady();
                return;
            }

            if (m_WaitingType == WaitingType.WaitForResource)
            {
                if (IsResourceLoading(m_Task.ResourceInfo) {
                    return;
                }

                ResourceObject resourceObject = ResourcePool.Spawn(m_Task.ResourceInfo);
                if (resourceObject == null) {
                    OnError(LoadResourceStatus.DependencyError);
                    return;
                }

                m_WaitingType = WaitingType.None;
                OnResourceObjectReady(resourceObject);
                return;
            }
        }
        void Shutdown()
        {
            m_Helper.LoadResourceAgentHelperReadFileComplete -= OnLoadResourceAgentHelperReadFileComplete;
        }
    
    }
    class LoadResourceAgnetHelper 
    {
        string m_FileFullPath = null;
        AssetBundleCreateRequest m_FileAssetBundleCreateRequest = null;
        AssetBundleRequest m_AssetBundleRequest = null;

        EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> m_LoadResourceAgentHelperReadFileCompleteEventHandler = null;

        event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete {}

        void Update()
        {
            UpdateAssetBundleRequest();
        }

        void ReadFile(string fullPath)
        {
            m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fullPath);
        }

        void LoadAsset(object resource)
        {
            AssetBundle assetBundle = resource as AssetBundle;
            m_AssetBundleRequest = assetBundle.LoadAssetAsync(resourceChildName);
        }

        void UpdateAssetBundleRequest()
        {
            if (m_AssetBundleRequest != null)
            {
                if (m_AssetBundleRequest.isDone)
                {
                    if (m_AssetBundleRequest.asset != null)
                    {
                        m_LoadResourceAgentHelperLoadCompleteEventHandler(this, m_AssetBundleRequest.asset);
                    }
                }
            }
        }

        void Dispose()
        {

        }
    }
    interface ITask
    {
        int SerialId { get; }
        bool Done { get; }
    }
    class LoadResourceTaskBase : ITask {
        static int s_Serial = 0;
        int m_SerialId;
        string m_AssetName;
        ResourceInfo m_ResourceInfo;
        string[] m_DependencyAssetNames;
        object m_Resource;

        object[] GetDependencyAsset()
        {
            return m_DependencyAssets.ToArray();
        }
        void LoadMain(LoadResourceAgent agent, object resource)
        {
            m_Resource = resource;
            agent.Helper.LoadAsset(resource);
        }

        virtual void OnLoadAssetSuccess(LoadResourceAgent agent)
        {

        }

        virtual void OnLoadDependencyAsset(LoadResourceAgent agent)
        {
            m_DependencyAssets.Add(dependencyAsset);
        }
    }
    class LoadAssetTask : LoadResourceTaskBase {
        private LoadAssetCallbacks m_LoadAssetCallback;

        public LoadAssetTask(string assetName, ResourceInfo resourceInfo)
            : base(assetName, resourceInfo, dependencyAssetNames)
        {

        }

        override void OnLoadAssetSuccess(LoadResourceAgent agent)
        {
            if (m_OnLoadAssetSuccess(agent, asset, duration)) {
                m_LoadAssetCallback.LoadAssetSuccessCallback(AssetName, asset);
            }
        }
    }

    class ResourceUpdater
    {
        ResourceManager m_ResourceManager;
        List<UpdateInfo> m_UpdateWaitingInfo;
        IDownloadManager m_DownloadManager;

        GameFrameworkAction<ResourceName, string> ResourceUpdateStart;
        GameFrameworkAction ResourceUpdateAllComplete;

        void Update()
        {
            if (m_UpdateWaitingInfo.Count > 0)
            {
                if (m_DownloadManager.FreeAgentCount > 0)
                {
                    m_DownloadManager.AddDownload(updateInfo.DownloadPath);
                    m_UpdateingCount++;
                }
            }
        }

        void AddResourceUpdate(ResourceName resourceName)
        {
            m_UpdateWaitingInfo.Add(new UpdateInfo(resourceName, loadType));
        }

        void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
        {
            bytes = File.ReadAllBytes(e.DownloadPath);

            if (!zip)
            {
                hashBytes = Utility.Converter.GetBytesFromInt(updateInfo.HashCode);
                bytes = Utility.Encryption.GetQuickXorBytes(bytes, hasBytes);
            }

            if (zip)
            {
                try
                {
                    bytes = Utility.Zip.Decompress(bytes);
                }
            }

        }

    }
}
