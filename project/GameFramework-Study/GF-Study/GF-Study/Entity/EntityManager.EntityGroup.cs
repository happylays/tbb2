--------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        private sealed class EntityGroup : IEntityGroup
        {
            private readonly string m_Name;
            private readonly IEntityGroupHelper m_EntityGroupHelper;
            private readonly IObjectPool<EntityInstanceObject> m_InstancePool;
            private readonly LinkedList<IEntity> m_Entities;

            public EntityGroup(string name, float instanceAutoReleaseInterval)
            {
                m_Name = name;
                m_EntityGroupHelper = entityGroupHelper;
                m_InstancePool = ObjectPoolManager.CreateSingleSpawnObjectPool<EntityInstanceObject>(instanceCapacity, instanceExpireTime);
                m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
                m_Entities = new LinkedList<IEntity>();
            }

            public int EntityCount
            {
                get
                {
                    return m_Entities.Count;
                }
            }

            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstancePool.AutoReleaseInterval;
                }
                set
                {
                    m_InstancePool.AutoReleaseInterval = value;
                }
                  
            }

            public void Update() {
                LinkedListNode<IEntity> current = m_Entities.First;
                while (current != null)
                {
                    LinkedListNode<IEntity> next = m_Entities.Next;
                    current.Value.OnUpdate();
                    current = next;
                }

            }

            public void RemoveEntity(IEntity entity)
            {
                m_Entities.Remove(entity);
            }

            public void RegisterEntityInstanceObject(EntityInstanceObject obj,bool spawned)
            {
                m_InstancePool.Register(obj, spawned);
            }

            public EntityInstanceObject SpawnEntityInstanceObject(string name)
            {
                return m_InstancePool.Spawn(name);
            }
    }

}