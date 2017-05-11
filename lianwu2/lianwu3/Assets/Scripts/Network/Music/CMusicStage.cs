using System;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Music
{
    public class CMusicEX
    {
        public UInt16 m_nMusicID = 0;
        public byte m_nStar;

        public bool Load(UInt16 musicID, ref XQFileStream file)
        {
            m_nMusicID = musicID;

            file.ReadByte(ref m_nStar);

            return true;
        }
    }

    public class CMusicStage
    {
        public UInt16 m_nMusicID = 0;
        public byte m_nModeID = 0;
        public byte m_nLevel = 0;

        public CMusicEX m_InfoEX = new CMusicEX();

        public bool Load(ref XQFileStream file)
        {
            file.ReadUShort(ref m_nMusicID);
            file.ReadByte(ref m_nModeID);
            file.ReadByte(ref m_nLevel);

            return m_InfoEX.Load(m_nMusicID, ref file);
        }
    }
}
