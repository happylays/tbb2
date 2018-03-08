using System.IO;

namespace GameFramework.Network
{
    public interface INetworkChannelHelper
    {
        int PacketHeaderLength
        {
            get;
        }

        void Initialize(INetworkChannel networChannel);
        void Shutdown();
        bool SendHeartBeat();
        byte[] Serialize<T>(T packet) where T : Packet;
        IPack DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData);
    }
}