using UnityEngine;
using System.Collections.Generic;
using System;
using LoveDance.Client.Data.Tips;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Data;
using LoveDance.Client.Network.Item;

namespace LoveDance.Client.Logic.Role
{
	public class PlayerAttr
    {
        public uint RoleID
        {
            get
            {
                return m_nRoleID;
            }
        }

        public string RoleName
        {
            get
            {
                return m_strRoleName;
            }
            set
            {
                m_strRoleName = value;
            }
        }

        public bool IsBoy
        {
            get
            {
                return m_bIsBoy;
            }
        }

		public bool DanceSexIsBoy
		{
			set
			{
				m_DanceSexIsBoy = value;
			}
			get
			{
				return m_DanceSexIsBoy;
			}
		}

        public byte SkinColor
        {
            get
            {
                return m_nSkinColor;
            }
            set
            {
                m_nSkinColor = value;
            }
        }

        public ushort Level
        {
            get
            {
                return m_nLev;
            }
            set
            {
                m_nLev = value;
            }
        }

        public uint Exp
        {
            get
            {
                return m_nExp;
            }
            set
            {
                m_nExp = value;
            }
        }

        public int FunUnLockID
        {
            get
            {
                return m_nFunID;
            }
            set
            {
                m_nFunID = value;
            }
        }

        public string Birthday
        {
            get
            {
                return m_strBirthday;
            }
            set
            {
                m_strBirthday = value;
            }
        }

        public byte Constellation
        {
            get
            {
                return m_nConstellation;
            }
            set
            {
                m_nConstellation = value;
            }
        }

        public string Signature
        {
            get
            {
                return m_strSignature;
            }
            set
            {
                m_strSignature = value;
            }
        }

        public uint Hot
        {
            get
            {
                return m_nHot;
            }
            set
            {
                m_nHot = value;
            }
        }

        public uint TitleID
        {
            get
            {
                return m_nTitleID;
            }
            set
            {
                m_nTitleID = value;
            }
        }

        public uint Prestige
        {
            get
            {
                return m_nPrestige;
            }
            set
            {
                m_nPrestige = value;
            }
        }

        public byte BadgeGridNum
        {
            get
            {
                return m_nBadgeGridNum;
            }
            set
            {
                m_nBadgeGridNum = value;
            }
        }

        // G coin
        public uint Money
        {
            get
            {
                return m_nMoney;
            }
            set
            {
                m_nMoney = value;
            }
        }

        // M coin
        public uint Pt
        {
            get
            {
                return m_nPt;
            }
            set
            {
                m_nPt = value;
            }
        }

        // 累计MCoin充值量
        public uint PtTotal
        {
            get
            {
                return m_nPtTotal;
            }
            set
            {
                m_nPtTotal = value;
            }
        }

        public uint PtBind
        {
            get
            {
                return m_nPtBind;
            }
            set
            {
                m_nPtBind = value;
            }
        }

        // discarded, connect Tanfeng
        public uint Change
        {
            get
            {
                return m_nChange;
            }
            set
            {
                m_nChange = value;
            }
        }

		public uint RoleCreateTime
		{
			get
			{
				return m_nRoleCreateTime;
			}
		}

        public long RecTag
        {
            get
            {
                return m_nRecTag;
            }
            set
            {
                m_nRecTag = value;
            }
        }

        public bool CompleteDanceAni
        {
            get
            {
                return m_bCompleteDanceAni;
            }
            set
            {
                m_bCompleteDanceAni = value;
            }
        }

        public bool HasFirstReCharge
        {
            get
            {
                return m_bHasFirstReCharge;
            }
            set
            {
                m_bHasFirstReCharge = value;
            }
        }

        public int CurLineID
        {
            get
            {
                return m_nCurLineID;
            }
            set
            {
                m_nCurLineID = value;
            }
        }

        public string CurLineName
        {
            get
            {
                return m_strCurLineName;
            }
            set
            {
                m_strCurLineName = value;
            }
        }

        public string DanceGroup
        {
            get
            {
                return m_strDanceGroup;
            }
            set
            {
                m_strDanceGroup = value;
            }
        }

        public byte DanceGroupPos
        {
            get
            {
                return m_nDanceGroupPos;
            }
            set
            {
                m_nDanceGroupPos = value;
            }
        }

        public byte BadgeId
        {
            get
            {
                return m_nBadgeId;
            }
            set
            {
                m_nBadgeId = value;
            }
        }

        public byte EffectId
        {
            get
            {
                return m_nEffectId;
            }
            set
            {
                m_nEffectId = value;
            }
        }

        public Vector3 RolePosition
        {
            get
            {
                return m_RolePosition;
            }
            set
            {
                m_RolePosition = value;
            }
        }

        public float RoleOrient
        {
            get
            {
                return m_Orient;
            }
            set
            {
                m_Orient = value;
            }
        }

        public bool IsVIP
        {
            get
            {
                return m_bIsVIP;
            }
            set
            {
                m_bIsVIP = value;
            }
        }

        public ushort VIPLevel
        {
            get
            {
                return m_nVIPLevel;
            }
            set
            {
                m_nVIPLevel = value;
            }
        }

        public bool IsCanChangeXingzuo
        {
            get
            {
                return m_IsCanChangeXingzuo;
            }
            set
            {
                m_IsCanChangeXingzuo = value;
            }
        }

        /// <summary>
        /// 人物头顶的勋章展示;
        /// </summary>
        public Dictionary<byte, string> DicMedalIcon
        {
            get
            {
                return m_dicMedalIcon;
            }
            set
            {
                m_dicMedalIcon = value;
            }
        }

        public bool EnableAudition
        {
            get
            {
                return m_bEnableAudition;
            }
        }

        public int TransformID
        {
            get
            {
                return m_TransformID;
            }
            set
            {
                m_TransformID = value;
            }
        }
		
		public uint VehicleID
		{
			get
			{
				return m_nVehicleID;
			}
			set
			{
				m_nVehicleID = value;
			}
		}
		
		public int VehiclePos
		{
			get
			{
				return m_nVehiclePos;
			}
			set
			{
				m_nVehiclePos = value;
			}
		}
		
		public bool MagicArrayNew
		{
			get
			{
				return m_bMagicArrayNew;
			}
			set
			{
				m_bMagicArrayNew = value;
			}
		}

        uint m_nRoleID = 0;
        string m_strRoleName;

        bool m_bIsBoy = false;
		bool m_DanceSexIsBoy = false;

        byte m_nSkinColor;
        ushort m_nLev;
        uint m_nExp;
        int m_nFunID = 0;

        string m_strBirthday;
        byte m_nConstellation;
        string m_strSignature;

        uint m_nHot;
        uint m_nTitleID;
        uint m_nPrestige;

        byte[] m_anBeiBaoKuozhan = new byte[CommonDef.BAG_MAX_ITEMTYPE];
        byte m_nBadgeGridNum;

        uint m_nMoney;
        uint m_nPt;
        uint m_nPtTotal;	// 累计MCoin充值量
        uint m_nPtBind;
        uint m_nChange;///[ycy] not in use
					   ///
		uint m_nRoleCreateTime = 0;	// 角色创建时间

        long m_nRecTag = 0;

        bool m_bCompleteTeachGuide;
        bool m_bCompleteDanceAni = false;
        bool m_bHasFirstReCharge;

        int m_nCurLineID;
        string m_strCurLineName;
        bool m_bEnableAudition = false;

        string m_strDanceGroup;
        byte m_nDanceGroupPos;
        byte m_nBadgeId = 0;
        byte m_nEffectId = 0;


        /// <summary>
        /// 勋章类型 to 勋章icon;
        /// </summary>
        Dictionary<byte, string> m_dicMedalIcon = new Dictionary<byte, string>();

        /// <summary>
        /// 修改装备中的勋章;
        /// </summary>
        public void RefreshMedalIcon(byte nType, string strIcon)
        {
            if (!m_dicMedalIcon.ContainsKey(nType))//未包含;
            {
                m_dicMedalIcon.Add(nType, strIcon);//添加;
            }
            else//包含;
            {
                m_dicMedalIcon[nType] = strIcon;//覆盖;
            }
        }

        Vector3 m_RolePosition = Vector3.zero;
        float m_Orient = 0;

        // VIP info 
        bool m_bIsVIP = false;
        ushort m_nVIPLevel = 0;

        int m_TransformID = 0;
		uint m_nVehicleID = 0;
		int m_nVehiclePos = 0;

        bool m_IsCanChangeXingzuo = false;
		uint m_FashionValue = 0;			// 时尚值
        uint m_nGuideData = 0x00;
		
		bool m_bMagicArrayNew = false;

		/// <summary>
		/// 克隆一个深拷贝对象
		/// </summary>
		/// <param name="attr"></param>
		public object Clone()
		{
			PlayerAttr res = new PlayerAttr();

			res.m_nRoleID = this.m_nRoleID;
			res.m_strRoleName = this.m_strRoleName;
			res.m_bIsBoy = this.m_bIsBoy;
			res.m_nSkinColor = this.m_nSkinColor;
			res.m_nLev = this.m_nLev;
			res.m_nExp = this.m_nExp;
			res.m_nFunID = this.m_nFunID;
			res.m_strBirthday = this.m_strBirthday;
			res.m_nConstellation = this.m_nConstellation;
			res.m_strSignature = this.m_strSignature;
			res.m_nHot = this.m_nHot;
			res.m_nTitleID = this.m_nTitleID;
			res.m_nPrestige = this.m_nPrestige;
			res.m_anBeiBaoKuozhan = new byte[this.m_anBeiBaoKuozhan.Length];
			for (int i = 0; i < this.m_anBeiBaoKuozhan.Length; i++)
			{
				res.m_anBeiBaoKuozhan[i] = this.m_anBeiBaoKuozhan[i];
			}
			res.m_nBadgeGridNum = this.m_nBadgeGridNum;
			res.m_nMoney = this.m_nMoney;
			res.m_nPt = this.m_nPt;
			res.m_nPtTotal = this.m_nPtTotal;
			res.m_nPtBind = this.m_nPtBind;
			res.m_nChange = this.m_nChange;
			res.m_nRecTag = this.m_nRecTag;
			res.m_bCompleteTeachGuide = this.m_bCompleteTeachGuide;
			res.m_bCompleteDanceAni = this.m_bCompleteDanceAni;
			res.m_bHasFirstReCharge = this.m_bHasFirstReCharge;
			res.m_nCurLineID = this.m_nCurLineID;
			res.m_strCurLineName = this.m_strCurLineName;
			res.m_bEnableAudition = this.m_bEnableAudition;
			res.m_strDanceGroup = this.m_strDanceGroup;
			res.m_nDanceGroupPos = this.m_nDanceGroupPos;
			res.m_nBadgeId = this.m_nBadgeId;
			res.m_nEffectId = this.m_nEffectId;
			res.m_dicMedalIcon = new Dictionary<byte, string>();
			foreach (KeyValuePair<byte, string> item in this.m_dicMedalIcon)
			{
				res.m_dicMedalIcon.Add(item.Key, item.Value);
			}
			res.m_RolePosition = this.m_RolePosition;
			res.m_Orient = this.m_Orient;
			res.m_bIsVIP = this.m_bIsVIP;
			res.m_nVIPLevel = this.m_nVIPLevel;
			res.m_TransformID = this.m_TransformID;
			res.m_nVehicleID = this.m_nVehicleID;
			res.m_nVehiclePos = this.m_nVehiclePos;
			res.m_IsCanChangeXingzuo = this.m_IsCanChangeXingzuo;
			res.m_nGuideData = this.m_nGuideData;

			return res;
		}

        public void SerializeAttr(NetReadBuffer wholeInfo)
        {
            m_nRoleID = wholeInfo.GetUInt();
            m_strRoleName = wholeInfo.GetPerfixString();

            Sex_Type sexType = (Sex_Type)wholeInfo.GetByte();
            m_bIsBoy = (sexType == Sex_Type.Male ? true : false);

            m_nMoney = wholeInfo.GetUInt();
            m_nExp = wholeInfo.GetUInt();
            m_nLev = wholeInfo.GetUShort();
            m_nHot = wholeInfo.GetUInt();

            m_strBirthday = wholeInfo.GetPerfixString();

            m_nConstellation = wholeInfo.GetByte();
            m_strSignature = wholeInfo.GetPerfixString();

            m_nSkinColor = wholeInfo.GetByte();

            ///m_nTitleID = DataIn.GetUInt();
            m_nPrestige = wholeInfo.GetUInt();

            for (int i = 0; i < (int)CommonDef.BAG_MAX_ITEMTYPE; i++)
            {
                m_anBeiBaoKuozhan[i] = wholeInfo.GetByte();
            }
            m_nBadgeGridNum = wholeInfo.GetByte();
            m_nPt = wholeInfo.GetUInt();
            m_nPtTotal = wholeInfo.GetUInt();
            m_nPtBind = wholeInfo.GetUInt();
            m_nChange = wholeInfo.GetUInt();

			m_nRoleCreateTime = wholeInfo.GetUInt();

            m_bCompleteTeachGuide = wholeInfo.GetBool();
            m_bCompleteDanceAni = wholeInfo.GetBool();

            m_nGuideData = wholeInfo.GetUInt();

            m_nCurLineID = wholeInfo.GetInt();
            m_strCurLineName = wholeInfo.GetPerfixString();
            m_bEnableAudition = wholeInfo.GetBool();
            m_bHasFirstReCharge = wholeInfo.GetBool();
            m_TransformID = wholeInfo.GetInt();

            m_IsCanChangeXingzuo = wholeInfo.GetBool();
			// 时尚值需要执行通知其他数据
			m_bMagicArrayNew = wholeInfo.GetBool();
        }

        public void Serialize(BriefAttr attrInfo)
        {
            if (attrInfo != null)
            {
                m_nRoleID = attrInfo.m_nRoleID;
                m_strRoleName = attrInfo.m_strRoleName;
                m_bIsBoy = attrInfo.m_bIsBoy;
                m_nSkinColor = attrInfo.m_nSkinColor;
                m_strDanceGroup = attrInfo.m_strDanceGroup;
                m_nDanceGroupPos = attrInfo.m_nDanceGroupPos;
                m_nBadgeId = attrInfo.m_nBadgeId;
                m_nEffectId = attrInfo.m_nEffectId;
                m_nBadgeId = attrInfo.m_nBadgeId;
                m_nEffectId = attrInfo.m_nEffectId;
                m_bIsVIP = attrInfo.m_bIsVIP;
                m_nVIPLevel = attrInfo.m_nVIPLevel;
                m_TransformID = attrInfo.m_nTransformId;

                m_dicMedalIcon = new Dictionary<byte, string>(attrInfo.m_dicMedals);
				
				m_nVehicleID = attrInfo.m_nVehicleID;
				m_nVehiclePos = attrInfo.m_nVehiclePos;
            }
        }

        public string GetConstellationByID(byte id)
        {
            string[] constell = {"All_Constell_1", "All_Constell_2", "All_Constell_3", "All_Constell_4",
								"All_Constell_5", "All_Constell_6", "All_Constell_7", "All_Constell_8",
								"All_Constell_9", "All_Constell_10", "All_Constell_11", "All_Constell_12"};
            return constell[id];
        }

        public string GetTitleNameByID(uint nID)
        {
            string str = "";
            str = SystemTips.GetTipsContent("Title" + (nID + 1).ToString());
            return str;
        }

        public string GetPrestigeNameByPoint(uint nPoint)
        {
            string str = "";
            str = SystemTips.GetTipsContent("SocialPre" + nPoint.ToString());
            return str;
        }


        public void OnRefreshTransformID(int transformId)
        {
            m_TransformID = transformId;
        }

        public void OnRefreshGuideData(uint guideData)
        {
            m_nGuideData = guideData;
        }

    }
}
