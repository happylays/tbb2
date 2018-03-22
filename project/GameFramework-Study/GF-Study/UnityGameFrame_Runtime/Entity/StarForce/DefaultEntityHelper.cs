
using GameFramework;
using GameFramework.Entity;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class DefaultEntityHelper : EntityHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        public override object InstantiateEntity(object entityAsset)
        {
            return Instantiate((Object)entityAsset);
        }

        public override IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData)
        {
            GameObject gameObject = entityInstance as gameObject;
            Transform transform = gameObject.transform;
            transform.SetParent((MonoBehaviour)entityGroup.Helper).transform);
            return gameObject.GetOrAddComponent<Entity>();
        }

        public override void ReleaseEntity(object entityAsset, object entityInstance)
        {
            m_ResourceComponent.UnloadAsset(entityAsset);
            DestroyObject((Object)entityInstance);
        }


    }
}