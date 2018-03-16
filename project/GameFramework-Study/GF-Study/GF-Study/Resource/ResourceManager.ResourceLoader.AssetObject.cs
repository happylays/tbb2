
using System;
using GameFramework.ObjectPool;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private sealed class AssetObject : ObjectBase
            {
                private readonly object[] m_DependencyAssets;
                private readonly object m_Resource;
                private readonly IObjectPool<AssetObject> m_AssetPool;
                private readonly IObjectPool<ResourceObject> m_ResourcePool;
                private readonly IResourceHelper m_ResourceHelper;

                public AssetObject(string name, object target, object[] dependencyAssets, object resource, IObjectPool<AssetObject> assetPool) 
                    :base(name, target)
                {

                }

                protected internal override void OnUnspawn()
                {
                    base.OnUnspawn();
                    foreach (object dependencyAsset in m_DependencyAssets)
                    {
                        m_AssetPool.Unspawn(dependencyAsset);
                    }
                }

                protected internal override void Release()
                {
                    m_ResourceHelper.Release(Target);
                    m_ResourcePool.Unspawn(m_Resource);
                }

            }
        }

    }
}