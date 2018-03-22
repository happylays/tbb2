
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;

namespace GameFramework.Entity
{
    public interface IEntityManager
    {
        int EntityCount
        {
            get;
        }

        event EventHandler<ShowEntitySuccessEventArgs> ShowEntitySuccess;

        void SetObjectPoolManager(IObjectPoolManager objectPoolManger);
        void SetEntityHelper(IEntityHelper entityHelper);
        IEntityGroup GetEntityGroup(string entityGroupName);
        bool AddEntityGroup(string entityGroupName, float instanceAutoReleaseInterval, int instanceCapacity);
        bool HasEntity(int entityId);
        IEntity GetEntity(int entityId);
        bool IsLoadingEntity(int entityId);
        void ShowEntity(int entityId, string entityAssetName, string entityGroupName);
        void HideEntity(int entityId);
        IEntity[] GetChildEntities(int parentEntityId);
        void AttachEntity(int childEntityId, int parentEntityId);
        void DetachChildEntities(int parentEntityId);
        
    }
}