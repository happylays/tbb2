using LoveDance.Client.Common;
using LoveDance.Client.Data.Tips;
using LoveDance.Client.Logic.Scene;
using LoveDance.Client.Network;
using LoveDance.Client.Network.Lantern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoveDance.Client.Logic.Room
{
    public class RoomLogic : RoomBaseLogic
    {
        /// <summary>
        /// 带参数构造方法
        /// </summary>
        public RoomLogic(NetMsgObserver netObserver)
            : base(netObserver)
        {
        }

        public static RoomLogic CreateLogic(NetMsgObserver NetObserver)
        {
            return new RoomLogic(NetObserver);
        }

        public void RegistNetMessage()
        {
            base.RegistNetMessage();

            //NetObserver.AddNetMsgProcessor(GameMsgType.MSG_S2C_CreateRoomSuc, this.OnCreateRoom);

        }

        void OnCreateRoom(GameMsgBase msg)
        {
            GameMsg_S2C_CreateRoomSuc res = msg as GameMsg_S2C_CreateRoomSuc;
            if (res != null)
            {
                SceneSwitchMgr.TrySwitch(new RoomSceneSwitch(CreateRoomType.Normal, res.m_RoomInfo));
            }
        }
    }
}