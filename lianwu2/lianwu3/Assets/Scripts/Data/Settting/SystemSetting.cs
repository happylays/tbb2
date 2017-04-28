using LoveDance.Client.Common;

namespace LoveDance.Client.Data.Setting
{
	public class SystemSetting
	{
		static XQIni SystemData
		{
			get
			{
				if (s_SystemData == null)
				{
					s_SystemData = new XQIni();
					s_SystemData.LoadIni(CommonValue.ConfDir + "SystemSetting.ini", false, true);
				}

				return s_SystemData;
			}
		}

		static XQIni s_SystemData = null;

		public static int AdjustRound
		{
			get
			{
				return SystemData.GetInt("TaiguMode", "AdjustRound", 0);
			}
		}

    /// <summary>
    /// 是否开启网络下载;
    /// </summary>
    public static bool IsWWWDownLoadOpen
    {
        get
        {
            return SystemData.GetInt("WWWDownLoad", "Open", 0) == 1;
        }
    }

		public static string GetInitEquip(string EquipPart, bool bMan)
		{
			if (bMan)
			{
				return SystemData.GetString("InitEquipAsset", EquipPart + "_boy", "");
			}
			else
			{
				return SystemData.GetString("InitEquipAsset", EquipPart + "_girl", "");
			}
		}

		public static string GetDefaultResEquip(string EquipPart, bool isBoy)
		{
			if (isBoy)
			{
				return SystemData.GetString("DefaultResEquipAsset", EquipPart + "_boy", "");
			}
			else
			{
				return SystemData.GetString("DefaultResEquipAsset", EquipPart + "_girl", "");
			}
		}

		public static string GetNpcEquip(string EquipPart)
		{
			return SystemData.GetString("NpcEquiptAsset", EquipPart, "");
		}

		public static string GetWeddingProp(string EquipPart)
		{
			return SystemData.GetString("WeddingPropAsset", EquipPart, "");
		}

		public static string GetNoviceProp(string EquipPart)
		{
			return SystemData.GetString("NovicePropAsset", EquipPart, "");
		}

		public static int GetClothEffectLevel(string effectLevel)
		{
			return SystemData.GetInt("ClothParticleEffect", effectLevel, -1);
		}

		public static bool IsShowFrame()
		{
			bool bShowFrame = (SystemData.GetInt("OtherSetting", "ShowFrame", 0) == 0 ? false : true);
			return bShowFrame;
		}

		public static bool IsLogFile()
		{
			bool bLogFile = (SystemData.GetInt("OtherSetting", "LogFile", 0) == 0 ? false : true);
			return bLogFile;
		}

		public static int GetProcGuideGood(bool isBoy)
		{
			int goodType = (isBoy ? SystemData.GetInt("ProcGuide", "MaleGood", 0) : SystemData.GetInt("ProcGuide", "FemaleGood", 0));
			return goodType;
		}

		public static bool IsShowAccountMgr()
		{
			bool bShowAccountMgr = (SystemData.GetInt("OtherSetting", "ShowAccountMgr", 0) == 0 ? false : true);
			return bShowAccountMgr;
		}

		public static float Model3PerfectOffset()
		{
			string strVal = SystemData.GetString("Model3", "PerfectOffset", "0");
			float nVal = 0;
			float.TryParse(strVal, out nVal);

			return (nVal / 1000 * -1);
		}

		public static float Model3MaxRingSize()
		{
			string strVal = SystemData.GetString("Model3", "MaxRingSize", "3");
			float nVal = 0;
			float.TryParse(strVal, out nVal);

			return nVal;
		}

        //充值开关
        public static bool IsOpenCharge()
        {
            bool bOpenCharge = (SystemData.GetInt("ChargeSetting", "openCharge", 1) == 0 ? false : true);
            return bOpenCharge;
        }

		/// <summary>
		/// 友盟开关
		/// </summary>
		public static bool IsUmengOpened()
		{
			bool bUmengOpened = (SystemData.GetInt("UmengSetting", "openUmeng", 0) == 0 ? false : true);
			return bUmengOpened;
		}

		public static bool IsExtraDownLoadOpened()
		{
			return SystemData.GetInt("ExtraDownLoad", "openExtraDownLoad", 0) == 1;
		}
		public static string GetPhotoWallDownloadIP(string def)
		{
			return SystemData.GetString("PhotoWall", "downloadIP", def);
		}
		public static int GetPhotoWallDownloadPort(int def) 
		{
			return SystemData.GetInt("PhotoWall", "downloadPort", def);
		}
		public static string GetPhotoWallUploadIP(string def) 
		{
			return SystemData.GetString("PhotoWall", "uploadIP", def);
		}
		public static int GetPhotoWallUploadPort(int def)
		{
			return SystemData.GetInt("PhotoWall", "uploadPort", def);
		}
	}
}