
using GameFramework;
using GameFramework.Entity;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed class Entity : MonoBehaviour, IEntity
    {
        private int m_Id;
        private string m_EntityAssetName;
        private IEntityGroup m_EntityGroup;
        private EntityLogic m_EntityLogic;

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public object Handle
        {
            get
            {
                return gameObject;
            }
        }

        public EntityLogic Logic
        {
            get
            {
                return m_EntityLogic;
            }
        }

        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup)
        {
            m_Id = entityId;
            m_EntityAssetName = entityAssetName;

            ShowEntityInfo showEntityInfo = (showEntityInfo)userData;
            Type entityLogicType = showEntityInfo.EntityLogicType;

            if (m_EntityLogic != null)
            {
                if (m_EntityLogic.GetType() == entityLogicType)
                {
                    m_EntityLogic.enabled = true;
                    return;
                }

                Destroy(m_EntityLogic);
                m_EntityLogic = null;
            }

            m_EntityLogic = gameObject.AddComponent(entityLogicType) as EntityLogic;
            m_EntityLogic.OnInit(showEntityInfo.UserData);
        }

        public void OnRecycle()
        {
            m_Id = 0;
            m_EntityLogic.enabled = false;
        }

        public void OnShow(object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            m_EntityLogic.OnShow(showEntityInfo.UserData);
        }

        public void OnAttached(IEntity childEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            m_EntityLogic.OnAttached((Entity)childEntity.Logic, attachEntityInfo.ParentTransform);
        }

        public void OnUpdate()
        {
            m_EntityLogic.OnUpdate();
        }
    }


}