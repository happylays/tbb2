using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Entity")]
    public sealed partial class EntityComponent : GameFrameworkComponent
    {
        private IEntityManager m_EntityManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableShowEntitySuccessEvent = true;

        private string m_EntityHelperTypeName = "UnityGameFrame.Runtime.DefaultEntityHelper";
        private EntityHelperBase m_CustomEntityHelper = null;
        private EntityGroupHelperBase m_CustomEntityGroupHelper = null;
        private EntityGroup[] m_EntityGroup[] m_EntityGroups = null;

        public int EntityGroup
        {
            get
            {
                return m_EntityManager.EntityCount;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_EntityManager = GameFrameworkEntry.GetModule<IEntityManager>();
            m_EntityManager.ShowEntitySuccess += OnShowEntitySuccess;
            m_EntityManager.ShowEntityComplete += OnHideEntityComplete;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            m_EntityManager.SetResourceManager(GameFrameworkEntry.GetModule<IObjectPoolManager>());

            EntityHelperBase entityHelper = Helper.CreateHelper(m_EntityHelperTypeName, m_CustomEntityHelper);

            entityHelper.name = string.Format("Entity Helper");
            Transform transform = entityHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_EntityManager.SetEntityHelper(entityHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Entity Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_EntityGroups.Length; i++)
            {
                if (!AddEntityGroup(m_EntityGroups[i].Name, m_EntityGroups[i].InstanceAutoReleaseInterval, m_EntityGroup[i].InstanceCapacity))
                {
                    Log.Warning();
                    continue;
                }
            }
        }

        public IEntityGroup[] GetAllEntityGroups()
        {
            return m_EntityManager.GetAllEntityGroups();
        }

        public bool AddEntityGroup(string entityGroupName, foat instanceAutoReleaseInterval, int instanceCapacity)
        {
            if (m_EntityManager.HasEntityGroup(entityGroupName))
            {
                return false;
            }

            EntityGroupHelperBase entityGroupHelper = Helper.CreateHelper(m_EntityGroupHelperTypeName, m_CustomEntityGroupHelper, EntityGroupCount);

            entityGroupHelper.name = string.Format();
            Transform transform = entityGroupHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            return m_EntityManager.AddEntityGroup(entityGroupName, instanceCapacity);
        }

        public bool HasEntity(int entityId)
        {
            return m_EntityManager.HasEntity(entityId);
        }

        public Entity[] GetAllLoadedEntities()
        {
            IEntity[] entities = m_EntityManager.GetAllLoadedEntities();
            Entity[] entityImpls = new Entity[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                entityImpls[i] = (Entity)entities[i];
            }

            return entityImpls;
        }

        public void ShowEntity(int entityId, Types entityLogicType, string entityAssetName, object userData)
        {
            if (entityLogicType == null)
            {
                return;
            }

            m_EntityManager.ShowEntity(entityId, entityAssetName, entityGroupName, new ShowEntityInfo(entityLogicType, userData));
        }

        public void AttachEntity(Entity childEntity, Entity parentEntity, string parentTrasnformPath, object userData)
        {
            if (childEntity == null)
            {
                return;
            }

            if (parentEntity == null)
            {
                return;
            }

            Transform parentTransform = null;
            if (string.IsNullOrEmpty(parentTransformPath))
            {
                parentTransform = parentTransform.Logic.CachedTransform;
            }
            else
            {
                parentTransform = parentEntity.Logic.CachedTransform.Find(parentTransformPath);
                if (parentTransform == null)
                {
                    parentTransform = parentEntity.Logic.CachedTransform;
                }
            }

            AttachEntity(childEntity, parentEntity, parentTransform, userData);
        }

        public void AttachEntity(Entity childEnity, Entity parentEntity, Transform parentTransform, object userData)
        {
            if (parentTransform)
            {
                parentTransform = parentEntity.Logic.CachedTransform;
            }

            m_EntityManager.AttachEntity(childEnity, parentEntity, new AttachEntityInfo(parentTransform, userData));
        }

        private void OnShowEntitySuccess(object sender, GameFramework.Entity.ShowEntitySuccessEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<ShowEntitySuccessEventArgs>().Fill(e));
        }
    }

}