using System.IO;

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class ReceiveState
            {
                private const int DefaultBufferLength = 1024 * 8;
                private readonly MemoryStream m_Stream;
                private IPacketHeader m_PacketHeader;

                public ReceiveState()
                {
                    m_Stream = new MemoryStream(DefaultBufferLength);
                    m_PacketHeader = null;
                }

                private void Reset(int targetlength, IPacketHeader packetHeader)
                {
                    if (targetlength < 0)
                    {

                    }
                    m_Stream.Position = 0L;
                    m_Stream.SetLength(targetlength);
                    m_PacketHeader = PacketHeader;
                }
            }
        }
    }
}