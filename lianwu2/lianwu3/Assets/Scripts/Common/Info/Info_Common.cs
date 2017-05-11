using UnityEngine;

//共用
namespace LoveDance.Client.Common
{
	public enum GameLayer
	{
		NONE = -1,

		Default = 0,

		Player = 8,
		Player_Title = 9,
		Player_UI = 10,
		StudioPlayer = 11,
		StudioPlayer_Title = 12,
		Scene = 20,
		Scene_UI = 21,
		Player_Multi = 22,
		Player_UI_Secnod = 23,
        Curtain = 25,

		UI = 31,
	}

	public enum UserType : sbyte
	{
		None = -1,	//无效值

		VisitorUser = 50,//游客
	}

	public enum Money_Type : byte
	{
		None,

		MB,
		GoldCoin,
		MBbind,
	}

	public enum Phone_OS : byte
	{
		None,

		Android,
		Ios,
	}

	public enum Platform_Define : byte
    {
        ANDROID,
        IPHONE,
        OTHER,
    }

	/// <summary>
	/// 场景移动方式;
	/// </summary>
	public enum SceneMove
	{
		None = -1,
		WeddingOut,
		Amusement,
		WaitingRoom,
		CeremonyOut,
		BigMama,
		GroupAmuseRoom,//跨线场景;
		CatWalkBackStage,//T台秀开放场景;
	}

	public enum FunctionSwitch : byte
	{
		None = 0,
		LBS = 1,                // LBS
		Transfrom,              // 变身
		BuffBar,                // buff栏
		HideForIos,		        // IOS审核开关
		Recharge,		        // 充值开关
		WeddingHall,	        // 甜蜜殿堂开关
		LuckyDraw,	            // 欢乐谷开关
		MagicLamp,		        // 魔法神灯开关
		Voice,			        // 语音开关
		Share,                  // 每日分享
		SpecialMall,            // 特殊商城
        Homosexual,             // 同性结婚
		Ranking,		        // 排行榜
		RankShowGay,	        // 排行榜是否显示同性数据
		ClothEffect,            //　光效分离
		Medal,                  // 勋章;
		DanceGroupCeremony,     // 舞团入团仪式
        Handbook,               // 图鉴
		Vehicle,				// 座驾
        WifiDownload,       //wifi;
		MagicArray,				// 魔法阵
		DanceGroupBigMama,		// 广场舞大妈
		DanceGroupFairy,		// 舞团秘境
		CatWalk,				// T台秀;
		MagicLampTheme,			// 神灯主题关卡开关
		StarShow,				// 明星秀场
		CatTreasure,			//喵宝藏
		Max,
	}

	public enum ShowNewFunction : byte
	{
		None,

		StarShow,				// 明星秀场
		CatTreasure,			//喵宝藏
	}

	public enum ContinuousBeatBonus : byte
	{
		None,

		Lv1,
		Lv2,
		Lv3,
		Lv4,
		Lv5,

		LvMax
	}

	public enum eConstellation : byte
	{
		Aquarius,	///水瓶座
		Pisces,		///双鱼座
		Aries,		///白羊座
		Taurus,		///金牛座
		Gemini,		///双子座
		Cancer,		///巨蟹座
		Leo,		///狮子座
		virgo,		///处女座
		libra,		///天秤座
		Scorpio,	///天蝎座
		Sagittarius,///射手座
		Capricornus	///摩羯座
	}

    /// <summary>
    /// Camera clipping Planes 's Near value
    /// </summary>
    public class CameraLevel
    {
        Vector2 mLevel = UI;

        public CameraLevel(Vector2 level)
        {
            mLevel = level;
        }

        public Vector2 Level
        {
            get
            {
                return mLevel;
            }
        }

        public static Vector2 Default = new Vector2(0.3f, 200);	/**用于创建默认相机, 用于perspective相机**/
        public static Vector2 UI = new Vector2(-2, 2);				/**UI层[-2, 2],用于orthographic相机**/

        public static Vector2 UIPlayer = new Vector2(1, 200);		/**和UI层相交, 用于perspective相机 for人物模型**/
        public static Vector2 UICross = new Vector2(2, 200);		/**和UI层相交, 用于perspective相机 for宠物或者高于人物的模型**/
        public static Vector2 UITopOne = new Vector2(4, 200);		/**高于UI层, 用于perspective相机**/
    }


	public enum Package_Type : byte
	{
		Invalid,
		New,
		All,
	}

	public enum Version_Type : byte
	{
		Free,
		Charge,
	}

	public enum Version_IOS_Type : byte
	{
		Official,   /**正式版**/
		Escape,     /**越狱版,宏命令: VERSION_IOS_ESCAPE **/
	}


	public enum EClothEffectPower : byte
	{
		None = 0,

		Transform,
		CreateRoom,
		AmusmentRoom,
		UseTitle,
		Horn,
	}

	/// <summary>
	/// 设置模式引导类型
	/// </summary>
	public enum ViewGuideType : byte
	{
		None,
		Mode,		// 模式引导
		Lantern,	// 神灯引导
		StarShow,	// 明星秀场

		Max,
	}

	/// <summary>
	/// 引导信息
	/// </summary>
	public struct ViewGuideInfo
	{
		public ViewGuideType m_GuideType;	// 引导类型
		public object m_Data;				// 引导数据
	}
}
