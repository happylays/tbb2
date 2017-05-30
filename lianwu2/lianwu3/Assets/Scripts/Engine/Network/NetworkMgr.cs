
//网络消息入口
namespace LoveDance.Client.Network
{
	public class NetworkMgr
	{
		private static NetworkMgr mInstance = new NetworkMgr();
		private ServerConnect m_NetConn = new ServerConnect();
		private GameMsgBase m_LoginMsg = null;

		private GameMsgBase mTempMsg = null;

		/// <summary>
		/// 是否与服务器连接
		/// </summary>
		public static bool IsConnected
		{
			get
			{
				if (mInstance.m_NetConn != null)
				{
					return mInstance.m_NetConn.IsConnected;
				}

				return false;
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public static void InitNetwork()
		{
			MsgFactoryBuilder.BuilderFactory();

			mInstance.m_NetConn.InitServerConnect();
		}

        public static void DoMessage(GameMsgType Type)
        {
            mInstance.m_NetConn.doMyDecode(Type);
        }

		/// <summary>
		/// 处理消息
		/// </summary>
		public static void ProcessNetwork()
		{
			if (mInstance.m_NetConn != null)
			{
				mInstance.mTempMsg = null;
				while ((mInstance.mTempMsg = (GameMsgBase)mInstance.m_NetConn.GetMessage()) != null)
				{
					OnProcessMsg(mInstance.mTempMsg);
				}

				mInstance.m_NetConn.TrySendMsg();
			}
		}

		/// <summary>
		/// 重新连接
		/// </summary>
		public static void ReConnectNet()
		{
			if (mInstance.m_NetConn != null)
			{
                if (mInstance.m_NetConn.IsConnected)
                {
                    mInstance.m_NetConn = new ServerConnect();
                    mInstance.m_NetConn.InitServerConnect();
                }
			}
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		public static void SendMsg(GameMsgBase msg)
		{

            if (mInstance.m_NetConn != null)
            {
                if (mInstance.m_NetConn.IsConnected)
                {
                    mInstance.m_NetConn.SendMessage(msg);
                }
                else
                {
                    if (msg.getMsgType() == GameMsgType.MSG_ACCOUNT_Login || msg.getMsgType() == GameMsgType.MSG_ACCOUNT_CreateAccount)
                    {
                        mInstance.m_NetConn.Connect("127.0.0.1", 7750);
                        mInstance.m_LoginMsg = msg;
                    }
                }
            }
		}

		/// <summary>
		/// 发送登录消息
		/// </summary>
		public static void SendLoginMessage()
		{
			if (mInstance.m_LoginMsg != null)
			{
				mInstance.m_NetConn.SendMessage(mInstance.m_LoginMsg);
				mInstance.m_LoginMsg = null;
			}
		}

		static void OnProcessMsg(GameMsgBase Msg)
		{
			NetMsgMap.DispatchNetMsg(Msg);
		}
	}
}