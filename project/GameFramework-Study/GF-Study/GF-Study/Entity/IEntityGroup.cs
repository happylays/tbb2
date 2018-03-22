
namespace GameFramework.Entity
{
    public interface IEntityGroup
    {
        string Name
        {
            get;
        }

        int EntityCount
        {
            get;
        }

        float InstanceAutoReleaseInterval
        {
            get;
        }

        IEntityGroupHelper Helper
        {
            get;
        }

        bool HasEntity(int entityId);
        IEntity GetEntity(int entityId);
        IEntity[] GetAllEntities();

    }
}