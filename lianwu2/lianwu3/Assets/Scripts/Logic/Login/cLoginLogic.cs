
using LoveDance.Client.Network;
using LoveDance.Client.Network.Login;

namespace LoveDance.Client.Logic.Login
{
    public class cLoginLogic : BaseLogic
    {
        /// <summary>
        /// 带参数构造方法
        /// </summary>
        public cLoginLogic(NetMsgObserver netObserver)
            : base(netObserver)
        {
        }

        public static cLoginLogic CreateLogic(NetMsgObserver NetObserver)
        {
            return new cLoginLogic(NetObserver);
        }

        public override void RegistNetMessage()
        {
            base.RegistNetMessage();
            
            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_LoginResult, OnRoleLoginRes);
            //NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_NotActivated, this.OnShowActiveWnd);
            //NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_CheckActivateResult, this.OnCheckActiveResult);
        }
        
        void OnRoleLoginRes(GameMsgBase msg)
        {
            GameMsg_LoginResult resMsg = msg as GameMsg_LoginResult;
            //if (resMsg != null && resMsg.nResult == 0)
            //{
                cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);
            //}
        }

    }
}