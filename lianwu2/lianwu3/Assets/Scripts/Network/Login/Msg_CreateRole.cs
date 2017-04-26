using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Login
{
	public class GameMsg_C2S_CreateRole : GameMsgBase
	{
		public string m_strName;
		public string m_strSignature;
//		public Sex_Type m_nSex;
		public byte m_nSkin;
		public string m_strYear;
		public string m_strMon;
		public string m_strDay;

		public GameMsg_C2S_CreateRole()
			: base(GameMsgType.MSG_C2S_CreateRole)
		{
			m_strName = "";
			m_strSignature = "";
			//m_nSex = 0;
			m_nSkin = 0;
			m_strYear = "";
			m_strMon = "";
			m_strDay = "";
		}

		public override bool doEncode(NetWriteBuffer DataOut)
		{
			DataOut.PutString(m_strName);
			//DataOut.PutByte((byte)m_nSex);
			DataOut.PutString(m_strSignature);
			DataOut.PutString(m_strYear);
			DataOut.PutString(m_strMon);
			DataOut.PutString(m_strDay);
			DataOut.PutByte(m_nSkin);
			return true;
		}
	}

	public class GameMsg_S2C_CreateRoleSuc : GameMsgBase
	{
		public Dictionary<string, NetReadBuffer> m_RolePacketDatas = new Dictionary<string, NetReadBuffer>();
		public string m_strPayment = "";
		public bool m_bSwitchLine;

		public GameMsg_S2C_CreateRoleSuc()
			: base(GameMsgType.MSG_S2C_CreateRoleSuc)
		{
		}

		public NetReadBuffer GetPacketBuffer(string Packet)
		{
			if (m_RolePacketDatas.ContainsKey(Packet))
			{
				return m_RolePacketDatas[Packet];
			}
			return null;
		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			ushort nCount = DataIn.GetUShort();

			for (int i = 0; i < nCount; i++)
			{
				byte[] buffer = DataIn.GetFixLenBytes();
				if (buffer != null)
				{
					string PartName = DataIn.GetPerfixString();
					m_RolePacketDatas.Add(PartName, new NetReadBuffer(buffer));
				}

			}

			// fufeng add: extra info
			byte[] boardBuf = DataIn.GetFixLenBytes();
			if (boardBuf != null)
			{
				string boardName = DataIn.GetPerfixString();
				m_RolePacketDatas.Add(boardName, new NetReadBuffer(boardBuf));
			}

			//music list info
			byte[] musicsBuf = DataIn.GetFixLenBytes();
			if (musicsBuf != null)
			{
				string musicsName = DataIn.GetPerfixString();
				m_RolePacketDatas.Add(musicsName, new NetReadBuffer(musicsBuf));
			}

			byte[] systemSettingBuf = DataIn.GetFixLenBytes();
			if (systemSettingBuf != null)
			{
				string systemName = DataIn.GetPerfixString();
				m_RolePacketDatas.Add(systemName, new NetReadBuffer(systemSettingBuf));
			}

			// mall data 
			//byte[] mallDataBuff = DataIn.GetFixLenBytes();
			//if (mallDataBuff != null)
			//{
			//    string mallName = DataIn.GetPerfixString();
			//    m_RolePacketDatas.Add(mallName, new NetReadBuffer(mallDataBuff));
			//}

			// mall commend 
			byte[] mallCommendBuff = DataIn.GetFixLenBytes();
			if (mallCommendBuff != null)
			{
				string mallCommendName = DataIn.GetPerfixString();
				m_RolePacketDatas.Add(mallCommendName, new NetReadBuffer(mallCommendBuff));
			}

			byte[] festivalConfigBuf = DataIn.GetFixLenBytes();
			if (festivalConfigBuf != null)
			{
				string festivalName = DataIn.GetPerfixString();
				m_RolePacketDatas.Add(festivalName, new NetReadBuffer(festivalConfigBuf));
			}

			//fufeng new todo: use two msg for the differences
			//非点点乐平台设置Payment
			m_strPayment = DataIn.GetUTF8String();

			m_bSwitchLine = DataIn.GetBool();
			return true;
		}

		public static GameMsgBase CreateMsg()
		{
			return new GameMsg_S2C_CreateRoleSuc();
		}
	}

	public class GameMsg_S2C_CreateRoleFail : GameMsgBase
	{
		public string m_strError;

		public GameMsg_S2C_CreateRoleFail()
			: base(GameMsgType.MSG_S2C_CreateRoleFail)
		{
		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			if (DataIn != null)
			{
				m_strError = DataIn.GetPerfixString();
			}
			return true;
		}

		public static GameMsgBase CreateMsg()
		{
			return new GameMsg_S2C_CreateRoleFail();
		}
	}
}