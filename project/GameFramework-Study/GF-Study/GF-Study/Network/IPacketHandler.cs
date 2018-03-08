
namespace GameFramework.Network
{
    public interface IPacketHandler
    {
        int Id { get; }
        void Handle(object sender, Packet packet);
    }
}