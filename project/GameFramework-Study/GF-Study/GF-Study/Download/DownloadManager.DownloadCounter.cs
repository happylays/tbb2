
using System.Collections.Generic;

namespace GameFramework.Download
{
    internal partial class DownloadManager
    {
        private sealed partial class DownloadCounter
        {
            private readonly Queue<DownloadCounterNode> m_DownloadCounterNodes;
            private float m_UpdateInterval;
            private float m_RecordInterval;
            private float m_CurrentSpeed;
            private float m_Accumulator;
            private float m_TimeLeft;

            public DownloadCounter(float updateInterval, float recordInterval)
            {
                if (updateInterval <= 0f)
                {

                }
                if (recordInterval <= 0f)
                {

                }

                m_DownloadCounterNodes = new Queue<DownloadCounterNode>();
                m_UpdateInterval = updateInterval;
                m_RecordInterval = recordInterval;
                Reset();
            }

            public void Update()
            {
                if (m_DownloadCounterNodes.Count <= 0)
                {
                    return;
                }

                m_Accumulator += realElapseSeconds;
                if (m_Accumulator > m_RecordInterval)
                {
                    m_Accumulator = m_RecordInterval;
                }

                m_TimeLeft -= realElapseSeconds;
                foreach (DownloadCounterNode downloadCounterNode in m_DownloadCounterNodes)
                {
                    downloadCounterNode.Update();
                }

                while (m_DownloadCounterNodes.Count > 0 && m_DownloadCounterNodes.Peek().ElapesSeconds >= m_RecordInterval)
                {
                    m_DownloadCounterNodes.Dequeue();
                }

                if (m_DownloadCounterNodes.Count <= 0)
                {
                    Reset();
                    return;
                }

                if (m_TimeLeft <= 0f)
                {
                    int totalDownloadLength = 0;
                    foreach (DownloadCounterNode node in m_DownloadCounterNodes)
                    {
                        totalDownloadLength += node.DownloadedLength;
                    }

                    m_CurrentSpeed = m_Accumulator > 0f ? totalDownloadLength / m_Accumulator : 0f;
                    m_TimeLeft += m_UpdateInterval;
                }
            }

            public void RecordDownloadedLength(int downloadedLength)
            {
                m_DownloadCounterNodes.Enqueue(new DownloadCounterNode(downloadedLength));
            }
        }
    }

}