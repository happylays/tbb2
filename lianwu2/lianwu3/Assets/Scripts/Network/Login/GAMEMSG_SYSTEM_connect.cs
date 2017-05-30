
namespace LoveDance.Client.Network.Login
{
    public class GAMEMSG_SYSTEM_connect : GameMsgBase
    {
        public bool m_bSucceed = false;
        public int nConnCode = 0;
        public GAMEMSG_SYSTEM_connect(bool bSucceed)
            : base(GameMsgType.MSG_SYSTEM_Connect)
        {
            m_bSucceed = bSucceed;
        }
        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutInt(nConnCode);
            return true;
        }
    }
}
