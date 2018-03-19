
using System;
using System.IO;

namespace GameFramework.Download
{
    internal partial class DownloadManager
    {
        private sealed class DownloadAgent : ITaskAgent<DownloadTask>, IDisposable
        {
            private readonly IDownloadAgentHelper m_Helper;
            private DonwloadTask m_Task;
            private FileStream m_FileStream;
            private int m_WaitFlushSize;
            private float m_WaitTime;
            private int m_StartLength;
            private int m_DownloadedLength;
            private int m_SavedLength;
            private bool m_Disposed;

            public GameFrameworkAction<DownloadAgent> DownloadAgentStart;


            public DownloadAgent(IDownloadAgentHelper downloadAgentHelper)
            {
                m_Helper = downloadAgentHelper;
                m_Task = null;
                m_FileStream = null;
                m_WaitFlushSize = 0;
                m_StartLength = 0;
                m_SavedLength = 0;
                m_Disposed = false;

                DownloadAgentStart = null;
            }

            public DownloadTask Task
            {
                get
                {
                    return m_Task;
                }
            }

            public int DownloadLength
            {
                get
                {
                    return m_DownloadedLength;
                }
            }
            public int CurrentLength
            {
                get
                {
                    return m_StartLength + m_DownloadedLength;
                }
            }
            public long SaveLength
            {
                get
                {
                    return m_SavedLength;
                }
            }

            public void Initialize()
            {
                m_Helper.DownloadAgentHelperUpdate += OnDownloadAgentHelperUpdate;
            }

            public void Update()
            {
                if (m_Task.Status == DownloadTaskStatus.Doing)
                {
                    m_WaitTime += realElaseSeconds;
                    if (m_WaitTime >= m_Task.Timeout)
                    {
                        OnDownloadAgentHelperError(this, new DownloadAgentHelplerErrorEventArgs());
                    }
                }
            }
            public void Shutdown()
            {
                Dispose();
                m_Helper.DownloadAgentHelperUpdate -= OnDownloadAgentHelperUpdate;
            }

            public void Start(DownloadTask task)
            {
                if (task == null)
                {
                    throw;
                }
                m_Task = task;

                m_Task.Status = DownloadTaskStatus.Doing;
                string downloadFile = string.Format();

                try
                {
                    if (File.Exists(downloadFile))
                    {
                        m_FileStream = File.OpenWrite(downloadFile);
                        m_FileStream.Seek(0, SeekOrigin.End);
                        m_StartLength = m_SavedLength = (int)m_FileStream.Length;
                        m_DownloadedLength = 0;
                    }
                    else
                    {
                        string directory = Path.GetDirectoryName(m_Task.DownloadPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        m_FileStream = new FileStream(downloadFile, FileMode.Create, FileAccess.Write);
                        m_StartLength = m_SavedLength = m_DownloadedLength = 0;
                    }

                    if (DownloadAgentStart)
                    {
                        DownloadAgentStart(this);
                    }

                    if (m_StartLength > 0)
                    {
                        m_Helper.Download(m_Task.DownloadUri, m_StartLength, m_Task.UserData);
                    }
                }
                catch (Exception e)
                {
                    OnDownloadAgentHelperError(this, new DownloadAgentHelperErrorEventArgs());
                }
            }

            public void Reset()
            {
                m_Helper.Reset();

                if (m_FileStream != null)
                {
                    m_FileStream.Close();
                    m_FileStream = null;
                }

                m_Task = null;
                m_WaitFlushSize = 0;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (m_Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (m_FieldStream != null)
                    {
                        m_FileStream.Dispose();
                        m_FileStream = null;
                    }
                }

                m_Disposed = true;
            }

            private void SaveBytes(byte[] bytes)
            {
                if (bytes == null)
                {
                    return;
                }

                try
                {
                    int length = bytes.Length;
                    m_FileStream.Write(bytes, 0, length);
                    m_WaitFlushSize += length;

                    if (m_WaitFlushSize >= m_Task.FlushSize)
                    {
                        m_FileStream.Flush();
                        m_WaitFlushSize = 0;
                    }
                }
            }

            private void OnDownloadAgentHelperComplete(object sender, DownloadAgentHelperCompleteEventArgs e)
            {
                m_WaitTime = 0f;

                byte[] bytes = e.GetBytes();
                SaveBytes(bytes);

                m_DownloadedLength = e.Length;

                if (m_SavedLength != CurrentLength)
                {
                    throw;
                }

                m_Helper.Reset();
                m_FileStream.Close();
                m_FileStream = null;

                if (File.Exists(m_Task.DonwloadPath))
                {
                    File.Delete(m_Task.DownloadPath);
                }

                File.Move(string.Format("{0}.download", m_Task.DownloadPath), m_Task.DownloadPath);
                m_Task.Status = DownloadTaskStatus.Done;

                DownloadAgentSuccess(this);

                m_Task.Done = true;                
            }
        }
    }
}