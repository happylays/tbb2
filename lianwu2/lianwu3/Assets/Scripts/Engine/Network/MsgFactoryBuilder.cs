using LoveDance.Client.Network.Login;

namespace LoveDance.Client.Network
{
	public class MsgFactoryBuilder
	{
		public static void BuilderFactory()
		{
			//Login
            MsgFactory.AddProductLine(GameMsg_LoginResult.CreateMsg, GameMsgType.MSG_ACCOUNT_LoginResult);				
		}
	}
}