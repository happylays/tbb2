using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveDance.Client.Common;
using LoveDance.Client.Network.Music;
using LoveDance.Client.Network.Lantern;
using LoveDance.Client.Network.Room;

namespace LoveDance.Client.Logic.Room
{
    public class RoomPlayerData
    {
        private uint m_roleID = 0;
        private RoomPlayer _roomPlayer = null;
        private int m_nProgress = 0;//当前加载进度;

        public uint RoleID
        {
            get { return m_roleID; }
        }

        public RoomPlayer mRoomPlayer
        {
            get { return _roomPlayer; }
            set { _roomPlayer = value; }
        }

        /// <summary>
        /// 资源下载的进度;
        /// </summary>
        public int nProgress
        {
            get
            {
                return m_nProgress;
            }
            set
            {
                m_nProgress = value;
            }
        }

        public RoomPlayerData(uint roleID)
        {
            m_roleID = roleID;
        }
    }

    public class RoomData
    {

        static int mPlaySceneID = CommonDef.SCENE_RANDOM_ID;
        static UInt16 mPlaySongID = (UInt16)CommonDef.SONG_RANDOM_ID;
        static SongMode mPlaySongMode = SongMode.None;

        static int mChoosedSceneID = CommonDef.SCENE_RANDOM_ID;
        static int mChoosedSongID = CommonDef.SONG_RANDOM_ID;
        static string mChoosedSongName = "";
        static SongMode mChoosedSongMode = SongMode.None;

        static CMusicInfo m_PlayMusciInfo = null;
        static RoomDanceMode m_DanceMode = RoomDanceMode.SINGLE;

        static PlayerBase[] mRoomPlayer = new PlayerBase[CommonDef.MAX_ROOM_PLAYER + CommonDef.MAX_ROOM_AUDIENCE];
        static RoomPlayer mMeInfo = null;
        static List<RoomPlayerData> mDancerList = new List<RoomPlayerData>();
        static List<RoomPlayerData> mAudienceList = new List<RoomPlayerData>();

        public static CMusicInfo PlayMusciInfo
        {
            get
            {
                if (m_PlayMusciInfo == null || m_PlayMusciInfo.m_nMusicID != mPlaySongID)
                {
                    m_PlayMusciInfo = CMusicInfoManager.MusicDataMgr.GetMusicByID(mPlaySongID);
                }
                return m_PlayMusciInfo;
            }
        }

        public static SongMode PlaySongMode
        {
            get
            {
                return mPlaySongMode;
            }
        }

        public static int PlayScene
        {
            get
            {
                return mPlaySceneID;
            }
        }
        
        public static RoomDanceMode DanceMode
        {
            get
            {
                return m_DanceMode;
            }
        }

        public static int DancerCount()
        {
            return 1;
        }

        public static void PrepareRoom(bool autoMatch, int scene, int song, SongMode mode, string strCheckKey, byte[] byteStage, ushort nCountDown)
        {
            mPlaySceneID = scene;
            mPlaySongID = (UInt16)song;
            mPlaySongMode = mode;
            mChoosedSongMode = mode;
        }

        public static PlayerBase GetRoomPlayerByPos(int nPos)
        {
            return CommonLogicData.MainPlayer;

            //int MaxCount = CommonDef.MAX_ROOM_PLAYER + CommonDef.MAX_ROOM_AUDIENCE;

            //if (nPos >= 0 && nPos < MaxCount && nPos < mRoomPlayer.Length)
            //{
            //    return mRoomPlayer[nPos];
            //}

            //return null;
        }

        public static string RoomSceneName()
        {
            string sceneName = CommonDef.ROOMSCENENAME;
            
            return sceneName;
        }

        public static void DestroyRoom()
        { 
            mChoosedSceneID = CommonDef.SCENE_RANDOM_ID;
            mChoosedSongID = CommonDef.SONG_RANDOM_ID;
            //mChoosedSongName = SystemTips.GetTipsContent(CommonDef.SONG_RANDOM_NAME_INDEX);
            mChoosedSongMode = SongMode.None;
            mPlaySceneID = CommonDef.SCENE_RANDOM_ID;
            mPlaySongID = (UInt16)CommonDef.SONG_RANDOM_ID;
            mPlaySongMode = SongMode.None;
            m_PlayMusciInfo = null;
            m_DanceMode = RoomDanceMode.SINGLE;

            for (int pos = 0; pos < CommonDef.MAX_ROOM_PLAYER; ++pos)
            {
                PlayerBase player = RoomData.GetRoomPlayerByPos(pos);
                if (player != null)
                {
                    player.transform.parent = null;
                }
            }

            for (int i = 0; i < mRoomPlayer.Length; ++i)
            {
                if (mRoomPlayer[i] != null)
                {
                    PlayerBase player = mRoomPlayer[i];
                    mRoomPlayer[i] = null;
                }
            }

            if (CommonLogicData.MainPlayer != null)
            {
                CommonLogicData.MainPlayer.IsToShow = true;
                CommonLogicData.MainPlayer.transform.position = Vector3.zero;

                CommonLogicData.MainPlayer.transform.rotation = Quaternion.identity;
            }
        }

        public static void Serialize(RoomWholeInfo roomInfo)
        {            
            mChoosedSceneID = roomInfo.m_nScene;
            mChoosedSongID = roomInfo.m_nMusic;
            mChoosedSongMode = (SongMode)roomInfo.m_nMode;
            m_DanceMode = (RoomDanceMode)roomInfo.m_nRoomDanceMode;
            
            SerializeRoomPlayer(roomInfo.m_lstDancer);
        }

        private static void SerializeRoomPlayer(List<RoomPlayerInfo> dancerList)
        {
            for (int i = 0; i < dancerList.Count; ++i)
            {
                RoomPlayerInfo playerInfo = dancerList[i];
                RoomPlayer dancerInfo = new RoomPlayer();
                dancerInfo.Serialize(playerInfo);

                if (CommonLogicData.MainPlayerID > 0 && CommonLogicData.IsMainPlayer(dancerInfo.RoleID))
                {
                    mMeInfo = dancerInfo;
                }

                int tempIndex = dancerInfo.RolePos;
                if (tempIndex < CommonDef.MAX_ROOM_PLAYER && tempIndex >= 0 && tempIndex < mRoomPlayer.Length)
                {
                    AddDancer(dancerInfo.RoleID, dancerInfo);
                    if (dancerInfo == mMeInfo)
                    {
                        mRoomPlayer[tempIndex] = CommonLogicData.MainPlayer;
                    }
                    else
                    {                        
                        mRoomPlayer[tempIndex] = PlayerManager.CreateLogic(dancerInfo.GetBriefAttr(), true, playerInfo.m_ItemPacket, playerInfo.m_GenePacket);
                    }
                    mRoomPlayer[tempIndex].PlayerMoveType = (PlayerMoveType)playerInfo.m_nMoveType;
                }
            }
        }

        static void AddDancer(uint roleID, RoomPlayer player)
        {
            RoomPlayerData apd = GetDancerPlayerByID(roleID);
            if (apd == null)
            {
                apd = new RoomPlayerData(roleID);
                mDancerList.Add(apd);
            }
            apd.mRoomPlayer = player;
        }

        public static IEnumerator CreateAllRoomPlayer()
        {
            List<IEnumerator> createlist = new List<IEnumerator>();

            int listCount = mDancerList.Count;
            for (int i = 0; i < listCount; ++i)
            {
                RoomPlayerData dancerData = mDancerList[i];
                if (dancerData != null && dancerData.mRoomPlayer != null)
                {
                    int dancerPos = dancerData.mRoomPlayer.RolePos;
                    if (dancerData.mRoomPlayer != mMeInfo)
                    {
                        PlayerBase dancerPlayer = mRoomPlayer[dancerPos];
                        if (dancerPlayer != null)
                        {
                            PhysicsType pType = PhysicsType.Player;                            

                            IEnumerator itor = dancerPlayer.CreatePhysics(true, pType);
                            createlist.Add(itor);
                        }
                    }
                }
            }

            while (createlist.Count != 0)
            {
                yield return null;

                for (int i = 0; i < createlist.Count; ++i)
                {
                    if (!createlist[i].MoveNext())
                    {
                        createlist.RemoveAt(i);
                    }
                }
            }
        }

        public static RoomPlayerData GetDancerPlayerByID(uint roleId)
        {
            int listCount = mDancerList.Count;
            for (int i = 0; i < listCount; ++i)
            {
                RoomPlayerData v = mDancerList[i];
                if (v != null)
                {
                    if (v.mRoomPlayer != null && v.mRoomPlayer.RoleID == roleId)
                        return v;
                }
                else
                {
                    Debug.LogError("RoomData GetDancerPlayerByPos failed.RoomPlayerData can not be null.");
                }
            }
            return null;
        }
    }

    public class RoomPlayer
    {
        public uint RoleID
        {
            get
            {
                return mRoleID;
            }
        }

        public string RoleName
        {
            get
            {
                return mRoleName;
            }
        }

        public bool IsHost
        {
            get
            {
                return mIsHost;
            }
        }

        public RoleRoomType RoleType
        {
            get
            {
                return mRoleType;
            }
        }

        public bool IsBoss
        {
            get
            {
                return m_bIsBoss;
            }
        }

        public int RolePos
        {
            get
            {
                return mRolePos;
            }
        }

        public RoleRoomState RoleState
        {
            get
            {
                return mRoleState;
            }
        }

        public Sex_Type RoleSex
        {
            get
            {
                return mRoleSex;
            }
        }

        public byte RoleSkin
        {
            get
            {
                return mRoleSkin;
            }
        }

        public string DanceGroup
        {
            get
            {
                return mDanceGroup;
            }
        }

        public byte DanceGroupPos
        {
            get
            {
                return mDanceGroupPos;
            }
        }


        public bool IsVIP
        {
            get
            {
                return m_bIsVIP && m_nVIPLevel > 0;
            }
        }

        public ushort VIPLevel
        {
            get
            {
                return m_nVIPLevel;
            }
        }
        
        uint mRoleID = 0;
        string mRoleName = "";
        bool mIsHost = false;
        RoleRoomType mRoleType = RoleRoomType.None;
        bool m_bIsBoss = false;
        int mRolePos = 0;
        RoleRoomState mRoleState = RoleRoomState.None;
        Sex_Type mRoleSex = Sex_Type.None;
        byte mRoleSkin = 1;
        string mDanceGroup = "";
        byte mDanceGroupPos = 0;
        ushort mDanceGroupBadge = 0;
        ushort mDanceGroupEffect = 0;
        bool m_bIsVIP;
        ushort m_nVIPLevel;
        ushort m_nTransformId;
        uint m_nSkinCandyColor;

        /// <summary>
        /// 当前所在座驾ID
        /// </summary>
        public uint m_nCurVehicleID = 0;
        /// <summary>
        /// 当前所在座驾拥有者ID
        /// </summary>
        public uint m_nCurVehicleOwnerID = 0;
        /// <summary>
        /// 当前所在座驾位置
        /// </summary>
        public int m_nCurVehiclePos = 0;
               

        public void Serialize(RoomPlayerInfo playerInfo)
        {
            mRoleID = playerInfo.m_nRoleID;
            mRoleName = playerInfo.m_strRoleName;
            mIsHost = playerInfo.m_bIsHost;
            mRoleType = (RoleRoomType)playerInfo.m_nRoleType;
            m_bIsBoss = playerInfo.m_bIsBoss;
            mRolePos = playerInfo.m_nRolePos;
            mRoleState = (RoleRoomState)playerInfo.m_nRoleState;            
            mRoleSex = (Sex_Type)playerInfo.m_nRoleSex;
            mRoleSkin = playerInfo.m_nRoleSkin;
            mDanceGroup = playerInfo.m_strDanceGroup;
            mDanceGroupPos = playerInfo.m_nDanceGroupPos;
            mDanceGroupBadge = playerInfo.m_nDanceGroupBadge;
            mDanceGroupEffect = playerInfo.m_nDanceGroupEffect;
            
            m_bIsVIP = playerInfo.m_bIsVIP;
            m_nVIPLevel = playerInfo.m_nVIPLevel;
            m_nTransformId = playerInfo.m_nTransformId;
            m_nSkinCandyColor = playerInfo.m_nSkinCandyColor;
            
            m_nCurVehicleID = playerInfo.m_nCurVehicleID;
            m_nCurVehicleOwnerID = playerInfo.m_nCurVehicleOwnerID;
            m_nCurVehiclePos = playerInfo.m_nCurVehiclePos;
            
        }

        public BriefAttr GetBriefAttr()
        {
            BriefAttr briefAttr = new BriefAttr();
            briefAttr.m_nRoleID = mRoleID;
            briefAttr.m_strRoleName = mRoleName;
            briefAttr.m_bIsBoy = (mRoleSex == Sex_Type.Male ? true : false);
            briefAttr.m_nSkinColor = RoleSkin;
            briefAttr.m_strDanceGroup = mDanceGroup;
            briefAttr.m_nDanceGroupPos = mDanceGroupPos;
            briefAttr.m_nBadgeId = (byte)mDanceGroupBadge;
            briefAttr.m_nEffectId = (byte)mDanceGroupEffect;
            briefAttr.m_bIsVIP = m_bIsVIP;
            briefAttr.m_nVIPLevel = m_nVIPLevel;
            briefAttr.m_nTransformId = m_nTransformId;
            
            if (m_nCurVehicleOwnerID == mRoleID)
            {
                briefAttr.m_nVehicleID = m_nCurVehicleID;
            }
            else
            {
                briefAttr.m_nVehicleID = 0;
            }
            briefAttr.m_nVehiclePos = m_nCurVehiclePos;

            return briefAttr;
        }

    }
}
