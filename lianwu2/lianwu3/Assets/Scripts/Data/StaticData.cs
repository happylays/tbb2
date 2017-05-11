using LoveDance.Client.Data.Item;
using LoveDance.Client.Data.Scene;
using LoveDance.Client.Data.Model;

namespace LoveDance.Client.Data
{
	public class StaticData
	{
		private static CItemInfoManager s_ItemInfoMgr = new CItemInfoManager();
        private static CSceneInfoManager s_SceneInfoMgr = new CSceneInfoManager();
        private static CModelInfoManager s_ModelInfoMgr = new CModelInfoManager();

		public static CItemInfoManager ItemDataMgr
		{
			get
			{
				return s_ItemInfoMgr;
			}
		}

        public static CSceneInfoManager SceneDataMgr
        {
            get
            {
                return s_SceneInfoMgr;
            }
        }

        public static CModelInfoManager ModelDataMgr
        {
            get
            {
                return s_ModelInfoMgr;
            }
        }

	}
}