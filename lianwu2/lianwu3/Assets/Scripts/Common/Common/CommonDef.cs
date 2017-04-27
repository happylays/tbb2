using UnityEngine;

namespace LoveDance.Client.Common
{
    public class CommonDef
    {
        //Photo extension
        public const string PHOTO_EXTENSION = ".lwp";

        public const int SCENE_RANDOM_ID = 0;
        public const int SONG_RANDOM_ID = 0;

        public const int MAX_ROOM_PLAYER = 4;
        public const int MAX_ROOM_AUDIENCE = 4;

        //LiveRoom
        public const string Operator_Name = "GM:";

        //Amusement
        public const int AMUSEMENT_ROOM_MAX_PLAYER = 8;

        //Wedding
        public const int WEDDING_ROOM_MAX_PLAYER = 8;

        //DanceGroup
        public const int DANCEGROUP_MAX_APPLY_COUNT = 5;

        // VIP sprite name 
        public const string VIP_LEVEL_SPRITE_NAME = "vip_{#1}";
        public const string VIP_LEVEL_SPRITE_NAME_DISABLE = "vip_{#1}_1";

        public const int MAX_LOADING_AD = 6;

        public const int PLAYER_MAX_LEVEL = 100;
        public const int PLAYER_MAX_SIGNATURE = 50;
        public const int PLAYER_DEFAUT_YEAR = 1990;
        public const int PLAYER_DEFAUT_MONTH = 1;
        public const int PLAYER_DEFAUT_DAY = 1;

        public const int BAG_MAX_ITEMTYPE = 4;

        public const int UPDATE_INTERVAL_ITEM = 5;
        public const int UPDATE_INTERVAL_GENE = 5;
        public const int UPDATE_INTERVAL_BAG = 30;

        public const int TICKETREFRESHCOUNT = 10;

        public const string SONG_RANDOM_NAME_INDEX = "Room_Random";
        public const string SONG_RANDOM_ICON = "t_song0002";
        public const string SONG_RANDOM_ATLAS = "fristUI_choice_gequ";

        public const int BEAT_SCOPE_PERFECT = 3;
        public const int BEAT_SCOPE_COOL = 7;
        public const int BEAT_SCOPE_GOOD = 11;
        public const int BEAT_SCOPE_TOTAL = 13;
        public const float ROLE_MANSCALE = 1.058F;

        public const int MAIL_MAX_CONTENT = 100;
        public const int MAIL_MAX_COUNT = 50;

        public const string GOLDTICKET_SPRITE_NAME = "jinquan";
        public const string MCOINTICKET_SPRITE_NAME = "Mb";
        public const string MBINDTICKET_SPRITE_NAME = "Mbpink";

        public const uint GOLDTICKET_ID = 65000;

        public const int MAX_STUDIO_PLAYER = 4;

        // sprite in mall 
        public const string MALL_GENDER_MALE = "sc_nan";
        public const string MALL_GENDER_FEMALE = "sc_nv";

        public const string QUEST_DESC_COLOR = "[2600D7]";//Quest description color

        public const string PLATFORM_PPHELP_ICON_SPRITE = "pp_logo";
        public const string PLATFORM_DJOY_ICON_SPRITE = "dangle_logo";
        public const string PLATFORM_MI_ICON_SPRITE = "xiaomi-logo";
        public const string PLATFORM_BAIDU_ICON_SPRITE = "baidu-logo";
        public const string PLATFORM_UC_ICON_SPRITE = "9yologo";
        public const string PLATFORM_91_ICON_SPRITE = "91Logo";
        public const string PLATFORM_ANZHI_ICON_SPRITE = "anzhi";
        public const string PLATFORM_TONGBUTUI_ICON_SPRITE = "tongbutui-logo";
        public const string PLATFORM_VIVO_ICON_SPRITE = "vivologo";
        public const string PLATFORM_KUAIYONG_ICON_SPRITE = "kuaiyong_logo";
        public const string PLATFORM_MUMAYI_ICON_SPRITE = "mumayi";
        public const string PLATFORM_AISI_ICON_SPRITE = "aisi";
        public const string PLATFORM_IIAPPLE_ICON_SPRITE = "iiapple_logo";

        public const float MAX_BLOCK_WIDTH = 2400f;
        public const float MAX_BLOCK_HEIGHT = 1440f;

        public const byte GUIDE_STAGE_SCENE = 15;
        public const string GUIDE_STAGE_ANIMATION = "lw002";
        public const float GUIDE_STAGE_BPM = 80;
        public const ContinuousBeatBonus GUIDE_STAGE_EFFECT_LEV = ContinuousBeatBonus.Lv5;

        public const int DANCEGROUP_MAX_INTRO_LEN = 100;
        public const int DANCEGROUP_MAX_BRIEF_LEN = 100;

        public const int DANCEGROUP_MAX_REQUEST_COUNT = 50;

        public const int DANCE_GROUP_DISMISS_RETENTION_TIME = 604800;
        public const int DANCE_GROUP_DISMISS_CD_TIME = 86400;
        public const int DANCE_GROUP_APPLY_EXPIRE_TIME = 259200;

        public const int DANCEGROUP_LOGO_COUNT = 5;
        public const int DANCEGROUP_POS_COUNT = 5;

        public const uint DANCEGROUP_RES_A = 31012;
        public const uint DANCEGROUP_RES_B = 31013;
        public const uint DANCEGROUP_RES_C = 31014;
        public const uint DANCEGROUP_CHANGE_NAME = 31023;
        public const uint DANCEGROUP_MEDAL_ID = 31545;

        public const uint LANTERNSHOP_MEDAL_ID = 31946;//神灯奖章id

        public const int COUPLE_LOVE_DECLARATION_MAX = 50;
        public const uint COUPLE_APPLY_ITEM_ID = 31017;
        public const uint COUPLE_DIVORCE_ITEM_ID = 31019;
        public const uint COUPLE_CEREMONY_ITEM_ID = 31018;

        public const uint FESTIVAL_WISHCARD_ID = 31024;
        public const uint FESTIVAL_BLESSCARD_ID = 31025;

        public const uint SIGNIN_RESIGNCARD_ID = 31004;

        //Change Name
        public const uint PERSON_CHANGE_NAME = 31003;

        // VIP Card 
        public const uint VIP_CARD_WEEK = 31005;
        public const uint VIP_CARD_MONTH = 31006;
        public const uint VIP_CARD_QUARTER = 31007;
        public const uint VIP_CARD_YEAR = 31008;

        //Horn Item
        public const uint HORN_SMALL = 32001;
        public const uint HORN_BIG = 32002;

        //Studio Card
        public const uint STUDIO_LW_FILM = 31020;
        public const uint STUDIO_ACTION_CARD = 31021;

        // 勋章
        public const uint MEDAL_COIN_ID = 31295;	// 勋章币

        public const ushort CHATPAPAW_GENE_ID = 7006;    //Chat papaw gene id
        //ucvip 
        public const string UCVIP_LEVEL_SPRITE_NAME = "uc_VIP ";
        //LiveRoom
        public static Vector3 RoleHidePos = new Vector3(1000, 1000, 1000);

        public const float VisitorTipsTime = 1200f;

        // UI_Room
        public static string SPRITE_FLAG_BLUE = "fj_zudui_blue";
        public static string SPRITE_FLAG_RED = "fj_zudui_red";
        public static string SPRITE_FLAG_FREE = "fj_zudui_free";
        public static string SPRITE_FLAG_DISABLED = "fj_zudui_hui";

        // pr_gongyong SpriteName
        public const string SRPITE_BINDMCOIN = "shangcheng_M_bd";
        public const string SRPITE_MCOIN = "shangcheng_M";
        public const string SRPITE_MONEY = "shangcheng_quan";

        //IOS Offical Used Only
        public static string s_DownloadChargeURL = "https://itunes.apple.com/cn/app/lian-wuol-xie-hou-ai-zun-xiang/id598866940?l=zh&ls-1&mt=8";
        public static string s_DownloadFreeURL = "https://itunes.apple.com/cn/app/lian-wuol-mian-fei-ban/id820140906?ls=1&mt=8";

        public const int TransformGeneID = 1501;//变身基因ID
        public const int ExperienceGeneID = 1001;//提升经验基因ID
        public const string ACTIVENESS_REWARD_SPRITE_NAME_PRE = "hy_gift";
        public const string ACTIVENESS_WEEKREWARD_SPRITE_NAME_PRE = "hy_weekgift";
        public const string ACTIVENESS_REWARD_SPRITE_NAME_FOL = "_0";

        //静态数据
        public const string SD_ITEM_INFO = "iteminfo.bytes";
        public const string SD_MUSIC_INFO = "musicinfo.bytes";
        public const string SD_EFFECT_INFO = "EffectInfo.bytes";
        public const string SD_LEVEXP_INFO = "LevExp.bytes";
        public const string SD_CHAT = "chat.bytes";
        public const string SD_CHATDENY = "chatdeny.txt";
        public const string SD_LOADINGTIP = "loadingtip.txt";
        public const string SD_SYSTEMTIP = "systemtip.txt";
        public const string SD_VERSIONTIP = "versiontip.txt";
        public const string SD_SHARETIP = "share.txt";
        public const string SD_COUPLEINTIMACY_INFO = "CoupleIntumacy.bytes";
        public const string SD_WEDDINGROOM_INFO = "WeddingInfo.bytes";
        public const string SD_STUDIO_INFO = "PhotoStudioInfo.bytes";
        public const string SD_AMUSEMENTINFO = "Amusement.bytes";
        public const string SD_LANTERN_INFO = "MagicLamp.bytes";
        public const string SD_TRANSANI = "TransformAni.bytes";
        public const string SD_ROLENAME = "RandomRoleName.bytes";
        public static string s_SDHandbookInfo = "Handbook.bytes";
        public static string s_SDVehicleInfo = "Vehicle.bytes";
        public static string s_SDDanceGroupInfo = "BigMamaClient.bytes";
        public static string SD_TRANSFROM = "Transform.bytes";
        public static string SD_CLOTHEFFECTTRANSFORM = "ClothEffectTransform.bytes";
        public static string SD_STARSHOW = "StarShowClient.bytes";
        public static string SD_POSE = "SinglePose.bytes";
        public static string s_CATWALKOSU = "catwalkosu.osu";
        public static string s_CATWALKRULE = "catwalkruleword.txt";
        public static string s_STARSHOWRULE = "starshowruleword.txt";
        public static string s_USERAGREEMENT = "UserArgeement.txt";
        public const string SD_ANDROIDASSET = "androidasset.bytes";
        public const string SD_IGNORENEWGUIDE = "noguide.txt";
        public const string SD_TITLE_INFO = "Title.bytes";
        public const string SD_SHININGJEWELRY = "ShiningJewelry.bytes";//闪亮饰品动作表
        public const string s_RECHARGEREBATE = "rechargeRebate.txt";
        public const string SD_STARSHOWFAN = "StarShowClientFan.bytes";

        public static string SD_ASSETBUNDLE_CONFIGS_VERSION = "ConfigVer.json";//服务器版本文件;
        public static string[] SD_ASSETBUNDLE_CONFIGS = {
                                                           "ResVer_Animations.json",
                                                           "ResVer_Effect.json",
                                                           "ResVer_Materials.json",
                                                           "ResVer_Music.json",
                                                           "ResVer_Other.json",
                                                           "ResVer_Scenes.json",
                                                           "ResVer_ShaderExtend.json",
                                                           "ResVer_StaticData.json",
                                                           "ResVer_UI.json",
                                                           "ResVer_Icon.json",
														   "ResVer_Prefab.json"
                                                       };

        //垂直同步
        public static int GAMESETTING_IDLE_VSYNC = 2;
        public static int GAMESETTING_PLAY_VSYNC = 0;

        //资源下载限制;
        public static int DOWNLOAD_MAX_LINK = 3;//最大链接;
        public static int DOWNLOAD_MAX_SIZE = 10 * 1024 * 1024;//最大size;

        public const ushort GuideRoleLevel = 14;

        //UnlockFunction
        public static string UNLOCK_FUNCTION_RES = "pr_unlockfunction";	// 功能解锁Prefab文件名

        // ToastTips
        public static string TOAST_TIPS_PREFAB_RES = "pr_toasttips";	// 渐隐提示框Prefab文件名

        // Freind
        public const string FRIEND_SIMPLE_ITEM_ONLINE = "chat_hy_button";						// 好友在线贴图
        public const string FRIEND_SIMPLE_ITEM_OFFLINE = "chat_hy_button_a";					// 好友离线贴图
        public static Color FRIEND_SIMPLE_ITEM_SELECT = new Color(49f / 255f, 100 / 255f, 1f);	// 好友被选中的颜色
        public const string FRIEND_ITEM_ONLINE = "wt_xx_buuton";								// 好友在线图标
        public const string FRIEND_ITEM_OFFLINE = "wt_xx_buuton_a";								// 好友离线图标
        public const string FRIEND_ITEM_SAMELINE = "wt_xx_buuton_tx";							// 好友同线图标

        //LevelUp
        public static string LEVELUP_PREFAB_RES = "pr_levelup";	        // 升级奖励

        //BubbleTips
        public static string BUBBLETIPS_PREFAB_RES = "pr_bubbletips";	// 冒泡

        public static string GUIDEARROW_PREFAB_RES = "pr_guidearrow";	// 新手引导

        public static string LOGINUI_PREFAB_RES = "pr_loginui";	            // 登录

        public static string FUNCTIONGUIDE_PREFAB_RES = "pr_functionguide";	// 功能引导

        public static string LOADINGSTAGE_PREFAB_RES = "pr_stageloading";//跳舞场景loading

        public static string ROOMSCENENAME = "room_modify_test";
        public static string FAIRY_ROOM_SCENE = "room_modify_dt";

        public static string FAIRY_SCENE_NAME = "stage_mijing";
        public static string FAIRYSCORE_PREFAB_RES = "pr_fairy_score";		// 舞团秘境游戏中分数Prefab

        public static string CATWALK_MAIN_SCENENAME = "t_tai_xiu";
        public static string CATWALK_PK_SCENENAME = "t_tai_xiu_pk";
        public static string CATWALK_PHOTO_SCENENAME = "t_tai_xiu_photo";

        // Res extension
        public static string RES_CLOTH_EXTENSION = ".clh";	// 服饰扩展名

        // Chat
        public const float EMOTICON_WIDTH = 24;					// 聊天表情每页个数
        public const int EMOTICON_COUNT_PER_PAGE = 16;			// 聊天表情每页个数
        public const string CHAT_PHOTO_SYSTEM = "tx_xtong";		// 聊天默认头像 系统频道
        public const string CHAT_PHOTO_DANCEGROUP = "tx_wtuan";	// 聊天默认头像 舞团频道
        public const string CHAT_SKIN_ADDITION = "_a";			// 聊天皮肤额外文字

        // 旋转常量
        public static Quaternion QUATERNION_IDENTITY = Quaternion.identity;				// 无旋转
        public static Quaternion QUATERNION_REVERSE_Y = Quaternion.Euler(0, 180, 0);	// Y轴翻转180

        public const string STRING_REDUCE = "...";				// 省略号

        public const byte LANTERN_STAGE_PER_ZONE = 18;	// 每章关卡数

        // 明星秀场
        public const string STARSHOW_RANK_PRENAME = "mxxc_";	// 明星秀场排行名次图片 名称前缀

    }
}
