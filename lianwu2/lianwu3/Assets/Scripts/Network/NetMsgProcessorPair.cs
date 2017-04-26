
namespace LoveDance.Client.Network
{
	public class NetMsgProcessorPair
	{
		private NetMsgProcessor m_processor;
		private GameMsgType m_uMsgType;

		public NetMsgProcessorPair(GameMsgType MsgType, NetMsgProcessor processor)
		{
			m_uMsgType = MsgType;
			m_processor = processor;
		}
		public NetMsgProcessor getProcessor()
		{
			return m_processor;
		}
		public void setProcessor(NetMsgProcessor value)
		{
			m_processor = value;
		}

		public GameMsgType getMsgType()
		{
			return m_uMsgType;
		}
		public void setMsgType(GameMsgType value)
		{
			m_uMsgType = value;
		}
	}
}