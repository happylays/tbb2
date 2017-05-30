using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine;
using LoveDance.Client.Network.Login;

namespace LoveDance.Client.Network
{
	public class ServerConnect
	{

		private Socket m_CliSocket = null;
		public const int s_nReadBufferSize = 1024 * 64;
		public const int s_nWriteBufferSize = 1024 * 64;
		private IPEndPoint m_IpEndPoint = null;
		private Queue<INetWorkMessage> m_ReadMsgQueue = null;
		private Queue<INetWorkMessage> m_WriteMsgQueue = null;
		//private Mutex writeDone = null;
		private NetReadBuffer m_Readbuffer = null;
		private NetWriteBuffer m_Writebuffer = null;
		private bool m_bConnecting = false;
		private bool m_bSending = false;

		private INetWorkMessage mTempMsg = null;

		public bool IsConnected
		{
			get
			{
				if (m_CliSocket != null && m_CliSocket.Connected)
				{
					return true;
				}

				return false;
			}
		}

		public void InitServerConnect()
		{
			m_ReadMsgQueue = new Queue<INetWorkMessage>();
			m_WriteMsgQueue = new Queue<INetWorkMessage>();
			//writeDone = new Mutex(false);
			m_Readbuffer = new NetReadBuffer(s_nReadBufferSize);
			m_Writebuffer = new NetWriteBuffer(s_nWriteBufferSize);

			m_Readbuffer.Clear();
			m_Writebuffer.Clear();
		}

		public void Disconnect()
		{
			if (m_CliSocket.Connected)
			{
				ReinitSocket();
				PostDisconnectMsg();
			}
		}

		public void Connect(string server, int port)
		{
			if (!m_bConnecting)
			{
				IPAddress servAD = null;

				try
				{
					if (server.Contains(".com") || server.Contains(".net") || server.Contains(".cn"))
					{
						IPAddress[] IPs = Dns.GetHostAddresses(server);
						if (IPs.Length > 0)
						{
							servAD = IPs[0];
						}
					}
					else
					{
						servAD = IPAddress.Parse(server);
					}
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					//Debug.LogError( "Parse server fail " + e.ToString() );
				}

				if (servAD != null)
				{
					m_IpEndPoint = new IPEndPoint(servAD, port);
					m_CliSocket = new Socket(m_IpEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

					ReConnect();
				}
			}
		}

		public void ReConnect()
		{
			try
			{
				if (m_CliSocket.Connected)
				{
					ReinitSocket();
				}

				if (m_IpEndPoint != null)
				{
					Debug.Log("Begin to connect server ip:" + m_IpEndPoint.Address.ToString() + " port:" + m_IpEndPoint.Port.ToString());

					m_bConnecting = true;
					m_CliSocket.BeginConnect(m_IpEndPoint.Address, m_IpEndPoint.Port, new AsyncCallback(ConnectCallback), this);
				}
				else
				{
					Debug.LogError("ServerConnect.ReConnect m_IpEndPoint can not be null");
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				//Debug.LogError( "Connect exception " + e.ToString() );
			}
		}

		public void ConnectCallback(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				m_bConnecting = false;

				try
				{
					m_CliSocket.EndConnect(ar);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					//Debug.LogError( "Connect callback exception " + e.ToString() );
					ReinitSocket();
				}

                INetWorkMessage msg = new GAMEMSG_SYSTEM_connect(m_CliSocket.Connected);
                m_ReadMsgQueue.Enqueue(msg);

				if (m_CliSocket.Connected)
				{
					Debug.Log(" Server connect suc");
					BeginRecv();
				}
				else
				{
					Debug.Log(" Server connect fail");
				}
			}
		}

		public void BeginRecv()
		{
			try
			{
				m_CliSocket.BeginReceive(m_Readbuffer.getRealBuffer(),
					m_Readbuffer.getMaxDataPostion(), m_Readbuffer.getCapability(), 0, new AsyncCallback(ReceiveCallback), this);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				//Debug.LogError( "Begin recieve msg exception " + e.ToString());
			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			//		if ( ar.IsCompleted )
			//		{
			try
			{
				NetReadBuffer buffer = m_Readbuffer;
				int nReadDataLen = m_CliSocket.EndReceive(ar);

				if (nReadDataLen == 0)
				{
					ReinitSocket();
					PostDisconnectMsg();
				}
				else
				{
					buffer.setMaxDataPostion(buffer.getMaxDataPostion() + nReadDataLen);
					DecodeMsg_Cumulative();
					m_Readbuffer.Settle();

					BeginRecv();
				}
			}
			catch (Exception e)
			{
				//Debug.LogError( "Recieve callback exception " + e.ToString() );
				Debug.LogException(e);
				ReinitSocket();
				PostDisconnectMsg();
			}
			//		}
		}

		private void DecodeMsg_Cumulative()
		{
			while (true)
			{
				int nOldPostion = m_Readbuffer.getPostion();

				bool decoded = doDecode();
				if (decoded)
				{
					if (m_Readbuffer.getPostion() == nOldPostion)
					{
						Debug.LogError("doDecode() can't return true when buffer is not consumed");
						break;
					}
				}
				else
				{
					break;
				}
			}
		}

		private bool doDecode()
		{
			if (m_Readbuffer.remaining() > 1)
			{
				int pos = m_Readbuffer.getPostion();

				int size = m_Readbuffer.GetUShort();
				if (size >= 4 && size <= 65535)
				{
					if (size - 2 <= m_Readbuffer.remaining())
					{
						GameMsgType Type = (GameMsgType)m_Readbuffer.GetUShort();

						INetWorkMessage gamemsg = MsgFactory.CreateMsgByType(Type);
						if (gamemsg != null)
						{
							if (gamemsg.decodeMessage(m_Readbuffer))
							{
								if (m_Readbuffer.getPostion() - pos != size)
								{
									Debug.LogError("handleXQMsg position error,Postion :"
										+ m_Readbuffer.getPostion() + ","
										+ pos + ","
										+ size + ",ID:" + Type);
									m_Readbuffer.setMaxDataPostion(pos + size);
									m_Readbuffer.setPostion(pos + size);
								}
								else
								{
									m_ReadMsgQueue.Enqueue(gamemsg);
								}
							}
							else
							{
								Debug.LogError("Decode message failed, Type:" + Type + ",Size:" + size);
								//m_Readbuffer.Clear();
							}
						}

						return true;

					}
					else
					{
						m_Readbuffer.setPostion(pos);
					}
				}
				else
				{
					return true;
				}
			}

			return false;
		}

        public void doMyDecode(GameMsgType Type)
        {
            INetWorkMessage gamemsg = MsgFactory.CreateMsgByType(Type);
            if (gamemsg != null)
            {                
                m_ReadMsgQueue.Enqueue(gamemsg);
            }
        }

		public INetWorkMessage GetMessage()
		{
			mTempMsg = null;

			if (m_ReadMsgQueue.Count > 0)
			{
				mTempMsg = m_ReadMsgQueue.Dequeue();
			}

			return mTempMsg;
		}

		//Send
		public void SendMessage(GameMsgBase Msg)
		{
			//writeDone.WaitOne();
			m_WriteMsgQueue.Enqueue(Msg);
			//writeDone.ReleaseMutex();

			//TrySendMsg();
		}

		public void TrySendMsg()
		{
			if (!m_bSending)
			{
				try
				{
					while (m_WriteMsgQueue.Count > 0)
					{
						INetWorkMessage gamemsg = m_WriteMsgQueue.Peek();

						if (!gamemsg.encodeMessage(m_Writebuffer))
						{
							break;
						}
						m_WriteMsgQueue.Dequeue();
					}

					//writeDone.ReleaseMutex();

					if (m_Writebuffer.getPostion() > 0)
					{
						m_bSending = true;
						m_CliSocket.BeginSend(m_Writebuffer.getWriteBuffer(), 0, m_Writebuffer.getPostion(), 0, new AsyncCallback(SendCallback), this);
					}
				}
				catch (Exception e)
				{
					//Debug.LogError( "Send exception " + e.ToString() );
					Debug.LogException(e);
					m_bSending = false;
				}
			}
		}

		void SendCallback(IAsyncResult ar)
		{
			//		if ( ar.IsCompleted )
			//		{
			try
			{
				m_Writebuffer.Clear();
				m_CliSocket.EndSend(ar);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			//TrySendMsg();
			//		}
			m_bSending = false;
		}

		void ReinitSocket()
		{
			try
			{
				m_CliSocket.Close();
				if (m_IpEndPoint != null)
				{
					m_CliSocket = new Socket(m_IpEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				}
				else
				{
					Debug.LogError("ServerConnect.ReinitSocket m_IpEndPoint can not be empty");
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				//Debug.LogError( "Reinit socket exception " + e.ToString() );
			}
		}

		void PostDisconnectMsg()
		{
            INetWorkMessage msg = new GAMEMSG_SYSTEM_disconnect();
            m_ReadMsgQueue.Enqueue(msg);
		}
	}
}