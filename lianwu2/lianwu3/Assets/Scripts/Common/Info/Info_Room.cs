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

    public enum CreateRoomType : byte
    {
        Unknown,	// eRoomType_Unknown eRoomType_None	未定义的房间

        Normal,		// eRoomType_Common					普通房间
        ProcGuide,	// eRoomType_NewPlayer				新手房间
        Lantern,	// eRoomType_Dungeon				副本
        Ceremony,	// eRoomType_DanceGroupCeremony		舞团入团仪式
        BigMama,	// eRoomType_BigMama				广场舞大妈
        Fairy,		// eRoomType_FairlyLandRoom			舞团秘境

        GroupAmuse,			// eRoomType_GroupAmuseRoom			全局开放场景
        CatWalkBackStage,	// eRoomType_TShowBackroom			T台秀后台

        WeddingRoom,		// eRoomType_WeddingRoom			婚房
        AmuseRoom,			// eRoomType_AmuseRoom				开放性场景
        // eRoomType_Max
    }
}

