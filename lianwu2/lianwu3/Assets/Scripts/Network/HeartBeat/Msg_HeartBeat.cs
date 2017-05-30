
namespace LoveDance.Client.Network.HeartBeat
{
    public class GameMsg_HeartBeatRequest : GameMsgBase
    {
        public GameMsg_HeartBeatRequest()
            : base(GameMsgType.MSG_ACCOUNT_HeartBeatRequest)
        {
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_HeartBeatRequest();
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            return true;
        }
    }


    public class GameMsg_HeartBeatResponse : GameMsgBase
    {
        public GameMsg_HeartBeatResponse()
            : base(GameMsgType.MSG_ACCOUNT_HeartBeatResponse)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }
}