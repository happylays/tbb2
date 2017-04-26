using LoveDance.Client.Network;

namespace LoveDance.Client.Logic
{
	/// <summary>
	/// 所有Logic的基类
	/// </summary>
	public class BaseLogic
	{
		private NetMsgObserver mNetObserver = null;	//监听者

		public BaseLogic(NetMsgObserver netObserver)
		{
			mNetObserver = netObserver;
		}

		protected NetMsgObserver NetObserver
		{
			get
			{
				return mNetObserver;
			}
		}

		/// <summary>
		/// 注册网络消息
		/// </summary>
		public virtual void RegistNetMessage()
		{

		}

		/// <summary>
		/// 注销网络消息
		/// </summary>
		public virtual void UnRegistNetMessage()
		{
			mNetObserver.ClearNetMsgProcessor();
		}
	}
}
