using System;
using LoveDance.Client.Common;
using LoveDance.Client.Data.Item;

namespace LoveDance.Client.Network.Item
{
	public class CEquipItem : CItemBase , ICloneable
	{
		public byte m_nHaveEquiped = 0;
		public uint m_EffectId = 0;

		public bool IsEquiped
		{
			get
			{
				return m_nHaveEquiped > 0;
			}
		}

		public byte BadgePos
		{
			get
			{
				return (byte)(m_nHaveEquiped - 1);
			}
		}

		/// <summary>
		/// 将当前对象的所有元素深拷贝到toObj中。
		/// </summary>
		protected override void CopyTo(object toObj)
		{
			CEquipItem item = toObj as CEquipItem;
			if (item != null)
			{
				base.CopyTo(toObj);

				item.m_nHaveEquiped = this.m_nHaveEquiped;
				item.m_EffectId = this.m_EffectId;
			}
		}

		/// <summary>
		/// 克隆一个深拷贝对象
		/// </summary>
		public override object Clone()
		{
			CEquipItem res = new CEquipItem();

			this.CopyTo(res);

			return (object)res;
		}

		public static CEquipItem CreateEquip(CItemInfo pItemInfo)
		{
			if (pItemInfo != null && pItemInfo.IsEquip())
			{
				CEquipItem pEquip = new CEquipItem();
				pEquip.ItemInfo = pItemInfo;

				return pEquip;
			}

			return null;
		}

		public override bool doDecode(NetReadBuffer DataIn)
		{
			if (base.doDecode(DataIn))
			{
				m_nHaveEquiped = (byte)DataIn.GetByte();
				m_EffectId = DataIn.GetUInt();
				return true;
			}

			return false;
		}

		public byte GetClothPos()
		{
			return m_ItemInfo.GetClothPos();
		}

		public ItemCloth_Type[] ExcludeItems
		{
			get
			{
				if (m_ItemInfo != null)
				{
					return m_ItemInfo.ExcludeItems;
				}

				return null;
			}
		}
	}
}