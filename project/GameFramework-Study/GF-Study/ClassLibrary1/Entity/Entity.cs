using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public abstract class Entity : EntityLogic
    {
        private EntityData m_EntityData = null;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        public Animation CachedAnimation
        {
            get;
            private set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedAnimation = GetComponent<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EntityData = userData as EntityData;
            Name = string.Format();
            CachedTransform.localPosition = m_EntityData.Position;
        }
    }
}