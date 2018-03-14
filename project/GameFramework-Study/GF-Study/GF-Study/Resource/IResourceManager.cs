using GameFramework.Download;
using GameFramework.ObjectPool;
using System;

namespace GameFramework.Resource
{
    public interface IResouceManager
    {
        string ReadOnlyPath
        {
            get;
        }
        string ReadWritePath
        {
            get;
        }
        ResourceMode ResourceMode
        {
            get;
        }
        string ApplicationGameVersion
        {
            get;
        }
        int InternalResourceVersion
        {
            get;
        }
        int AssetCount
        {
            get;
        }
        int ResourceCount
        {
            get;
        }
        int UpdateRetryCount
        {
            get;
            set;
        }
        int UpdateWaitingCount
        {
            get;
        }
        int UpdateingCount
        {
            get;
        }
        int LoadFreeAgentCount
        {
            get;
        }
        int LoadWorkingAgentCount
        {
            get;
        }
        int LoadWaitingTaskCount
        {
            get;
        }
        float AssetAutoReleaseInterval
        {
            get;
            set;
        }
        float AssetCapacity
        {
            get;
            set;
        }
        float AssetExpireTime
        {
            get;
            set;
        }
        float ResourceAutoReleaseInterval
        {
            get;
            set;
        }
        event EventHandler<ResourceInitCompleteEventArgs> ResourceInitComplete;
        event EventHandler<VersionListUpdateSuccessEventArgs> VersionListUpdateSuccess;
        event EventHandler<ResourceCheckCompleteEventArgs> ResourceCheckComplete;
        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;
        event EventHandler<resourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;

        void SetReadOnlyPath(string readOnlyPath);
        void SetCurrentVariant(string currentVariant);
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);
        void SetDownloadManager(IDownloadManager downloadManager);
        void SetResourceHelper(IResourceHelper resourceHelper);
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper);
        void InitResources();
        CheckVersionListResult CheckVersionList(int latestInternalResourceVersion);
        void UpdateVersionList(int versionListLength, int versionListHashCode);
        void CheckResources();
        void UdateResouces();
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks);
        void UnloadAsset(object asset);
        bool GetResourceGroupReady(string resourceGroupName);
        float GetResourceGroupProgress(string resourceGroupName);
    }
}