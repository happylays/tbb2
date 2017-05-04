
namespace LoveDance.Client.Network
{
	public interface INetWorkMessage
	{
		bool decodeMessage(NetReadBuffer DataIn);
		bool encodeMessage(NetWriteBuffer DataOut);
	}
}