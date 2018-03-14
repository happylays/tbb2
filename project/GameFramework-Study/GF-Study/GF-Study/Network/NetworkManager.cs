using System;
using System.Collections.Generic;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private readonly Dictionary<string, NetworkChannel> m_NetworkChannels;

        private EventHandler<NetworkConnectedEventArgs> m_NetworkConnectedEventHandler;

        public NetworkManager()
        {
            m_NetworkChannels = new Dictionary<string, NetworkChannel>();

            m_NetworkConnectedEventHandler = null;
        }

        public event EventHandler<NetworkConnectedEventArgs> NetworkConnected
        {
            add
            {
                m_NetworkConnectedEventHandler += value;
            }
            remove
            {
                m_NetworkConnectedEventHandler -= value;
            }
        }

        internal override void Update()
        {
            foreach (KeyValuePair<string, NetworkChannel> networkChannel in m_NetworkChannels)
            {
                networkChannel.Value.Update();
            }
        }

        internal override void Shutdown()
        {
            foreach(KeyValuePair<string, NetworkChannel> networkChannel in m_NetworkChannels)
            {
                NetworkChannel nc = networkChannel.Value;
                nc.NetworkChannelConnected -= OnNetworkChannelConnected;
                nc.Shutdown();
            }
            m_NetworkChannels.Clear();
        }

        public INetworkChannel GetNetworkChannel(string name)
        {
            NetworkChannel networkChannel = null;
            if (m_NetworkChannels.TryGetValue(name ?? string.Empty, out NetworkChannel))
            {
                return networkChannel;
            }

            return null;
        }

        public INetworkChannel CreateNetworkChannel(string name, INetworkChannelHelper networkChannelHelper)
        {
            if (networkChannelHelper == null)
            {
                throw;
            }
            if (networkChannelHelper.PacketHeaderLength <= 0)
            {
                throw;
            }
            if (HasNetworkChannel(name))
            {

            }

            NetworkChannel nc = new NetworkChannel(name, networkChannelHelper);
            nc.NetworkChannelConnected += OnNetworkChannelConnected;
            m_NetworkChannels.Add(name, nc);
            return networkChannel;
        }

        public bool DestroyNetworkChannel(string name)
        {
            NetworkChannel nc = null;
            if (m_NetworkChannels.TryGetValue(name ?? string.Empty, out networkChannel))
            {
                nc.NetworkChannelConnected -= OnNetworkChannelConnected;
                nc.Shutdown();
                return m_NetworkChannels.Remove(name);
            }
            return false;
        }

        private void OnNetworkChannelConnected(NetworkChannel nc, object userData)
        {
            if (m_NetworkConnectedEventHandler != null)
            {
                lock (m_NetworkConnectedEventHandler)
                {
                    m_NetworkConnectedEventHandler(this, new NetworkConnectedEventArgs(NetworkChannel, userData0))
                }
            }
        }
    }
}