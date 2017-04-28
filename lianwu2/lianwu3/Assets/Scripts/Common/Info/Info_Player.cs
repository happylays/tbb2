using System.Collections.Generic;
using UnityEngine;

//人物相关
namespace LoveDance.Client.Common
{
	public enum Sex_Type : byte
	{
		None = 0,

		Male,
		Female,
	}

	public class BriefAttr
	{
		public uint m_nRoleID = 0;
		public string m_strRoleName = "";
		public bool m_bIsBoy = false;
		public byte m_nSkinColor = 0;
		public string m_strDanceGroup = "";
		public byte m_nDanceGroupPos = 0;
		public bool m_bIsVIP = false;
		public ushort m_nVIPLevel = 0;
		public byte m_nBadgeId = 0;
		public byte m_nEffectId = 0;
		public ushort m_nTransformId = 0;
		public uint m_nTitleId = 0;
		public Dictionary<byte, string> m_dicMedals = new Dictionary<byte, string>();//人物信息勋章部分;
		public uint m_nVehicleID = 0;
		public int m_nVehiclePos = 0;
	}

	public enum ChangeRoleNameResult : byte
	{
		CHANGEROLENAME_SUCCESS = 0,
		CHANGEROLENAME_INVADROLENAME,	// 含有非法字符
		CHANGEROLENAME_DUPILICATED,		// 角色名称已存在
		CHANGEROLENAME_CD,
	}

	public enum ChangeConstellationResult : byte
	{
		CHANGECONSTELLATION_SUCCESS = 0,
		CHANGECONSTELLATION_INVALID_BIRTHDAY,           //生日无效
		CHANGECONSTELLATION_IS_SAME,                    //生日相同
		CHANGECONSTELLATION_CANNOT_CHANGE,              //已修改过
	};

	/// <summary>
	/// Location
	/// </summary>
	public enum RoleLocation
	{
		None = -1,

		World,
		Lobby,
		Room,

		max
	}

	public enum PlayerMoveType : byte
	{
		None,
		Walk,
		Fly,
	}

	public enum PlayerStyleType : byte
    {
        None,

        World,
        Room,
        Stage,
        Guide,
        WeddingOut,
        WeddingIn,
/*        Studio,*/
        Amusement,
        Encounter,
        Create,
        CeremonyOut,
        CeremonyIn,
        Handbook,
		BigMamaPlayer,
		BigMamaNpc,
		Transform,
		StarShowHeadline,	// 秀场头条
		StarShowAgent,		// 秀场经纪人
    };

    /// <summary>
    /// Only use in animation controller, "Stand" & "Move" can not be used in other animation clip name
    /// </summary>
	public enum AniMoveState : byte
    {
        Stand,	//Idle
        Move,	//Walk or Fly
    }

	public enum PhysicsType : byte
    {
        Player,
        NPCMinister,
        NPCGuide,
        NPCDancePartner,
        NPCLantern,
		NPCBigMama,
    }

	public class PlayerMoveToInfo
	{
		private Vector3 m_Direction = Vector3.zero;
		private Vector3 m_Pos = Vector3.zero;

		public Vector3 Pos
		{
			set
			{
				m_Pos = value;
			}
			get
			{
				return m_Pos;
			}
		}

		public Vector3 Direction
		{
			set
			{
				m_Direction = value;
			}
			get
			{
				return m_Direction;
			}
		}

		public bool IsDirctionXZEmpty
		{
			get
			{
				return m_Direction.x == 0 && m_Direction.z == 0;
			}
		}
	}

	public enum AnimationType : byte
	{
		None = 0,
		Default,
		Shining,
		Transform,
	}
}