using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//房间相关
namespace LoveDance.Client.Common
{
    public enum RoleRoomType : byte
    {
        None,

        Dancer,
        Audience,

        Max,
    }

    public enum RoleRoomState : byte
    {
        None,

        Wait,
        Equip,
        Ready,
        ToStart,
        Start,
        ToEnd,
        End,
    }


    public enum RoomPosState : byte
    {
        Open,
        Close,
    }


    public enum RoomDanceMode : byte
    {
        SINGLE = 0,
        TEAM
    }
}
