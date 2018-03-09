
namespace GameFramework.UI
{
    public interface IUIForm
    {
        int SerialId
        {
            get;
        }
        string UIFormAssetName { get; }
        object Handle { get; }
        IUIGroup UIGroup { get; }
        int DepthInUIGroup { get; }
        void OnInit(int serialId, string uiFormAssetName, IUIGroup, bool pauseCoveredUIForm, bool isNewInstance, object userData);
        void OnRecycle();
        void OnOpen(object userData);
        void OnClose(object userData);
        void OnPause();
        void OnResume();
        void OnUpdate();
        void OnDepthChanged();
    }
}