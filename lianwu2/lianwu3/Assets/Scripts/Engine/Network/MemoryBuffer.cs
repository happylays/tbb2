using System;

namespace LoveDance.Client.Network
{
	public class MemoryBuffer
	{

		private int m_nMaxBufferLen;


		public byte[] m_OrgBuffer;
		public int m_nCurPos;

		protected int getCurPos()
		{
			return m_nCurPos;
		}
		protected void setCurPos(int value)
		{
			m_nCurPos = value;
		}

		public int getCapability()
		{
			return m_nMaxBufferLen - m_nCurPos;
		}
		protected byte[] getOrgBuffer()
		{
			return m_OrgBuffer;
		}
		public int getMaxBufferLen()
		{
			return m_nMaxBufferLen;
		}

		public MemoryBuffer(int len)
		{
			m_nMaxBufferLen = len;
			m_OrgBuffer = new byte[len];
			Array.Clear(m_OrgBuffer, 0, m_nMaxBufferLen);
			m_nCurPos = 0;
		}

		public MemoryBuffer(byte[] Buffer)
		{
			m_nMaxBufferLen = Buffer.Length;
			m_OrgBuffer = Buffer;
			m_nCurPos = 0;

		}

		public virtual void Clear()
		{
			Array.Clear(m_OrgBuffer, 0, m_nMaxBufferLen);
			m_nCurPos = 0;
		}
	}
}