
using System;
using System.Net;
using System.Net.Sockets;

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private sealed partial class NetworkChannel : INetworkChannel, IDisposable
        {
            private const float DefaultHeartBeatInterval = 30f;

            private readonly string m_Name;
            private readonly EventPool<Packet> m_EventPool;
            private readonly INetworkChannelHelper m_NetworkChannelHelper;
            private NetworkType m_NetworkType;
            private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            private float m_HeartBeatInterval;
            private Socket m_Socket;
            private readonly ReceiveState m_ReceiveState;
            private readonly HeartBeatState m_HeartBeatState;
            private bool m_Active;
            private bool m_Disposed;

            public GameFrameworkAction<NetworkChannel, object> NetworkChannelConnected;
            public GameFrameworkAction<NetworkChannel> NetworkChannelClosed;

            public NetworkChannel(string name, INetworkChannel networkChannelHelper)
            {
                m_Name = name ?? string.Empty;
                m_EventPool = new EventPool<Packet>(EventPoolMode.Defalut);
                m_NetworkChannelHelper = networkChannelHelper;
                m_Socket = null;
                m_ReceiveState = new ReceiveState();
                m_Disposed = false;

                NetworkChannelConnected = null;

                networkChannelHelper.Initialize(this);
            }

            public IPAddress LocalIPAddress
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw;
                    }

                    IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.LocalEndPoint;
                    if (IPEndPoint == null)
                    {

                    }
                    return IPEndPoint.Address;
                }
            }

            public int ReceiveBufferSize
            {
                get
                {
                    if (m_Socket == null)
                    {

                    }
                    return m_Socket.ReceiveBufferSize;
                }
                set
                {
                    m_Socket.ReceiveBufferSize = value;
                }
            }

            public void Update()
            {
                if (m_Socket == null)
                {
                    return;
                }

                if (m_HeartBeatInterval > 0)
                {
                    lock (m_HeartBeatState)
                    {

                    }

                    if (sendHeartBeat && m_NetworkChannelHelper.SendHeartBeat())
                    {
                        if (miss)
                        {
                            NetworkChannelMissHeartBeat(this, missHeartBeatCount);
                        }
                    }
                }
            }

            public void Shutdown()
            {
                Close();
                m_EventPool.Shutdown();
                m_NetworkChannelHelper.Shutdown();
            }

            public void RegisterHandler(IPacketHandler handler)
            {
                m_EventPool.Subscribe(handler.Id, handler.Handle);
            }

            public void Connect(IPAddress ipAddress, int port, object userData)
            {
                if (m_Socket != null)
                {
                    Close();
                    m_Socket = null;
                }

                switch (ipAddress.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        m_NetworkType = NetworkType.IPv4;
                    
                }

                m_Socket = new Socket(ipAddress.AddressFamily, socketType.Stream, ProtocolType.Tcp);
                if (m_Socket == null)
                {
                    string errorMessage = "";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, SocketError, errorMessage);
                    }
                    throw;
                }

                m_ReceiveState.PrepareForPacketHeader();

                try
                {
                    m_Socket.BeginConnect(ipAddress, port, ConnectCallback, new SocketUserdata());
                }
                catch (Exception e)
                {
                    NetworkChannelError();

                    throw;
                }
            }

            public void Close()
            {
                if (m_Socket == null)
                {
                    return;
                }

                m_EventPool.Clear();

                m_Active = false;
                try
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {

                }
                finally
                {
                    m_Socket.Close();
                    m_Socket = null;

                    NetworkChannelClosed(this);
                }
            }

            public void Send(byte[] buffer, int offset, int size, object userData)
            {
                if (m_Socket == null)
                {

                }

                try
                {
                    m_Socket.BeginSend(buffer, offset, size, SocketFlags.None, SendCallback, new SocetUserData(userData));
                }
                catch (e)
                {
                    m_Active = false;
                    NetworkChannelError();
                    return;

                    throw;
                }
            }

            public void Send<T>(T packet, object userData) where T : Packet
            {
                if (packet == null)
                {
                    NetworkChannelError();
                    return;

                    throw;
                }

                byte[] buffer = null;
                try
                {
                    buffer = m_NetworkChannelHelper.Serialize(packet);
                }

                if (buffer == null)
                {

                }

                Send(buffer, userData);

            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {

            }

            private void Receive()
            {
                try
                {
                    m_Socket.BeginReceive(m_ReceiveState.Stream.GetBuffer(), (int)m_ReceiveState.Stream.Position, (int)(m_ReceiveState.stream.Length - m_ReceiveState.Stream.Position), SocketFlags.None, ReceiveCallback, m_Socket);

                }
                catch ()
                {

                }

            }

            private bool ProcessPacket()
            {
                lock (m_HeartBeatState)
                {

                }

                try
                {
                    Packet packet = m_NetworkChannelHelper.DeserializePacket(m_ReceiveState.PacketHeader, m_ReceiveState.Stream);

                    m_EventPool.Fire(this, packet);
                }

            }

            private void ConnectCallback(IAsyncResult ar)
            {
                SocketUserData socketUserData;
                try
                {
                    socketUserData.Socket.EndConnect(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (e)
                {
                    Error();
                    return;

                    throw;
                }

                m_Active = true;

                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(true);
                }

                NetworkChannelConnected();

                Receive();
            }

            private void SendCallback(IAsyncResult ar)
            {
                SocketUserData socketUserData = (SocketUserData)ar.AsyncState;
                int bytesSent = 0;
                try
                {
                    bytesSent = socketUserData.Socket.EndSend(ar);
                }


                NetworkChannelSended();
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                Socket socket = (Socket)ar.AsyncState;
                int bytesReceived = 0;
                try
                {
                    bytesReceived = socket.EndReceive(ar;
                }

                if (bytesReceived <= 0)
                {
                    Close();
                    return;
                }

                m_ReceiveState.Stream.Position += bytesReceived;
                if (m_ReceiveState.Stream.Position < m_ReceiveState.Stream.Length)
                {
                    Receive();
                    return;
                }

                m_ReceiveState.Stream.Position = 0L;

                bool porcessSuccess  false;
                if (m_ReceiveState.PacketHeader != null)
                {
                    processSuccess = ProcessPacket();
                }

                if (processSuccess)
                {
                    Receive();
                    return;
                }
            }
        }
    }
}