
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private sealed partial class LoadResourceAgent : ITaskAgent<LoadResourceTaskBase>
            {
                private static readonly HashSet<string> s_LoadingAssetNames = new HashSet<string>();
                private static readonly HashSet<string> s_LoadingResourcName = new HashSet<string>();

                private readonly ILoadResourceAgentHelper m_Helper;
                private readonly IResourceHelper m_ResourceHelper;
                private readonly IObjectPool<AssetObject> m_AssetPool;
                private readonly IObjectPool<AssetObject> m_ResourcePool;
                private readonly ResourceLoader m_ResourceLoader;
                private readonly string m_ReadOnlyPath;
                private readonly LinkedList<string> m_LoadingDependencyAssetNames;
                private LoadResourceTaskBase m_Task;
                private WaitingType m_WaitingType;
                private bool m_LoadingAsset;
                private bool m_LoadingResource;

                public LoadResourceAgent(ILoadResourceAgentHelper loadResourceAgentHelper, IResourceHelper resourceHelper, IObjectPool<AssetObject> assetPool, IAssetPool<ResourceObject)
                {
                    m_Helper = loadResourceAgentHelper;
                    m_ResourceHelper = resourceHelper;
                    m_AssetPool = assetPool;
                    m_ResourcePool = resourcePool;
                    m_Task = null;
                    m_LoadingAsset = false;
                    m_LoadingResource = false;
                }

                public void Initialize()
                {
                    m_Helper.LoadResourceAgentHelperUpdate += OnLoadResourceAgentHelperUpdate;
                    m_Helper.LoadResourceAgentReadBytesComplete += OnLoadResourceAgentHelperReadBytesComplete;

                }
                public void Update()
                {
                    if (m_WaitingType == WaitingType.None)
                    {
                        return;
                    }

                    if (m_WaitingType == WaitingType.WaitForAsset)
                    {
                        if (IsAssetLoading(m_Task.AssetName) {
                            return;
                        }

                        m_WaitingType = WaitingType.None;
                        AssetObject assetObject = m_AssetPool.Spawn(m_Task.AssetName);
                        if (assetObject == null)
                        {
                            TryLoadAsset();
                            return;
                        }

                        OnAssetObjectReady(assetObject);
                        return;
                    }

                    if (m_WaitingType == WaitingType.WaitForDependencyAsset)
                    {
                        LinkedListNode<string> current = m_LoadingDependencyAssetNames.First;
                        while (current != null)
                        {
                            if (!IsAssetLoading(current.Value))
                            {
                                LinkedListNode<string> next = current.Next;
                                if (!m_AssetPool.CanSpawn(current.Value))
                                {
                                    OnError();
                                    return;
                                }

                                m_LoadingDependencyAssetNames.Remove(current);
                                current = next;
                                continue;
                            }

                            current = current.Next;
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
                        if (IsResourceLoading(m_Task.ResourceInfo.ResourceName.Name))
                        {
                            return;
                        }

                        ResourceObject resourceObject = m_ResourcePool.Spawn(m_Task.ResourceInfo.ResourceName.Name);
                        if (resourceObject == null)
                        {
                            OnError();
                            return;
                        }

                        m_WaitingType = WaitingType.None;
                        OnResourceObjectReady(resourceObject);
                        return;
                    }
                }

                public void Shutdown()
                {
                    Reset();
                    m_Helper.LoadResourceAgentHelperUpdate -= OnLoadResourceAgentHelperUpdate;
                }

                public void Start(LoadResourceTaskBase task)
                {
                    if (task == null)
                    {

                    }

                    m_Task = task;
                    m_Task.StartTime = DateTime.Now;

                    if (IsAssetLoading(m_Task.AssetName))
                    {
                        m_WaitingType = WaitingType.WaitForAsset;
                        return;
                    }

                    TryLoadAsset();
                }

                private void TryLoadAsset()
                {
                    m_LoadingAsset = true;
                    s_LoadingAssetNames.Add(m_Task.AssetName);

                    foreach (string dependencyAssetName in m_Task.GetDependencyAssetNames())
                    {
                        if (!m_AssetPool.CanSpawn(dependencyAssetName))
                        {
                            if (!IsAssetLoading(dependencyAssetName))
                            {
                                OnError(LoadResourceStatus.DependencyError);
                                return;
                            }

                            m_LoadingDependencyAssetNames.AddLast(dependencyAssetName);
                        }
                    }

                    if (m_LoadingDependencyAssetNames.Count > 0)
                    {
                        m_WaitingType = WaitingType.WaitForDependencyAsset;
                        return;
                    }

                    OnDependencyAssetReady();
                }

                private void OnAssetObjectReady(AssetObject assetObject)
                {
                    m_Helper.Reset();

                    object asset = assetObject.Target;

                    m_Task.OnLoadAssetSuccess(this, asset, (float)(DateTime.Now - m_Task.StartTime).TotalSeconds);
                    m_Task.Done = true;
                }

                private void OnDependencyAssetReady()
                {
                    if (IsResourceLoading(m_Task.ResourceInfo.ResourceName.Name))
                    {
                        m_WaitingType = WaitingType.WaitForResource;
                        return;
                    }

                    ResourceObject resourceObject = m_ResourcePool.Spawn(m_Task.ResourceInfo.ResourceName.Name);
                    if (resourceObject != null)
                    {
                        OnResourceObjectReady(resourceObject);
                        return;
                    }

                    m_LoadingResource = true;
                    s_LoadingResourceNames.Add(m_Task.ResourceInfo.ResourceName.Name);

                    string fullPath = Utility.Path.GetCombinePath(m_Task.ResourceInfo.storageInReadOnly ? m_ReadOnly);
                    if (m_Task.ResourceInfo.LoadType == LoadType.LoadFromFile)
                    {
                        m_Helper.ReadFile(fullPath);
                    }
                    else
                    {
                        m_Helper.ReadBytes(fullPath, (int)m_Task.ResourceInfo.LoadType);
                    }

                }

                private void OnResourceObjectReady(ResourceObject resourceObject)
                {
                    m_Task.LoadMain(this, resourceObject.Target);
                }

                private void OnLoadResourceAgentHelperReadFileComplete(object sender, LoadResourceAgentHelperReadFileCompleteEventArgs e)
                {
                    OnResourceObjectReady resourceObject = new ResourceObject(m_Task.ResourceInfo.ResourceName.Name, e.Resource, m_ResourceHelper);
                    m_ResourcePool.Register(resourceObject, true);
                    m_LoadingResource = false;
                    s_LoadingResourceNames.Remove(m_Task.ResourceInfo.ResourceName.Name);
                    OnResourceObjectReady(resourceObject);

                }

                private void OnLoadResourceAgentHelperReadBytesComplete(object sender, LoadResourceAgentHelperReadBytesCompleteEventArgs e)
                {
                    byte[] bytes = e.GetBytes();
                    LoadType loadType = (LoadType)e.LoadType;
                    if (loadType == loadType.LoadFromMemoryAndQuickDecrypt)
                    {
                        bytes = m_DeCryptResourceCallback(m_Task.ResourceInfo.ResourceName.Name);
                    }

                    m_Helper.ParseBytes(bytes);
                }
            }
        }
    }
}