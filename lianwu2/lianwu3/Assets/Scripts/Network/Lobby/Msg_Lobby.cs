using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Network.Room;

namespace LoveDance.Client.Network.Lantern
{
    public class RoomBriefInfo
    {
        public uint m_nRoomID = 0;
        public string m_strRoomName = "";
        public bool m_bHasPwd = false;
        public bool m_bHasStart = false;
        public byte m_nModeID = (byte)SongMode.None;
        public byte m_nDifficulty = 0;
        public byte m_nRoomColor = 0;
        public byte m_nRoomCapacity = CommonDef.MAX_ROOM_PLAYER;
        public bool m_bTop = false;
        public byte[] m_szPlayerSex = new byte[CommonDef.MAX_ROOM_PLAYER];

        public void doDecode(NetReadBuffer DataIn)
        {
            m_nRoomID = DataIn.GetUInt();
            m_strRoomName = DataIn.GetPerfixString();
            m_bHasPwd = DataIn.GetBool();
            m_bHasStart = DataIn.GetBool();
            m_nModeID = DataIn.GetByte();
            m_nDifficulty = DataIn.GetByte();
            m_nRoomColor = DataIn.GetByte();
            m_nRoomCapacity = DataIn.GetByte();
            m_bTop = DataIn.GetBool();

            for (int i = 0; i < m_szPlayerSex.Length; ++i)
            {
                m_szPlayerSex[i] = DataIn.GetByte();
            }
        }
    }

    public class RoomWholeInfo
    {
        public uint m_nRoomID = 0;
        public string m_strRoomName = "";
        public byte m_nRoomColor = 0;
        public string m_strRoomPwd = "";
        public bool m_bHasPwd = false;
        public bool m_bIsHost = false;
        public byte m_nScene = CommonDef.SCENE_RANDOM_ID;
        public short m_nMusic = CommonDef.SONG_RANDOM_ID;
        public byte m_nMode = (byte)SongMode.None;
        public byte m_nRoomDanceMode = (byte)RoomDanceMode.SINGLE;
        public byte[] m_szPosDancer = new byte[CommonDef.MAX_ROOM_PLAYER];
        public byte[] m_szPosAudience = new byte[CommonDef.MAX_ROOM_AUDIENCE];
        public uint m_ClothEffectTransformID = 0;

        public List<RoomPlayerInfo> m_lstDancer = new List<RoomPlayerInfo>();        

        public void doDecode(NetReadBuffer DataIn)
        {
            m_nRoomID = DataIn.GetUInt();
            m_strRoomName = DataIn.GetPerfixString();
            m_nRoomColor = DataIn.GetByte();
            m_strRoomPwd = DataIn.GetPerfixString();
            m_bHasPwd = DataIn.GetBool();
            m_bIsHost = DataIn.GetBool();
            m_nScene = DataIn.GetByte();
            m_nMusic = DataIn.GetShort();
            m_nMode = DataIn.GetByte();
            m_nRoomDanceMode = DataIn.GetByte();
            m_ClothEffectTransformID = DataIn.GetUInt();

            for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; ++i)
            {
                m_szPosDancer[i] = DataIn.GetByte();
            }

            for (int i = 0; i < CommonDef.MAX_ROOM_AUDIENCE; ++i)
            {
                m_szPosAudience[i] = DataIn.GetByte();
            }

            ushort nDancerCount = DataIn.GetUShort();
            for (int i = 0; i < nDancerCount; ++i)
            {
                RoomPlayerInfo playerInfo = new RoomPlayerInfo();
                playerInfo.doDecode(DataIn);

                m_lstDancer.Add(playerInfo);
            }

            
        }
    }

    public class GameMsg_C2S_EnterLobby : GameMsgBase
    {
        public GameMsg_C2S_EnterLobby()
            : base(GameMsgType.MSG_C2S_EnterLobby)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }

    public class GameMsg_C2S_ExitLobby : GameMsgBase
    {
        public GameMsg_C2S_ExitLobby()
            : base(GameMsgType.MSG_C2S_ExitLobby)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }

    public class GameMsg_C2S_GetRoomList : GameMsgBase
    {
        public bool m_bAll = false;
        public byte m_nMode = (byte)SongMode.None;
        public bool m_bOnlySuper = false;
        public ushort m_nPage = 0; //页数从0开始

        public GameMsg_C2S_GetRoomList()
            : base(GameMsgType.MSG_C2S_GetRoomList)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutBool(m_bAll);
            DataOut.PutByte(m_nMode);
            DataOut.PutBool(m_bOnlySuper);
            DataOut.PutUShort(m_nPage);

            return true;
        }
    }

    public class GameMsg_S2C_GetRoomListRes : GameMsgBase
    {
        public ushort m_nCurPage = 0;
        public ushort m_nTotalCount = 0;
        ushort m_nCurCount = 0;
        public List<RoomBriefInfo> m_RoomInfoList = new List<RoomBriefInfo>();

        public GameMsg_S2C_GetRoomListRes()
            : base(GameMsgType.MSG_S2C_GetRoomListRes)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nCurPage = DataIn.GetUShort();
            m_nTotalCount = DataIn.GetUShort();
            m_nCurCount = DataIn.GetUShort();

            for (int i = 0; i < m_nCurCount; ++i)
            {
                RoomBriefInfo roomItem = new RoomBriefInfo();
                roomItem.doDecode(DataIn);

                m_RoomInfoList.Add(roomItem);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_GetRoomListRes();
        }
    }

    public class GameMsg_C2S_CreateRoom : GameMsgBase
    {
        public string m_strRoomName = "";
        public string m_strRoomPwd = "";
        public byte m_nRoomType = (byte)CreateRoomType.Normal;
        public byte m_nPhoneOS = (byte)Phone_OS.None;
        public bool m_ThemeRoom = false;

        public GameMsg_C2S_CreateRoom()
            : base(GameMsgType.MSG_C2S_CreateRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutString(m_strRoomName);
            DataOut.PutString(m_strRoomPwd);
            DataOut.PutByte(m_nRoomType);
            DataOut.PutByte(m_nPhoneOS);
            DataOut.PutBool(m_ThemeRoom);

            return true;
        }
    }

    public class GameMsg_S2C_CreateRoomSuc : GameMsgBase
    {
        public RoomWholeInfo m_RoomInfo = new RoomWholeInfo();

        public GameMsg_S2C_CreateRoomSuc()
            : base(GameMsgType.MSG_S2C_CreateRoomSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_RoomInfo.doDecode(DataIn);

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_CreateRoomSuc();
        }
    }

    public class GameMsg_S2C_CreateRoomFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_CreateRoomFail()
            : base(GameMsgType.MSG_S2C_CreateRoomFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_CreateRoomFail();
        }
    }


    public class GameMsg_C2S_EnterRoom : GameMsgBase
    {
        public uint m_nRoomID = 0;
        public bool m_bAudience = false;
        public string m_strRoomPwd = "";

        public GameMsg_C2S_EnterRoom()
            : base(GameMsgType.MSG_C2S_EnterRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutUInt(m_nRoomID);
            DataOut.PutBool(m_bAudience);
            DataOut.PutString(m_strRoomPwd);

            return true;
        }
    }


    public class GameMsg_S2C_EnterRoomFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_EnterRoomFail()
            : base(GameMsgType.MSG_S2C_EnterRoomFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_EnterRoomFail();
        }
    }

    public class GameMsg_C2S_ApplyMatch : GameMsgBase
    {
        public byte m_nMode = (byte)SongMode.None;
        public byte m_nPhoneOS = (byte)Phone_OS.None;

        public GameMsg_C2S_ApplyMatch()
            : base(GameMsgType.MSG_C2S_ApplyMatch)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nMode);
            DataOut.PutByte(m_nPhoneOS);

            return true;
        }
    }

    public class GameMsg_C2S_CancelMatch : GameMsgBase
    {
        public GameMsg_C2S_CancelMatch()
            : base(GameMsgType.MSG_C2S_CancelMatch)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }

    public class GameMsg_S2C_PrepareMatch : GameMsgBase
    {
        public RoomWholeInfo m_RoomInfo = new RoomWholeInfo();

        public byte m_nPlayScene = CommonDef.SCENE_RANDOM_ID;
        public short m_nPlayMusic = CommonDef.SONG_RANDOM_ID;
        public byte m_nPlayMode = (byte)SongMode.None;
        public string m_strCheckKey = "";
        public byte[] m_szStage = null;

        public GameMsg_S2C_PrepareMatch()
            : base(GameMsgType.MSG_S2C_PrepareMatch)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_RoomInfo.doDecode(DataIn);

            m_nPlayScene = DataIn.GetByte();
            m_nPlayMusic = DataIn.GetShort();
            m_nPlayMode = DataIn.GetByte();
            m_strCheckKey = DataIn.GetPerfixString();
            m_szStage = DataIn.GetFixLenBytes();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_PrepareMatch();
        }
    }

    public class GameMsg_C2S_InviteeResponse : GameMsgBase
    {
        public byte m_nRoomType = 0;
        public bool m_bAccept = false;
        public byte m_nPhoneOS = (byte)Phone_OS.None;
        public uint m_nRoomID = 0;

        public GameMsg_C2S_InviteeResponse()
            : base(GameMsgType.MSG_C2S_InviteeResponse)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            DataOut.PutBool(m_bAccept);
            DataOut.PutByte(m_nPhoneOS);
            DataOut.PutUInt(m_nRoomID);

            return true;
        }
    }

    public class GameMsg_S2C_InviteeFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_InviteeFail()
            : base(GameMsgType.MSG_S2C_InviteeFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_InviteeFail();
        }
    }

    public class GameMsg_S2C_InviteeNotice : GameMsgBase
    {
        public byte m_nRoomType = 0;
        public uint m_nRoomID = 0;
        public byte m_nSongMode = (byte)SongMode.None;
        public byte m_chDifficulty = 0;
        public uint m_nInviterId = 0;
        public string m_strInviterName = "";
        public bool m_bIsVIP = false;
        public ushort m_nVIPLevel = 0;

        public GameMsg_S2C_InviteeNotice()
            : base(GameMsgType.MSG_S2C_InviteeNotice)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoomType = DataIn.GetByte();
            m_nRoomID = DataIn.GetUInt();
            m_nSongMode = DataIn.GetByte();
            m_chDifficulty = DataIn.GetByte();
            m_nInviterId = DataIn.GetUInt();
            m_strInviterName = DataIn.GetPerfixString();
            m_bIsVIP = DataIn.GetBool();
            m_nVIPLevel = DataIn.GetUShort();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_InviteeNotice();
        }
    }

    public class GameMsg_C2S_TryEnterRoom : GameMsgBase
    {
        public bool m_bAudience = false;
        public byte m_nPhoneOS = (byte)Phone_OS.None;
        public uint m_nRoomID = 0;

        public GameMsg_C2S_TryEnterRoom()
            : base(GameMsgType.MSG_C2S_TryEnterRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutBool(m_bAudience);
            DataOut.PutByte(m_nPhoneOS);
            DataOut.PutUInt(m_nRoomID);
            return true;
        }
    }

    public class GameMsg_S2C_RoomRequriePwd : GameMsgBase
    {
        public uint m_nRoomID;

        public GameMsg_S2C_RoomRequriePwd()
            : base(GameMsgType.MSG_S2C_RoomRequirePwd)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoomID = DataIn.GetUInt();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_RoomRequriePwd();
        }
    }
}