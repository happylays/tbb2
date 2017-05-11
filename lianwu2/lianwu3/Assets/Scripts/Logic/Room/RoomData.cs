using System;
using UnityEngine;
using LoveDance.Client.Common;
using LoveDance.Client.Network.Music;

namespace LoveDance.Client.Logic.Room
{

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
    }
}
