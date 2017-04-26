using System.Collections.Generic;

namespace LoveDance.Client.Network.Login
{
	public enum CreateAccountRes : byte
	{
		NONE,
		CREATE_SUCCESS,
		INVALID_PASSWORD_LENGTH,	//密码长度过长	
		INVALID_NAME_LENGTH,		//名称长度过长
		CONTAIN_INVALID_WORDS,		//名称包含非法字符
		INVALID_NAME_OR_PASSWORD,	//名称不对
		DUPLICATED_ACCOUNT_NAME,	//重复的注册名称
		DUPLICATED_ACCOUNT_UUID,	//重复UUID,由合作平台传递
	};


	public class GameMsg_C2S_Login : GameMsgBase
	{
		public int m_nLine = 0;
		public byte m_PackageType = 0;
		public byte m_nVID = 0;
		public ushort m_nPID = 0;
		public string m_nPUID = "";
		public string m_strToken = "";
		public string m_phoneNumber = "";
		public string m_strAccountName = "";
		public byte[] m_arPwd = null;

		public GameMsg_C2S_Login()
			: base(GameMsgType.MSG_ACCOUNT_Login)
		{
		}

		public override bool doEncode(NetWriteBuffer DataOut)
		{
			DataOut.PutInt(m_nLine);
			DataOut.PutByte(m_PackageType);
			DataOut.PutByte(m_nVID);
			DataOut.PutUShort(m_nPID);
			DataOut.PutString(m_nPUID);
			DataOut.PutString(m_strToken);
			DataOut.PutString(m_phoneNumber);
			DataOut.PutString(m_strAccountName);
			DataOut.PutBytes(m_arPwd);

			return true;
		}
	}


	//fufeng new todo: use two msg for the differences
	public class GameMsg_LoginResult : GameMsgBase
	{
		public int nResult;
		public string m_strRecTag;
		public string m_strError;
		public GameMsg_LoginResult()
			: base(GameMsgType.MSG_ACCOUNT_LoginResult)
		{
		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			nResult = DataIn.GetInt();
			m_strRecTag = DataIn.GetPerfixString();
			m_strError = DataIn.GetPerfixString();

			return true;
		}

		public static GameMsgBase CreateMsg()
		{
			return new GameMsg_LoginResult();
		}
	}


	public class GameMsg_C2S_PlayerLogout : GameMsgBase
	{
		public GameMsg_C2S_PlayerLogout()
			: base(GameMsgType.MSG_ACCOUNT_C2S_PlayerLogout)
		{
		}

		public override bool doEncode(NetWriteBuffer DataOut)
		{
			return true;
		}
	}


	public class GameMsg_C2S_CreateAccount : GameMsgBase
	{
		public byte m_PackageType = 0;
		public byte m_nVID = 0;
		public string m_strAccount = "";
		public byte[] m_arPwd = null;

		public string m_strIDCard = "";
		public byte m_nSex = 0;

		public GameMsg_C2S_CreateAccount()
			: base(GameMsgType.MSG_ACCOUNT_CreateAccount)
		{
		}

		public override bool doEncode(NetWriteBuffer DataOut)
		{
			DataOut.PutByte(m_PackageType);
			DataOut.PutByte(m_nVID);
			DataOut.PutString(m_strAccount);
			DataOut.PutBytes(m_arPwd);
			DataOut.PutString(m_strIDCard);
			DataOut.PutByte(m_nSex);

			return true;
		}
	}

	//fufeng new todo: use two msg for the differences
	public class GameMsg_S2C_CreateAccountRes : GameMsgBase
	{
		public byte m_nRes = 0;
		public string m_strError = "";

		public GameMsg_S2C_CreateAccountRes()
			: base(GameMsgType.MSG_ACCOUNT_CreateAccountResult)
		{
		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			if (DataIn != null)
			{
				m_nRes = DataIn.GetByte();
				m_strError = DataIn.GetPerfixString();
			}
			return true;
		}

		public static GameMsgBase CreateMsg()
		{
			return new GameMsg_S2C_CreateAccountRes();
		}
	}


	public class GameMsg_S2C_RequireCreateRole : GameMsgBase
	{
		public List<string> m_LoadingAD = new List<string>();

		public GameMsg_S2C_RequireCreateRole()
			: base(GameMsgType.MSG_S2C_RequireCreateRole)
		{

		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			m_LoadingAD.Clear();
			ushort count = DataIn.GetUShort();
			for (int i = 0; i < count; ++i)
			{
				m_LoadingAD.Add(DataIn.GetPerfixString());
			}

			return true;
		}

		public static GameMsgBase CreateMsg()
		{
			return new GameMsg_S2C_RequireCreateRole();
		}
	}
}