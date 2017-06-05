using LoveDance.Client.Network.Login;
using LoveDance.Client.Network.HeartBeat;
using LoveDance.Client.Network.Currency;
using LoveDance.Client.Network.Room;
using LoveDance.Client.Network.Lantern;

namespace LoveDance.Client.Network
{
	public class MsgFactoryBuilder
	{
		public static void BuilderFactory()
		{
			//Login
            MsgFactory.AddProductLine(GameMsg_LoginResult.CreateMsg, GameMsgType.MSG_ACCOUNT_LoginResult);
            MsgFactory.AddProductLine(GameMsg_S2C_RequireCreateRole.CreateMsg, GameMsgType.MSG_S2C_RequireCreateRole);
            MsgFactory.AddProductLine(GameMsg_HeartBeatRequest.CreateMsg, GameMsgType.MSG_ACCOUNT_HeartBeatRequest);
            MsgFactory.AddProductLine(GameMsg_S2C_CreateAccountRes.CreateMsg, GameMsgType.MSG_ACCOUNT_CreateAccountResult);
            MsgFactory.AddProductLine(GameMsg_S2C_AllowCurrencyList.CreateMsg, GameMsgType.MSG_S2C_AllowCurrencyList);	//Currency
            MsgFactory.AddProductLine(GameMsg_S2C_StartRoomSuc.CreateMsg, GameMsgType.MSG_S2C_StartRoomSuc);
            MsgFactory.AddProductLine(GameMsg_S2C_CreateRoomSuc.CreateMsg, GameMsgType.MSG_S2C_CreateRoomSuc);
		}
	}
}