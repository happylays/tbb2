using System;
using LoveDance.Client.Common;

namespace LoveDance.Client.Data.Scene
{
    public class CSceneInfo
    {

        public byte m_nSceneID = 0;
        public string m_strSceneName = "a";
        public string m_strChooseMusicIcon = "a";
        public string m_strRoomSceneTexture = "a";
        public string m_strSceneStage = "a";
        public bool m_bIsFresher = false;

        public bool Load(ref XQFileStream file)
        {

            file.ReadByte(ref m_nSceneID);

            UInt16 nSize = 0;
            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strSceneName, nSize);

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strChooseMusicIcon, nSize);
            if (m_strChooseMusicIcon.Length == 1)
                m_strChooseMusicIcon = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strRoomSceneTexture, nSize);
            if (m_strRoomSceneTexture.Length == 1)
                m_strRoomSceneTexture = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strSceneStage, nSize);
            if (m_strSceneStage.Length == 1)
                m_strSceneStage = "";

            file.ReadBool(ref m_bIsFresher);

            return true;
        }
    }
}

