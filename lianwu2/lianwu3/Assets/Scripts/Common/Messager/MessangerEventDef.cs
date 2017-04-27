
namespace LoveDance.Client.Common.Messengers
{
	/// <summary>
	/// This class is used to define all eventId.
	/// Event string must be different.
	/// </summary>
	public class MessangerEventDef
	{
		public const string Demo_EventType = "Demo";

		public const string Quest_ChangeCloth = "ChangeCloth";
		public const string Quest_ChangeLocaltion = "ChangeLocaltion";
		public const string Quest_QuestFunction = "QuestFunction";
		public const string Quest_FriendCount = "QuestFriend";
		public const string Quest_AddDanceGroup = "QuestAddDanceGroup";
		public const string Quest_UpdateQuestTips = "QuestUpdateTips";
		public const string Quest_CoupleSceneEffect = "QuestCoupleSceneEffect";

		//Friend search
		public const string Show_FriendSearchUIEvent = "Show_FriendSearchUI";

		public const string Show_LoginBtn = "Game_LoadResEnd";
		public const string Third_LoginSuc = "Third_LoginSuc";
		public const string Third_LoginGame = "Third_LoginGame";
		public const string Show_ThirdLogin = "Show_ThirdLogin";

		//Cloth Group
		public const string ClothGroup_SelectGroupEvent = "ClothGroup_SelectGroup";
		public const string ClothGroup_ChangeGroupNameEvent = "ClothGroup_ChangeGroupName";

		//开放场景
		public const string ShowAllAmusementUIEvent = "ShowAllUI";
		public const string AMUSEMENT_CHANGE_TO_DEVICE_UI = "AMUSEMENT_CHANGE_TO_DEVICE_UI";
		public const string AMUSEMENT_CHANGE_TO_ROOM_UI = "AMUSEMENT_CHANGE_TO_ROOM_UI";
		public const string AMUSEMENT_CHANGE_DEVICE_INVITE_BUTTON = "AMUSEMENT_CHANGE_DEVICE_INVITE_BUTTON";
		public const string AMUSEMENT_REQUEST_STOP_POST = "AMUSEMENT_REQUEST_STOP_POST";
		public const string AMUSEMENT_REQUEST_STOP_MOVE = "AMUSEMENT_REQUEST_STOP_MOVE";						// 停止开放型场景移动
		public const string AMUSEMENT_CHANGE_ON_DEVICE_UI = "AMUSEMENT_CHANGE_ON_DEVICE_UI";
		public const string AMUSEMENT_SHOW_PHOTO_MODE = "AMUSEMENT_SHOW_PHOTO_MODE";							// 显示或退出开放型场景照相机模式

		//NewGuide
		public const string NewGuide_ShowGuideArrow = "NewGuide_ShowGuideArrow";
		public const string NewGuide_ShowGuideArrow_LobbyCreate = "NewGuide_ShowGuideArrow_LobbyCreate";
		public const string NewGuide_ShowGuideArrow_BagUseItem = "NewGuide_ShowGuideArrow_BagUseItem";
		public const string NewGuide_ShowGuideArrow_BagTryOnCloth = "NewGuide_ShowGuideArrow_BagTryOnCloth";
		public const string NewGuide_ShowGuideArrow_MusicChoose = "NewGuide_ShowGuideArrow_MusicChoose";
		public const string NewGuide_ShowGuideArrow_ChatUseHorn = "NewGuide_ShowGuideArrow_ChatUseHorn";
		public const string NewGuide_ShowGuideArrow_SwitchWorld = "NewGuide_ShowGuideArrow_SwitchWorld";
		public const string NewGuide_ShowGuideArrow_MallClose = "NewGuide_ShowGuideArrow_MallClose";
		public const string NewGuide_ShowGuideArrow_SignedClose = "NewGuide_ShowGuideArrow_SignedClose";
		public const string NewGuide_ShowGuideArrow_OnCloseQuest = "NewGuide_ShowGuideArrow_OnCloseQuest";
		public const string NewGuide_ShowGuideArrow_InfoCardClose = "NewGuide_ShowGuideArrow_InfoCardClose";
		public const string NewGuide_ShowGuideArrow_BackToInfoCard = "NewGuide_ShowGuideArrow_BackToInfoCard";
		public const string NewGuide_ShowGuideArrow_OnCloseInfoCard = "NewGuide_ShowGuideArrow_OnCloseInfoCard";
		public const string NewGuide_ShowGuideArrow_OnModeChange = "NewGuide_ShowGuideArrow_OnModeChange";
		public const string NewGuide_ShowGuideDialog = "NewGuide_ShowGuideDialog";
        public const string NewGuide_FresherRewardListInit = "NewGuide_FresherRewardListInit";
        public const string NewGuide_FresherRewardClose = "NewGuide_FresherRewardClose";
		public const string NewGuide_OpenSortCutArea = "NewGuide_OpenSortCutArea";//打开世界地图左下区域
		public const string NewGuide_OpenMbMagic = "NewGuide_OpenMbMagic";
		public const string NewGuide_CloseMagicArray = "NewGuide_CloseMagicArray";
		public const string NewGuide_MbMagicOnceCall = "NewGuide_MbMagicOnceCall";
		public const string NewGudie_OnCloseStarShow = "NewGudie_OnCloseStarShow";
		public const string NewGudie_OnCloseCatTreasure = "NewGudie_OnCloseCatTreasure";
		public const string NewGuide_CompleteGudieDanceGroupHall = "NewGuide_CompleteGudieDanceGroupHall";

		//share
		public const string ShareSuc = "ShareSuc";

		//GameSetting Feedback
		public const string Feedback = "Feedback";

		public const string UpdateNewMailCount = "UpdateNewMailCount";
		public const string UpdateNewQuestCount = "UpdateNewQuestCount";

		//排行榜页面，打开和关闭个人信息页面的事件
		public const string ShowInfoCardInRanking = "ShowInfoCardInRanking";

		// 活跃度页面，打开和关闭奖励领取页面的事件
		public const string ShowRewardViewInActiveness = "ShowRewardViewInRanking";
		public const string HAS_OPEN_ACTIVENESS = "HAS_OPEN_ACTIVENESS";	// 打开过活跃度界面

		//加载模式图集
		public const string UIMusicChooseLoadModeAtlas = "UI_MusicChoose_LoadModeAtlas";

		public const string UIAllGameSettingUnloadAtlas = "UI_AllGameSetting_UnloadAtlas";

		//个人消息盒子 计数变更;
		public const string MsgBoxNormalCountChange = "MsgBoxNormalCountChange";
		public const string MsgBox_NoticeMsgBox = "MsgBox_NoticeMsgBox";

		//聊天
		public const string Chat_AutoSpeech_Accumlate = "Chat_AutoSpeech_Accumlate";
		public const string Chat_Refresh = "Chat_Refresh";

		//舞团入团仪式
		public const string Ceremony_ShowPraisePlayer = "Ceremony_ShowPraisePlayer";
		public const string Ceremony_HidePraisePlayer = "Ceremony_HidePraisePlayer";
		public const string Ceremony_ShowPraiseButton = "Ceremony_ShowPraiseButton";

		public const string Item_BugReport = "Item_BugReport";
		//TipBox
		public const string TIPBOX_SHOW_OK_PARAM2 = "TIPBOX_SHOW_OK_PARAM2";
		public const string TIPBOX_SHOW_OK_PARAM2_2 = "TIPBOX_SHOW_OK_PARAM2_2";
		public const string TIPBOX_SHOW_OK_PARAM3 = "TIPBOX_SHOW_OK_PARAM3";
		public const string TIPBOX_SHOW_OK_PARAM4 = "TIPBOX_SHOW_OK_PARAM4";
		public const string TIPBOX_SHOW_YRSNO_PARAM3 = "TIPBOX_SHOW_YRSNO_PARAM3";
		public const string TIPBOX_SHOW_YRSNO_PARAM4 = "TIPBOX_SHOW_YRSNO_PARAM4";
		public const string TIPBOX_SHOW_YRSNO_PARAM5 = "TIPBOX_SHOW_YRSNO_PARAM5";
		public const string TIPBOX_SHOW_YRSNO_PARAM6 = "TIPBOX_SHOW_YRSNO_PARAM6";
		public const string TIPBOX_HIDE = "TIPBOX_Hide";

		//UnlockFounction
		public const string UNLOCK_FUNCTION_SHOW = "UNLOCK_FUNCTION_SHOW";//通知显示功能解锁提示
		public const string UNLOCK_WORLDREFRESH = "UNLOCK_WORLDREFRESH";//解锁时世界地图图标移动

		// ToastTips
		public const string TOASTTIPS_SHOW = "TOASTTIPS_SHOW";				// 通知界面显示渐隐提示框

		//LuckDraw
		public const string LUCKDRAW_REFRESH_TIP = "LUCKDRAW_REFRESH_TIP";	//提示是否需要刷新
		
		//特殊商店
		public const string AUCTION_BID_NOTICE = "AUCTION_BID_NOTICE";

        //区域切换响应;
		public const string WORLD_ZONE_CHANGE_BEFORE_ANIMATION = "WORLD_ZONE_CHANGE_BEFORE_ANIMATION";	// 切换世界地图结束动画前刷新界面
        
		public const string GYRO_STATE_CHANGE = "GYRO_STATE_CHANGE";

		//截图
		public const string PHOTOCAPTURE_SHOW_EFFECT = "PHOTOCAPTURE_SHOW_EFFECT";	//截图特效
		//刷新玩家头像
		public const string PHOTOIPLOAD_SHOW_PHOTO = "PHOTOIPLOAD_SHOW_PHOTO";
		
		// VIP
		public const string FIRST_OPEN_VIP = "FIRST_OPEN_VIP";	// 首次打开VIP界面刷新世界地图VIP抖动

        //梦工坊
        public const string Close_ClothEffectSuc = "Close_ClothEffectSuc"; // 关闭魔法梦工坊成功界面界面

        public const string OnNewRedEnvelopeNotice = "OnNewRedEnvelopeNotice"; // 新红包通知
		
		//座驾
		public const string SYNC_VEHICLE_PASSENGER_ANIMATION = "SYNC_VEHICLE_PASSENGER_ANIMATION";	//同步座驾上所有乘客动画
		public const string VEHICLE_CHANGE_UI = "VEHICLE_CHANGE_UI";	//改变UI

        // LevelUp
        public const string LEVELUP_SHOW = "LEVELUP_SHOW";

        // bubble
        public const string BUBBLETIPS_SHOW = "BUBBLETIPS_SHOW";
        public const string BUBBLETIPS_HIDE = "BUBBLETIPS_HIDE";

        // guidearrow
        public const string GUIDEARROW_SHOW = "GUIDEARROW_SHOW";
        public const string GUIDEARROW_HIDE = "GUIDEARROW_HIDE";

        public const string StartLoadStage = "StartLoadStage";

        //Medal
        public const string Medal_UpdateMedalTips = "Medal_UpdateMedalTips";

        //功能引导
        public const string FUNCTIONGUIDE_SHOW = "FUNCTIONGUIDE_SHOW";
        public const string FUNCTIONGUIDE_HIDE = "FUNCTIONGUIDE_HIDE";

        public const string ItemBagShowGuide = "ItemBagShowGuide";

		// LoadingControl
		public const string SHOW_LOADING = "SHOW_LOADING";
		public const string HIDE_VIOLENTLY = "HIDE_VIOLENTLY";
		public const string LOADING_STAGE_SHOW = "LOADING_STAGE_SHOW";	//跳舞loading显示

        //DownLoad
        public const string DOWNLOAD_MOBILE_TIPSBOX = "DOWNLOAD_MOBILE_TIPSBOX";
        public const string LOAD_ONEASSET_FINISH = "LOAD_ONEASSET_FINISH";//每个资源下载完成后的事件;
		
		//广场舞大妈引导
		public const string BIGMAMA_GUIDE_SHOWHELP = "BIGMAMA_GUIDE_SHOWHELP";
		public const string BIGMAMA_GUIDE_SHOWREWARD = "BIGMAMA_GUIDE_SHOWREWARD";
		
		//换装 下载服饰
        public const string DOWNROLERES_START = "DOWNROLERES_START";
        public const string DOWNROLERES_END = "DOWNROLERES_END";

        public const string ROOM_ENTER_EQUIP = "ROOM_ENTER_EQUIP";
        public const string ROOM_QUIT_EQUIP = "ROOM_QUIT_EQUIP";

		//MediaPlayer
		public const string MEDIA_PLAYER_ATTACH = "MEDIA_PLAYER_ATTACH";
		public const string MEDIA_PLAYER_DEATTACH = "MEDIA_PLAYER_DEATTACH";
		public const string MEDIA_PLAYER_START = "MEDIA_PLAYER_START";
		public const string MEDIA_PLAYER_STOP = "MEDIA_PLAYER_STOP";
		public const string MEDIA_PLAYER_PAUSE = "MEDIA_PLAYER_PAUSE";
		public const string MEDIA_PLAYER_RESUME = "MEDIA_PLAYER_RESUME";
		public const string MEDIA_PLAYER_VOLUME = "MEDIA_PLAYER_VOLUME";				// 背景音音量调控

		//MsgWait
		public const string MSGWAIT_START = "MSGWAIT_START";
		public const string MSGWAIT_STOP = "MSGWAIT_STOP";

		// 秘境游戏中Prefab相关
		public const string FAIRYSCORE_SHOW = "FAIRYSCORE_SHOW";						// 显示Prefab
		public const string FAIRYSCORE_SETALLCOMBOMARK = "FAIRYSCORE_SETALLCOMBOMARK";	// 设置全P分数
		public const string FAIRYSCORE_REFRESHSCORE = "FAIRYSCORE_REFRESHSCORE";		// 刷新分数

		//T台秀;
		public const string CATWALK_WAIT_TIMESTAGE = "CATWALK_WAIT_TIMESTAGE";		//通知头像展示部分关闭;

		public const string CATWALK_ROUNDSWITCH_BEGIN = "CATWALK_ROUNDSWITCH_BEGIN";	//通知头像展示部分显示;
		public const string CATWALK_ROUNDSWITCH_END = "CATWALK_ROUNDSWITCH_END";		//通知头像展示部分关闭;

		public const string CATWALK_TURN_CAMERA_BLACK = "CATWALK_TURN_CAMERA_BLACK";	//通知主摄像机显示内容变黑,因为有分镜头加入;
		public const string CATWALK_TURN_CAMERA_LIGHT = "CATWALK_TURN_CAMERA_LIGHT";	//通知主摄像机显示内容变亮,分镜头移出;

		public const string CATWALK_PK_RESULT = "CATWALK_PK_RESULT";					//开始结算画面;
		public const string CATWALK_PK_REFRESH_SCORE = "CATWALK_PK_REFRESH_SCORE";		//通知界面可以刷新分数

		public const string CATWALK_PK_ROUND_RESULT_SHOW = "CATWALK_PK_ROUND_RESULT_SHOW";		// 显示回合阶段
		public const string CATWALK_PK_ROUND_RESULT_HIDE = "CATWALK_PK_ROUND_RESULT_HIDE";		// 隐藏回合阶段
		public const string CATWALK_PK_JOIN_RESULT_HIDE = "CATWALK_PK_JOIN_RESULT_HIDE";		// 隐藏回合阶段
		
		public const string CATWALK_PK_ALL_END = "CATWALK_PK_ALL_END";						// 全部结束了

		public const string CATWALK_PK_NOTIFY_ROLE_SCORE = "CATWALK_PK_NOTIFY_ROLE_SCORE";	// 延迟通知刷新PK分数

		//活动红点;
		public const string ACTIVITY_CONTROL_UI_REFRESH_ACTIVITY = "ACTIVITY_CONTROL_UI_REFRESH_ACTIVITY";  //刷新活动红点状态;
		public const string ACTIVITY_CONTROL_UI_REFRESH_ANNOUN = "ACTIVITY_CONTROL_UI_REFRESH_ANNOUN";	//刷新咨询红点状态;

		public const string LanternGuide_QuitLanternMall = "LanternGuide_QuitLanternMall";	// 神灯引导 退出神灯商城
		public const string LanternGuide_QuitStageInfo = "LanternGuide_QuitStageInfo";		// 神灯引导 退出关卡详情

		// 秀场商城
		public const string STAR_SHOW_MALL_OPEN = "STAR_SHOW_MALL_OPEN";				// 秀场商城打开
		public const string STAR_SHOW_MALL_CLOSE = "STAR_SHOW_MALL_CLOSE";				// 秀场商城关闭
		
		//喵宝藏
		public const string CatTreasure_CloseBindMB = "CatTreasure_CloseBindMB";		//绑定MB关闭通知

		public const string MallSwitch_CloseMainMall = "MallSwitch_CloseMainMall";		//商城切换时关闭恋物商城

		//称号
		public const string TITLE_SELECTED = "TITLE_SELECTED";          //选中称号

		//退出游戏弹框，也用于登陆前弹框提示
		public const string SHOW_GAME_EXIT_BOX = "SHOW_GAME_EXIT_BOX";

		//切换样式按钮延迟
		public const string BTNCLICKDELAY = "BTNCLICKDELAY";

		public const string STARSHOWGUIDE_SLIDER = "STARSHOWGUIDE_SLIDER";	//明星秀场引导滑动
		public const string STARSHOWGUIDE_SHOWPOPULARITY = "STARSHOWGUIDE_SHOWPOPULARITY";				//明星秀场显示人气框
		public const string STARSHOWGUIDE_CLOSEITEMBAGPICK = "STARSHOWGUIDE_CLOSEITEMBAGPICK";

		public const string CHECK_RES_LOADING_SHOW = "CHECK_RES_LOADING_SHOW";	// 动态下载检测Loading界面

		public const string SHINING_SWITCHANITION_EVT = "SHINING_SWITCHANITION_EVT";	//闪亮饰品切换动画
	}
}