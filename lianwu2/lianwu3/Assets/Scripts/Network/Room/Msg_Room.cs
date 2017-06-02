using System.Collections.Generic;
using System.Text;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Room
{
    public class InviteePlayerInfo
    {
        public uint m_nRoleID = 0;
        public string m_strRoleName = "";
        public byte m_nRoleLevel = 0;
        public byte m_nRoleSex = (byte)Sex_Type.None;
        public bool m_bIsVIP = false;
        public ushort m_nVIPLevel = 0;

        public void doDecode(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_strRoleName = DataIn.GetPerfixString();
            m_nRoleLevel = DataIn.GetByte();
            m_nRoleSex = DataIn.GetByte();
            m_bIsVIP = DataIn.GetBool();
            m_nVIPLevel = DataIn.GetUShort();
        }
    }

    public class RoleRoomTeamInfo
    {
        public uint m_nRoleID;
        public byte m_eRoomTeamMode;
        public byte m_eRoomTeamColor;

        public void doDecode(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_eRoomTeamMode = DataIn.GetByte();
            m_eRoomTeamColor = DataIn.GetByte();
        }
    }

    public class RoleTeamMarkInfo
    {
        public byte m_eRoomTeamColor;
        public int m_TeamMark;

        public void doDecode(NetReadBuffer DataIn)
        {
            m_eRoomTeamColor = DataIn.GetByte();
            m_TeamMark = DataIn.GetInt();
        }

        public override string ToString()
        {
            return "TeamColor" + m_eRoomTeamColor + " TeamMark:" + m_TeamMark;
        }
    }

    public class HeartBeatTeamInfo
    {
        public byte m_HeartBeatTeam = 0;
        public bool m_SelectEachOther = false;
        public List<uint> m_TeamMember = new List<uint>();

        public void doDecode(NetReadBuffer DataIn)
        {
            m_HeartBeatTeam = DataIn.GetByte();
            m_SelectEachOther = DataIn.GetBool();

            ushort count = DataIn.GetUShort();
            for (int i = 0; i < count; ++i)
            {
                uint id = DataIn.GetUInt();
                m_TeamMember.Add(id);
            }
        }
    }

    public class HeartBeatMarkInfo
    {
        public byte m_HeartBeatTeam = 0;
        public uint m_HeartBeatMark = 0;
        public uint m_HeartBeatTeamMark = 0;

        public void doDecode(NetReadBuffer DataIn)
        {
            m_HeartBeatTeam = DataIn.GetByte();
            m_HeartBeatMark = DataIn.GetUInt();
            m_HeartBeatTeamMark = DataIn.GetUInt();
        }
    }

    public class HeartBeatEffectInfo
    {
        public byte m_EffectLev = 0;
        public int m_HBPoint = 0;

        public void doDecode(NetReadBuffer DataIn)
        {
            m_EffectLev = DataIn.GetByte();
            m_HBPoint = DataIn.GetInt();
        }
    }

    public class GameMsg_C2S_StartRoom : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_StartRoom()
            : base(GameMsgType.MSG_C2S_StartRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            return true;
        }
    }

    public class GameMsg_C2S_ReadyRoom : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_ReadyRoom()
            : base(GameMsgType.MSG_C2S_ReadyRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            return true;
        }
    }


    public class GameMsg_S2C_StartRoomSuc : GameMsgBase
    {
        public GameMsg_S2C_StartRoomSuc()
            : base(GameMsgType.MSG_S2C_StartRoomSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_StartRoomSuc();
        }
    }

    public class GameMsg_S2C_StartRoomFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_StartRoomFail()
            : base(GameMsgType.MSG_S2C_StartRoomFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_StartRoomFail();
        }
    }

    public class GameMsg_C2S_EndRoom : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_EndRoom()
            : base(GameMsgType.MSG_C2S_EndRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            return true;
        }
    }

    public class GameMsg_S2C_EndRoomSuc : GameMsgBase
    {
        public byte[] m_szDancerPlace = null;
        public byte[] m_szDancerGrade = null;
        public uint[] m_szDancerMark = null;
        public uint[] m_szDancerExp = null;
        public int[] m_szDancerPerfect = null;
        public int[] m_szDancerCool = null;
        public int[] m_szDancerGood = null;
        public int[] m_szDancerBad = null;
        public int[] m_szDancerMiss = null;
        public int[] m_szDancerSpecial = null;
        public int[] m_szDancerIntimacy = null;	//获得的亲密度值
        public List<RoleTeamMarkInfo> m_szDanceTeamMark = null;
        public List<HeartBeatMarkInfo> m_HeartBeatMarkInfo = null;

        public GameMsg_S2C_EndRoomSuc()
            : base(GameMsgType.MSG_S2C_EndRoomSuc)
        {
            m_szDancerPlace = new byte[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerGrade = new byte[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerMark = new uint[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerExp = new uint[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerPerfect = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerCool = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerGood = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerBad = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerMiss = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerSpecial = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDancerIntimacy = new int[CommonDef.MAX_ROOM_PLAYER];
            m_szDanceTeamMark = new List<RoleTeamMarkInfo>();
            m_HeartBeatMarkInfo = new List<HeartBeatMarkInfo>();
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; ++i)
            {
                m_szDancerPlace[i] = DataIn.GetByte();
                m_szDancerGrade[i] = DataIn.GetByte();
                m_szDancerMark[i] = DataIn.GetUInt();
                m_szDancerExp[i] = DataIn.GetUInt();
                m_szDancerPerfect[i] = DataIn.GetInt();
                m_szDancerCool[i] = DataIn.GetInt();
                m_szDancerGood[i] = DataIn.GetInt();
                m_szDancerBad[i] = DataIn.GetInt();
                m_szDancerMiss[i] = DataIn.GetInt();
                m_szDancerSpecial[i] = DataIn.GetInt();
                m_szDancerIntimacy[i] = DataIn.GetInt();
            }

            ushort count = DataIn.GetUShort();
            for (int i = 0; i < count; ++i)
            {
                RoleTeamMarkInfo teamMarkInfo = new RoleTeamMarkInfo();
                teamMarkInfo.doDecode(DataIn);
                m_szDanceTeamMark.Add(teamMarkInfo);
            }

            count = DataIn.GetUShort();
            for (int i = 0; i < count; ++i)
            {
                HeartBeatMarkInfo info = new HeartBeatMarkInfo();
                info.doDecode(DataIn);
                m_HeartBeatMarkInfo.Add(info);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_EndRoomSuc();
        }
    }

    public class GameMsg_C2S_QuitRoom : GameMsgBase
    {
        public byte m_nQuitTo = 0;

        public GameMsg_C2S_QuitRoom()
            : base(GameMsgType.MSG_C2S_QuitRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nQuitTo);

            return true;
        }
    }


    public class GameMsg_S2C_QuitRoomFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_QuitRoomFail()
            : base(GameMsgType.MSG_S2C_QuitRoomFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_QuitRoomFail();
        }
    }

    public class GameMsg_C2S_KickPlayer : GameMsgBase
    {
        public byte m_nRoleRoomType;
        public byte m_nRoleRoomPos;
        public uint m_nRoleID = 0;

        public GameMsg_C2S_KickPlayer()
            : base(GameMsgType.MSG_C2S_KickPlayer)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoleRoomType);
            DataOut.PutByte(m_nRoleRoomPos);
            DataOut.PutUInt(m_nRoleID);

            return true;
        }
    }

    public class GameMsg_S2C_KickPlayerFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_KickPlayerFail()
            : base(GameMsgType.MSG_S2C_KickPlayerFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_KickPlayerFail();
        }
    }

    public class GameMsg_S2C_QuitMany : GameMsgBase
    {
        public byte m_nQuitType;

        public byte m_nRoleRoomType = (byte)RoleRoomType.None;
        public List<byte> m_lstRoleRoomPos = new List<byte>();

        public byte m_nHostRoomType = (byte)RoleRoomType.None;
        public byte m_nHostRoomPos = 0;
        public byte m_nHostRoomState = (byte)RoleRoomState.None;
        public string m_strRoomPwd = "";

        public GameMsg_S2C_QuitMany()
            : base(GameMsgType.MSG_S2C_QuitMany)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nQuitType = DataIn.GetByte();
            m_nRoleRoomType = DataIn.GetByte();

            ushort nCount = DataIn.GetUShort();
            for (ushort i = 0; i < nCount; ++i)
            {
                m_lstRoleRoomPos.Add(DataIn.GetByte());
            }

            m_nHostRoomType = DataIn.GetByte();
            m_nHostRoomPos = DataIn.GetByte();
            m_nHostRoomState = DataIn.GetByte();
            m_strRoomPwd = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_QuitMany();
        }
    }

    public class GameMsg_C2S_GetInviteeList : GameMsgBase
    {
        public GameMsg_C2S_GetInviteeList()
            : base(GameMsgType.MSG_C2S_GetInviteeList)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }

    public class GameMsg_S2C_GetInviteeListSuc : GameMsgBase
    {
        ushort m_nPlayerCount = 0;
        public List<InviteePlayerInfo> m_InviteeInfoList = new List<InviteePlayerInfo>();

        public GameMsg_S2C_GetInviteeListSuc()
            : base(GameMsgType.MSG_S2C_GetInviteeListSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nPlayerCount = DataIn.GetUShort();

            for (int i = 0; i < m_nPlayerCount; ++i)
            {
                InviteePlayerInfo playerItem = new InviteePlayerInfo();
                playerItem.doDecode(DataIn);

                m_InviteeInfoList.Add(playerItem);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_GetInviteeListSuc();
        }
    }

    public class GameMsg_S2C_GetInviteeListFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_GetInviteeListFail()
            : base(GameMsgType.MSG_S2C_GetInviteeListFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_GetInviteeListFail();
        }
    }

    public class GameMsg_C2S_InvitePlayer : GameMsgBase
    {
        public byte mInviteType = 0;
        public List<uint> mInviteList = new List<uint>();

        public GameMsg_C2S_InvitePlayer()
            : base(GameMsgType.MSG_C2S_InvitePlayer)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(mInviteType);

            DataOut.PutUShort((ushort)mInviteList.Count);

            for (int i = 0; i < mInviteList.Count; ++i)
            {
                DataOut.PutUInt(mInviteList[i]);
            }

            return true;
        }
    }

    public class GameMsg_S2C_InvitePlayerSuc : GameMsgBase
    {
        public GameMsg_S2C_InvitePlayerSuc()
            : base(GameMsgType.MSG_S2C_InvitePlayerSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_InvitePlayerSuc();
        }
    }

    public class GameMsg_S2C_InvitePlayerFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_InvitePlayerFail()
            : base(GameMsgType.MSG_S2C_InvitePlayerFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_InvitePlayerFail();
        }
    }

    public class GameMsg_C2S_InEquip : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Unknown;

        public GameMsg_C2S_InEquip()
            : base(GameMsgType.MSG_C2S_InEquip)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            return true;
        }
    }

    public class GameMsg_C2S_OutEquip : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Unknown;

        public GameMsg_C2S_OutEquip()
            : base(GameMsgType.MSG_C2S_OutEquip)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoomType);
            return true;
        }
    }

    public class GameMsg_C2S_ChangeRoomPosState : GameMsgBase
    {
        public byte m_nRoleRoomType = (byte)RoleRoomType.None;
        public byte m_nRoleRoomPos = 0;
        public byte m_nRoomPosState = (byte)RoomPosState.Open;

        public GameMsg_C2S_ChangeRoomPosState()
            : base(GameMsgType.MSG_C2S_ChangeRoomPosState)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoleRoomType);
            DataOut.PutByte(m_nRoleRoomPos);
            DataOut.PutByte(m_nRoomPosState);

            return true;
        }
    }

    public class GameMsg_S2C_ChangeRoomPosStateSuc : GameMsgBase
    {
        public byte m_nRoleRoomType = (byte)RoleRoomType.None;
        public byte m_nRoleRoomPos = 0;
        public byte m_nRoomPosState = (byte)RoomPosState.Open;

        public GameMsg_S2C_ChangeRoomPosStateSuc()
            : base(GameMsgType.MSG_S2C_ChangeRoomPosStateSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoleRoomType = DataIn.GetByte();
            m_nRoleRoomPos = DataIn.GetByte();
            m_nRoomPosState = DataIn.GetByte();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoomPosStateSuc();
        }
    }

    public class GameMsg_S2C_ChangeRoomPosStateFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_ChangeRoomPosStateFail()
            : base(GameMsgType.MSG_S2C_ChangeRoomPosStateFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoomPosStateFail();
        }
    }

    public class GameMsg_C2S_ChangeRoomInfo : GameMsgBase
    {
        public string m_strRoomName = "";
        public string m_strRoomPwd = "";

        public GameMsg_C2S_ChangeRoomInfo()
            : base(GameMsgType.MSG_C2S_ChangeRoomInfo)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutString(m_strRoomName);
            DataOut.PutString(m_strRoomPwd);

            return true;
        }
    }

    public class GameMsg_S2C_ChangeRoomInfoSuc : GameMsgBase
    {
        public string m_strRoomName = "";
        public string m_strRoomPwd = "";
        public bool m_bHasPwd = false;

        public GameMsg_S2C_ChangeRoomInfoSuc()
            : base(GameMsgType.MSG_S2C_ChangeRoomInfoSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strRoomName = DataIn.GetPerfixString();
            m_strRoomPwd = DataIn.GetPerfixString();
            m_bHasPwd = DataIn.GetBool();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoomInfoSuc();
        }
    }

    public class GameMsg_S2C_ChangeRoomInfoFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_ChangeRoomInfoFail()
            : base(GameMsgType.MSG_S2C_ChangeRoomInfoFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoomInfoFail();
        }
    }


    public class GameMsg_S2C_ChangeMusicInfoFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_ChangeMusicInfoFail()
            : base(GameMsgType.MSG_S2C_ChangeMusicInfoFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeMusicInfoFail();
        }
    }

    public class GameMsg_C2S_ChangeRoleRoomType : GameMsgBase
    {
        public byte m_nRoleRoomType = (byte)RoleRoomType.None;

        public GameMsg_C2S_ChangeRoleRoomType()
            : base(GameMsgType.MSG_C2S_ChangeRoleRoomType)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRoleRoomType);

            return true;
        }
    }


    public class GameMsg_S2C_ChangeRoleRoomTypeFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_ChangeRoleRoomTypeFail()
            : base(GameMsgType.MSG_S2C_ChangeRoleRoomTypeFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoleRoomTypeFail();
        }
    }

    public class GameMsg_C2S_ChangeRoleRoomState : GameMsgBase
    {
        public byte m_RoomType = (byte)CreateRoomType.Unknown;
        public byte m_nRoleRoomState = (byte)RoleRoomState.None;

        public GameMsg_C2S_ChangeRoleRoomState()
            : base(GameMsgType.MSG_C2S_ChangeRoleRoomState)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_RoomType);
            DataOut.PutByte(m_nRoleRoomState);

            return true;
        }
    }

    public class GameMsg_S2C_ChangeRoleRoomStateSuc : GameMsgBase
    {
        public byte m_nRoleRoomType = (byte)RoleRoomType.None;
        public byte m_nRoleRoomPos = 0;
        public byte m_nRoleRoomState = (byte)RoleRoomState.None;

        public GameMsg_S2C_ChangeRoleRoomStateSuc()
            : base(GameMsgType.MSG_S2C_ChangeRoleRoomStateSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoleRoomType = DataIn.GetByte();
            m_nRoleRoomPos = DataIn.GetByte();
            m_nRoleRoomState = DataIn.GetByte();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoleRoomStateSuc();
        }
    }

    public class GameMsg_S2C_ChangeRoleRoomStateFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_ChangeRoleRoomStateFail()
            : base(GameMsgType.MSG_S2C_ChangeRoleRoomStateFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoleRoomStateFail();
        }
    }

    public class GameMsg_C2S_ReportRoundMark : GameMsgBase
    {
        public byte m_nRound = 0;		//current round
        public byte m_nKeyRank = 0;		//current round rank
        public uint m_nMark = 0;		//current round mark
        public List<byte> m_lstRoundRank = new List<byte>();
        public string m_strMD5Code = "";
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_ReportRoundMark()
            : base(GameMsgType.MSG_C2S_ReportRoundMark)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRound);
            DataOut.PutByte(m_nKeyRank);
            DataOut.PutUInt(m_nMark);

            string strSrcCode = m_strMD5Code;
            strSrcCode += m_nRound;
            strSrcCode += m_nKeyRank;
            strSrcCode += m_nMark;

            DataOut.PutUShort((ushort)m_lstRoundRank.Count);
            for (int i = 0; i < m_lstRoundRank.Count; ++i)
            {
                DataOut.PutByte(m_lstRoundRank[i]);
                strSrcCode += m_lstRoundRank[i];
            }

            string strCheckCode = XQMD5.GetByteMd5String(ASCIIEncoding.ASCII.GetBytes(strSrcCode));
            DataOut.PutString(strCheckCode);

            return true;
        }
    }

    public class GameMsg_S2C_SyncRoundMark : GameMsgBase
    {
        public byte m_nDancerPos = 0;
        public byte m_nRound = 0;		//current round
        public byte m_nRank = 0;
        public uint m_nMark = 0;		//mark till current round
        public int m_nSpecial = 0;

        public GameMsg_S2C_SyncRoundMark()
            : base(GameMsgType.MSG_S2C_SyncRoundMark)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nDancerPos = DataIn.GetByte();
            m_nRound = DataIn.GetByte();
            m_nRank = DataIn.GetByte();
            m_nMark = DataIn.GetUInt();
            m_nSpecial = DataIn.GetInt();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SyncRoundMark();
        }
    }
    public class GameMsg_C2S_ReportEffectChange : GameMsgBase
    {
        public uint m_nMark = 0;
        public int m_nSpecial = 0;
        public byte m_nRound = 0;		//current round
        public string m_strMD5Code = "";
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_ReportEffectChange(uint nMark, int nSpecial, byte nRound, string strMD5Code)
            : base(GameMsgType.MSG_C2S_ReportEffectChange)
        {
            m_nMark = nMark;
            m_nSpecial = nSpecial;
            m_nRound = nRound;
            m_strMD5Code = strMD5Code;
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutUInt(m_nMark);
            DataOut.PutInt(m_nSpecial);
            DataOut.PutByte(m_nRound);

            string strSrcCode = m_strMD5Code;
            strSrcCode += m_nMark;
            strSrcCode += m_nSpecial;
            strSrcCode += m_nRound;

            string strCheckCode = XQMD5.GetByteMd5String(ASCIIEncoding.ASCII.GetBytes(strSrcCode));
            DataOut.PutString(strCheckCode);

            return true;
        }
    }

    public class GameMsg_S2C_SyncEffectState : GameMsgBase
    {
        public uint m_nMark = 0;
        public int m_nSpecial = 0;
        public byte m_nRound = 0;		//current round
        public byte m_nDancerPos = 0;
        public GameMsg_S2C_SyncEffectState()
            : base(GameMsgType.MSG_S2C_SyncEffectState)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nMark = DataIn.GetUInt();
            m_nSpecial = DataIn.GetInt();
            m_nRound = DataIn.GetByte();
            m_nDancerPos = DataIn.GetByte();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SyncEffectState();
        }
    }

    public class GameMsg_C2S_QuitMarkAward : GameMsgBase
    {
        public byte m_nRoomType = (byte)CreateRoomType.Normal;

        public GameMsg_C2S_QuitMarkAward()
            : base(GameMsgType.MSG_C2S_QuitMarkAward)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            return true;
        }
    }

    public class GameMsg_C2S_PromoteRoomHost : GameMsgBase
    {
        public byte m_nNewHostType = 0;
        public byte m_nNewHostPos = 0;

        public GameMsg_C2S_PromoteRoomHost()
            : base(GameMsgType.MSG_C2S_PromoteRoomHost)
        {

        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nNewHostType);
            DataOut.PutByte(m_nNewHostPos);

            return true;
        }
    }

    public class GameMsg_S2C_PromoteRoomOwnerSuc : GameMsgBase
    {
        public byte m_nOldHostType = 0;
        public byte m_nOldHostPos = 0;
        public byte m_nOldHostState = 0;

        public byte m_nNewHostType = 0;
        public byte m_nNewHostPos = 0;
        public byte m_nNewHostState = 0;

        public string m_strRoomPwd = "";

        public GameMsg_S2C_PromoteRoomOwnerSuc()
            : base(GameMsgType.MSG_S2C_PromoteRoomHostSuc)
        {

        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nOldHostType = DataIn.GetByte();
            m_nOldHostPos = DataIn.GetByte();
            m_nOldHostState = DataIn.GetByte();

            m_nNewHostType = DataIn.GetByte();
            m_nNewHostPos = DataIn.GetByte();
            m_nNewHostState = DataIn.GetByte();

            m_strRoomPwd = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_PromoteRoomOwnerSuc();
        }
    }

    public class GameMsg_S2C_PromoteRoomOwnerFail : GameMsgBase
    {
        public string m_strError = "";

        public GameMsg_S2C_PromoteRoomOwnerFail()
            : base(GameMsgType.MSG_S2C_PromoteRoomHostFail)
        {

        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_strError = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_PromoteRoomOwnerFail();
        }
    }

    public class GameMsg_C2S_LoadingStartRoomProgress : GameMsgBase
    {
        public byte m_nRate;

        public GameMsg_C2S_LoadingStartRoomProgress()
            : base(GameMsgType.MSG_C2S_LoadingStartRoomProgress)
        {

        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nRate);

            return true;
        }
    }

    public class GameMsg_S2C_LoadingStartRoomProgress : GameMsgBase
    {
        public uint m_nRoleID = 0;
        public byte m_nRate = 0;

        public GameMsg_S2C_LoadingStartRoomProgress()
            : base(GameMsgType.MSG_S2C_LoadingStartRoomProgress)
        {

        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_nRate = DataIn.GetByte();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_LoadingStartRoomProgress();
        }
    }


    #region TeamMode
    public class GameMsg_C2S_SwitchRoomTeamMode : GameMsgBase
    {
        public byte m_TeamMode = 0;
        public byte m_TeamType = 0;
        public GameMsg_C2S_SwitchRoomTeamMode()
            : base(GameMsgType.MSG_C2S_SwitchDanceRoomTeamMode)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_TeamMode);
            DataOut.PutByte(m_TeamType);
            return true;
        }
    }

    public class GameMsg_S2C_SwitchRoomTeamModeSuc : GameMsgBase
    {
        public byte m_TeamType = 0;
        public GameMsg_S2C_SwitchRoomTeamModeSuc()
            : base(GameMsgType.MSG_S2C_SwitchRoomTeamModeSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_TeamType = DataIn.GetByte();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SwitchRoomTeamModeSuc();
        }
    }

    public class GameMsg_S2C_SwitchRoomTeamModeFail : GameMsgBase
    {
        public byte m_ErrorType = 0;
        public string m_strError = "";
        public GameMsg_S2C_SwitchRoomTeamModeFail()
            : base(GameMsgType.MSG_S2C_SwitchRoomTeamModeFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_ErrorType = DataIn.GetByte();
            m_strError = DataIn.GetPerfixString();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SwitchRoomTeamModeFail();
        }
    }

    public class GameMsg_C2S_JoinRoomTeam : GameMsgBase
    {
        public byte m_TeamType = 0;
        public GameMsg_C2S_JoinRoomTeam()
            : base(GameMsgType.MSG_C2S_JoinDanceRoomTeam)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_TeamType);
            return true;
        }
    }

    public class GameMsg_S2C_JoinRoomTeamSuc : GameMsgBase
    {
        public byte m_TeamType = 0;
        public GameMsg_S2C_JoinRoomTeamSuc()
            : base(GameMsgType.MSG_S2C_JoinRoomTeamSuc)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_TeamType = DataIn.GetByte();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_JoinRoomTeamSuc();
        }
    }

    public class GameMsg_S2C_JoinRoomTeamFail : GameMsgBase
    {
        public byte m_ErrorType = 0;
        public string m_strError = "";
        public GameMsg_S2C_JoinRoomTeamFail()
            : base(GameMsgType.MSG_S2C_JoinRoomTeamFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_ErrorType = DataIn.GetByte();
            m_strError = DataIn.GetPerfixString();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_JoinRoomTeamFail();
        }
    }

    public class GameMsg_S2C_UpdateRoleRoomTeamInfo : GameMsgBase
    {
        public RoleRoomTeamInfo m_RoleTeamInfo = new RoleRoomTeamInfo();
        public GameMsg_S2C_UpdateRoleRoomTeamInfo()
            : base(GameMsgType.MSG_S2C_UpdateRoleRoomTeamInfo)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_RoleTeamInfo.doDecode(DataIn);
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_UpdateRoleRoomTeamInfo();
        }
    }

    public class GameMsg_S2C_UpdateWholeRoomTeamInfo : GameMsgBase
    {
        public byte m_RoomDanceMode = 0;
        public List<RoleRoomTeamInfo> m_ListRoleRoomTeamInfo = new List<RoleRoomTeamInfo>();
        public GameMsg_S2C_UpdateWholeRoomTeamInfo()
            : base(GameMsgType.MSG_S2C_UpdateWholeRoomTeamInfo)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_RoomDanceMode = DataIn.GetByte();

            ushort count = DataIn.GetUShort();
            for (int i = 0; i < count; ++i)
            {
                RoleRoomTeamInfo roleTeamInfo = new RoleRoomTeamInfo();
                roleTeamInfo.doDecode(DataIn);
                m_ListRoleRoomTeamInfo.Add(roleTeamInfo);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_UpdateWholeRoomTeamInfo();
        }
    }

    #endregion

    public class GameMsg_S2C_BeginToSelectPartner : GameMsgBase
    {

        public GameMsg_S2C_BeginToSelectPartner()
            : base(GameMsgType.MSG_S2C_BeginToSelectPartner)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_BeginToSelectPartner();
        }
    }

    public class GameMsg_S2C_EndToSelectPartner : GameMsgBase
    {
        public List<HeartBeatTeamInfo> m_HBTeamInfo = new List<HeartBeatTeamInfo>();

        public GameMsg_S2C_EndToSelectPartner()
            : base(GameMsgType.MSG_S2C_EndToSelectPartner)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            ushort teamCount = DataIn.GetUShort();

            for (int i = 0; i < teamCount; ++i)
            {
                HeartBeatTeamInfo info = new HeartBeatTeamInfo();
                info.doDecode(DataIn);

                m_HBTeamInfo.Add(info);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_EndToSelectPartner();
        }
    }

    public class GameMsg_C2S_SelectPartner : GameMsgBase
    {
        public uint m_PartnerID = 0;

        public GameMsg_C2S_SelectPartner()
            : base(GameMsgType.MSG_C2S_SelectPartner)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutUInt(m_PartnerID);

            return true;
        }
    }

    public class GameMsg_S2C_SelectPartner : GameMsgBase
    {
        public uint m_SelectorID = 0;
        public uint m_PartnerID = 0;

        public GameMsg_S2C_SelectPartner()
            : base(GameMsgType.MSG_S2C_SelectPartner)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_SelectorID = DataIn.GetUInt();
            m_PartnerID = DataIn.GetUInt();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SelectPartner();
        }
    }

    public class GameMsg_S2C_SelectPartnerFail : GameMsgBase
    {
        public string m_Error = "";

        public GameMsg_S2C_SelectPartnerFail()
            : base(GameMsgType.MSG_S2C_SelectPartnerFail)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_Error = DataIn.GetPerfixString();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SelectPartnerFail();
        }
    }

    public class GameMsg_S2C_UpdateHeartBeatMark : GameMsgBase
    {
        public List<HeartBeatMarkInfo> m_HeartBeatMarkInfo = new List<HeartBeatMarkInfo>();

        public GameMsg_S2C_UpdateHeartBeatMark()
            : base(GameMsgType.MSG_S2C_UpdateSweetValue)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            ushort count = DataIn.GetUShort();

            for (int i = 0; i < count; ++i)
            {
                HeartBeatMarkInfo info = new HeartBeatMarkInfo();
                info.doDecode(DataIn);
                m_HeartBeatMarkInfo.Add(info);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_UpdateHeartBeatMark();
        }
    }


    public class GameMsg_S2C_ChangeRoomColor : GameMsgBase
    {
        public byte mRoomColor = 0;
        public GameMsg_S2C_ChangeRoomColor()
            : base(GameMsgType.MSG_S2C_ChangeRoomColor)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            mRoomColor = DataIn.GetByte();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ChangeRoomColor();
        }
    }

    public class GameMsg_C2S_PlayerDownloading : GameMsgBase
    {
        public byte m_nProgress = 0;

        public GameMsg_C2S_PlayerDownloading()
            : base(GameMsgType.MSG_C2S_PlayerDownloading)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutUInt(m_nProgress);

            return true;
        }
    }

    public class GameMsg_S2C_PlayerDownloading : GameMsgBase
    {
        public uint m_nRoleID = 0;
        public byte m_nProgress = 0;

        public GameMsg_S2C_PlayerDownloading()
            : base(GameMsgType.MSG_S2C_PlayerDownloading)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_nProgress = DataIn.GetByte();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_PlayerDownloading();
        }
    };

    public class GameMsg_C2S_SwitchToOtherRoom : GameMsgBase
    {
        public byte m_nOtherRoomType = 0;

        public GameMsg_C2S_SwitchToOtherRoom()
            : base(GameMsgType.MSG_C2S_SwitchToOtherRoom)
        {
        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutByte(m_nOtherRoomType);

            return true;
        }
    }

    public class GameMsg_S2C_SwitchToOtherRoomResult : GameMsgBase
    {
        public uint m_ErrorCode = 0;

        public GameMsg_S2C_SwitchToOtherRoomResult()
            : base(GameMsgType.MSG_S2C_SwitchToOtherRoomResult)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_ErrorCode = DataIn.GetUInt();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SwitchToOtherRoomResult();
        }
    }

    public class GameMsg_S2C_SwitchToOtherRoomTimeBegin : GameMsgBase
    {
        public int m_CountDownTime = 0;
        public GameMsg_S2C_SwitchToOtherRoomTimeBegin()
            : base(GameMsgType.MSG_S2C_SwitchToOtherRoomTimeBegin)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_CountDownTime = DataIn.GetInt();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_SwitchToOtherRoomTimeBegin();
        }
    }

    public class GameMsg_S2C_RoomEventNotice : GameMsgBase
    {
        public byte m_EventType = 0;
        public uint m_RoleID = 0;
        public int m_Exp = 0;

        public GameMsg_S2C_RoomEventNotice()
            : base(GameMsgType.MSG_S2C_RoomEventNotice)
        {
        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_EventType = DataIn.GetByte();
            m_RoleID = DataIn.GetUInt();
            m_Exp = DataIn.GetInt();
            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_RoomEventNotice();
        }
    }
}