using System;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
//using LoveDance.Client.Network.SystemSetting;
//using LoveDance.Client.Network.Player;
using LoveDance.Client.Network.Item;
using LoveDance.Client.Data.Item;
using LoveDance.Client.Data;


namespace LoveDance.Client.Logic.Role
{
    public class PlayerItem
    {
        internal List<CItemBase>[] m_AllItem = new List<CItemBase>[(int)Item_Column.ItemColumn_MaxNumber];
        internal CEquipItem[] m_DefaultEquip = new CEquipItem[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
		internal List<uint> m_ListVehicleItemID = new List<uint>();
		internal bool m_bNeedArrange = false;
		
		/// <summary>
		/// 克隆一个深拷贝对象
		/// </summary>
        //public virtual object Clone()
        //{
        //    PlayerItem res = new PlayerItem();
			
        //    this.CopyTo(res);

        //    return res;
        //}

		/// <summary>
		/// 将当前对象的所有元素深拷贝到toObj对象中。
		/// </summary>
        //protected virtual void CopyTo(object toObj)
        //{
        //    PlayerItem to = toObj as PlayerItem;

        //    if (to != null)
        //    {
        //        to.m_AllItem = new List<CItemBase>[this.m_AllItem.Length];
        //        for (int i = 0; i < this.m_AllItem.Length; i++)
        //        {
        //            to.m_AllItem[i] = new List<CItemBase>();

        //            List<CItemBase> list = this.m_AllItem[i];
        //            for (int j = 0; j < list.Count; j++)
        //            {
        //                CItemBase itemBase = null;
        //                if (list[j] != null)
        //                {
        //                    itemBase = (CItemBase)(list[j].Clone());
        //                }
        //                to.m_AllItem[i].Add(itemBase);
        //            }
        //        }

        //        for (int i = 0; i < this.m_DefaultEquip.Length; i++)
        //        {
        //            if (m_DefaultEquip[i] != null)
        //            {
        //                to.m_DefaultEquip[i] = (CEquipItem)(m_DefaultEquip[i].Clone());
        //            }
        //        }

        //        for (int i = 0; i < this.m_ListClothEffectHandbookProgress.Count; i++)
        //        {
        //            ClothEffectHandbookProgress progress = m_ListClothEffectHandbookProgress[i];
        //            ClothEffectHandbookProgress clone = null;
        //            if (progress != null)
        //            {
        //                clone = (ClothEffectHandbookProgress)(progress.Clone());
        //            }
        //            to.m_ListClothEffectHandbookProgress.Add(clone);
        //        }

        //        for (int i = 0; i < this.m_ListSevenColorInfo.Count; i++)
        //        {
        //            ClothEffectSevenColorProgress progress = this.m_ListSevenColorInfo[i];
        //            ClothEffectSevenColorProgress clone = null;
        //            if (progress != null)
        //            {
        //                clone = (ClothEffectSevenColorProgress)(progress.Clone());
        //            }
        //            to.m_ListSevenColorInfo.Add(clone);
        //        }
        //    }
        //}

        //public virtual void SerializeItem(NetReadBuffer DataIn)
        //{
        //    ushort itemCount = DataIn.GetUShort();
        //    if (itemCount > 0)
        //    {
        //        for (ushort i = 0; i < itemCount; ++i)
        //        {
        //            uint nItemType = DataIn.GetUInt();
        //            ushort nColumn = DataIn.GetUShort();
        //            ushort nGrid = DataIn.GetUShort();

        //            byte size = 0;
        //            size = DataIn.GetByte();
        //            CItemInfo itemInfo = StaticData.ItemDataMgr.GetByID(nItemType);
        //            CItemBase itemBase = CItemBase.CreateItem(itemInfo);
        //            if (itemBase != null)
        //            {
        //                itemBase.doDecode(DataIn);
        //                itemBase.m_nColumn = nColumn;
        //                itemBase.m_nIndex = nGrid;

        //                PrepareGrid((Item_Column)nColumn, nGrid);
        //                UpdateItem((Item_Column)nColumn, nGrid, itemBase);
        //            }
        //            else
        //            {
        //                string s = "PlayerItem Docode Error_" + "ItemType: " + nItemType + " Not Exist in iteminfo";
        //                int curPos = DataIn.getPostion();
        //                curPos += size;
        //                DataIn.setMaxDataPostion(curPos);
        //                DataIn.setPostion(curPos);
        //                GameMsg_C2S_Msg_BugReport msg = new GameMsg_C2S_Msg_BugReport();
        //                msg.m_BugList = s;
        //                NetworkMgr.SendMsg(msg);
        //            }
        //        }
				
        //        CheckNeedArrangeItem();
        //    }
        //}

        public void InitItemList(ushort nBadgeGridNum)
        {
            for (Item_Column column = 0; column < Item_Column.ItemColumn_MaxNumber; ++column)
            {
                ushort maxCount = 0;
                if (column == Item_Column.ItemPlayerColumn_Badge)
                {
                    maxCount = nBadgeGridNum;
                }
                else if (column == Item_Column.ItemPlayerColumn_ClothCapsule)
                {
                    maxCount = (ushort)ItemCloth_Type.ItemCloth_Type_MaxNumber;
                }
                else if (column == Item_Column.ItemPlayerColumn_WeddingRing)
                {
                    maxCount = 1;
                }

                List<CItemBase> itemList = new List<CItemBase>();
                for (ushort itemGrid = 0; itemGrid < maxCount; ++itemGrid)
                {
                    itemList.Add(null);
                }

                m_AllItem[(int)column] = itemList;
            }
        }

        public List<CItemBase> GetItemList(Item_Column column)
        {
            if (IsColumnValid(column))
            {
                return m_AllItem[(ushort)column];
            }

            return null;
        }
		
		/// <summary>
		/// 获取座驾列表
		/// </summary>
		public List<CItemBase> GetVehicleList()
		{
			List<CItemBase> tempItemList = new List<CItemBase>();
			List<uint> tempDelList = new List<uint>();
			
			for (int i = 0; i < m_ListVehicleItemID.Count; ++i)
			{
				uint tempItemID = m_ListVehicleItemID[i];
				CItemBase tempItem = GetItemByType(tempItemID);
				if (tempItem != null)
				{
					tempItemList.Add(tempItem);
				}
				else
				{
					tempDelList.Add(tempItemID);
				}
			}
			
			for (int i = 0; i < tempDelList.Count; ++i)
			{
				uint tempItemID = tempDelList[i];
				if (m_ListVehicleItemID.Contains(tempItemID))
				{
					m_ListVehicleItemID.Remove(tempItemID);
				}
			}
			
			return tempItemList;
        }

        public CItemBase GetItemByPos(Item_Column column, ushort grid)
        {
            if (IsGridValid(column, grid))
            {
                return m_AllItem[(ushort)column][grid];
            }

            return null;
        }

        public CItemBase GetItemByType(uint itemType)
        {
            CItemBase itemBase = null;

            for (Item_Column column = 0; column < Item_Column.ItemColumn_MaxNumber; ++column)
            {
                if (column != Item_Column.ItemPlayerColumn_ClothCapsule)
                {
                    itemBase = GetItemByType(column, itemType);
                    if (itemBase != null)
                    {
                        break;
                    }
                }
            }

            return itemBase;
        }

        public uint GetItemTotalCountInBag(uint itemType)
        {
            uint count = 0;

            for (Item_Column column = 0; column < Item_Column.ItemColumn_MaxNumber; ++column)
            {
                if (column != Item_Column.ItemPlayerColumn_ClothCapsule && column != Item_Column.ItemBagColumn_Storage)
                {
                    List<CItemBase> itemBaseList = GetItemList(column);
                    if (itemBaseList != null && itemBaseList.Count > 0)
                    {
                        int baseCount = itemBaseList.Count;
                        for (int i = 0; i < baseCount; ++i)
                        {
                            CItemBase itemBase = itemBaseList[i];
                            if (itemBase != null && itemBase.IsValid() && itemBase.ItemInfo.m_nType == itemType)
                            {
                                count += itemBase.m_nCount;
                            }
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 获取背包和仓库物品的总数量
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public uint GetAllItemTotalCount(uint itemType)
        {
            return GetItemTotalCountInBag(itemType) + GetItemTotalCountInStore(itemType);
        }

        /// <summary>
        /// 获取仓库中的数量
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public uint GetItemTotalCountInStore(uint itemType)
        {
            uint count = 0;
            List<CItemBase> itemBaseList = GetItemList(Item_Column.ItemBagColumn_Storage);
            if (itemBaseList != null && itemBaseList.Count > 0)
            {
                int baseCount = itemBaseList.Count;
                for (int i = 0; i < baseCount; ++i)
                {
                    CItemBase itemBase = itemBaseList[i];
                    if (itemBase != null && itemBase.IsValid() && itemBase.ItemInfo.m_nType == itemType)
                    {
                        count += itemBase.m_nCount;
                    }
                }
            }
            return count;
        }

        public CItemBase GetItemByType(Item_Column column, uint itemType)
        {
            List<CItemBase> itemList = GetItemList(column);
            if (itemList != null)
            {
                int itemCount = itemList.Count;
                for (int i = 0; i < itemCount; ++i)
                {
                    CItemBase itemBase = itemList[i];
                    if (itemBase != null && itemBase.IsValid() && itemBase.ItemInfo.m_nType == itemType)
                    {
                        return itemBase;
                    }
                }
            }

            return null;
        }

        public CEquipItem GetCurrentEquip(ItemCloth_Type clothType)
        {
            //judge whether has transform cloth
            if (HasTransformCloth())
            {
                return (CEquipItem)GetItemByPos(Item_Column.ItemPlayerColumn_ClothTransform, (ushort)clothType);
            }
            return (CEquipItem)GetItemByPos(Item_Column.ItemPlayerColumn_ClothCapsule, (ushort)clothType);
        }

        public CEquipItem GetRelatedEquip(ItemCloth_Type clothType)
        {
            CEquipItem equipItem = GetCurrentEquip(clothType);
            if (equipItem == null)
            {
                if (!HasTransformCloth())
                {
                    equipItem = GetDefaultEquip(clothType);
                }
            }

            return equipItem;
        }

        public CEquipItem GetNormalRelatedEquip(ItemCloth_Type clothType)
        {
            CEquipItem equipItem = GetNormalCurrentEquip(clothType);
            if (equipItem == null)
            {
                equipItem = GetDefaultEquip(clothType);
            }

            return equipItem;
        }

        private CEquipItem GetNormalCurrentEquip(ItemCloth_Type clothType)
        {
            return (CEquipItem)GetItemByPos(Item_Column.ItemPlayerColumn_ClothCapsule, (ushort)clothType);
        }

        public CEquipItem GetDefaultEquip(ItemCloth_Type clothType)
        {
            return m_DefaultEquip[(int)clothType];
        }

        public void PrepareGrid(Item_Column itemColumn, ushort itemGrid)
        {
            if (IsColumnValid(itemColumn))
            {
                int nCount = itemGrid - m_AllItem[(ushort)itemColumn].Count + 1;
                for (int i = 0; i < nCount; ++i)
                {
                    m_AllItem[(ushort)itemColumn].Add(null);
                }
            }
        }

        public bool IsColumnValid(Item_Column itemColumn)
        {
            if (itemColumn <= Item_Column.ItemColumn_Begin || itemColumn >= Item_Column.ItemColumn_MaxNumber)
            {
                return false;
            }

            return true;
        }

        public bool IsGridValid(Item_Column itemColumn, ushort itemGrid)
        {
            if (IsColumnValid(itemColumn) && itemGrid < m_AllItem[(ushort)itemColumn].Count)
            {
                return true;
            }

            return false;
        }

        public virtual void UpdateItem(Item_Column column, ushort grid, CItemBase itemBase)
        {
            if (IsGridValid(column, grid))
            {
                //蛋道具 加入data;
                m_AllItem[(ushort)column][grid] = itemBase;
            }
			
			if (itemBase != null)
			{
				if (itemBase.IsVehicle() && !m_ListVehicleItemID.Contains(itemBase.ItemInfo.m_nType))
				{
					m_ListVehicleItemID.Add(itemBase.ItemInfo.m_nType);
				}
			}
        }

        //public void RevertBody()
        //{
        //    OwnerPlayer.RevertBodySync(false);
        //}

        public bool HasTransformCloth()
        {
            int transformClothCount = 0;

            if (m_AllItem[(int)Item_Column.ItemPlayerColumn_ClothTransform] != null)
            {
                List<CItemBase> itemList = m_AllItem[(int)Item_Column.ItemPlayerColumn_ClothTransform];
                for (int i = 0; i < itemList.Count; ++i)
                {
                    if (itemList[i] != null)
                    {
                        transformClothCount++;
                    }
                }
            }

            return transformClothCount == 0 ? false : true;
        }

        public uint GetWeddingRingID()
        {
            uint ringId = 0;

            CItemBase item = m_AllItem[(int)Item_Column.ItemPlayerColumn_WeddingRing][0];
            if (item != null)
            {
                ringId = item.ItemInfo.m_nType;
            }

            return ringId;
        }
        
        public ushort GetItemColumn(uint itemType)
        {
            ushort column = 0;
            CItemBase itemBase = GetItemByType(itemType);
            if (itemBase != null)
            {
                column = itemBase.m_nColumn;
            }

            return column;
        }

        public ushort GetItemSlot(uint itemType)
        {
            ushort slot = 0;
            CItemBase itemBase = GetItemByType(itemType);
            if (itemBase != null)
            {
                slot = itemBase.m_nIndex;
            }

            return slot;
        }


        public virtual void UpdateEx()
        {
        }

		public List<CEquipItem> GetItemListByClothType(ItemCloth_Type clothType)
		{
			List<CEquipItem> listItem = new List<CEquipItem>();

			CItemBase item = null;
			CEquipItem equipItem = null;
			for (Item_Column column = 0; column < Item_Column.ItemBagColumn_Storage + 1; ++column)
			{
				List<CItemBase> itemBaseList = GetItemList(column);
				if (itemBaseList != null && itemBaseList.Count > 0)
				{
					int baseCount = itemBaseList.Count;
					for (int i = 0; i < baseCount; ++i)
					{
						item = itemBaseList[i];
						if (item != null && item.IsEquip())
						{
							equipItem = item as CEquipItem;
							if(equipItem != null)
							{
								if(equipItem.GetClothPos() == (byte)clothType)
								{
									listItem.Add(equipItem);
								}
							}
						}
					}
				}
			}

			return listItem;
		}
		
		/// <summary>
		/// 检查是否需要整理背包
		/// </summary>
		public void CheckNeedArrangeItem()
		{
			m_bNeedArrange = false;
			List<uint> tmpListItemType = new List<uint>();
			// 需要整理堆叠的物品列表-道具类
			List<CItemBase> gtList = GetItemList(Item_Column.ItemBagColumn_Expandable);
			for (int i = 0; i < gtList.Count; ++i)
			{
				CItemBase gtItem = gtList[i];
				if (gtItem != null && gtItem.ItemInfo != null && gtItem.ItemInfo.m_nMaxStackNumber > 1)
				{
					uint tmpItemType = gtItem.ItemInfo.m_nType;
					// 检查物品数量是否达到物品最大堆叠数
					if (gtItem.Count < gtItem.ItemInfo.m_nMaxStackNumber)
					{
						// 未到达堆叠数的记录物品类型
						// 如果列表已经有该物品类型说明有多个栏位的物品可以堆叠在一起
						if (tmpListItemType.Contains(tmpItemType))
						{
							m_bNeedArrange = true;
							break;
						}
						else
						{
							tmpListItemType.Add(tmpItemType);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// 是否需要整理物品
		/// </summary>
		public bool NeedArrangeItem
		{
			get
			{
				return m_bNeedArrange;
			}
		}
    }
}