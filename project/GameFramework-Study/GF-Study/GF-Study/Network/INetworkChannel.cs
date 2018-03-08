
using System.Net;

namespace GameFramework.Network
{
    public interface INetworkChannel
    {
        string Name { get; }
        bool Connected { get; }
        NetworkType NetworkType { get; }
        IPAddress LocalIPAddress { get; }
        int LocaPort { get; }
        IPAddress RemoteIPAddress { get; }
        int RemotePort { get; }
        bool ResetHeartBeatElapseSecondsWhenReceivePacket
        {
            get;
            set;
        }
        float HeartBeatInterval { get; set; }
        int ReceiveBufferSize { get; set; }
        int SendBufferSize { get; set; }
        void RegisterHandler(IPacketHandler handler);
        void Connect(IPAddress ipAddress, int port);
        void Close();
        void Send(byte[] buffer, int offset, int size, object userData);
        void Send<T>(T packet, object userData) where T : Packet;
    }
}