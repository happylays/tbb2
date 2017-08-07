using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Download
{
    public class DownloadManager
    {
        TaskPool<DownloadTask> m_TaskPool;
        DownloadCounter m_DownloadCounter;
        private int m_FlushSize;

        private EventHandler<DownloadStartEventArgs> m_DownloadStartEventHandler;

        public event EventHanler<DownloadStartEventArgs> DownloadStart
        {
            add { m_DownloadStartEventHandler += value; }
            remove { m_DownloadStartEventHandler -= value; }
        }

        public DownloadManager()
        {
            m_TaskPool = new TaskPool<DownloadTask>();
            m_DownloadCounter = new m_DownloadCounter(1,10);
            m_FlushSize = 1024 * 1024;
            m_TimeOut = 30f;
        }
        void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_TaskPool.Update(elapseSeconds, realElapseSeconds);

        }
        void Shutdown()
        {
            m_TaskPool.Shutdown();
            m_DownloadCounter.Shutdown();
        }
        void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper)
        {
            DownloadAgent agent = new DownloadAgent(downloadAgentHelper);
            agent.DownloadAgentStart += OnDownloadAgentStart;         
            agent.DownloadAgentUpdate += OnDownloadAgentUpdate;

            m_TaskPool.AddAgent(agent);
        }
        void AddDownload(string downloadPath, string downloadUri, object userData) {
            if (string.IsNullOrEmpty(downloadPath))
                throw;

            
            DownloadTask downloadTask = new DownloadTask(downloadPath, downloadUri);
            m_TaskPool.AddTask(downloadTask);

            return downloadTask.SerialId;
        }
        void RemoveDownload(int serialId) {
            return m_TaskPool.RemoveTask(serialId) != null;
        }

        void OnDownloadAgentStart(DownloadAgent sender)
        {
            if (m_DownloadStartEventHandler != null)
            {
                m_DownloadStartEventHandler(this, new DownloadStartEventArgs(sender.Task.SerialId));
            }
        }

        void OnDownloadAgentUpdate(DownloadAgent sender, int lastDownloadedLength)
        {
            m_DownloadCounter.RecordDownloadedLength(lastDownloadedLength);
            if (m_DownloadUpdateEventHandler != null)
            {
                m_DownloadUpdateEventHandler(this, new DownloadUpdateEventArgs(sender.Task.SerialId));
            }
        }


    }

    class TaskPool {
        Stack<ITaskAgent<T>> m_FreeAgents;
        LinkedList<ITaskAgent<T>> m_WorkingAgents;
        LinkedList<T> m_WaitingTask;

        public TaskPool() {
            m_FreeAgents = new Stack<ITaskAgent<T>>();
            m_WorkingAgents = new LinkedList<ITaskAgent<T>>();
            m_WaitingTask = new LinkedList<ITaskAgent<T>>();
        }

        void Update(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<ITaskAgent<T>> current = m_WorkingAgents.First;
            while (current != null)
            {
                if (current.Value.Task.Done)
                {
                    LinkedListNode<ITaskAgent<T>> next = current.Next;
                    current.Value.Reset();
                    m_FreeAgents.Push(current.Value);
                    m_WorkingAgents.Remove(current);
                    current = next;
                    continue;
                }

                current.Value.Update();
                current = current.Next;
            }

            while (FreeAgentCount > 0 && WaitingTaskCount > 0)
            {
                ITaskAgent<T> agent = m_FreeAgents.Pop();
                LinkedListNode<ITaskAgent<T>> agentNode = m_WorkingAgent.AddLast(agent);
                T task = m_WaitingTask.First.Value;
                m_WaitingTask.RemoveFirst();
                agent.Start(task);
                if (task.Done)
                {
                    agent.Reset();
                    m_FreeAgents.Push(agent);
                    m_WorkingAgents.Remove(agentNode);
                }
            }

        }

        void AddAgent(ITaskAgent<T> agent) {
            agent.Initialize();
            m_FreeAgents.Push(agent);
        }
    
    }
    
    class DownloadTask : ITask {
        static int s_Serial = 0;
        int m_SerialId;
        bool m_Done;
        DownloadTaskStatus m_Status;
        string m_DownloadPath;
        float m_Timeout;

        public DownloadTask(string downloadPath, string downloadUri, int flushSize, float timeout) {
            m_SerialId = s_Serial++;
            m_Done = false;
            m_Status = DownloadTaskStatus.Todo;
        }
    }

    class DownloadAgent {
        void Initialize() {
            m_Helper.DownloadAgentHelperUpdate += OnDownloadAgentHelperUpdate;
        }

        void Start(DownloadTask task)
        {

            m_Task = task;
            m_Task.Status = DownloadTaskStatus.Doing;

            try
            {
                if (File.Exists(downloadFile))
                {
                    m_FileStream = File.OpenWrite(downloadFile);
                    m_FileStream.Seek(0, SeekOrigin.End);
                    m_StartLength = m_SaveLength = (int)m_FileStream.Length;
                    
                }
                else
                {
                    string directory = Path.GetDirectoryName(m_Task.DownloadPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);

                    }
                    m_FileStream = new FileStream(downloadFile, FileMode.Create, FileAccess.Write;

                }

                if (DownloadAgentStart != null) {
                    DownloadAgentStart(this);
                }

                if (m_StartLength > 0) {
                    m_Helper.Download(m_Task.DownloadUri, m_StartLength, m_Task.UserData);

                }
            }
            catch (Exception exception) {
                OnDownloadAgentHelperError(this, new DownloadAgentHelperErrorEventArgs(exception.Message));
            }
        }

        void SaveBytes(byte[] bytes)
        {
            if (bytes == null) {
                return;
            }

            try { 
                m_FileStream.Write(bytes, 0, length);
                m_WaitFlushSize += length;
                m_SaveLength += length;

                if (m_WaitFlushSize >= m_Task.FlushSize)
                {
                    m_FileStream.Flush();
                    m_WaitFlushSize = 0;
                }
            }
            catch (Exception exception) {
                OnDownloadAgentHelperError(this, new DownloadAgentHelperErrorEventArgs(e.msg));
            }

        void OnDownloadAgentHelperUpdate(object sender, DownloadAgentUpdateEventArgs e) {
            byte[] bytes = e.GetBytes();
            SaveBytes(bytes);

            m_DownloadedLength = e.Length;

            if (DownloadAgentUpdate != null) {
                DownloadAgentUpdate(this, bytes != null ? bytes.Length, 0);
            }
        }

        void Update() {
            if (m_Task.Status == DownloadTaskStatus.Doing) {
                m_WaitTime += realElapseSeconds;
                if (m_WaitTime >= m_Task.Tiemout) {
                    OnDownloadAgentHelperError(this, new DownloadAgentHelperErrorArgs("TimeOut"))
                }
            }
        }
    }

    class DownloadAgentHelper { }

    class DefaultDownloadAgentHelper : DownloadAgentHelperBase, IDisposable 
    {
        WWW m_WWW = null;
        void Download(string downloadUri, int fromPosition, object userData)
        {
            if (m_DownloadAgentHelperUpdateEventHandler == null)
            {
                Log.Fatal();
                return;
            }

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Range", string.Format("bytes={0}-{1}", fromPosition.ToString()));
            m_WWW = new m_WWW(downloadUri, null, header);
        }

        void Update()
        {
            if (m_WWW == null)
            {
                return;
            }

            if (!m_WWW.isDone)
            {
                if (m_LastDownloadSize < m_WWW.bytesDownloaded)
                {
                    m_LastDownloadSize = m_WWW.bytesDownloaded;
                    m_DownloadAgentHelperUpdateEventHandler(this, new DownloadAgentHelperUpdateEventArgs(m_WWW.bytesDownloaded, null);
                }

                return;
            }

            if (!string.IsNullOrEmpty(m_WWW.error)) {
                m_DownloadAgentHelperErrorEventHandler(this, new DownloadAgentHelperErrorEventArgs(m_WWW.error))
            }
            else {
                m_DownloadAgentHelperCompleteEventHandler(this, new DownloadAgentHelperCompleteEvengArgs(m_WWW.bytesDownloaded, m_WWW.bytes));
            }
        }

    }

    class DownloadComponent
    {
        void OnDownloadUpdate(object sender, GameFramework.Download.DownloadUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, new DownloadUpdateEventArgs());
        }
    }

    class ReousrceUpdater {
        void OnDownloadStart(object sender, DownloadStartEventArgs e) {
            if (e.CurrentLength > updateInfo.ZipLength) {

                m_DownloadManager.RemoveDownload(e.Serialid);
                string downloadFile = string.Format("{0}.download", e.DownloadPath);
                if (File.Exists(downloadFile)) {
                    File.Delete(downloadFile);
                }

                string errorMessage = string.Format("When download");
                OnDownloadFailure(this, new DownloadFailureEventArgs());
                return;

            }
        }
    }
}
