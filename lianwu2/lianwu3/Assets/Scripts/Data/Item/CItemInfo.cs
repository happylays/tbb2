using System;
using LoveDance.Client.Common;
using System.Collections.Generic;

namespace LoveDance.Client.Data.Item
{
	public class GeneInfo
	{
		public UInt16 m_nGeneID = 0;		     //id
		public int m_nParam1 = 0;          //参数
		public int m_nParam2 = 0;          //参数
		public string m_strParam = "";        //参数
	};

	public class CItemInfo
	{
		public uint m_nType = 0;
		public string m_strName = "a";
		public byte[] m_anType = new byte[3];
		public Sex_Type m_SexNeed = 0;
		public Int32 m_nMatune = 0;
		public Int16 m_nUseTimes = 0;
		public UInt16 m_nNeedLevel = 0;
		public bool m_bUnique = false;
		public UInt16 m_nMaxStackNumber = 0;

		public UInt16 m_nCoolDownTag = 0;
		public Int32 m_nCooldownTime = 0;

		public string m_strIcon = "a";
		public string m_strAtlas = "a";
		public string m_strAnimation = "a";

		public uint m_nClothEffect = 0;  //服饰特效
		public byte m_ClothColor = 0;//服饰颜色
		public bool m_bIsOldEffectCloth = false;

		public UInt16 m_nVIP = 0;
		public uint m_nIntimacy = 0;
		public string m_strIntro = "a";
		public bool m_bIsNew = true;
		public uint m_nTriggerType = 0; //触发物品

		public ClothAttributeInfo m_ClothAttr = null;	// 服饰属性信息
		public int m_EffectLevel = 0;
		public uint m_Fashion = 0;						// 时尚值

		public XQHashtable m_aGeneMap = new XQHashtable();

		private static ItemCloth_Type[] m_ExcludeItemsBodyLeg = new ItemCloth_Type[1] { ItemCloth_Type.ItemCloth_Type_Suit };
		private static ItemCloth_Type[] m_ExcludeItemsAll = new ItemCloth_Type[2] { ItemCloth_Type.ItemCloth_Type_Body, ItemCloth_Type.ItemCloth_Type_Leg };

		public bool Load(ref XQFileStream file)
		{

			file.ReadUInt(ref m_nType);

			UInt16 nSize = 0;
			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strName, nSize);

			file.ReadByte(ref m_anType[0]);
			file.ReadByte(ref m_anType[1]);
			file.ReadByte(ref m_anType[2]);

			short lTemp = 0;
			file.ReadShort(ref lTemp);
			m_SexNeed = (Sex_Type)lTemp;

			file.ReadInt(ref m_nMatune);
			file.ReadShort(ref m_nUseTimes);
			file.ReadUShort(ref m_nNeedLevel);
			file.ReadBool(ref m_bUnique);
			file.ReadUShort(ref m_nMaxStackNumber);

			file.ReadUShort(ref m_nCoolDownTag);
			file.ReadInt(ref m_nCooldownTime);

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strIcon, nSize);
			if (m_strIcon.Length == 1)
				m_strIcon = "";

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strAtlas, nSize);
			if (m_strAtlas.Length == 1)
				m_strAtlas = "";

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strAnimation, nSize);
			if (m_strAnimation.Length == 1)
				m_strAnimation = "";

			m_nClothEffect = file.ReadUInt();

			m_ClothColor = file.ReadByte();
			file.ReadBool(ref m_bIsOldEffectCloth);

			file.ReadUShort(ref m_nVIP);
			file.ReadUInt(ref m_nIntimacy);

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strIntro, nSize);
			if (m_strIntro.Length == 1)
				m_strIntro = "";
			CheckReturn(ref m_strIntro);
			file.ReadBool(ref m_bIsNew);

			file.ReadUInt(ref m_nTriggerType);

			m_ClothAttr = new ClothAttributeInfo((byte)file.ReadInt(), (uint)file.ReadInt());
			m_EffectLevel = file.ReadInt();
			m_Fashion = file.ReadUInt();

			UInt16 nGeneCount = 0;
			file.ReadUShort(ref nGeneCount);
			for (UInt16 i = 0; i < nGeneCount; i++)
			{
				GeneInfo aGene = new GeneInfo();
				file.ReadUShort(ref aGene.m_nGeneID);
				file.ReadInt(ref aGene.m_nParam1);
				file.ReadInt(ref aGene.m_nParam2);
				file.ReadUShort(ref nSize);
				file.ReadString(ref aGene.m_strParam, nSize);

				m_aGeneMap.Add(aGene.m_nGeneID, aGene);
			}

			return true;
		}

		public bool IsHasAttr(byte nAttrID)
		{
			return m_ClothAttr.m_nAttrID == nAttrID;
		}

		void CheckReturn(ref string str)
		{
			str.Replace("\\n", "\r\n");
		}

		/////////////////////////////////////////////////////////////
		//一级类型判断
		/////////////////////////////////////////////////////////////
		public bool IsEquip()
		{
			return (m_anType[0] == (byte)ItemClass_Type.ItemClassType_Equip);
		}

		public bool IsExpandable()
		{
			return (m_anType[0] == (byte)ItemClass_Type.ItemClassType_Expendable);
		}

		/// <summary>
		/// 是否是数值类奖励;
		/// </summary>
		/// <returns></returns>
		public bool IsNumerical()
		{
			return (m_anType[0] == (byte)ItemClass_Type.ItemClassType_Numerical);
		}

		/////////////////////////////////////////////////////////////
		//二级类型判断
		/////////////////////////////////////////////////////////////

		//服饰
		public bool IsCloth()
		{
			return IsEquip() && (m_anType[1] == (byte)ItemEquip_Type.ItemEquipType_Cloth);
		}

		//徽章 不包括婚戒
		public bool IsBadge()
		{
			return IsEquip() && (m_anType[1] == (byte)ItemEquip_Type.ItemEquipType_Badge && m_anType[2] != (byte)ItemBadge_Type.ItemBadge_Type_WeddingRing);
		}
		
		//座驾
		public bool IsVehicle()
		{
			return IsEquip() && m_anType[1] == (byte)ItemEquip_Type.ItemEquipType_Vehicle;
		}

		//功能类道具
		public bool IsFunctionExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Function);
		}

		//社交类道具
		public bool IsSocialExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Social);
		}

		//增益类道具
		bool IsAdditionExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Addition);
		}

		//礼包类道具
		public bool IsPacketExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Packet);
		}

		//宝箱类道具
		public bool IsBoxExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Box);
		}

		//变身类道具
		public bool IsTransformExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Transform);
		}

		//动态宝箱类道具
		public bool IsDynamicBoxExpandable()
		{
			return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_DynamicBox);
		}

		//主动型道具
		public bool IsInitiativeExpandable()
		{
			return IsExpandable() && (m_anType[2] == (byte)1);
		}

		//被动型道具
		public bool IsPassiveExpandable()
		{
			return IsExpandable() && (m_anType[2] == (byte)0);
		}

		/// <summary>
		/// 是否是光效石
		/// </summary>
		/// <returns></returns>
		public bool IsEffectSealExpandable()
		{
            return IsExpandable() && (m_anType[1] == (byte)ItemExpendable_Type.ItemExpendableType_Seal);
		}

		/// <summary>
		/// 是否需要触发道具
		/// </summary>
		/// <returns></returns>
		public bool IsTriggerExpandable()
		{
			return IsBoxExpandable() && m_nTriggerType != 0;
		}

		/////////////////////////////////////////////////////////////
		//三级类型判断
		/////////////////////////////////////////////////////////////
		public byte GetClothPos()
		{
			if (IsCloth())
			{
				return m_anType[2];
			}
			return 0;
		}


		//3 level type
		public bool IsItemCloth_Type_Hair()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Hair);
		}

		public bool IsItemCloth_Type_Face()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Face);
		}

		public bool IsItemCloth_Type_Body()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Body);
		}

		public bool IsItemCloth_Type_Gloves()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Gloves);
		}

		public bool IsItemCloth_Type_Leg()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Leg);
		}

		public bool IsItemCapsule_Type_Cap()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Cap);
		}

		public bool IsItemCloth_Type_Facial_Content()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Facial_Content);
		}

		public bool IsItemCloth_Type_Shoulders()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Shoulders);
		}

		public bool IsItemCloth_Type_Wing()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Wing);
		}

		public bool IsItemCloth_Type_LeftHand()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_LeftHand);
		}

		public bool IsItemCloth_Type_RightHand()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_RightHand);
		}

		public bool IsItemCloth_Type_Wrist()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Wrist);
		}

		public bool IsItemCloth_Type_Hip()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Hip);
		}

		public bool IsItemCloth_Type_Socks()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Socks);
		}
		public bool IsItemCloth_Type_LegWear() 
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_LegWear);
		}

		public bool IsItemCloth_Type_Feet()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Feet);
		}

		public bool IsItemCloth_Type_Skin()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Skin);
		}

		public bool IsItemCloth_Type_Suit()
		{
			return IsCloth() && (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_Suit);
		}
		
		//纹身
		public bool IsItemCloth_Type_Tattoo()
		{
			return IsCloth() 
				&& (m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_TattooArm
				|| m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_TattooLeg
				|| m_anType[2] == (byte)ItemCloth_Type.ItemCloth_Type_TattooBody);
		}

		public bool IsWeddingRing()
		{
			return m_anType[1] == (byte)ItemEquip_Type.ItemEquipType_Badge && m_anType[2] == (byte)ItemBadge_Type.ItemBadge_Type_WeddingRing;
		}

		public ItemCloth_Type[] ExcludeItems
		{
			get
			{
				if (IsItemCloth_Type_Suit())
				{
					return m_ExcludeItemsAll;
				}
				else if (IsItemCloth_Type_Body() || IsItemCloth_Type_Leg())
				{
					return m_ExcludeItemsBodyLeg;
				}
				return null;
			}
		}
		public static bool IsMainBodyCloth(ItemCloth_Type it)
		{
			if ((it >= ItemCloth_Type.ItemCloth_Type_Hair && it <= ItemCloth_Type.ItemCloth_Type_Leg) || it == ItemCloth_Type.ItemCloth_Type_Feet)
				return true;
			return false;
		}
	}

	public class CSkinInfo
	{
		public uint m_nType;
		public string m_strName;
		public byte m_nR;
		public byte m_nG;
		public byte m_nB;
		public byte m_nA;

		public bool Load(ref XQFileStream file)
		{

			file.ReadUInt(ref m_nType);

			UInt16 nSize = 0;
			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strName, nSize);

			file.ReadByte(ref m_nR);
			file.ReadByte(ref m_nG);
			file.ReadByte(ref m_nB);
			file.ReadByte(ref m_nA);

			return true;
		}

	}

	public class CItemAniInfo
	{
		public uint m_nType;
		public byte m_MoveState;//1-Fly
		public string m_strPlayerIdleAni;
		public string m_strPlayerMoveAni;
		public string m_strWingMoveAni;

		public bool Load(ref XQFileStream file)
		{
			file.ReadUInt(ref m_nType);
			file.ReadByte(ref m_MoveState);

			UInt16 nSize = 0;
			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strPlayerIdleAni, nSize);

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strPlayerMoveAni, nSize);

			file.ReadUShort(ref nSize);
			file.ReadString(ref m_strWingMoveAni, nSize);

			return true;
		}
	}

    public class PlaceFashionInfo : IStaticDataBase
    {
        public uint ID
        {
            get { return m_Place; }
        }

        public uint m_Place = 0;//部位
        public List<uint> m_FashionValue = new List<uint>();//时尚值

        public bool Load(XQFileStream xFs)
        {
            m_Place = xFs.ReadUInt();

            ushort valueCount = xFs.ReadUShort();
            uint fashionValue = 0;
            for (int i = 0; i < valueCount; ++i)
            {
                fashionValue = xFs.ReadUInt();
                m_FashionValue.Add(fashionValue);
            }

            return true;
        }
    }

}