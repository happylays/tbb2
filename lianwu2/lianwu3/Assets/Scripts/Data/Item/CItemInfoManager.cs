using System;
using System.Text;
using LoveDance.Client.Common;
using LoveDance.Client.Data.Tips;
using System.Collections.Generic;

namespace LoveDance.Client.Data.Item
{
	public class CItemInfoManager : StaticDataMgrBase
	{
		private const string GOLDTICKET_SPRITE_NAME = "t_jinquan";
		private const string MCOINTICKET_SPRITE_NAME = "t_mb";
		private const string MBINDTICKET_SPRITE_NAME = "t_mbpink";
		private const string EXP_SPRITE_NAME = "t_exp";
		private const string INTIMACY_SPRITE_NAME = "t_qinmidu";
		private const string VIPVALUE_SPRITE_NAME = "t_vipchengzhang";
		private const string CONTRIBUTION_SPRITE_NAME = "t_gongxiandu";
		private const string HONOR_SPRITE_NAME = "t_wutuanrongyu";

		private XQHashtable s_ItemInfoMap = new XQHashtable();
		private XQHashtable s_ItemInfoMap_Numerical = new XQHashtable();

		private XQHashtable s_SkinInfoMap = new XQHashtable();
		private XQHashtable s_ItemAniInfoMap = new XQHashtable();
		
		private Dictionary<byte, uint> m_dicEffectStoneInfo = new Dictionary<byte, uint>();	// 光效石属性

		/// <summary>
		/// 初始化数值类道具虚拟数据;
		/// </summary>
		public CItemInfoManager()
		{
			for (int i = (int)ItemNumerical_Type.None; i < (int)ItemNumerical_Type.Max; i++)
			{
				ItemNumerical_Type eKey = (ItemNumerical_Type)i;
				CItemInfo gtItem = CreateNumericalItemInfo(eKey);
				if (gtItem != null)
				{
					s_ItemInfoMap_Numerical.Add(eKey, gtItem);
				}
			}
		}

        private CItemInfo CreateNumericalItemInfo(ItemNumerical_Type numType)
        {
            string strPic = "";
            string strName = "";
            switch (numType)
            {
                case ItemNumerical_Type.Money:
                    strPic = GOLDTICKET_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Goods_Gold");
                    break;
                case ItemNumerical_Type.MCoin:
                    strPic = MCOINTICKET_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Goods_Mcoin");
                    break;
                case ItemNumerical_Type.MBCoin:
                    strPic = MBINDTICKET_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Goods_MbindCoin");
                    break;
                case ItemNumerical_Type.EXP:
                    strPic = EXP_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Quest_Exp");
                    break;
                case ItemNumerical_Type.Honor:
                    strPic = HONOR_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Quest_Honor");
                    break;
                case ItemNumerical_Type.Contribution:
                    strPic = CONTRIBUTION_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("DanceGroup_ShopContribution");
                    break;
                case ItemNumerical_Type.VipValue:
                    strPic = VIPVALUE_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Quest_VipValue");
                    break;
                case ItemNumerical_Type.Intimacy:
                    strPic = INTIMACY_SPRITE_NAME;
                    strName = SystemTips.GetTipsContent("Quest_Intimacy");
                    break;
                default:
                    return null;
            }

            CItemInfo itemInfo = new CItemInfo();
            itemInfo.m_strIcon = strPic;
            itemInfo.m_strName = strName;
            itemInfo.m_anType[0] = (byte)ItemClass_Type.ItemClassType_Numerical;
            itemInfo.m_anType[1] = (byte)numType;

            return itemInfo;
        }

		public bool Load(XQFileStream file)
		{
			Destroy();

			return ParseFile(file);
		}

		public bool Add(XQFileStream file)
		{
			return ParseFile(file);
		}

		bool ParseFile(XQFileStream file)
		{
			if (file != null && file.IsOpen())
			{
				UInt16 usNumber = 0;
				file.ReadUShort(ref usNumber);

				for (UInt16 i = 0; i < usNumber; i++)
				{
					CItemInfo iteminfo = new CItemInfo();
					iteminfo.Load(ref file);
					RegistItemInfo(iteminfo);
				}

				// 额外时尚值 客户端不使用
				List<PlaceFashionInfo> m_PlaceFashionList = new List<PlaceFashionInfo>();
				_Load<PlaceFashionInfo>(file, m_PlaceFashionList);

				// 光效石属性
				XQDataLoadHelper.LoadToDic<byte, uint>(ref m_dicEffectStoneInfo, file, XQDataLoadHelper.ReadByte, XQDataLoadHelper.ReadUInt, false);

				// skin 
				usNumber = 0;
				file.ReadUShort(ref usNumber);

				for (UInt16 i = 0; i < usNumber; i++)
				{
					CSkinInfo skinInfo = new CSkinInfo();
					skinInfo.Load(ref file);
					AddSkinInfo(skinInfo);
				}

				//Item animation
				file.ReadUShort(ref usNumber);
				for (UInt16 i = 0; i < usNumber; i++)
				{
					CItemAniInfo itemAniInfo = new CItemAniInfo();
					itemAniInfo.Load(ref file);
					AddItemAniInfo(itemAniInfo);
				}

				file.Close();
				s_ItemInfoMap.Sort();
				s_SkinInfoMap.Sort();
				s_ItemAniInfoMap.Sort();
				return true;
			}
			return false;
		}

		public CItemInfo GetByID(uint nType)
		{
			if (s_ItemInfoMap != null && s_ItemInfoMap.Contains(nType))
			{
				return (CItemInfo)s_ItemInfoMap[nType];
			}
			else
			{
				return null;
			}
		}

		public CItemInfo GetByNumType(ItemNumerical_Type numType)
		{
			if (s_ItemInfoMap_Numerical != null && s_ItemInfoMap_Numerical.Contains(numType))
			{
				return (CItemInfo)s_ItemInfoMap_Numerical[numType];
			}
			else
			{
				return null;
			}
		}

		public CSkinInfo GetSkinInfoByID(uint nType)
		{
			if (s_SkinInfoMap != null && s_SkinInfoMap.Contains(nType))
			{
				return (CSkinInfo)s_SkinInfoMap[nType];
			}
			else
			{
				return null;
			}
		}

		public CItemAniInfo GetItemAniInfoByID(uint nType)
		{
			if (s_ItemAniInfoMap != null && s_ItemAniInfoMap.Contains(nType))
			{
				return (CItemAniInfo)s_ItemAniInfoMap[nType];
			}
			return null;
		}

		public XQHashtable GetAllItem()
		{
			return s_ItemInfoMap;
		}

		public string GetItemName(uint itemid)
		{
			CItemInfo itemInfo = GetByID(itemid);
			if (itemInfo != null)
			{
				return itemInfo.m_strName;
			}

			return null;
		}

		public string[] GetItemsNameList<T>(IList<T> items) where T : IItemBriefInfo
		{
			if (items != null && items.Count > 0)
			{
				List<string> res = new List<string>();
				string name = null;
				IItemBriefInfo item = null;
				for (int i = 0; i < items.Count; i++)
				{
					item = items[i];
					if (item != null)
					{
						name = GetItemName(item.ItemId);
						if (string.IsNullOrEmpty(name))
						{
							res.Add(name);
						}
					}
				}

				return res.ToArray();
			}

			return null;
		}
		
        //public string GetItemNameAndCount(uint itemid, ushort count, int matune)
        //{
        //    StringBuilder info = new StringBuilder();
			
        //    bool needcount = true;
        //    if(count == 0 && matune == 0)
        //    {
        //        needcount = false;
        //    }
			
        //    CItemInfo itemInfo = GetByID(itemid);
        //    if(itemInfo != null)
        //    {
        //        if(needcount)
        //        {
        //            StringBuilder tempStr = new StringBuilder();
					
        //            if(itemInfo.IsExpandable())
        //            {
        //                tempStr.Append(count.ToString());
        //                tempStr.Append(SystemTips.GetTipsContent("ItemBag_Unit"));
						
        //                info.Append(SystemTips.GetTipsContent("SpecialMall_ItemInfo", new string[]{"", itemInfo.m_strName, tempStr.ToString()}));
        //            }
        //            else
        //            {
        //                if(matune >= 0)
        //                {
        //                    if(matune == 0)
        //                    {
        //                        matune = itemInfo.m_nMatune;
        //                    }
							
        //                    GameTime gt = new GameTime(matune);

        //                    if (gt.Day > 0)
        //                    {
        //                        tempStr.Append(gt.Day.ToString());
        //                        tempStr.Append(SystemTips.GetTipsContent("ItemBag_Day"));
        //                    }
        //                    if (gt.Hour > 0)
        //                    {
        //                        tempStr.Append(gt.Hour.ToString());
        //                        tempStr.Append(SystemTips.GetTipsContent("ItemBag_Hour"));
        //                    }
        //                    if (gt.Minute > 0)
        //                    {
        //                        tempStr.Append(gt.Minute.ToString());
        //                        tempStr.Append(SystemTips.GetTipsContent("ItemBag_Minute"));
        //                    }
        //                }
        //                else if(matune == -1)
        //                {
        //                    tempStr.Append(SystemTips.GetTipsContent("ItemBag_Forever"));
        //                }
						
        //                info.Append(SystemTips.GetTipsContent("SpecialMall_ItemInfo", new string[]{"", itemInfo.m_strName, tempStr.ToString()}));
        //            }
        //        }
        //        else
        //        {
        //            info.Append(itemInfo.m_strName);
        //        }
        //    }
			
        //    return info.ToString();
        //}

		/// <summary>
		/// 获取特效等级
		/// </summary>
		/// <param name="nType">物品ID</param>
		/// <returns></returns>
		public int GetEffectLevelById(uint nType)
		{
			int res = 0;
			CItemInfo info = GetByID(nType);

			if (info != null)
			{
				res = info.m_EffectLevel;
			}

			return res;
		}

		/// <summary>
		/// 获取时尚值
		/// </summary>
		/// <param name="nType">物品ID</param>
		/// <returns></returns>
		public uint GetFashionValueById(uint nType)
		{
			uint res = 0;
			CItemInfo info = GetByID(nType);

			if (info != null)
			{
				res = info.m_Fashion;
			}

			return res;
		}

		/// <summary>
		/// 获取服饰属性信息
		/// </summary>
		/// <param name="nType">物品ID</param>
		/// <returns></returns>
        //public ClothAttributeInfo GetClothAttrById(uint nType)
        //{
        //    ClothAttributeInfo res = null;
        //    CItemInfo info = GetByID(nType);

        //    if (info != null)
        //    {
        //        res = info.m_ClothAttr;
        //    }

        //    return res;
        //}

		/// <summary>
		/// 获取光效石对属性的加成值
		/// <param name="stoneID">光效石 等级</param>
		/// <param name="groupID">分数</param>
		/// </summary>
		/// <returns>
		/// 分组ID
		/// </returns>
		public uint GetAddedValueByAttrGroup(byte nLevel)
		{
			if (m_dicEffectStoneInfo.ContainsKey(nLevel))
			{
				return m_dicEffectStoneInfo[nLevel];
			}
			return 0;
		}

		/// <summary>
		/// 获取光效石属性加成信息
		/// <param name="stoneID"><光效石ID></param>
		/// </summary>
		/// <returns>
		/// 加成信息列表
		/// </returns>
		public uint GetEffectLevelScore(ushort nLevel)
		{
			if (m_dicEffectStoneInfo.ContainsKey((byte)nLevel))
			{
				return m_dicEffectStoneInfo[(byte)nLevel];
			}
			else
			{
				UnityEngine.Debug.LogError("CHandbookInfoMgr GetEffectLevelScore,m_dicEffectStoneInfo not ContainsKey " + nLevel);
			}

			return 0;
		}

		void RegistItemInfo(CItemInfo iteminfo)
		{
			if (iteminfo != null && iteminfo.m_nType > 0)
			{
				if (s_ItemInfoMap.Contains(iteminfo.m_nType))
				{
					//Debug.Log("RegistItemInfo Duplicate,nType:" + iteminfo.m_nType);
					s_ItemInfoMap.Remove(iteminfo.m_nType);
				}

				s_ItemInfoMap.Add(iteminfo.m_nType, iteminfo);
			}
		}

		void AddSkinInfo(CSkinInfo skinInfo)
		{
			if (skinInfo != null && skinInfo.m_nType > 0)
			{
				if (!s_SkinInfoMap.Contains(skinInfo.m_nType))
				{
					s_SkinInfoMap.Add(skinInfo.m_nType, skinInfo);
				}
			}
		}

		void AddItemAniInfo(CItemAniInfo itemAniInfo)
		{
			if (itemAniInfo != null && itemAniInfo.m_nType > 0)
			{
				if (!s_ItemAniInfoMap.Contains(itemAniInfo.m_nType))
				{
					s_ItemAniInfoMap.Add(itemAniInfo.m_nType, itemAniInfo);
				}
			}
		}

		void Destroy()
		{
			if (s_ItemInfoMap != null)
			{
				s_ItemInfoMap.Clear();
			}

			if (null != s_SkinInfoMap)
			{
				s_SkinInfoMap.Clear();
			}
		}
	}
}