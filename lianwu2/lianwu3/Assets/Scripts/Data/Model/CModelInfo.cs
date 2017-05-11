using System;
using LoveDance.Client.Common;

namespace LoveDance.Client.Data.Model
{
    public class CModelInfo
    {

        public byte m_nModelID = 0;
        public string m_strModelName = "a";
        public string m_strChooseMusicIcon = "a";
        public string m_strChooseMusicAtlas = "a";

        public bool Load(ref XQFileStream file)
        {

            file.ReadByte(ref m_nModelID);

            UInt16 nSize = 0;
            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strModelName, nSize);
            if (m_strModelName.Length == 1)
                m_strModelName = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strChooseMusicIcon, nSize);
            if (m_strChooseMusicIcon.Length == 1)
                m_strChooseMusicIcon = "";

            file.ReadUShort(ref nSize);
            file.ReadString(ref m_strChooseMusicAtlas, nSize);
            if (m_strChooseMusicAtlas.Length == 1)
            {
                m_strChooseMusicAtlas = "";
            }

            return true;
        }
    }
}