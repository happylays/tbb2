using System;
using UnityEngine;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network
{
	public class NetWriteBuffer : MemoryBuffer
	{
		private bool m_bIsOverflow = false;
		public NetWriteBuffer(int nlen)
			: base(nlen)
		{

		}
		public byte[] getWriteBuffer()
		{
			return getOrgBuffer();
		}

		public int getPostion()
		{
			return getCurPos();
		}
		public void setPostion(int value)
		{
			setCurPos(value);
		}

		public void PutByte(byte bValue)
		{
			getOrgBuffer()[getCurPos()] = bValue;
			AddCurPos(1);
		}

		public void AddCurPos(int nValue)
		{
			setCurPos(getCurPos() + nValue);
		}

		public void PutBytes(byte[] nValue)
		{
			PutBytes(nValue, 0, nValue.Length);
		}

		public void PutBytes(byte[] nValue, int nPos, int nLen)
		{
			try
			{
				if (nLen <= getMaxBufferLen() - getCurPos() && !m_bIsOverflow)
				{
					Buffer.BlockCopy(nValue, nPos, getOrgBuffer(), getCurPos(), nLen);
					AddCurPos(nValue.Length);
				}
				else
				{
					m_bIsOverflow = true;
				}
			}
			catch (ArgumentOutOfRangeException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e);
			}
		}

		public void PutInt64(Int64 nValue)
		{
			byte[] bitValue = BitConverter.GetBytes(nValue);
			PutBytes(bitValue);
		}

		public void PutInt(int nValue)
		{
			//Buffer.BlockCopy((byte[])nValue, 0, OrgBuffer, CurPos, sizeof(int));
			byte[] bitValue = BitConverter.GetBytes(nValue);
			PutBytes(bitValue);
		}

		public void PutUInt(uint nUValue)
		{
			//Buffer.BlockCopy( (byte[])nUValue, 0, OrgBuffer, CurPos, sizeof(uint) );
			byte[] bitValue = BitConverter.GetBytes(nUValue);
			PutBytes(bitValue);
		}

		public void PutShort(short nValue)
		{
			byte[] bitValue = BitConverter.GetBytes(nValue);
			PutBytes(bitValue);
		}

		public void PutUShort(ushort nUValue)
		{
			byte[] bitValue = BitConverter.GetBytes(nUValue);
			PutBytes(bitValue);
		}

		public void PutString(string strValue)
		{
			//byte[] bValue = ConverString2Byte( strValue );
			byte[] bValue = CommonFunc.GetCharsetEncoding().GetBytes(strValue);

			int nLength = bValue.Length;
			if (nLength > 0)
			{
				PutUShort((UInt16)nLength);
				PutBytes(bValue);
			}
			else
				PutUShort(0);
		}

		public void PutBool(bool bValue)
		{
			byte value = 0;
			if (bValue)
			{
				value = 1;
			}
			PutByte(value);
		}

		public void PutFloat(float fValue)
		{
			byte[] bitValue = BitConverter.GetBytes(fValue);
			PutBytes(bitValue);
		}

		public override void Clear()
		{
			m_bIsOverflow = false;
			base.Clear();
		}

		public bool IsOverFlow()
		{
			return m_bIsOverflow;
		}

		private byte[] ConverString2Byte(string strValue)
		{
			int nLen = strValue.Length;
			if (nLen > 0)
			{
				byte[] bResult = new byte[(nLen + 1) * 2];
				for (int i = 0; i < nLen; i++)
				{
					byte[] bytevalue = BitConverter.GetBytes(strValue[i]);
					bResult[i * 2] = bytevalue[0];
					bResult[i * 2 + 1] = bytevalue[1];
				}
				bResult[nLen * 2] = 0;
				bResult[nLen * 2 + 1] = 0;

				return bResult;
			}
			else
			{
				byte[] bResult = new byte[2];
				bResult[0] = 0;
				bResult[1] = 0;
				return bResult;
			}
			//return null;
		}
	}
}
