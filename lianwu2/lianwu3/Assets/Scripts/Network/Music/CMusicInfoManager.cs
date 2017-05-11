using System;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;


namespace LoveDance.Client.Network.Music
{
    public class CMusicInfoManager
    {
        private static CMusicInfoManager s_MusicInfoMgr = new CMusicInfoManager();
        public static CMusicInfoManager MusicDataMgr
        {
            get
            {
                return s_MusicInfoMgr;
            }
        }

        private XQHashtable s_MusicInfoMap = new XQHashtable();
        private Hashtable m_StageMap = new Hashtable();
        private Hashtable m_BoardMap = new Hashtable();
        private Hashtable m_GuideMusicMap = new Hashtable();

        public CMusicInfo GetMusicByID(UInt16 nID)
        {
            if (nID > 0 && s_MusicInfoMap != null && s_MusicInfoMap.Contains(nID))
            {
                return (CMusicInfo)s_MusicInfoMap[nID];
            }
            else
            {
                return null;
            }
        }


        public bool HaveStage(byte mode, byte level, UInt16 nMusicID)
        {
            if (m_StageMap.Contains(mode))
            {
                Hashtable levelMap = (Hashtable)m_StageMap[mode];
                if (levelMap != null)
                {
                    if (levelMap.Contains(level))
                    {
                        Hashtable musicMap = (Hashtable)levelMap[level];
                        if (musicMap != null)
                        {
                            if (musicMap.Contains(nMusicID))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public bool LoadMusic(XQFileStream file)
        {
            DestroyMusic();
            if (file != null && file.IsOpen())
            {
                UInt16 usNumber = 0;
                file.ReadUShort(ref usNumber);

                for (UInt16 i = 0; i < usNumber; i++)
                {
                    CMusicInfo musicinfo = new CMusicInfo();
                    musicinfo.Load(ref file);
                    RegistMusicInfo(musicinfo);
                }
                s_MusicInfoMap.Sort();
                return true;
            }
            return false;
        }

        public bool LoadStage(XQFileStream file)
        {
            DestroyStage();
            if (file != null && file.IsOpen())
            {
                UInt16 usNumber = 0;
                file.ReadUShort(ref usNumber);

                for (UInt16 i = 0; i < usNumber; i++)
                {
                    CMusicStage musicstage = new CMusicStage();
                    musicstage.Load(ref file);
                    RegistMusicStage(musicstage);
                }
                return true;
            }
            return false;
        }

        void RegistMusicStage(CMusicStage stageInfo)
        {
            if (stageInfo != null && stageInfo.m_nMusicID > 0)
            {
                if (m_StageMap.Contains(stageInfo.m_nModeID))
                {
                    Hashtable levelMap = (Hashtable)m_StageMap[stageInfo.m_nModeID];
                    if (levelMap != null)
                    {
                        if (levelMap.Contains(stageInfo.m_nLevel))
                        {
                            Hashtable musicMap = (Hashtable)levelMap[stageInfo.m_nLevel];
                            if (musicMap != null)
                            {
                                if (musicMap.Contains(stageInfo.m_nMusicID))
                                {
                                    //Debug.Log( "Same music in music stage, music id:" + stageInfo.m_nMusicID );
                                }
                                else
                                {
                                    musicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);
                                }
                            }
                        }
                        else
                        {
                            Hashtable musicMap = new Hashtable();
                            musicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);

                            levelMap.Add(stageInfo.m_nLevel, musicMap);
                        }
                    }
                }
                else
                {
                    Hashtable musicMap = new Hashtable();
                    musicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);

                    Hashtable levelMap = new Hashtable();
                    levelMap.Add(stageInfo.m_nLevel, musicMap);

                    m_StageMap.Add(stageInfo.m_nModeID, levelMap);
                }

                //把所有歌曲都加入到随机模式的列表中
                byte tempSongMode = (byte)SongMode.None;
                if (m_StageMap.Contains(tempSongMode))
                {
                    Hashtable tempLeveMap = (Hashtable)m_StageMap[tempSongMode];
                    if (tempLeveMap != null)
                    {
                        if (tempLeveMap.Contains(stageInfo.m_nLevel))
                        {
                            Hashtable tempMusicMap = (Hashtable)tempLeveMap[stageInfo.m_nLevel];
                            if (tempMusicMap != null)
                            {
                                if (tempMusicMap.Contains(stageInfo.m_nMusicID))
                                {
                                    //Debug.Log( "Same music in music stage, music id:" + stageInfo.m_nMusicID );
                                }
                                else
                                {
                                    tempMusicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);
                                }
                            }
                        }
                        else
                        {
                            Hashtable tempMusicMap = new Hashtable();
                            tempMusicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);

                            tempLeveMap.Add(stageInfo.m_nLevel, tempMusicMap);
                        }
                    }
                }
                else
                {
                    Hashtable musicMap = new Hashtable();
                    musicMap.Add(stageInfo.m_nMusicID, stageInfo.m_InfoEX);

                    Hashtable tempLeveMap = new Hashtable();
                    tempLeveMap.Add(stageInfo.m_nLevel, musicMap);

                    m_StageMap.Add(tempSongMode, tempLeveMap);
                }
            }
        }

        void RegistMusicInfo(CMusicInfo musicinfo)
        {
            if (s_MusicInfoMap != null && musicinfo != null && musicinfo.m_nMusicID > 0)
            {
                if (s_MusicInfoMap.Contains(musicinfo.m_nMusicID))
                {
                    //Debug.Log("RegistMusicInfo Duplicate,nType:" + musicinfo.m_nMusicID);
                }
                else
                {
                    s_MusicInfoMap.Add(musicinfo.m_nMusicID, musicinfo);
                }
            }
        }

        void DestroyMusic()
        {
            if (s_MusicInfoMap != null)
            {
                s_MusicInfoMap.Clear();
            }
        }


        void DestroyStage()
        {
            m_StageMap.Clear();
        }


        public void DestoryBoard()
        {
            m_BoardMap.Clear();
            m_GuideMusicMap.Clear();
        }
    }
}
