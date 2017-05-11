
//UI相关
namespace LoveDance.Client.Common
{
	public enum UIFlag
	{
		none = 0x0000,								///reserved location -- [0,20000), depth -- [70,100)
		///stage location -- 0, depth -- -1
		///player location -- 5000, depth -- 70
		///extra ui location -- 10000, depth -- tipsbox(80), loading(85), msgwait(90), switching(95)

		ui_ownlogin = 0x0010,						///location -- x: 20000 y: 0 - 2000,		depth -- 50,51
		ui_thirdlogin,								///location -- x: 20000 y: 4000,			depth -- 94
		ui_createrole,								///location -- x: 20000 y: 6000,			depth -- 10

		ui_world = 0x0020,							///location -- x: 10000 y: 0 - 6000,		depth -- 10,12,14,15,16
		ui_room = 0x0040,							///location -- x: 10000 y: 26000 - 34000,	depth -- 10,12,14,15,16,18
		ui_musicchoose = 0x0050,					///location -- x: 20000 y: 9000,			depth -- 20
		ui_infocard = 0x0060,						///location -- x: 20000 y: 12000 - 16000,	depth -- 28,29
		ui_signin = 0x0090,							///location -- x: 20000 y: 18000,			depth -- 20

		ui_lobby = 0x0030,							///location -- x: 10000 y: 8000 - 24000,	depth -- 10 - 15

		ui_chat = 0x0070,							///location -- x: 20000 y: 20000 - 24000,	depth -- 25,26,27,28
		ui_friend = 0x0080,							///location -- x: 20000 y: 26000 - 28000,	depth -- 20,22
		ui_friend_apply = 0x00A0,					///location -- x: 20000 y: 30000,			depth -- 30
		ui_friend_accept = 0x00B0,					///location -- x: 20000 y: 32000,			depth -- 27

		ui_itembag = 0x0110,						///location -- x: 20000 y: 35000 - 41000,	depth -- 22,23,24,25

		ui_mall = 0x0120,							///location -- x: 20000 y: 41000 - 48000,	depth -- 20,21,22,23,24

		ui_mallcart = 0x0121,						///location	-- x: 20000 y: -2000 - -7000,	depth -- 32

		ui_activity = 0x0130,						///location -- x: 20000 y: -9000 - -11000,	depth -- 20,21

		ui_desire = 0x0140,							///location -- x: 20000 y: -13000,			depth -- 30

		ui_mail = 0x0150,							///location -- x: 20000 y: -15000,			depth -- 22

		ui_mailwriter = 0x0151,						///location -- x: 20000 y: -17000,			depth -- 24

		ui_askforby = 0x0160,						///location -- x: 20000 y: -19000,			depth -- 27

		ui_quest = 0x0180,							///location -- x: 20000 y: -21000,			depth -- 20

		ui_invitee = 0x0170,						///location -- x: 20000 y: -23000,			depth -- 27.5

		ui_mywish = 0x0190,							///location -- x: 20000 y: -26000,			depth -- 20

		ui_switchline = 0x01A0,						///location -- x: 20000 y: -29000,			depth -- 20

		ui_score = 0x01B0,							///location -- x: 20000 y: -32000,			depth -- 10

		ui_taigu = 0x01C0,
		ui_supertaigu = 0x01C1,
		ui_tranditional = 0x01D0,
		ui_supertranditional = 0x01D1,
		ui_osu = 0x01E0,
		ui_superosu = 0x01E1,
		ui_audition = 0x01F0,
		ui_superaudition = 0x01F1,
		//ui_rainbow = 0x01F1,

		ui_reward = 0x0200,							///location -- x: 20000 y: -34000,			depth -- 48,49
		ui_rewardlevup = 0x0201,					///location -- x: 20000 y: -36000,			depth -- 46
		ui_rewardwithvip = 0x0202,					///location -- x: 20000 y: -50000,			depth -- 50
		ui_rewardequalize = 0x0203,					///location -- x: 20000 y: -60000,			depth -- 47
		ui_firstrecharge = 0x0210,					///location -- x: 20000 y: -38000,			depth -- 23,24
		ui_recharge = 0x0211,						///location -- x: 20000 y: -56000,			depth -- 22
		ui_rechargecumulation = 0X0212,				///location -- x: 20000 y: -52000,			depth -- 29
		ui_rechargerebate = 0x0213,					///location -- x: 20000 y: -57000,			depth -- 22

		ui_msgbox = 0x220,							///location -- x: 20000 y: -40000,			depth -- 19

		//ui_rhythm = 0x0230,

		ui_guide = 0x0240,

		ui_communitysharing = 0x0250,				///location -- x: 20000 y: -42000,			depth --32

		ui_mydancegroup = 0x0260,					///location -- x: 23000 y: 0 - 16000,		depth -- 20,21,22,23,24
		ui_dancegroupinfo = 0x0270,					///location -- x: 20000 y: -44000,			depth -- 31
		ui_dancegrouphall = 0x0280,					///location -- x: 20000 y: -46000 - -48000, depth -- 10,14

		ui_couple_apply = 0x0290,					///location -- x: 23000 y: 18000,			depth -- 27
		ui_couple_accept = 0x02A0,					///location -- x: 23000 y: 20000 - 24000,	depth -- 27
		ui_couple_separate = 0x02C0,				///location -- x: 23000 y: 32000 - 36000,	depth -- 31

		ui_weddinghall = 0x02E0,					///location -- x: 23000 y: 40000 - 46000,	depth -- 11,12,13,14
		///location -- x: 26000 y: 40000 - 42000,   depth -- 10,15
		ui_weddingblesscard = 0x02E1,				///location -- x: 26000 y: 48000,   depth --- 28,30
		ui_photowall = 0x02E5,							///location -- x: 29000 y: 51000,	--x:31000,51000   depth --- 32,31
		ui_photowall_upload = 0x02E3,			///location -- x: 32000 y: 54000,	   depth --- 33


		ui_weddingroom = 0x0300,					///location -- x: 10000 y: 36000 - 48000,	depth -- 10,12,13,14,15,16,17
		ui_weddingstart = 0x0310,					///location -- x: 23000 y: 48000,			depth -- 10

		ui_roomkick = 0x02F0,						///location -- x: 23000 y: -2000,			depth -- 27

		ui_vip = 0x0320,							///location -- x: 23000 y: -4000 - -6000,	depth -- 47 

		ui_monthcard = 0x0321,						///location -- x: 23000 y: -6000 -          depth -- 30,29
		ui_activeness = 0x0322,						///location -- x: 23000 y: -6000             depth -- 27               

		ui_welfare = 0x0325,						///location -- x: 26000  y:28000 -          depth -- 19
		ui_invitecode = 0x0326,						///location -- x: 23000 y: -46000 -         depth --  25                     

		ui_allgamesetting = 0x0327,					///location -- x: 23000 y: -0 -             depth -- 20


		ui_photoupload = 0x0330,					///location -- x: 23000 y: -8000,			depth --22
		ui_photoscan = 0x0340,						///location -- x: 23000 y: -10000,			depth --30

		ui_festival = 0x0350,						///location -- x: 23000 y: -12000 - -18000,	depth -- 20,21,22,23

		ui_activitycenter = 0x0360,					///location -- x: 23000 y: -20000,			depth -- 22

		ui_luckdraw = 0x0370,						///location -- x: 23000 y: -22000 - -24000,	depth -- 20,21

		// 		ui_studio_create = 0x0380,					///location -- x: 23000 y: -26000,			depth -- 10
		// 		ui_studio_interior = 0x0381,				///location -- x: 10000 y: 48000 - 52000,	depth -- 10,11,16
		// 		ui_studio_invitee = 0x0382,					///location -- x: 23000 y: -28000,			depth -- 27.5 

		ui_puzzle = 0x0390,							///location -- x: 23000 y: -30000 - -32000,	depth -- 20,21

		ui_amusementroom = 0x03A0,					///location -- x: 10000 y: -2000 - -12000,	depth -- 10,12,13,14,15,16
		ui_amusementhall = 0x03B0,					///location -- x: 23000 y: -34000 - -40000,	depth -- 10,12,13,14
		ui_amusementinvite = 0x03C0,				///location -- x: 23000 y: -42000,	 		depth -- 27.5
		ui_amusementinvitedevice = 0x03D0,			///location -- x: 23000 y: -44000,			depth -- 27.5

		ui_recruit = 0x03E0,						///location -- x: 23000 y: -46000,			depth -- -4,25,26,27
		ui_securityquiz = 0x03E2,					///location -- x: 23000 y: -74000,
		ui_heartbeat = 0x03F0,
		ui_heartbeatmakefriend = 0x03F1,			///location -- x: 26000 y: 0				depth -- 27

		ui_fresherreward = 0x0400,					///location -- x: 23000 y: -50000,          depth -- 22

		ui_activityxuliang = 0x0410,				///location -- x: 23000 y: -48000,          depth -- 27 ,30

		ui_ucvipreward = 0x0460,     				///location -- x:24000 y;  -1000  .    depth --- 29
		ui_fresherquest = 0x0470,					///location -- x: 24000 y: -3000,   depth --- 22

		ui_encounterreward = 0x0504,				///location -- x: 24000 y: -17000,   depth --- 30
		ui_encounterstart = 0x0505,					///location -- x: 24000 y: -21000, depth  --- 10
		ui_noviceteaching = 0x0506,

		ui_lanternworld = 0x0510,					///location -- x: 10000 y: 36000 - 40000, depth --- 10,12,14,15
		ui_lanterncrystaltrade = 0x0511,			///location -- x: 26000 y: 0, depth --- 22
		ui_lanternzone = 0x0512,					///location -- x: 26000 y: 2000 - 6000, depth --- 10,11,12,15
		ui_lanternhelpprop = 0x0513,				///location -- x: 26000 y: 8000 - 12000, depth --- 15,16,17
		ui_lanternstagescore = 0x0514,				///location -- x: 26000 y: 14000, depth --- 10
		ui_lanternstory = 0x0516,					///location -- x: 26000 y: 18000 - 20000, depth --- 19,20
		ui_lanterninvite = 0x0517,					///location -- x: 26000 y: 22000, depth --- 27.5
		ui_lantern_multi_choose = 0x0518,			///location -- x: 26000 y: 46000, depth --- 20
		ui_lantern_stage_info = 0x0519,				///location -- x: 26000 y: 48000, depth --- 21
		ui_lanternshop = 0x051A,					///location -- x: 26000 y: 52000, depth --- 27

		ui_playerbuff = 0x0600,						///location -- x: 24000 y: -23000, depth  --- 20
		ui_guiderhythm = 0x0610,					///location -- x: 24000 y: -25000, depth  --- 10
		ui_guideaudition = 0x0611,					///location -- x: 24000 y: -25000, depth  --- 10
		ui_guiderainbow = 0x0612,					///location -- x: 24000 y: -25000, depth  --- 10
		ui_guidesuperosu = 0x0613,					///location -- x: 24000 y: -25000, depth  --- 10
		ui_guidecatwalk = 0x0614,					///	T台秀模式的新手引导	location -- x: 24000 y: -25000, depth  --- 10

		ui_changeconstellation = 0x0710,			///location -- x:26000 y:24000,depth --- 20
		ui_sendredenvelope = 0x02E2,				///location -- x: 26000 y: -42000,   depth -- 20

		ui_specialmall = 0x0720,					///location -- x:26000 y:26000,depth --- 10

		ui_ranking = 0x0730,						///location -- x:26000 y:42000-44000, depth --- 10,11

		ui_guideosu = 0x0740,						///location -- x: 24000 y: -25000, depth --- 10

		ui_friendrecommend = 0x0741,				///location -- x: 26000 y: 50000, depth --- 20

		ui_guidedialog = 0x1000,					//location -- x: 5000 y: 12000, depth --- 41

		ui_dancegroupceremonyroom = 0x0920,			///舞团入团仪式房间 location -- x: 28000 y: 30000 depth --- 10
		ui_dancegroupceremony = 0x0930,				///舞团入团仪式 location --- x: 28000 y: 32000 depth --- 10
		ui_dancegroupceremonyrecord = 0X0940,		///舞团入团仪式日志 location -- x: 28000 y: 34000 depth --- 20

		ui_clotheffect = 0x1010,					//location -- x: 30000 y: 0- 10000, depth --- 22
		ui_clotheffectsuc = 0x1011,					//location -- x: 30000 y: 0- 14000, depth --- 23

		ui_medal = 0x1100,
		ui_handbook = 0x1200,                       //location -- x: 34000 y: 0- 14000, depth --- 24
		ui_redenvelope = 0x1300,

		ui_vehicle = 0x1400,						//座驾选择界面	location -- x: 28000 y : 40000-42000 depth --- 20,21
		ui_vehicleinvite = 0x1410,					//座驾邀请界面	location -- x: 28000 y : 44000 depth --- 20
		ui_vehiclereceive = 0x1420,					//座驾被邀请界面	location -- x: 28000 y : 46000 depth --- 27.5

		ui_magicarray = 0x1430,						//魔法阵界面		location -- x: 28000 y : 48000 depth --- 20
		ui_cattreasure = 0x1463,					//喵的宝藏			location -- x: 28000 y: 50000  depth --- 70

		ui_dancegroupbigmama = 0x1440,				//广场舞大妈房间界面	location -- x: 28000 y: -2000 - -10000 depth --- 10,14,15,16,17
		ui_dancegroupshop = 0x1450,					//舞团商店界面	

		ui_exchangeseal = 0x1460,					//兑换光效石界面 location -- x: 20000 y: -34000  depth --- 48

		ui_catwalkpk = 0x1500,						//T台秀PK场景UI;			location -- x: 30000 y: 10000  depth --- 10
		ui_catwalkmain = 0x1510,					//T台秀场景主UI;			location -- x: 30000 y: 8000  depth --- 9
		ui_catwalkswitching = 0x1550,				//T台秀功能用的切场景Loading, 大Loading;	location -- x: 30000 y: 0  depth --- 98,99
		ui_catwalkamuserank = 0x1560,				//T台秀舞团排名				location -- x: 30000 y: 12000  depth --- 11
		ui_catwalktempomode = 0x1570,				//T台秀节奏模式UI			location -- x: 30000 y: 14000  depth --- 12
		ui_catwalkphoto = 0x1580,				//T台秀节奏模式UI			location -- x: 30000 y: 16000  depth --- 10

		ui_vistiortips = 0x1470,					//游客提示界面   location -- x: 20000 y: -36000  depth --- 70

		ui_dancegroup_fairy = 0x1480,				// 舞团秘境
		ui_dancegroup_fairy_score = 0x1481,			// 舞团秘境结算

		ui_title = 0x1461,                         // 称号列表界面  20000 y: 35000 - 43000,	depth -- 22
		ui_themeroomswitch = 0x1462,               // 主题房间切换  23000 y: -72000,	depth -- 30

		ui_itembagpick = 0x1490,					// 背包筛选
		ui_starshow = 0x1491,						// 变身列表界面
		ui_starshow_mall = 0x1492,					// 变身列表界面
	}

	public enum UIType
	{
		Normal,
		Menu,
	}

	public class BackToWorldData
	{
		public string m_strTips = "";
		public uint m_nRoleID = 0;
		public string m_strRoleName = "";
		public bool m_bIsVIP = false;
		public ushort m_nVIPLevel = 0;
		public ISecondFlagToWorld m_SecondFlagToWorld = null;
	}

	// BackToWorld Second UI
	public interface ISecondFlagToWorld
	{
		UIFlag NextUIFlag { get; }
		object ExData { get; }
	}

	public class GuideSwitchInfo : ISecondFlagToWorld
	{
		public UIFlag NextUIFlag { get; private set; }
		public object ExData { get; private set; }

		public GuideSwitchInfo(UIFlag flag, object exData)
		{
			NextUIFlag = flag;
			ExData = exData;
		}
	}

	public class ChallengeSwitchInfo : ISecondFlagToWorld
	{
		public UIFlag NextUIFlag { get; private set; }
		public object ExData { get; private set; }

		public ChallengeSwitchInfo(UIFlag flag, object exData)
		{
			NextUIFlag = flag;
			ExData = exData;
		}
	}

	public class BigMamaSwitchInfo : ISecondFlagToWorld
	{
		public UIFlag NextUIFlag { get; private set; }
		public object ExData { get; private set; }

		public BigMamaSwitchInfo(UIFlag flag, object exData)
		{
			NextUIFlag = flag;
			ExData = exData;
		}
	}
}