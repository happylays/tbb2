using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;

namespace GameFramework.UI
{
    public interface IUIManager
    {
        int UIGroupCount { get; }
        float InstanceAutoReleaseInterval { get; set; }
        int InstanceCapacity { get; set; }
        float InstanceExpireTime { get; set; }
        int InstancePriority { get; set; }

        event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess;
        event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure;

        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);
        void SetResourceManager(IResourceManager resourceManager);
        void SetUIFormHelper(IUIFormHelper uiFormHelper);
        bool HasUIGroup(string uiGroupName);
        IUIGroup GetUIGroup(string uiGroupName);
        IUIGroup[] GetAllUIGroups();
        bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHeler);
        bool HasUIForm(int serialId);
        IUIForm GetUIForm(int serialId);
        bool IsLoadingUIForm(int serialId);
        bool IsLoadingUIForm(string uiFormAssetName);
        int OpenUIForm(string uiFormAssetName, string uiGroupName);
        int CloseUIForm(int serialId);
        void CloseUIForm(IUIForm uiForm);

    }
}