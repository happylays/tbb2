
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {
        private readonly Dictionary<int, EntityInfo> m_EntityInfos;
        private readonly Dictionary<string, EntityGroup> m_EntityGroups;
        private readonly List<int> m_EntitiesBeingLoaded;
        private readonly HashSet<int> m_EntitiesToReleaseOnLoad;
        private readonly LinkedList<EntityInfo> m_RecycleQueue;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IEntityHelper m_EntityHelper;
        private EventHandler<ShowEntitySuccessEventArgs> m_ShowEntitySuccessEventHandler;

        public EntityManager()
        {
            m_EntityInfos = new Dictionary<int, EntityInfo>();
            m_EntityGroups = new Dictionary<string, EntityGroup>();
            m_RecycleQueue = new LinkedList<EntityInfo>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadEntitySuccessCallback);
            m_ShowEntitySuccessEventHandler = null;
        }

        public int EntityCount
        {
            get
            {
                return m_EntityInfos.Count;
            }
        }

        internal override void Update()
        {
            while (m_RecycleQueue.Count > 0)
            {
                m_EntityInfos entityInfo = m_RecycleQueue.First.Value;
                m_RecycleQueue.RemoveFirst();
                IEntity entity = entityInfo.Entity;
                m_EntityGroups entityGroup = (EntityGroup)entity.EntityGroup;

                entityInfo.Status = EntityStatus.WillRecycle;
                entity.OnRecycle();
                entityInfo.Status = EntityStatus.Recycled;
                entityGroup.UnspawnEntity(entity);
            }

            foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
            {
                entityGroup.Value.Update();
            }
        }

        internal override void Shutdown()
        {
            HideAllLoadedEntities();
            m_EntityGroups.Clear();
            m_EntitiesBeingLoaded.Clear();
            m_RecycleQueue.Clear();
        }

        public IEntityGroup[] GetAllEntityGroups()
        {
            int index = 0;
            IEntityGroup[] entityGroups = new IEntityGroup[m_EntityGroups.Count];
            foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
            {
                entityGroups[index++] = entityGroup.Value;
            }

            return entityGroups;
        }

        public bool AddEntityGroup(string entityGroupName, float instnceAutoReleaseInterval)
        {
            if (HasEntityGroup(entityGroupName))
            {
                return false;
            }

            m_EntityGroups.Add(entityGroupName, new EntityGroup(entityGroupName, instnceAutoReleaseInterval));
            return true;
        }

        public IEntity GetEntity(string entityAssetName)
        {
            foreach (KeyValuePair<int, EntityInfo> entityInfo in m_EntityInfos)
            {
                if (entityInfo.Value.Entity.EntityAssetName == entityAssetName)
                {
                    return entityInfo.Value.Entity;
                }
            }

            return null;
        }

        public void ShowEntity(int entityId, string entityAssetName, string entityGroupName, object userData)
        {
            if (m_EntityInfos.ContainsKey(entityId))
            {
                throw;
            }

            EntityGroup entityGroup = (EntityGroup)GetAllEntityGroups(entityGroupName);
            EntityInstanceObject entityInstanceObject = entityGroup.SpawnEntityInstanceObject(entityAssetName);
            if (entityInstanceObject == null)
            {
                if (m_EntitiesBeingLoaded.Contains(entityId))
                {
                    throw;
                }

                m_EntitesBeingLoaded.Add(entityId);
                m_ResourceManager.LoadAsset(entityAssetName, m_LoadAssetCallbacks, new ShowEntityInfo(entityId, entityGroup, userData));
                return;
            }

            InternalShowEntity(entityId, entityAssetName, entityGroup, entityInstanceObject.Target);
        }
        public void HideEntity(int entityId, object userData)
        {
            if (IsLoadingEntity(entityId))
            {
                m_EntitiesToReleaseOnLoad.Add(entityId);
                return;
            }

            m_EntityInfos enittyInfo = GetEntityInfo(entityId);
            if (entityInfo == null)
            {

            }

            InternalHideEntity(m_EntityInfos, userData);
        }

        private void InternalShowEntity(int entityId, string entityAssetName)
        {
            try
            {
                IEntity entity = m_EntityHelper.CreateEntity(entityInstance, entityGroup, userData);
                EntityInfo entityInfo = new EntityInfo(entity);
                m_EntityInfos.Add(entityId, entityInfo);
                entityInfo.Status = EntityStatus.WillInit;
                entity.OnInit(entityId, entityAssetName, entityGroup);
                entityInfo.Status = EntityStatus.Inited;
                entityGroup.AddEntity(entity);
                entityInfo.Status = EntityStatus.WillShow;
                entity.OnShow(userData);
                entityInfo.Status = EntityStatus.Showed;

                m_ShowEntitySuccessEventHandler(this, new ShowEntitySuccessEventArgs(entity));
            }
            catch (Exception e)
            {
                m_ShowEntityFailureEventHandler();
                return;

                throw;
            }

        }

        private void InternalHideEntity(EntityInfo entityInfo, object userData)
        {
            IEntity entity = entityInfo.Entity;
            IEntity[] childEntities = entityInfo.GetChildEntities();
            foreach (IEntity childEntity in childEntities)
            {
                HideEntity(childEntity.Id, userData);
            }

            DetachEntity(entity.Id, userData);
            entityInfo.Status = EntityStatus.WillHide;
            entity.OnHide(userData);
            entityInfo.Status = EnityStatus.Hidden;

            EntityGroup entityGroup = (EntityGroup)entity.EntityGroup;

            entityGroup.RemoveEntity(entity);
            if (!m_EntityInfos.Remove(entity.Id))
            {
                throw;
            }

            m_HideEntityCompleteEventHandler();

            m_RecycleQueue.AddLast(entityInfo);
        }

        private void LoadEntitySuccessCallback(string entityAssetName, object entityAsset)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;

            m_EntitiesBeingLoaded.Remove(showEntityInfo.EntityId);
            if (m_EntitiesToReleaseOnLoad.Contains(showEntityInfo.EntityId))
            {
                Log.Debug();
                m_EntitiesToReleaseOnLoad.Remove(showEntityInfo.EntityId);
                m_EntityHelper.ReleaseEntity(entityAsset, null);
                return;
            }

            EntityInstanceObject entityInstanceObject = new EntityInstanceObject(entityAssetName, entityAsset, m_EntityHelper.InstantiateEntity(entityAsset), m_EntityHelper);
            showEntityInfo.EntityGroup.RegisterEntityInstanceObject(entityInstanceObject, true);

            InternalShowEntity(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup, entityInstanceObject.Target, true);
        }
    }
}