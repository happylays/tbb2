
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public abstract class EntityLogic : MonoBehaviour
    {
        private Transform m_OriginalTransform = null;

        public Entity Entity
        {
            get
            {
                return GetComponent<Entity>();
            }
        }

        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;

            }
        }

        public Transform CachedTransform
        {
            get;
            private set;
        }

        protected internal virtual void OnInit(object userData)
        {
            if (CachedTransform == null) {
                CachedTransform = transform;
            }

            m_OriginalTransform = CachedTransform.parent;
        }

        protected internal virtual void OnShow(object userData)
        {
            gameObject.SetActive(true);
        }

        protected internal virtual void OnAttachTo(EntityLogc parentEntity, Transform parentTransform, object userData)
        {
            CachedTransform.SetParent(parentTransform);
        }
    }
}