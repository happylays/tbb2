
using GameFramework;
using GameFramework.Download;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    [AddComponentMenu("Game Framework/Download")]
    public sealed class DownloadComponent : GameFrameworkComponent
    {
        private IDownloadManager m_DownloadManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;
        [SerializeField]
        private string m_DownloadAgentHelperTypeName = "UnityGameFramework.Runtime.UnityWebRequestDownloadAgentHelper";
        private DownloadAgentHelperBase m_CustomDownloadAgentHelper = null;
        private int m_DownloadAgentHelperCount = 3;
        private int m_Timeout = 30f;
        private int m_FlushSize = 1024 * 1024;

        public int TotalAgentCount
        {
            get
            {
                return m_DownloadManager.TotalAgentCount;
            }
        }
        public int WorkingAgentCount
        {
            get
            {
                return m_DownloadManager.WorkingAgentCount;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_DownloadManager = GameFrameworkEntry.GetModule<IDownloadManager>();

            m_DownloadManager.DownloadStart += OnDownloadStart;
            m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
            m_DownloadManager.FlushSize = m_FlushSize;
            m_DownloadManager.Timeout = m_Timeout;
        }

        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Download Agent Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_DownloadAgentHelperCount; i++)
            {
                AddDownloadAgentHelper(i);
            }
        }

        public int AddDownload(string downloadPath, string downloadUri)
        {
            return m_DownloadManager.AddDownload(downloadPath, downloadUri);
        }
        private void AddDownloadAgentHelper(int index)
        {
            DownloadAgentHelperBase downloadAgentHelper = Helper.CreateHelper(m_DownloadAgentHelperTypeName, m_CustomDownloadAgentHelper, index);
            m_DownloadManager.AddDownloadAgentHelper(downloadAgentHelper);
        }
        private void OnDownloadStart(object sender, GameFramework.Download.DownloadStartEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<DownloadStartEventArgs>().Fill(e));
        }
    }
}