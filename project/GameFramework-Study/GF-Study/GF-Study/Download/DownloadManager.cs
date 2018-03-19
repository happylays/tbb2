
using System;

namespace GameFramework.Download

{

    internal sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        private readonly TaskPool<DownloadTask> m_TaskPool;
        private readonly DownloadCounter mDownloadCounter;
        private int m_FlushSize;
        private float m_Timeout;
        private EventHandler<DownloadStartEventArgs> m_DownloadStartEventHandler;

        public DownloadManager()
        {
            m_TaskPool = new TaskPool<DownloadTask>();
            m_FlushSize = 1024 * 1024;
            m_Timeout = 30f;
            m_DownloadStartEventHandler = null;
        }

        public int FreeAgentCount
        {
            get
            {
                return m_TaskPool.FreeAgentCount;
            }
        }

        public float CurrentSpeed
        {
            get
            {
                return m_DownloadCounter.CurrentSpeed;
            }
        }

        internal override void Update()
        {
            m_TaskPool.Update();
            m_DownloadCounter.Update();
        }

        public void AddDownloadAgentHelper(IDownloadAgentHelper downlodAgentHeler)
        {
            DownloadAgent agent = new DownloadAgent(AddDownloadAgentHelper);
            agent.DownloadAgentStart += OnDownlodAgentStart;
            m_TaskPool.AddAgent(agent);
        }

        public int AddDownload(string downloadPath, string downloadUri)
        {
            if (TotalAgentCount <= 0)
            {

            }

            DownloadTask downloadTask = new DownloadTask(downloadPath, downloadUri, m_FlushSize);
            m_TaskPool.AddTask(downloadTask);

            return downloadTask.SerialId;
        }

        private void OnDownloadAgentStart(DownloadAgent sender)
        {
            m_DownloadStartEventHandler(this, new DownloadStartEventArgs(sender.Task.SerialId, sender.Task.DownloaPath;))
        }
    }
}