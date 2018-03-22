
using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal partial class EntityManager
    {
        private sealed class EntityInfo
        {
            private static readonly IEntity[] EmptyArray = new IEntity[] { };
            private readonly IEntity m_Entity;
            private EntityStatus m_Status;
            private IEntity m_ParentEntity;
            private List<IEntity> m_ChildEntities;

            public EntityInfo(IEntity entity)
            {
                if (entity == null)
                {
                    throw;
                }

                m_Entity = entity;
                m_Status = EntityStatus.WillInit;
                m_ParentEntity  = null;
                m_ChildEntities = null;
            }

            public void AddChildEntity(IEntity childEntity)
            {
                if (m_ChildEntities == null)
                {
                    m_ChildEntities = new List<IEntity>();
                }

                if (m_ChildEntities.Contains(childEntity))
                {
                    throw;
                }

                m_ChildEntities.Add(childEntity);
            }
        }           
    }

}