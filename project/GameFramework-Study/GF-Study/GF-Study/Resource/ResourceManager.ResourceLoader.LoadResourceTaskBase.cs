
using System;
using System.Collections.Generic;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private abstract class LoadResourceTaskBase : ITask
            {
                private static int s_Serial = 0;

                private readonly int m_SerialId;
                private bool m_Done;
                private readonly string m_AssetName;
                private readonly ResourceInfo m_ResourceInfo;
                private readonly string[] m_DependencyAssetNames;
                private readonly string m_ResourceChildName;
                private readonly object userData;
                private readonly List<Object> m_DependencyAssets;
                private object m_Resource;

                public LoadResourceTaskBase(string assetName) { }

                public void LoadMain(LoadResourceAgent agent, object resource)
                {
                    m_Resource = resource;
                    agent.Helper.LoadAsset(resource);
                }

                public virtual void OnLoadAssetSuccess(LoadResourceAgent agent, object asset)
                {

                }

                public virtual void OnLoadDependencyAsset(LoadResourceAgent agent, dependencyAsset)
                {
                    m_DependencyAssets.Add(dependencyAsset);
                }
            }
        }
    }
}