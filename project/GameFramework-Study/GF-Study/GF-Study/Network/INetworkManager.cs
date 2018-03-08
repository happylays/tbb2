
using System;

namespace GameFramework.Network
{
    public interface INetworkManager
    {
        int NetworkChannelCount
        {
            get;
        }
        event EventHandler<NetworkConnectedEventArgs> NetworkConnected;

        bool HasNetworkChannel(string name);
        INetworkChannel GetNetworkChannel(string name);
        INetworkChannel[] GetAllNetworkChannels();
        INetworkChannel CreateNetworkChannel(string name, INetworkChannelHelper networkChannelHelper);
        bool DestroyNetworkChannel(string name);
    }
}