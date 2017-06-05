
using LoveDance.Client.Network;
using LoveDance.Client.Network.Login;
using LoveDance.Client.Network.HeartBeat;

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

            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_HeartBeatRequest, this.OnHeartBeatRequest);
            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_LoginResult, OnRequireRole);
            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_S2C_RequireCreateRole, this.OnRequireRole);
            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_SYSTEM_Connect, this.OnConnected);

            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_ACCOUNT_CreateAccountResult, this.OnCreateAccountRes);

            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_S2C_StartRoomSuc, this.OnSelectRole);
            NetObserver.AddNetMsgProcessor(GameMsgType.MSG_S2C_CreateRoomSuc, this.OnCreateRoom);
        }
        
        void OnRoleLoginRes(GameMsgBase msg)
        {
            return;

            GameMsg_LoginResult resMsg = msg as GameMsg_LoginResult;
            //if (resMsg != null && resMsg.nResult == 0)
            //{
            cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);
            //}
        }

        void OnRequireRole(GameMsgBase msg)
        {
            cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);
        }

        void OnSelectRole(GameMsgBase msg)
        {
            GameMsg_S2C_RequireCreateRole resMsg = msg as GameMsg_S2C_RequireCreateRole;
            //if (resMsg != null && resMsg.nResult == 0)
            //{
                cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Game); // SelectStage
            //}
        }

        void OnCreateRoom(GameMsgBase msg)
        {            
            cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Room); // SelectStage
            
        }

        void OnHeartBeatRequest(GameMsgBase msg)
        {
            GameMsg_HeartBeatResponse responseMsg = new GameMsg_HeartBeatResponse();
            NetworkMgr.SendMsg(responseMsg);
        }

        void OnConnected(GameMsgBase msg)
        {
            GAMEMSG_SYSTEM_connect connMsg = msg as GAMEMSG_SYSTEM_connect;
            if (connMsg != null)
            {
                if (!connMsg.m_bSucceed)
                {
                    //string strTips = SystemTips.GetTipsContent("All_ConnectFail");
                    //GameExitControl.ShowYesBox(strTips, null);
                }
                else
                {
                    NetworkMgr.SendLoginMessage();
                }
            }
        }

        void OnCreateAccountRes(GameMsgBase msg)
        {
            GameMsg_S2C_CreateAccountRes resMsg = msg as GameMsg_S2C_CreateAccountRes;
            if (resMsg != null)
            {
                if (resMsg.m_nRes != (byte)CreateAccountRes.CREATE_SUCCESS)
                {
                    //TipsBoxMgr.ShowOKTipsBox(SystemTips.GetTipsContent(resMsg.m_strError), null);
                }
                else
                {
                    //_RemeberRealAcc();
                }
            }
        }
    }
}