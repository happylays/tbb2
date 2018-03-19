
using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly TaskPool<LoadResourceTaskBase> m_TaskPool;
            private readonly Dictionary<string, object> m_SceneToAssetMap;
            private IObjectPool<AssetObject> m_AssetPool;
            private IObjectPool<ResourceObject> m_ResourcePool;

            public ResourceLoader(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_TaskPool = new TaskPool<LoadResourceTaskBase>();
                m_AssetPool = null;
                m_ResourcePool = null;
            }

            public int TotalAgentCount
            {
                get
                {
                    return m_TaskPool.TotalAgentCount;
                }
            }

            public int WorkingAgentCount
            {
                get
                {
                    return m_TaskPool.WorkingAgentCount;
                }
            }

            public void Update()
            {
                m_TaskPool.Update();
            }

            public void Shutdown()
            {
                m_TaskPool.Shutdown();

            }

            public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
            {
                m_AssetPool = objectPoolManager.CreateMultiSpawnObjectPool<AssetObject>("Asset Pool");
                m_ResourcePool = objectPoolManager.CreateMultiSpawnObjectPool<ResourceObject>();
            }

            public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper helper)
            {
                LoadResourceAgent agent = new LoadResourceAgent(loadResourceAgentHelper, resourceHelper, m_ResourcePool, this, readOnlyPath);
                m_TaskPool.AddAgent(agent);
            }

            public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
            {
                ResourceInfo? resourceInfo = null;
                string[] dependencyAssetNames = null;
                string resourceChildName = null;

                if (!CheckAsset(assetName, out resourceInfo, out dependencyAssetNames, out resourceChildName))
                {
                    string err;
                    loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.NotReady);
                    return;
                }

                LoadAssetTask mainTask = new LoadAssetTask(assetName, resourceInfo.Value, dependencyAssetNames, loadAssetCallbacks, userData);
                foreach (string dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetNames, mainTask, userData))
                    {
                        string err;
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.dependencyerror);
                        return
                    }
                }

                m_TaskPool.AddTask(mainTask);
            }

            private bool CheckAsset(string assetName, out ResourceInfo? resourceInfo, out string[] dependencyAssetNames, out string resourceChildName)
            {
                resourceInfo = null;
                dependencyAssetNames = null;
                resourceChildName = null;

                AssetInfo? assetInfo = m_ResourceManager.GetAssetInfo(assetName);
                if (!assetInfo.HasValue)
                {
                    return false;
                }

                resourceInfo = m_ResourceManager.GetResourceInfo(assetInfo.Value.ResourceName);

                AssetDependencyInfo? assetDependencyInfo = m_ResourceManager.GetAssetDependencyInfo(assetName);

                dependencyAssetNames = assetDependencyInfo.Value.GetDependencyAssetNames();
                return true;
            }

            private bool LoadDependencyAsset(string assetName, LoadResourceTaskBase mainTask, userdata)
            {
                m_TaskPool.AddTask(dependencyTask);
                return true;
            }
            public void UnloadAsset(object asset)
            {
                m_AssetPool.Unspawn(asset);
            }
        }

    }
}