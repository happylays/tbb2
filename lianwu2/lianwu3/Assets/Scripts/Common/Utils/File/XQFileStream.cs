using UnityEngine;
using System.IO;
using System;

namespace LoveDance.Client.Common
{
	public class XQFileStream
	{

		private Stream m_fsSource = null;

		public bool IsOpen()
		{
			return m_fsSource != null;
		}

		// Closes a file. 
		public void Close()
		{
			if (m_fsSource != null)
			{
				m_fsSource.Close();
				m_fsSource = null;
			}
		}

		public bool Open(string strPath)
		{
			try
			{
				TextAsset file = Resources.Load(strPath) as TextAsset;
				if (file != null)
				{
					m_fsSource = new MemoryStream(file.bytes);
				}
			}
			catch (System.IO.FileNotFoundException e)
			{
				Debug.LogException(e);
				//            Debug.LogError(e.ToString());
			}
			catch (System.IO.DirectoryNotFoundException e)
			{
				//            Debug.LogError(e.ToString());
				Debug.LogException(e);
			}

			return m_fsSource != null;
		}

		//从assetbundle load出的textasset初始?filestream
		public bool Open(byte[] bytes)//TextAsset textasset
		{
			if (null != bytes && bytes.Length > 0)
			{
				try
				{
					m_fsSource = new MemoryStream(bytes);
				}
				catch (System.IO.FileNotFoundException e)
				{
					//            	Debug.LogError(e.ToString());
					Debug.LogException(e);
				}
				catch (System.IO.DirectoryNotFoundException e)
				{
					Debug.LogException(e);
					//Debug.LogError(e.ToString());
				}
			}
			return m_fsSource != null;
		}

		public bool OpenOverWrite(string strPath)
		{
			Close();
			if (strPath != null && strPath.Length > 0)
			{
				m_fsSource = new FileStream(strPath, FileMode.Create);
			}
			return m_fsSource != null;
		}

		public void Save()
		{
			m_fsSource.Flush();
		}

		public void ReadShort(ref Int16 nValue)
		{
			byte[] bytes = new byte[sizeof(Int16)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int16)) == sizeof(Int16)))
				nValue = BitConverter.ToInt16(bytes, 0);
			else
				nValue = 0;
		}

		public Int16 ReadShort()
		{
			Int16 v = 0;
			ReadShort(ref v);
			return v;
		}

		public void ReadUShort(ref UInt16 nValue)
		{
			byte[] bytes = new byte[sizeof(UInt16)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(UInt16)) == sizeof(UInt16)))
				nValue = BitConverter.ToUInt16(bytes, 0);
			else
				nValue = 0;
		}

		public UInt16 ReadUShort()
		{
			UInt16 v = 0;
			ReadUShort(ref v);
			return v;
		}

		public void ReadInt(ref Int32 nValue)
		{
			byte[] bytes = new byte[sizeof(Int32)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int32)) == sizeof(Int32)))
				nValue = BitConverter.ToInt32(bytes, 0);
			else
				nValue = 0;
		}

		public int ReadInt()
		{
			int v = 0;
			ReadInt(ref v);
			return v;
		}

		public void ReadUInt(ref UInt32 nValue)
		{
			byte[] bytes = new byte[sizeof(UInt32)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(UInt32)) == sizeof(UInt32)))
				nValue = BitConverter.ToUInt32(bytes, 0);
			else
				nValue = 0;
		}

		public uint ReadUInt()
		{
			uint nValue = 0;
			ReadUInt(ref nValue);
			return nValue;
		}

		public void ReadInt64(ref Int64 nValue)
		{
			byte[] bytes = new byte[sizeof(Int64)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(Int64)) == sizeof(Int64)))
				nValue = BitConverter.ToInt64(bytes, 0);
			else
				nValue = 0;
		}

		public void ReadBool(ref bool bValue)
		{
			byte nByte = 0;
			ReadByte(ref nByte);
			bValue = (nByte != 0);
		}

		public void ReadBoolEx(ref bool bValue)
		{
			byte[] bytes = new byte[sizeof(byte)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(bool)) == sizeof(bool)))
			{
				bValue = BitConverter.ToBoolean(bytes, 0);
			}
			else
			{
				bValue = false;
			}
		}

		public bool ReadBoolEx()
		{
			bool bValue = false;
			ReadBoolEx(ref bValue);
			return bValue;
		}

		public void ReadByte(ref byte nValue)
		{
			byte[] bytes = new byte[sizeof(byte)];
			if (m_fsSource != null && (m_fsSource.Read(bytes, 0, sizeof(byte)) == sizeof(byte)))
				nValue = bytes[0];
			else
				nValue = 0;
		}

		public byte ReadByte()
		{
			byte bValue = 0;
			ReadByte(ref bValue);
			return bValue;
		}

		public string ReadString()
		{
			short nSize = 0;
			ReadShort(ref nSize);
			byte[] buff = new byte[nSize];
			m_fsSource.Read(buff, 0, nSize);
			return CommonFunc.GetCharsetEncoding().GetString(buff, 0, buff.Length);
		}

		public void ReadString(ref string strOut, UInt16 nSize)
		{
			strOut = "";
			if (nSize > 0)
			{
				byte[] bytes = new byte[nSize];
				if (m_fsSource != null && (m_fsSource.Read(bytes, 0, nSize) == nSize))
				{
					try
					{
						strOut = CommonFunc.GetCharsetEncoding().GetString(bytes, 0, bytes.Length);
					}
					catch (System.ArgumentException e)
					{
						//		            Debug.LogError(e);
						Debug.LogException(e);
					}
				}
			}
		}

		public void ReadString2(ref string strOut, UInt16 nSize)
		{
			strOut = "";
			if (nSize > 0)
			{
				byte[] bytes = new byte[nSize];
				if (m_fsSource != null && (m_fsSource.Read(bytes, 0, nSize) == nSize))
				{
					strOut = CommonFunc.GetCharsetEncoding().GetString(bytes, 0, bytes.Length);
				}
			}
		}

		public void WriteShort(Int16 nValue)
		{
			byte[] bytes = new byte[sizeof(Int16)];
			bytes = BitConverter.GetBytes(nValue);

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(Int16));
			}
		}

		public void WriteUShort(UInt16 nValue)
		{
			byte[] bytes = new byte[sizeof(UInt16)];
			bytes = BitConverter.GetBytes(nValue);

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(UInt16));
			}
		}

		public void WriteInt(Int32 nValue)
		{
			byte[] bytes = new byte[sizeof(Int32)];
			bytes = BitConverter.GetBytes(nValue);

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(Int32));
			}
		}

		public void WriteUInt(UInt32 nValue)
		{
			byte[] bytes = new byte[sizeof(UInt32)];
			bytes = BitConverter.GetBytes(nValue);

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(UInt32));
			}
		}

		public void WriteInt64(Int64 nValue)
		{
			byte[] bytes = new byte[sizeof(Int64)];
			bytes = BitConverter.GetBytes(nValue);

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(Int64));
			}
		}

		public void WriteBool(bool bValue)
		{
			byte nByte = 0;
			if (bValue)
			{
				nByte = 1;
			}
			WriteByte(nByte);
		}

		public void WriteByte(byte nValue)
		{
			byte[] bytes = new byte[sizeof(byte)];
			bytes[0] = nValue;

			if (m_fsSource != null)
			{
				m_fsSource.Write(bytes, 0, sizeof(byte));
			}
		}

		public void WriteString2(string strIn)
		{
			byte[] bytes = CommonFunc.GetCharsetEncoding().GetBytes(strIn);

			int nLength = bytes.Length;
			if (nLength > 0)
			{
				WriteUShort((UInt16)nLength);
				m_fsSource.Write(bytes, 0, nLength);
			}
			else
				WriteUShort(0);
		}
	}
}