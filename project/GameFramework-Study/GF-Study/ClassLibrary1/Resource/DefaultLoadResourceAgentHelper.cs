
using GameFramework;
using GameFramework.Resource;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    public class DefaultLoadResourceAgentHelper : DefaultLoadResourceAgentHelper, IDisposable
    {
        private string m_FileFullPath = null;
        private string m_BytesFllPath = null;
        private int m_LoadType = 0;
        private string m_ResourceChildName = null;
        private bool m_Disposed = false;
        private WWW m_WWW = null;
        private AssetBundleCreateRequest m_FileAssetBundleCreateRequest = null;
        private AssetBundleCreateRequest m_BytesAssetBundleCreateRequest = null;
        private AssetBundleRequest m_AssetBundleRequest = null;
        private AsyncOperation m_AsyncOpertion = null;

        private EventHandler<LoadResourceAgentHelperUpdateEventArgs> m_LoadResourceAgentHelperUpdateEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> m_LoadResourceAgentHelperReadFileComplteEventHandler = null;

        public override void ReadFile(string fullPath)
        {
            if (m_LoadResourceAgentHelperReadFileComplteEventHandler == null)
            {
                Log.Fatal();
                return;
            }

            m_FileFullPath = fullPath;
            m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(m_FileFullPath);
        }

        public override void ReadBytes(string fullPath, int loadType)
        {
            m_BytesFullPath = fullPath;
            m_LoadType = loadType;
            m_WWW = new WWW(Utility.Path.GetRemotePath(fullPath));
        }

        public override void LoadAsset(object resource, string resourceChildName)
        {
            if (m_LoadResourceAgentHelperReadFileComplteEventHandler == null)
            {
                return;
            }

            AssetBundle assetBundle = resource as AssetBundle;
            if (assetBundle == null)
            {
                m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs());
                return;
            }

            m_ResourceChildName = resourceChildName;
            m_AssetBundleRequest = assetBundle.LoadAssetAsync(resourceChildName);
        }

        public override void Reset()
        {
            m_FileFullPath = null;
            m_BytesFullPath = null;
            if (m_WWW)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }

            m_FileAssetBundleCreateRequest = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disosing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disosing)
            {
                if (m_WWW != null)
                {
                    m_WWW.Dispose();
                    m_WWW = null;
                }
            }

            m_Disposed = true;
        }

        private void Update()
        {
            UpdateWWW();
            UpdateFileAssetBundleCreateRequest();
            UpdateAssetBundleRequest();
            UpdateAsyncOperation();
        }

        private void UpdateWWW()
        {
            if (m_WWW != null)
            {
                if (m_WWW.isDone)
                {
                    if (string.IsNullOrEmpty(m_WWW.error))
                    {
                        m_LoadResourceAgentHelperReadFileComplteEventHandler(this, new LoadResourceAgentHelperReadBytesCompleteEventArgs(m_WWW.bytes, m_LoadType));
                        m_WWW.Dispose();
                        m_WWW = null;
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new());
                    }
                }
                else
                {

                }
            }
        }

        private void UpdateFileAssetBundleCreateRequest()
        {
            if (m_FileAssetBundleCreateRequest != null)
            {
                if (m_FileAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = m_FileAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldFileAssetBundleCreateRequest = m_FileAssetBundleCreateRequest;
                        m_LoadResourceAgentHelperReadFileComplteEventHandler(this, new LoadResourceAgentHelperReadFileCompleteEventArgs(assetBundle));
                        if (m_FileAssetBundleCreateRequest == oldFileAssetBundleCreateRequest)
                        {
                            m_FileAssetBundleCreateRequest = null;
                        }
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler();
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler();
                }
            }
        }
    }
}