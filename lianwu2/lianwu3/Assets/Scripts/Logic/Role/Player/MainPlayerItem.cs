using UnityEngine;
using System;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Network.Player;
using LoveDance.Client.Network.Item;

namespace LoveDance.Client.Logic.Role
{
    public class MainPlayerItem : PlayerItem , ICloneable
    {
        float m_fElapseTime = 0f;
		private int mIntervalcount = 0;
		private int mElapsetime = 0;
		private List<CItemBase> mItemList = null;
		private CItemBase mItemBase = null;

        uint m_nCollectCount = 0;//收藏总数
        uint m_nBrilliantCount = 0;//闪亮值

        public uint CollectCount
        {
            get
            {
                return m_nCollectCount;
            }
            set
            {
                m_nCollectCount = value;
            }
        }

        public uint BrilliantCount
        {
            get
            {
                return m_nBrilliantCount;
            }
            set
            {
                m_nBrilliantCount = value;
            }
        }

		/// <summary>
		/// 克隆一个深拷贝对象
		/// </summary>
		public override object Clone()
		{
			MainPlayerItem res = new MainPlayerItem();

			this.CopyTo(res);

			return res;
		}

		/// <summary>
		/// 将当前对象的所有元素深拷贝到toObj对象中。
		/// </summary>
		protected override void CopyTo(object toObj)
		{
			MainPlayerItem to = toObj as MainPlayerItem;

			if (to != null)
			{
				base.CopyTo(toObj);

				to.m_fElapseTime = this.m_fElapseTime;
			}
		}

        public override void SerializeItem(NetReadBuffer DataIn)
        {
            base.SerializeItem(DataIn);

            ushort defaultClothCount = DataIn.GetUShort();
            for (int i = 0; i < defaultClothCount; ++i)
            {
                ushort column = DataIn.GetUShort();
                ushort grid = DataIn.GetUShort();

                CItemBase itembase = GetItemByPos((Item_Column)column, grid);
                if (itembase != null)
                {
                    m_DefaultEquip[itembase.ItemInfo.GetClothPos()] = (CEquipItem)itembase;
                }
            }

            ushort itemCount = DataIn.GetUShort();
            for (int i = 0; i < itemCount; ++i)
            {
                ClothEffectHandbookProgress info = new ClothEffectHandbookProgress();
                info.doDecode(DataIn);
                m_ListClothEffectHandbookProgress.Add(info);
            }

            itemCount = DataIn.GetUShort();
            for (int i = 0; i < itemCount; ++i)
            {
                ClothEffectSevenColorProgress info = new ClothEffectSevenColorProgress();
                info.doDecode(DataIn);
                m_ListSevenColorInfo.Add(info);
            }

            m_nCollectCount = DataIn.GetUInt();
            m_nBrilliantCount = DataIn.GetUInt();
        }

        public override void UpdateEx()
        {
            m_fElapseTime += Time.deltaTime;

			mIntervalcount = (int)(m_fElapseTime / CommonDef.UPDATE_INTERVAL_ITEM);
			if (mIntervalcount > 0)
            {
				mElapsetime = CommonDef.UPDATE_INTERVAL_ITEM * mIntervalcount;
				m_fElapseTime -= mElapsetime;

                for (Item_Column column = 0; column <= Item_Column.ItemBagColumn_Storage; ++column)
                {
					mItemList = m_AllItem[(int)column];
					for (int i = 0; i < mItemList.Count; ++i)
                    {
						mItemBase = mItemList[i];
						if (mItemBase != null && mItemBase.m_nMatune >= 0)
                        {
							if (mItemBase.m_nMatune > mElapsetime)
                            {
								mItemBase.m_nMatune -= mElapsetime;
                            }
                            else
                            {
								mItemBase.m_nMatune = 0;
                            }
                        }
                    }
                }
            }
        }
        
        //the equiped items of main player will not be serialized in separate array, use the flag IsEquiped to check
        public override void UpdateItem(Item_Column column, ushort grid, CItemBase itemBase)
        {
            if (column == Item_Column.ItemBagColumn_ClothCapsule)
            {
                CEquipItem oldEquip = (CEquipItem)m_AllItem[(int)column][grid];
                base.UpdateItem(column, grid, itemBase);

                if (oldEquip != null && oldEquip.IsEquiped)
                {
                    m_AllItem[(int)Item_Column.ItemPlayerColumn_ClothCapsule][oldEquip.GetClothPos()] = null;
                }

                CEquipItem newEquip = itemBase as CEquipItem;
                if (newEquip != null && newEquip.IsEquiped)
                {
                    m_AllItem[(int)Item_Column.ItemPlayerColumn_ClothCapsule][newEquip.GetClothPos()] = newEquip;
                }
            }
            else
            {
                base.UpdateItem(column, grid, itemBase);
				CheckNeedArrangeItem();
            }
        }   
    }
}