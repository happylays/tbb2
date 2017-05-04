using System;
using System.Collections.Generic;

namespace LoveDance.Client.Network
{
	public class NetMsgObserver : IDisposable
	{
		private List<NetMsgProcessorPair> m_ProcessorList = new List<NetMsgProcessorPair>();
		private bool m_bEnable = true;

		~NetMsgObserver()
		{
			Dispose();
		}

		public void AddNetMsgProcessor(GameMsgType nMsgType, NetMsgProcessor Porcessor)
		{
			m_ProcessorList.Add(new NetMsgProcessorPair(nMsgType, Porcessor));
			if (m_bEnable)
			{
				NetMsgMap.RegistMsgProcessor(nMsgType, Porcessor);
			}
		}

		public void ClearNetMsgProcessor()
		{
			foreach (NetMsgProcessorPair Pair in m_ProcessorList)
			{
				NetMsgMap.UnRegistMsgProcessor(Pair.getMsgType(), Pair.getProcessor());
			}
			m_ProcessorList.Clear();
		}

		public void Enable()
		{
			EnableProcessor();
			m_bEnable = true;
		}

		public void Disable()
		{
			DisableProcessor();
			m_bEnable = false;
		}

		void EnableProcessor()
		{
			foreach (NetMsgProcessorPair Pair in m_ProcessorList)
			{
				NetMsgMap.RegistMsgProcessor(Pair.getMsgType(), Pair.getProcessor());
			}
		}

		void DisableProcessor()
		{
			foreach (NetMsgProcessorPair Pair in m_ProcessorList)
			{
				NetMsgMap.UnRegistMsgProcessor(Pair.getMsgType(), Pair.getProcessor());
			}
		}

		public void Dispose()
		{
			ClearNetMsgProcessor();
		}
	}
}