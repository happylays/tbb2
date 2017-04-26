using System;
using UnityEngine;
using System.Text;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network
{
	public class NetReadBuffer : MemoryBuffer
	{
		private int m_nCurReadPos = 0;//标识已经读取过的位置
		public NetReadBuffer(int len)
			: base(len)
		{

		}

		public NetReadBuffer(byte[] Buffer)
			: base(Buffer)
		{

		}

		public byte[] getRealBuffer()
		{
			return getOrgBuffer();
		}

		public int getMaxDataPostion()
		{

			//        get { return CurPos; }
			return getCurPos();
		}
		public void setMaxDataPostion(int value)
		{
			setCurPos(value);
		}

		public void setPostion(int value)
		{
			m_nCurReadPos = value;
			if (m_nCurReadPos < 0)
			{
				m_nCurReadPos = 0;
			}
			if (m_nCurReadPos > getMaxDataPostion())
			{
				m_nCurReadPos = getMaxDataPostion();
			}
		}
		public int getPostion()
		{
			return m_nCurReadPos;
		}

		public bool hasRemaining()
		{
			return getCurPos() > m_nCurReadPos;
		}
		public int remaining()
		{
			return getCurPos() - m_nCurReadPos;
		}
		public override void Clear()
		{
			base.Clear();
			m_nCurReadPos = 0;
		}
		public void Settle()
		{
			//整理内存，把已经用过的数据清理掉，把后面的数据拿到前面来。
			if (getMaxDataPostion() > 0)
			{
				if (m_nCurReadPos < getMaxDataPostion())
				{
					Buffer.BlockCopy(getOrgBuffer(), m_nCurReadPos, getOrgBuffer(), 0, getMaxDataPostion() - m_nCurReadPos);

					setMaxDataPostion(getMaxDataPostion() - m_nCurReadPos);
					m_nCurReadPos = 0;
					if (getMaxDataPostion() < 0)
					{
						setMaxDataPostion(0);
					}
				}
				else
				{
					Clear();
				}
			}
		}

		public byte GetByte()
		{
			byte bResult = 0;
			try
			{
				bResult = getOrgBuffer()[m_nCurReadPos];
				m_nCurReadPos++;
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//Debug.LogError(e);
			}
			return bResult;
		}

		public bool GetBool()
		{
			byte bValue = GetByte();
			if (bValue != 0)
			{
				return true;
			}

			return false;
		}

		public int GetInt()
		{
			int nResult = 0;
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nResult, 0, sizeof(int));
				nResult = BitConverter.ToInt32(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(int);
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}
			return nResult;
		}

		public uint GetUInt()
		{
			uint nResult = 0;
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nResult, 0, sizeof(uint));
				nResult = BitConverter.ToUInt32(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(uint);
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}
			return nResult;
		}

		public short GetShort()
		{
			short nResult = 0;
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nResult, 0, sizeof(short));
				nResult = BitConverter.ToInt16(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(ushort);
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}
			return nResult;
		}

		public ushort GetUShort()
		{
			ushort nResult = 0;
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nResult, 0, sizeof(ushort));
				nResult = BitConverter.ToUInt16(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(ushort);
			}
			catch (System.ArgumentException e)
			{
				//            Debug.LogError(e);
				Debug.LogException(e);
			}
			return nResult;
		}

		public Int64 GetInt64()
		{
			Int64 nResult = 0;
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nResult, 0, sizeof(int));
				nResult = BitConverter.ToInt64(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(Int64);
			}
			catch (System.ArgumentException e)
			{
				//            Debug.LogError(e);
				Debug.LogException(e);
			}
			return nResult;
		}

		/*
		public string GetString()
		{
			int nLen = 0  ;
			byte[] buffer = getOrgBuffer() ;
			byte curByte = buffer[m_nCurReadPos];
			while(curByte != '\0')
			{
				curByte ++ ;
				nLen ++ ;
			}
			if(nLen != 0 )
			{
				return FixLenString(nLen) ;
			}
			else
			{
				return "" ;
			}
		}*/

		//获取长度在前的字符串
		public string GetPerfixString()
		{
			int nPerfixLen = 0;
			string strResult = "";
			try
			{
				//Buffer.BlockCopy( OrgBuffer, m_nCurReadPos, (byte[])nPerfixLen, 0, sizeof(uint));
				nPerfixLen = GetUShort();//GetInt();
				if (nPerfixLen >= 0)
				{
					//strResult = Encoding.Unicode.GetString(getOrgBuffer(), m_nCurReadPos, nPerfixLen*2 );
					//m_nCurReadPos += nPerfixLen*2;
					strResult = CommonFunc.GetCharsetEncoding().GetString(getOrgBuffer(), m_nCurReadPos, nPerfixLen);
					m_nCurReadPos += nPerfixLen;
				}
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}

			return strResult;
		}

		public string GetUTF8String()
		{
			int nPerfixLen = 0;
			string strResult = "";
			try
			{
				nPerfixLen = GetUShort();
				if (nPerfixLen > 0)
				{
					strResult = Encoding.GetEncoding("utf-8").GetString(getOrgBuffer(), m_nCurReadPos, nPerfixLen);
					m_nCurReadPos += nPerfixLen;
				}
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}

			return strResult;
		}

		//获取固定长度字符串
		public string FixLenString(int nLen)
		{
			string strResult = "";
			try
			{
				strResult = Encoding.Unicode.GetString(getOrgBuffer(), m_nCurReadPos, nLen);
				m_nCurReadPos += nLen;
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}
			return strResult;
		}

		public Byte[] GetFixLenBytes()
		{
			Byte[] bs = null;
			if (remaining() >= sizeof(UInt16))
			{
				UInt16 nLen = GetUShort();
				if (nLen > 0 && remaining() >= nLen)
				{
					bs = new byte[nLen];
					Buffer.BlockCopy(getOrgBuffer(), m_nCurReadPos, bs, 0, nLen);
					m_nCurReadPos += nLen;
				}
			}
			return bs;
		}

		public float GetFloat()
		{
			float nResult = 0;
			try
			{
				nResult = BitConverter.ToSingle(getOrgBuffer(), m_nCurReadPos);
				m_nCurReadPos += sizeof(float);
			}
			catch (System.ArgumentException e)
			{
				Debug.LogException(e);
			}
			return nResult;
		}
	}
}