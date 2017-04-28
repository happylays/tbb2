//物品相关
using System.Collections.Generic;
namespace LoveDance.Client.Common
{
	/////////////////////////////////////////////////////////////////////
	//一级类型
	/////////////////////////////////////////////////////////////////////

	//装备，道具
	public enum ItemClass_Type : byte
	{
		ItemClassType_None = 0,

		ItemClassType_Equip,		//装备
		ItemClassType_Expendable,	//道具(消耗品)
		ItemClassType_Numerical,    //数值类（金钱，经验，荣誉等数值类奖励）

		ItemClassType_MaxNumber
	};

	////////////////////////////////////////////////////////////////////
	//二级类型
	////////////////////////////////////////////////////////////////////

	//子类型，装备之衣服，首饰等
	public enum ItemEquip_Type : byte
	{
		ItemEquipType_None = 0,

		ItemEquipType_Cloth,		//衣服
		ItemEquipType_Badge,		//徽章
		ItemEquipType_Vehicle,		//座驾

		ItemEquipType_MaxNumber
	};

	//子类型
	public enum ItemExpendable_Type : byte
	{
		ItemExpendableType_None = 0,

		ItemExpendableType_Function = 1,    //功能类
		ItemExpendableType_Social = 2,      //社交类
		ItemExpendableType_Addition = 3,	//增益类
		ItemExpendableType_Packet = 4,		//礼包类
		ItemExpendableType_Box = 5,         //宝箱类
		ItemExpendableType_Transform = 6,   //变身类
		ItemExpendableType_DynamicBox = 7,  //动态宝盒类

		ItemExpendableType_Seal = 10,         //魔法石

		ItemExpendableType_MaxNumber
	};

	public enum ItemNumerical_Type : byte
	{
		None = 0,

		Money,
		MCoin,
		MBCoin,
		EXP,
		Honor,//荣誉;
		Contribution,//贡献;
		VipValue,//VIP经验;
		Intimacy,//亲密;

		Max
	};


	////////////////////////////////////////////////////////////////////
	//三级类型
	////////////////////////////////////////////////////////////////////

	//衣服子类型
	public enum ItemCloth_Type //ItemCloth_Type
	{
		ItemCloth_Type_Begin = -1,

		ItemCloth_Type_Hair,          //发型
		ItemCloth_Type_Face,          //表情
		ItemCloth_Type_Body,          //上衣
		ItemCloth_Type_Gloves,        //手套
		ItemCloth_Type_Leg,           //下装

		ItemCloth_Type_Cap,           //头饰
		ItemCloth_Type_Facial_Content,//脸饰
		ItemCloth_Type_Shoulders,     //肩膀
		ItemCloth_Type_Wing,          //翅膀
		ItemCloth_Type_LeftHand,      //左手持
		ItemCloth_Type_RightHand,     //右手持
		ItemCloth_Type_Wrist,         //手腕
		ItemCloth_Type_Hip,           //臀部
		ItemCloth_Type_Socks,         //袜子
		ItemCloth_Type_Feet,          //鞋子

		ItemCloth_Type_Skin,          //肤色
		ItemCloth_Type_Transform,
		ItemCloth_Type_Suit,			//套装
		
		ItemCloth_Type_TattooArm,		//纹身-双臂
		ItemCloth_Type_TattooLeg,		//纹身-双腿
		ItemCloth_Type_TattooBody,		//纹身-身体

		ItemCloth_Type_LegWear,	//腿部装饰

		ItemCloth_Type_MaxNumber,


	};

	public enum ItemBadge_Type : byte   //徽章类别
	{
		ItemBadge_Type_WeddingRing = 1,//结婚戒指

		ItemBadge_Type_SpecialRing = 2,//个性指环
		ItemBadge_Type_VIPBadge = 3,//VIP徽章
		ItemBadge_Type_ShowEffect = 4,//展示特效
		ItemBadge_Type_ExpEffect = 5,//经验效果
		ItemBadge_Type_CardDesign = 6,//名片装饰

		Max,

	}

	public enum Item_Column
	{
		ItemColumn_Begin = -1,

		//////////////////////////////////////////////////////////////////////////
		//背包栏  可扩展
		//////////////////////////////////////////////////////////////////////////
		ItemBagColumn_ClothCapsule,		//服装饰品栏
		ItemBagColumn_Expandable,		//道具栏
		ItemBagColumn_Badge,			//徽章栏
		ItemBagColumn_Storage,			//仓库栏

		//////////////////////////////////////////////////////////////////////////
		//角色身上的装备位置  不可以扩展
		//////////////////////////////////////////////////////////////////////////
		ItemPlayerColumn_Badge,			//身上戴的徽章栏
		ItemPlayerColumn_ClothCapsule,	//身上穿的衣服&服饰栏
		ItemPlayerColumn_DefaultCloth,
		ItemPlayerColumn_ClothTransform, //变身服饰栏
		ItemPlayerColumn_WeddingRing,//身上婚戒栏

		ItemColumn_MaxNumber,
	};

	public enum ClothEffectSevenColorType : byte
	{
		Begin = 0,

		/// <summary>
		/// 赤
		/// </summary>
		Red,
		/// <summary>
		/// 橙
		/// </summary>
		Orange,
		/// <summary>
		/// 黄
		/// </summary>
		Yellow,
		/// <summary>
		/// 绿
		/// </summary>
		Green,
		/// <summary>
		/// 青
		/// </summary>
		Cyanogen,
		/// <summary>
		/// 蓝
		/// </summary>
		Blue,
		/// <summary>
		/// 紫
		/// </summary>
		Purple,
	}

	public enum eTuanhuiItemType : byte
	{
		ETuanhuiItem_None = 0,
		ETuanhuiItem_Badge = 1,
		ETuanhuiItem_Effect = 2
	}

	public interface IItemBriefInfo
	{
		uint ItemId { get; }
		ushort Count { get; }
		int Matune { get; }
	}

	public class ItemBriefInfo
	{
		private uint m_nItemID;
		private ushort m_nCount;
		private int m_nMatune;

		public uint ItemID { get { return m_nItemID; } }
		public ushort Count { get { return m_nCount; } }
		public int Matune { get { return m_nMatune; } }

		public ItemBriefInfo(uint itemID, ushort count, int matune)
		{
			m_nItemID = itemID;
			m_nCount = count;
			m_nMatune = matune;
		}
	}

	public class ItemBagPickParam
	{
		public UIFlag m_UIFrom = UIFlag.none;
		public object m_OtherData = null;
		public ItemCloth_Type[] m_ClothTypeArr = null;
		public bool m_PermanentFilter = false;										// true 开启永久服饰过滤
		public Dictionary<ItemCloth_Type, uint> m_DefaultShow = null;
		public Callback<Dictionary<ItemCloth_Type, uint>> m_SaveCallBack = null;
	}
}