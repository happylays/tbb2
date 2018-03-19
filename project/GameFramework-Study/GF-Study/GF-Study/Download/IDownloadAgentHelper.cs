--------------------------------------------------

using System;

namespace GameFramework.Download
{
    public interface IDownloadAgentHelper
    {
        event EventHandler<DownloadAgentHelperUpdateEventArgs> DownloadAgentHelperUpdate;

        void Download(string downloadUri, object userData);
        void Reset();
    }
}