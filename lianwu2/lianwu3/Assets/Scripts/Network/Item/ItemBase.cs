using System;
using LoveDance.Client.Data.Item;
using LoveDance.Client.Data;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Item
{
	public class CItemBase : ICloneable
	{
		public ushort m_nColumn = 0;
		public UInt16 m_nIndex = 0;
		public Int16 m_nUseTimes = 0;	     // 还可以使用的，-1表示可以无限使用
		public UInt16 m_nCount = 0;		     // 物品数量
		public int m_nMatune = 0;	     // >0表示到期时间，<0表示无期效的物品，永久有效
		public CItemInfo m_ItemInfo = null;
		
		public uint m_nTopAttrValue = 0;
		public float m_nTotalScore = 0;//物品的属性分,用于T台秀装备显示的排序和不显示剔除等逻辑;

		/// <summary>
		/// 将当前对象的所有元素深拷贝到toObj中。
		/// </summary>
		protected virtual void CopyTo(object toObj)
		{
			CItemBase itemBase = toObj as CItemBase;

			if (itemBase != null)
			{
				itemBase.m_nColumn = this.m_nColumn;
				itemBase.m_nIndex = this.m_nIndex;
				itemBase.m_nUseTimes = this.m_nUseTimes;
				itemBase.m_nCount = this.m_nCount;
				itemBase.m_nMatune = this.m_nMatune;
				// CItemInfo从静态数据中加载，直接拷贝引用
				itemBase.m_ItemInfo = this.m_ItemInfo;
			}
		}

		/// <summary>
		/// 克隆一个深拷贝对象
		/// 其中m_ItemInfo为静态数据，做浅拷贝
		/// </summary>
		public virtual object Clone()
		{
			CItemBase res = new CItemBase();

			this.CopyTo(res);

			return (object)res;
		}

		public static CItemBase CreateItem(uint nType)
		{
			CItemInfo pItemInfo = StaticData.ItemDataMgr.GetByID(nType);
			return CreateItem(pItemInfo);
		}

		public static CItemBase CreateItem(CItemInfo pItemInfo)
		{
			CItemBase pItem = null;
			if (pItemInfo == null)
				return pItem;

			if (pItemInfo.IsEquip())
			{
				pItem = CEquipItem.CreateEquip(pItemInfo);
			}
			else
			{
				pItem = new CItemBase();
				if (pItem != null)
				{
					pItem.ItemInfo = pItemInfo;
				}
			}
			return pItem;
		}

		public virtual bool doDecode(NetReadBuffer DataIn)
		{
			if (DataIn != null)
			{
				m_nUseTimes = DataIn.GetShort();
				m_nCount = DataIn.GetUShort();
				m_nMatune = DataIn.GetInt();
				return true;
			}
			return false;
		}

		public bool IsValid()
		{
			if (IsPlayerColumn())
			{
				return m_ItemInfo != null;
			}
			else
			{
				return m_ItemInfo != null && m_nCount > (UInt16)0;
			}
		}


		/// <summary>
		/// 是否是人物身上栏位，目前只用于衣服服饰栏，可扩展
		/// </summary>
		/// <returns></returns>
		public bool IsPlayerColumn()
		{
			return m_nColumn == (ushort)Item_Column.ItemPlayerColumn_ClothCapsule;
		}

		public bool IsExpandable()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsExpandable();
		}

		public bool IsEquip()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsEquip();
		}

		public bool IsCloth()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsCloth();
		}

		public bool IsBadge()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsBadge();
		}

		public bool IsItemCloth_Type_Suit()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsItemCloth_Type_Suit();
		}

		public bool IsWeddingRing()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsWeddingRing();
		}
		
		public bool IsVehicle()
		{
			return m_ItemInfo == null ? false : m_ItemInfo.IsVehicle();
		}

		public string ClothResource
		{
			get
			{
				string r = "";
				if (IsCloth() && ItemInfo != null)
				{
                    r = ItemInfo.m_strAtlas + ".clh";
				}
				return r;
			}
		}


		/// <summary>
		/// Gets or sets the item info.
		/// </summary>
		public CItemInfo ItemInfo
		{
			set
			{
				m_ItemInfo = value;
			}
			get
			{
				return m_ItemInfo;
			}
		}

		public int Matune
		{
			set
			{
				m_nMatune = value;
			}
			get
			{
				return m_nMatune;
			}
		}

		public UInt16 Count
		{
			set
			{
				m_nCount = value;
			}
			get
			{
				return m_nCount;
			}
		}

		public Int16 UseTimes
		{
			set
			{
				m_nUseTimes = value;
			}
			get
			{
				return m_nUseTimes;
			}
		}

		public string Resource
		{
			get
			{
				return m_ItemInfo != null ? m_ItemInfo.m_strIcon : "";
			}
		}
	}
}

