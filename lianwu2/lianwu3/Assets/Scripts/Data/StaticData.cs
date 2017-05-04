using LoveDance.Client.Data.Item;

namespace LoveDance.Client.Data
{
	public class StaticData
	{
		private static CItemInfoManager s_ItemInfoMgr = new CItemInfoManager();
		
		public static CItemInfoManager ItemDataMgr
		{
			get
			{
				return s_ItemInfoMgr;
			}
		}

	}
}