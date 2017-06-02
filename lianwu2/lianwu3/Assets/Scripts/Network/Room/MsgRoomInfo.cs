using System.Collections.Generic;
using LoveDance.Client.Network.Lantern;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Room
{
    public class RoomPlayerInfo
    {
        public uint m_nRoleID = 0;
        public string m_strRoleName = "";
        public bool m_bIsHost = false;
        public byte m_nRoleType = (byte)RoleRoomType.None;
        public bool m_bIsBoss = false;
        public byte m_nRolePos = 0;
        public byte m_nRoleState = (byte)RoleRoomState.None;        
        public byte m_nRoleSex = (byte)Sex_Type.None;
        public byte m_nRoleSkin = 1;
        public string m_strDanceGroup = "";
        public byte m_nDanceGroupPos = 0;
        public ushort m_nDanceGroupBadge = 0;
        public ushort m_nDanceGroupEffect = 0;
        public NetReadBuffer m_ItemPacket = null;
        public NetReadBuffer m_GenePacket = null;
        public bool m_bIsVIP;
        public ushort m_nVIPLevel;
        public byte m_nMoveType = 0;
        public ushort m_nTransformId = 0;
        public uint m_nSkinCandyColor = 0;
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
        
        public void doDecode(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_strRoleName = DataIn.GetPerfixString();
            m_bIsHost = DataIn.GetBool();
            m_nRoleType = DataIn.GetByte();
            m_nRolePos = DataIn.GetByte();
            m_nRoleState = DataIn.GetByte();
            m_nRoleSex = DataIn.GetByte();
            m_nRoleSkin = DataIn.GetByte();
            m_nDanceGroupPos = DataIn.GetByte();
            m_strDanceGroup = DataIn.GetPerfixString();
            m_nDanceGroupBadge = DataIn.GetUShort();
            m_nDanceGroupEffect = DataIn.GetUShort();
            m_bIsVIP = DataIn.GetBool();
            m_nVIPLevel = DataIn.GetUShort();
            m_nMoveType = DataIn.GetByte();
            m_nTransformId = DataIn.GetUShort();
            m_nSkinCandyColor = DataIn.GetUInt();

            DataIn.GetUShort();	// 保持和服务器对称,表示以下数据接收长度

            byte[] itemBuf = DataIn.GetFixLenBytes();
            if (itemBuf != null)
            {
                m_ItemPacket = new NetReadBuffer(itemBuf);
            }

            byte[] geneBuf = DataIn.GetFixLenBytes();
            if (geneBuf != null)
            {
                m_GenePacket = new NetReadBuffer(geneBuf);
            }
            
            m_nCurVehicleID = DataIn.GetUInt();
            m_nCurVehicleOwnerID = DataIn.GetUInt();
            m_nCurVehiclePos = DataIn.GetInt();
        }

        public void doDecode_Lantern(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_strRoleName = DataIn.GetPerfixString();
            m_bIsHost = DataIn.GetBool();
            m_nRoleType = DataIn.GetByte();
            m_nRolePos = DataIn.GetByte();
            m_nRoleState = DataIn.GetByte();
            m_nRoleSex = DataIn.GetByte();
            m_nRoleSkin = DataIn.GetByte();
            m_nDanceGroupPos = DataIn.GetByte();
            m_strDanceGroup = DataIn.GetPerfixString();
            m_nDanceGroupBadge = DataIn.GetUShort();
            m_nDanceGroupEffect = DataIn.GetUShort();
            m_bIsVIP = DataIn.GetBool();
            m_nVIPLevel = DataIn.GetUShort();
            m_nMoveType = DataIn.GetByte();
            m_nTransformId = DataIn.GetUShort();
            m_nSkinCandyColor = DataIn.GetUInt();

            byte[] itemBuf = DataIn.GetFixLenBytes();
            if (itemBuf != null)
            {
                m_ItemPacket = new NetReadBuffer(itemBuf);
            }

            byte[] geneBuf = DataIn.GetFixLenBytes();
            if (geneBuf != null)
            {
                m_GenePacket = new NetReadBuffer(geneBuf);
            }

            m_nCurVehicleID = DataIn.GetUInt();
            m_nCurVehicleOwnerID = DataIn.GetUInt();
            m_nCurVehiclePos = DataIn.GetInt();
        }

        public void doDecode_Challenge(NetReadBuffer DataIn)
        {
            m_nRoleID = DataIn.GetUInt();
            m_strRoleName = DataIn.GetPerfixString();
            m_bIsHost = DataIn.GetBool();
            m_nRoleType = DataIn.GetByte();
            m_bIsBoss = DataIn.GetBool();
            m_nRolePos = DataIn.GetByte();
            m_nRoleState = DataIn.GetByte();
            m_nRoleSex = DataIn.GetByte();
            m_nRoleSkin = DataIn.GetByte();
            m_nDanceGroupPos = DataIn.GetByte();
            m_strDanceGroup = DataIn.GetPerfixString();
            m_nDanceGroupBadge = DataIn.GetUShort();
            m_nDanceGroupEffect = DataIn.GetUShort();
            m_bIsVIP = DataIn.GetBool();
            m_nVIPLevel = DataIn.GetUShort();
            m_nMoveType = DataIn.GetByte();
            m_nTransformId = DataIn.GetUShort();
            m_nSkinCandyColor = DataIn.GetUInt();

            byte[] itemBuf = DataIn.GetFixLenBytes();
            if (itemBuf != null)
            {
                m_ItemPacket = new NetReadBuffer(itemBuf);
            }

            byte[] geneBuf = DataIn.GetFixLenBytes();
            if (geneBuf != null)
            {
                m_GenePacket = new NetReadBuffer(geneBuf);
            }

        }
    }
}
