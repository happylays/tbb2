
using GameFramework;
using GameFramework.Download;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime

{
    public class DefaultDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private WWW m_WWW = null;
        private int m_LastDwonloadedSize = 0;
        private bool m_Disposed = false;

        private EventHandler<DownloadAgentHelperUpdateEventArgs> m_DownloadAgentHelperUpdateEventArgs = null;

        public override void Download(string downloadUri, objet userData)
        {
            if (m_DownloadAgnetHelperUpdateEventHandler == null)
            {
                Log.Fatal();
                return;
            }

            m_WWW = new WWW(downloadUri);
        }

        private void Update()
        {
            if (!m_WWW.isDone)
            {
                if (m_LastDwonloadedSize < m_WWW.bytesDownloaded)
                {
                    m_LastDwonloadedSize = m_WWW.bytesDownloaded;
                    m_DownloadAgentHelperUpdateEventHandler(this, new DownloadAgentHelperEventArgs());

                }
                return;
            }

            if (!string.IsNullOrEmpty(m_WWW.error))
            {                
                m_DownloadAgentHelperErrorEventHandler(this, new ());
            }
        }
    }
}