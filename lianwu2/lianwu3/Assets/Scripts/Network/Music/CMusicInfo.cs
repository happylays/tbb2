using System;
using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Music
{

    public class CMusicInfo
    {
        public UInt16 m_nMusicID = 0;
        public string m_strMusicName = "a";
        public string m_strSingerName = "a";
        public string m_strCollectionName = "a";
        public byte m_nType = 0;
        public UInt16 m_nTime = 0;
        public UInt16 m_nBpm = 0;

        public string m_strIcon = "a";
        public string m_strAtlas = "a";
        public string m_strMusicSource = "a";
        public string m_strIntro = "a";
        public bool m_bIsNew = true;

        public bool Load(ref XQFileStream file)
        {

            file.ReadUShort(ref m_nMusicID);

            UInt16 nSize = 0;
            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strMusicName, nSize);

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strSingerName, nSize);

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strCollectionName, nSize);

            file.ReadByte(ref m_nType);

            file.ReadUShort(ref m_nTime);
            file.ReadUShort(ref m_nBpm);

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strIcon, nSize);
            if (m_strIcon.Length == 1)
                m_strIcon = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strAtlas, nSize);
            if (m_strAtlas.Length == 1)
                m_strAtlas = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strMusicSource, nSize);
            if (m_strMusicSource.Length == 1)
                m_strMusicSource = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strIntro, nSize);
            if (m_strIntro.Length == 1)
                m_strIntro = "";

            file.ReadBool(ref m_bIsNew);

            return true;
        }
    }
}