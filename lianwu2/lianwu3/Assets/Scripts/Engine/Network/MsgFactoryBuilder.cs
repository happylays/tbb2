using LoveDance.Client.Network.Login;

namespace LoveDance.Client.Network
{
	public class MsgFactoryBuilder
	{
		public static void BuilderFactory()
		{
			//Login
            MsgFactory.AddProductLine(GameMsg_LoginResult.CreateMsg, GameMsgType.MSG_ACCOUNT_LoginResult);
            MsgFactory.AddProductLine(GameMsg_S2C_RequireCreateRole.CreateMsg, GameMsgType.MSG_S2C_RequireCreateRole);				
		}
	}
}