
namespace GameFramework.Entity
{
    public interface IEntity
    {
        int Id
        {
            get;
        }

        string EntityAssetName
        {
            get;
        }

        object Handle
        {
            get;
        }

        IEntityGroup EntityGroup
        {
            get;
        }

        void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup);
        void OnRecycle();
        void OnShow(object userData);
        void OnHide(object userData);
        void OnAttached(IEntity childEntity, object userData);
        void OnAttachTo(IEntity parentEntity, object userData);
        void OnUpdate();
    }

}