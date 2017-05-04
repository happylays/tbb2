
namespace LoveDance.Client.Network
{
	class NetMsgTypeDef
	{
		public const ushort MSG_TYPE_SYSTEM = 0x000;
		public const ushort MSG_TYPE_ACCOUNT = 0x100;
		public const ushort MSG_TYPE_LOGIN = 0x200;
		public const ushort MSG_TYPE_CHAT = 0x300;
		public const ushort MSG_TYPE_SERVER = 0x400;
		public const ushort MSG_TYPE_PLAYER = 0x500;
		public const ushort MSG_TYPE_QUEST = 0x600;
		public const ushort MSG_TYPE_EXCHANGE = 0x0800;
		public const ushort MSG_TYPE_FRIEND = 0x0900;
		public const ushort MSG_TYPE_FAMILY = 0x1000;
		public const ushort MSG_TYPE_MAIL = 0x1200;
		public const ushort MSG_TYPE_MALL = 0x1300;
		public const ushort MSG_TYPE_ROOM = 0x1400;
		public const ushort MSG_TYPE_COUPLE = 0x1500;
		public const ushort MSG_TYPE_RANKING = 0x1600;
		public const ushort MSG_TYPE_VIP = 0x1800;
		public const ushort MSG_TYPE_CHECKIN = 0X1900;
		public const ushort MSG_TYPE_ACTIVITY = 0X1A00;	//活动
		public const ushort MSG_TYPE_DANCEGROUP = 0X1B00;
		public const ushort MSG_TYPE_STUDIO = 0X1C00;
		public const ushort MSG_TYPE_AMUSEMENT = 0X1D00;
		public const ushort MSG_TYPE_PLATFORMPRIVILEGE = 0x1F00;
		public const ushort MSG_TYPE_LANTERN = 0X2000;
		public const ushort MSG_TYPE_NOTIFICATION = 0x2100;	// IOS Official Used Only
		public const ushort MSG_TYPE_MONTHCARD = 0X2200;//月卡
		public const ushort MSG_TYPE_ACTIVENESS = 0X2300; //活跃度
		public const ushort MSG_TYPE_MEDAL = 0x2500;	//勋章;
		public const ushort MSG_TYPE_HANDBOOK = 0x2600;	//勋章;
		public const ushort MSG_TYPE_VEHICLE = 0x2700;	//座驾
		public const ushort MSG_TYPE_FAIRYLAND = 0x2900;			// 舞团秘境
		public const ushort MSG_TYPE_NEWROOM = 0x2A00;			// 秘境房间
		public const ushort MSG_TYPE_NEWROOMBROADCAST = 0x2B00;	// 状态同步消息
		public const ushort MSG_TYPE_CATWALK = 0x2C00;	//T台;
		public const ushort MSG_TYPE_STARSHOW = 0x2D00;	//明星秀场
	}
	public enum GameMsgType : ushort
	{
		//系统

		MSG_SYSTEM_Connect = NetMsgTypeDef.MSG_TYPE_SYSTEM + 1,
		MSG_SYSTEM_Disconnect,
		MSG_SYSTEM_Ping,
		MSG_SYSTEM_Packet,
		MSG_SYSTEM_Slot,
		MSG_SYSTEM_Invalid,
		MSG_SYSTEM_Test,


		//登入
		MSG_ACCOUNT_MSGBEGIN = NetMsgTypeDef.MSG_TYPE_ACCOUNT,
		MSG_ACCOUNT_Login,						//账号登录=MSG_TYPE_ACCOUNT+1
		MSG_ACCOUNT_LoginResult,				//账号登录结果
		MSG_ACCOUNT_ServerState,				//获取服务器状态

		MSG_S2C_IsCanInviteFriend,				//这两个是内测期间 邀请好友加入
		MSG_C2S_IsCanInviteFriend,				//这两个是内测期间 邀请好友加入

		MSG_C2S_ChongZhi,						//在游戏内充值
		MSG_S2C_ChongZhiResult,

		MSG_C2S_CheckAccountIsExist,			//查询账号在游戏中存不存在
		MSG_S2C_CheckAccountIsExist,

		MSG_S2C_KBINFO,							//客户端收到的KB信息
		MSG_C2C_BUISNESSID,						//客户端收到的交易号。

		MSG_ACCOUNT_CreateAccount,				//创建账号
		MSG_ACCOUNT_QuickCreateAccoout,			//快速创建账号(系统自动生成name,accout,uuid,password，返回name以及passwo
		MSG_ACCOUNT_QuickCreateAccooutResult,	//快速创建账号结果
		MSG_ACCOUNT_CreateAccountResult,		//创建账号的结果，失败的时候才能收到
		MSG_ACCOUNT_NotActivated,				//未激活
		MSG_ACCOUNT_CheckActivate,				//验证激活码
		MSG_ACCOUNT_CheckActivateResult,		//验证激活码结果
		MSG_ACCOUNT_HeartBeatRequest,			//心跳请求
		MSG_ACCOUNT_HeartBeatResponse,			//心跳回应
		MSG_ACCOUNT_C2S_PlayerLogout,			//玩家退出

		MSG_Account_C2S_GetGameServersInfo,		//获取服务器全部信息
		MSG_Account_S2C_GetGameServersInfoRes,	//返回服务器全部信息

		MSG_S2C_AllowCurrencyList,
		MSG_C2S_ClientDeviceInfo,
		MSG_S2C_ValidChargeDevice,

		MSG_C2S_UploadChargeInfo,

		//player attribute
		MSG_PLAYER_MSGBEGIN = NetMsgTypeDef.MSG_TYPE_PLAYER,

		MSG_C2S_CreateRole,				//创建角色
		MSG_S2C_CreateRoleSuc,
		MSG_S2C_CreateRoleFail,

		MSG_C2S_SwitchLine,
		MSG_S2C_SwitchLineRes,

		MSG_C2S_CheckName,				//检查角色名称是否重复
		MSG_S2C_CheckNameRes,

		MSG_Role_CreateQuestList,		//创建任务列表（原来的创建角色消息分成两部分，中间插上动态任务的处理）
		MSG_S2C_SwapItem,
		MSG_C2S_IncItemLevel,
		MSG_S2C_RefreshItem,
		MSG_S2C_BatchRefreshItem,		//刷新多个物品
		MSG_S2C_UseItemFailed,		//刷新多个物品
		MSG_C2S_UseItem,				//客户端发出使用物品的动作，双击装备也发这个消息
		MSG_C2S_UseMutipleItem,
		MSG_S2C_UseMutipleItemSuc,
		MSG_C2S_PickItem,				//客户端发出捡起物品的动作
		MSG_S2C_PickItem,				//server....
		MSG_C2S_SwapItem,
		MSG_C2S_ItemRemove,
		MSG_S2C_UpdateCoolDown,
		MSG_C2S_GetCoolDown,
		MSG_S2C_GetCoolDown,			//cooldown时间到，可以使用了
		MSG_C2S_ItemSend,				//物品赠送
		MSG_S2C_ItemSendResult,
		MSG_C2S_EquipOperate,			//装备的操作，包括穿和卸
		MSG_C2S_ItemColumn,
		MSG_S2C_ItemColumn,				//物品栏扩展
		MSG_C2S_GetRoleInfo,			//请求获得玩家信息
		MSG_S2C_GetRoleInfo,			//返回玩家信息

		MSG_C2S_TouristSignInfo,		//游客登陆后发送信息给服务器

		MSG_C2S_TouristRewardInfo,		//请求奖励信息
		MSG_S2C_TouristRewardInfoSuc,   //绑定奖励信息
		MSG_C2S_RequestTouristReward,	//请求奖励
		MSG_S2C_GetTouristRewardSuc,	//游客绑定请求奖励结果
		
		//更新客户端数据，这里都是同步客户端数据
		MSG_S2C_RefreshPt,
		MSG_S2C_RefreshChange,
		MSG_S2C_RefreshLuk,
		MSG_S2C_RefreshPre,
		MSG_S2C_RefreshInt,
		MSG_S2C_RefreshAPt,
		MSG_S2C_RefreshMoney,//金币
		MSG_S2C_RefreshExp,
		MSG_S2C_RefreshLevel,
		MSG_S2C_RefreshAPoint,
		MSG_S2C_RefreshMoneyGain,
		MSG_S2C_RefreshExpGain,
		MSG_S2C_RefreshPreGain,
		MSG_S2C_RefreshIgnoreMis,
		MSG_S2C_RefreshVip,
		MSG_S2C_RefreshChatColor,
		MSG_S2C_RefreshBagCapacity,
		MSG_S2C_RefreshTransformID,
        MSG_S2C_RefreshCollectCount,	//收藏总数
        MSG_S2C_RefreshBrilliant,		//闪亮值

		MSG_S2C_SerializeItemInfo,		//序列化物品

		MSG_TITLE_SET,
		MSG_TITLE_QUERYLIST,

		MSG_C2S_RequireChongzhiItem,
		MSG_S2C_ChongzhiItemInfo,
		MSG_S2C_RefreshChongzhiItemInfo,

		MSG_C2S_CheckRoleIsExist,			//查询账号在游戏中存不存在
		MSG_S2C_CheckRoleIsExist,

		MSG_S2C_RefreshMusic,				//添加了或者删除了某音乐
		MSG_S2C_RefreshScene,				//添加了或者删除了某场景

		MSG_C2S_UpdateSex,
		MSG_S2C_UpdateSex,
		MSG_C2S_UpdateRoleName,
		MSG_S2C_UpdateRoleName,
		MSG_S2C_FreshRoleName,
		MSG_S2C_DropItemError,				//丢弃物品的结果
		MSG_C2S_RoleLottery,				//角色抽奖

		MSG_S2C_RequireCreateRole,			//告知客户端需要创建角色
		MSG_C2S_ChangeRoleName,
		MSG_C2S_ChangeSignature,
		MSG_S2C_ChangeSignatureRes,
		MSG_S2C_RefreshHot,


		MSG_C2S_ChangeEquipItem,
		MSG_S2C_ChangeEquipItem,
		
		MSG_C2S_NeedArrangeBagItem,		// 整理背包物品堆叠

		MSG_C2S_ItemAward,
		MSG_S2C_ItemAwardSuc,
		MSG_S2C_ItemAwardFail,

		MSG_S2C_FinishedFirstPayment,
		MSG_C2S_GetFirstPaymentRewards,
		MSG_S2C_GetFirstPaymentRewardsRes,
		MSG_S2C_FirstPaymentStateChange,		// 服务器 返回 首充奖励 是否开放

		MSG_C2S_RequsetQuotaInfo,				//请求定额双倍信息
		MSG_S2C_QuotaRecharge,					//定额首冲双倍

		MSG_C2S_GetAnnouncement,
		MSG_S2C_UpdateAnnouncement,
		MSG_S2C_AnnoucementUpdated,
		MSG_C2S_ReadAnnouncement,	//通知服务器消息已读;

		MSG_C2S_EnterZone,

		MSG_C2S_CompleteTeachGuide,
		MSG_S2C_CompleteTeachGuideSec,

		MSG_C2S_CompleteProcGuide,
		MSG_S2C_CompleteProcGuideSec,

		MSG_S2C_SendPlayerSetting,
		MSG_C2S_OptionSetting,
		MSG_S2C_OptionSettingSuc,
		MSG_S2C_OptionSettingFail,

		MSG_C2S_CloseNewFuntionPoint,

		MSG_S2C_SimulateBoxItem,

		MSG_C2S_PlayerMoveTo,
		MSG_S2C_PlayerMoveTo,
		MSG_C2S_PlayerMoveType,
		MSG_S2C_PlayerMoveType,

		// TransformId realtime syncronized
		MSG_C2S_PlayerMotion,
		MSG_S2C_PlayerMotion,

		MSG_S2C_UpdateUnlockedPose,	// 更新解锁动作

		MSG_C2S_UnlockPose,			// 主动解锁动作
		MSG_S2C_UnlockPoseRes,		// 主动解锁动作结果

		MSG_S2C_OpenBoxResult,
		MSG_S2C_OpenPackageResult,

		MSG_S2G_EqualizeResult,		// 开包获得相同服饰补偿;

		MSG_S2C_ChangeEquipItemFail,

		MSG_S2C_SynExtendItemBinData,

		MSG_S2C_AntiAddictionTip,	//IOS Offical Not Used
		MSG_C2S_SyncAdult,

		MSG_C2S_BugReport,

		MSG_S2C_GetClothGroups,
		MSG_C2S_SelectClothGroup,
		MSG_S2C_SelectClothGroupRes,
		MSG_C2S_UpdateClothGroup,
		MSG_S2C_UpdateClothGroupRes,
		MSG_C2S_UpdateClothGroupName,
		MSG_S2C_UpdateClothGroupNameRes,

		MSG_C2S_SelectFace,
		MsG_S2C_SelectFaceRes,

		MSG_C2S_CompleteDanceAni,
		MSG_S2C_CompleteDanceAniSuc,

		MSG_C2S_RechargeRewardEnable,		/** 客户端 询问 充值奖励 是否开放**/
		MSG_S2C_RechargeRewardsEnableSuc,	/**服务器 返回 充值奖励 是否开放**/
		MSG_S2C_GetRechargeRewardsRulesSuc,	/**服务器响应 充值奖励规则**/
		MSG_C2S_GetRechargeRewardsRules,	/**客户端请求 充值奖励规则**/
		MSG_S2C_SendRechargeRewardFinished,	/**奖励 发送 完成**/

		MSG_C2S_UpdateLBSPositon,
		MSG_C2S_GetNearbyPlayers,
		MSG_S2C_NearbyPlayers,
		MSG_C2S_GetAllBuff,				// 查询所有buff
		MSG_S2C_GetAllBuff,             // 返回所有buff
		MSG_C2S_UnApplyBuff,            //解除buff
		MSG_S2C_UnApplyBuff,            // 解除buff返回

		MSG_S2C_RefreshPtBind,

		MSG_C2S_ChangeConstellation,//修改星座
		MSG_S2C_ChangeConstellationRes,
		MSG_C2S_ReceiveGuideExperienceCard,
		MSG_S2C_ReceiveGuideExperienceCard,

		MSG_C2S_ReplaceClothEffect,
		MSG_S2C_ReplaceClothEffectFail,
		MSG_S2C_ReplaceClothEffectResult,

		MSG_C2S_LevelUpEffect,
		MSG_S2C_LevelUpEffectFail,
		MSG_S2C_LevelUpEffectResult,

		MSG_S2C_RefreshClothEffectHandbookProgress,

		MSG_S2C_RefreshSevenColorProgress,

        MSG_C2S_PlayerDownloading,//玩家下载头顶进度条状态同步;
        MSG_S2C_PlayerDownloading,//玩家下载头顶进度条状态同步;
		
		MSG_C2S_RequestMagicArrayConfig,		//请求魔法阵配置信息
		MSG_S2C_SyncMagicArrayConfig,			//同步魔法阵配置信息
		
		MSG_C2S_RequestPreviewReward,			//请求预览产出物品
		MSG_S2C_RequestPreviewRewardRes,		//获取预览产出物品结果
		
		MSG_C2S_RequestExchangeList,			//请求兑换物品列表
		MSG_C2S_RequestRefreshExchangeList,		//请求刷新兑换物品列表
		MSG_S2C_RequestExchangeListRes,			//获取兑换物品列表结果
		MSG_S2C_RequestRefreshExchangeListFail,	//请求刷新兑换物品列表失败
		
		MSG_C2S_RequestDoExchangeItem,			//请求兑换物品
		MSG_S2C_RequestDoExchangeItemRes,		//兑换物品结果
		
		MSG_C2S_RequestCallMagicArray,			//请求召唤魔法阵
		MSG_S2C_RequestCallMagicArrayRes,		//召唤魔法阵结果
		
		MSG_S2C_NoticeFreeCallMagicArray,		//免费召唤魔法阵通知

        MSG_C2S_MoveItem,                         // 移动物品到背包或者仓库
        MSG_S2C_MoveItemFail,                     // 移动物品到背包或者仓库(失败)
        MSG_S2C_MoveItemSuccess,                  // 移动物品到背包或者仓库(成功)
        MSG_C2S_AddStorageCapacity,               // 增加仓库容量
        MSG_S2C_AddStorageFail,                   // 增加仓库容量(失败)
        MSG_S2C_AddStorageSuccess,                // 增加仓库容量（成功）
        MSG_S2C_SyncBagConfig,                    // 同步背包配置 
		
		MSG_S2C_NoticeStorageItemUsed ,          // 通知仓库物品被使用
        MSG_S2C_NoticeMergeToStorage,            // 通知合并到仓库

        MSG_C2S_RequestGuideFinish ,            // 背包教学引导完成
        MSG_S2C_RequestGuideFinishResult,       //　同步引导数据

		#region ClothEffectTransform
		MSG_S2C_SyncClothEffectTransformInfo,   // 同步变身信息
		MSG_C2S_Transform,                      // 启用变身 
		MSG_S2C_TransformResult,                // 启用变身结果
		MSG_S2C_TransformToOther,               // 变身广播
		MSG_C2S_CancelTransform,                // 取消变身
		MSG_S2C_CancelTransformResult,          // 取消变身结果
		MSG_C2S_SelectTransformTitle,           // 启用变身称号
		MSG_S2C_SelectTransformTitleResult,     // 启用变身称号结果
		MSG_C2S_CancelTransformTitle,           // 取消变身称号
		MSG_S2C_CancelTransformTitleResult,     // 取消变身称号结果

		#endregion

		MSG_C2S_GetCatTreasureInfo,				//获取猫宝藏的基本信息
		MSG_S2C_GetCatTreasureInfoSuc,			//返回猫宝藏信息数据

		MSG_C2S_RequestTreasureTimes,			//请求摇宝库
		MSG_S2C_RequestTreasureTimesSuc,		//请求摇宝库结算数据

		MSG_S2C_BindMBTreasureStart,			//开启摇绑定MB

		MSG_S2C_FreeCallRefresh,				//免费摇宝库通知

		#region  //称号
		
		MSG_C2S_ChangeTitle,				 //更换称号请求
		MSG_S2C_ChangeTitleResult,	 //更换称号结果
		MSG_S2C_SyncTitleInfo,            //同步称号信息
		MSG_S2C_SyncTitleToOthers,    //同步称号广播
		
		#endregion

		MSG_LOGIN_BEGIN = NetMsgTypeDef.MSG_TYPE_SERVER,
		MSG_SERVER_Connected,
		MSG_SERVER_LoginInfo,
		MSG_SERVER_Login,


		MSG_CHAT_BEGIN = NetMsgTypeDef.MSG_TYPE_CHAT,

		MSG_C2S_CHAT,					// 客户端发往服务器的聊天消息
		MSG_S2C_CHAT,					// 客户端发往服务器的聊天消息
		MSG_S2C_CHAT_PRIVTOSELF,		// 服务器发往客户端的私聊消息，此消息是私聊时，回给自己的

		MSG_S2C_SyncAutoChatInfo,		// 同步自动发言信息
		MSG_S2C_AutoChatResponse,		// 自动发言结果

		MSG_C2S_GroupDanceChatHistory,	// 发送 聊天记录

		// 聊天皮肤相关
		MSG_C2S_ChangeChatSkin,			// 更改聊天皮肤
		MSG_S2C_ChangeChatSkin,			// 更新聊天皮肤结果
		MSG_S2C_SyncChatSkinStatus,		// 同步聊天皮肤的状态

		//room
		Room_Msg_BeginID = NetMsgTypeDef.MSG_TYPE_ROOM,

		MSG_C2S_EnterLobby,
		MSG_C2S_ExitLobby,

		MSG_C2S_GetRoomList,
		MSG_S2C_GetRoomListRes,

		MSG_C2S_CreateRoom,
		MSG_S2C_CreateRoomSuc,
		MSG_S2C_CreateRoomFail,

		MSG_C2S_TryEnterRoom,
		MSG_S2C_RoomRequirePwd,

		MSG_C2S_EnterRoom,
		MSG_S2C_EnterRoomFail,

		MSG_C2S_ApplyMatch,
		MSG_C2S_CancelMatch,
		MSG_S2C_PrepareMatch,

		MSG_C2S_QuitRoom,
		MSG_S2C_QuitRoomSuc,
		MSG_S2C_QuitRoomFail,

		MSG_C2S_StartRoom,
		MSG_C2S_ReadyRoom,
		MSG_S2C_PrepareRoom,
		MSG_S2C_StartRoomSuc,
		MSG_S2C_StartRoomFail,

		MSG_C2S_EndRoom,
		MSG_S2C_EndRoomSuc,

		MSG_C2S_KickPlayer,
		MSG_S2C_KickPlayerFail,

		MSG_S2C_EnterPlayer,
		MSG_S2C_QuitPlayer,
		MSG_S2C_QuitMany,

		MSG_C2S_GetInviteeList,
		MSG_S2C_GetInviteeListSuc,
		MSG_S2C_GetInviteeListFail,

		MSG_C2S_InvitePlayer,
		MSG_S2C_InvitePlayerSuc,
		MSG_S2C_InvitePlayerFail,

		MSG_C2S_InviteeResponse,
		MSG_S2C_InviteeFail,
		MSG_S2C_InviteeNotice,

		MSG_C2S_InEquip,
		MSG_C2S_OutEquip,

		MSG_C2S_ChangeRoomPosState,
		MSG_S2C_ChangeRoomPosStateSuc,
		MSG_S2C_ChangeRoomPosStateFail,

		MSG_C2S_ChangeRoomInfo,
		MSG_S2C_ChangeRoomInfoSuc,
		MSG_S2C_ChangeRoomInfoFail,

		MSG_C2S_ChangeMusicInfo,
		MSG_S2C_ChangeMusicInfoSuc,
		MSG_S2C_ChangeMusicInfoFail,

		MSG_C2S_ChangeRoleRoomType,
		MSG_S2C_ChangeRoleRoomTypeSuc,
		MSG_S2C_ChangeRoleRoomTypeFail,

		MSG_C2S_ChangeRoleRoomState,
		MSG_S2C_ChangeRoleRoomStateSuc,
		MSG_S2C_ChangeRoleRoomStateFail,

		MSG_C2S_ReportRoundMark,
		MSG_S2C_SyncRoundMark,

		MSG_C2S_QuitMarkAward,

		MSG_C2S_PromoteRoomHost,
		MSG_S2C_PromoteRoomHostFail,
		MSG_S2C_PromoteRoomHostSuc,

		MSG_C2S_ReportEffectChange,
		MSG_S2C_SyncEffectState,

		MSG_C2S_LoadingStartRoomProgress,
		MSG_S2C_LoadingStartRoomProgress,

		MSG_S2C_RoomReward,

		MSG_C2S_SwitchDanceRoomTeamMode,
		MSG_S2C_SwitchRoomTeamModeSuc,
		MSG_S2C_SwitchRoomTeamModeFail,

		MSG_C2S_JoinDanceRoomTeam,
		MSG_S2C_JoinRoomTeamSuc,
		MSG_S2C_JoinRoomTeamFail,

		MSG_S2C_UpdateRoleRoomTeamInfo,		//个人组队信息修改:譬如加入别的队伍
		MSG_S2C_UpdateWholeRoomTeamInfo,	//更新整个房间组队信息

		MSG_S2C_BeginToSelectPartner,
		MSG_S2C_EndToSelectPartner,
		MSG_S2C_SelectPartner,
		MSG_S2C_SelectPartnerFail,
		MSG_C2S_SelectPartner,
		MSG_S2C_UpdateSweetValue,

		MSG_S2C_ChangeRoomColor,

		MSG_C2S_SwitchToOtherRoom,				// 切到其它房间
		MSG_S2C_SwitchToOtherRoomResult,		// 切换结果
		MSG_S2C_SwitchToOtherRoomTimeBegin,		// 切换倒计时

		MSG_S2C_RoomEventNotice,
		MSG_S2C_RoomIntimacyNoticy,

		Room_Msg_EndID,

		MSG_Friend_MsgBegin = NetMsgTypeDef.MSG_TYPE_FRIEND,//Friend

		MSG_C2S_FriendApplication,			// 申请成为好友(complete)
		MSG_C2S_AddBlackFriend,				// 添加黑名单好友(从黑名单拉至好友列表)(complete)
		MSG_C2S_AcceptApplication,			// 接受好友申请(complete)
		MSG_C2S_RefuseApplication,			// 拒绝好友申请(complete)
		MSG_C2S_AddBlackList,				// 添加黑名单(complete)
		MSG_C2S_RemoveBlackList,			// 删除黑名单(complete)
		MSG_C2S_RemoveFriend,				// 删除好友(complete)
		MSG_C2S_GetBlackListOnlineInfo,		// 获得黑名单在线信息(complete)
		MSG_C2S_GetTempListPlayerOnlineInfo,// 最近玩伴、最近聊天的列表玩家信息(complete)
		MSG_C2S_FindPlayer,
		MSG_C2S_ReworkFriendNotes,          // 修改好友备注



		MSG_S2C_FriendApplication,			// 通知客户端玩家申请成为好友(complete)

		MSG_S2C_FriendApplicationSuc,		// 发送申请成功(complete)
		MSG_S2C_FriendApplicationFail,		// 发送申请失败(complete)

		MSG_S2C_AddBlackFriendFail,			// 添加黑名单好友失败(complete)

		MSG_S2C_AcceptApplicationSuc,		// 同意添加好友消息成功返回(complete)
		MSG_S2C_AcceptApplicationFail,		// 同意添加好友消息失败返回(complete)

		MSG_S2C_RefuseApplicationSuc,		// 拒绝好友申请成功返回(complete)
		MSG_S2C_RefuseApplicationFail,		// 拒绝好友申请失败返回(complete)

		MSG_S2C_GetFriendOnlineInfoReuslt,	// 好友在线信息(complete)

		MSG_S2C_RemoveFriendSuc,			// 删除好友成功(complete)
		MSG_S2C_RemoveFriendFail,			// 删除好友失败(complete)

		MSG_S2C_AddBlackListSuc,			// 加入黑名单成功(complete)
		MSG_S2C_AddBlackListFail,			// 删除黑名单失败(complete)

		MSG_S2C_RemoveBlackListSuc,			// 删除黑名单成功(complete)
		MSG_S2C_RemoveBlackListFail,		// 删除黑名单失败(complete)

		MSG_S2C_UpdateFriendInfo,			// 更新好友状态(complete)
		MSG_S2C_NotityHasBeenAccepted,		// 自己已经被添加为好友(complete)

		MSG_S2C_HasBeenRemovedFriend,		// 通知客户端(别人)好友被删除(complete)
		MSG_S2C_HasBeenRemovedBlackList,	// 通知客户端(别人)黑名单被删除(complete)

		MSG_S2C_GetTempListPlayerInfoSuc,	// 最近玩伴、最近聊天的列表玩家信息(complete)
		MSG_S2C_GetTempListPlayerInfoFail,	// (complete)

		MSG_S2C_GetBlackListOnlineInfoSuc,		// 黑名单在线信息 
		MSG_S2C_GetBlackListOnlineInfoFail,		// 获取黑名单失败
		MSG_S2C_BlackList,                  //黑名单名单

		MSG_S2C_FindPlayerResult,
		MSG_S2C_FindPlayerFail,

		MSG_S2C_HeartBeatMakeFriend,

		MSG_S2C_FindPlayerSetting,

		MSG_C2S_GetRecommendFriends,
		MSG_S2C_GetRecommendFriendsResult,

		MSG_C2S_FriendFollowTryEnterRoom,	// 好友陪伴请求进入房间
		MSG_S2C_FriendFollowRequirePwd,		// 好友陪伴房间需要密码
		MSG_C2S_FriendFollowEnterRoom,		// 好友陪伴发送房间密码
		MSG_S2C_FriendFollowFail,			// 好友陪伴失败

		MSG_S2C_ResultFriendNum,			// 好友上限;

		MSG_QUEST_BEGIN = NetMsgTypeDef.MSG_TYPE_QUEST,

		// msg for quests   
		MSG_C2S_GetQuestReward,
		MSG_C2S_GetQuestList,
		MSG_C2S_QuestRead,
		MSG_C2S_AcceptQuest,

		MSG_S2C_AutoPushQuest,
		MSG_S2C_QuestStateChanged,
		MSG_S2C_GetQuestListResult,
		MSG_S2C_GetQuestRewardResult,
		MSG_S2C_AcceptQuestResult,
		MSG_S2C_SendQuestStaticBinData,
		MSG_S2C_UnRewardQuest,
		MSG_C2S_SendFresherQuest,
		MSG_S2C_SendFresherQuest,
		MSG_C2S_GetFresherQuestReward,
		MSG_S2C_GetFresherQuestRewardSuc,
		MSG_S2C_GetFresherQuestRewardFail,
		MSG_S2C_UnRewardFresherQuest,
		MSG_S2C_SendQuestNewStaticBinData,
		MSG_C2S_ClientCompleteQuest,
		MSG_C2S_GetQuestState,
		MSG_S2C_GetQuestStateRes,


		//mall
		Mall_Msg_BeginID = NetMsgTypeDef.MSG_TYPE_MALL,
		MSG_C2S_MALL_GETGOODS,
		MSG_C2S_MALL_GETSOMEGOODS,
		MSG_C2S_MALL_BUY,
		MSG_C2S_MALL_SEND,
		MSG_C2S_MALL_ASKFOR,

		MSG_C2S_MALL_RENEW,
		MSG_C2S_SendFriendDesire,

		MSG_S2C_MALL_GETGOODSRESULT,
		MSG_S2C_MALL_GETSOMEGOODSRESULT,
		MSG_S2C_MALL_BUYRESULT,
		MSG_S2C_MALL_SENDRESULT,
		MSG_S2C_MALL_ASKFORRESULT,
		MSG_S2C_MALL_BEASKED,
		MSG_S2C_MALL_RENEWRESULT,
		MSG_S2C_SendFriendDesireSec,
		MSG_S2C_SendFriendDesireFail,

		MSG_C2S_MALL_BEASKED_SEND,			// 索要赠送  
		MSG_S2C_MALL_BEASKED_SENDRESULT,

		MSG_C2S_MALL_EnterMall,
		MSG_S2C_MALL_EnterMallRes,

		MSG_S2C_MALL_AllGoodsInfo,

		MSG_C2S_MALLAction_EnterMall,//进入商城请求数据刷新
		MSG_S2C_MALLAction_RefreshInfoRes,//数据刷新结果


		MSG_C2S_RequestGroupBuyingList,
		MSG_S2C_RequestGroupBuyingListRes,
		MSG_C2S_RequestGroupBuyingBuyItem,
		MSG_S2C_RequestGroupBuyingBuyItemRes,

		MSG_C2S_RequestLimitedEditionList,
		MSG_S2C_RequestLimitedEditionListRes,
		MSG_C2S_RequestLimitedEditionBuyItem,
		MSG_S2C_RequestLimitedEditionBuyItemRes,

		MSG_C2S_RequestAuctionList,
		MSG_S2C_RequestAuctionListRes,
		MSG_C2S_RequestAuctionBid,
		MSG_S2C_RequestAuctionBidRes,
		MSG_S2C_NoticeAuctionBidSurpassed,
		MSG_S2C_NoticeAuctionBidSuccess,
		MSG_C2S_ExperienceCardInfo,
		MSG_S2C_ExperienceCardInfoRes,
		MSG_S2C_ExperienceCardInfoNotify,

        MSG_C2S_MALL_DESIRE,
		MSG_S2C_MALL_DESIRERESULT,

        MSG_C2S_MALL_REMOVE_DESIRE,
        MSG_S2C_MALL_REMOVE_DESIRE_FAIL,
        MSG_S2C_RemoveDesireSuc,

        MSG_C2S_GetDesireList,
        MSG_S2C_GetDesireListSuc,

		Mall_Msg_EndID,

		//mail
		Mail_Msg_BeginID = NetMsgTypeDef.MSG_TYPE_MAIL,
		MSG_C2S_GETMAILBOX,
		MSG_C2S_GETMAILINFO,
		MSG_C2S_SENDMAIL,
		MSG_C2S_DELETEMAIL,
		MSG_C2S_GETMAILITEM,

		MSG_S2C_GETMAILBOX,
		MSG_S2C_GETMAILINFO,
		MSG_S2C_SENDMAIL,
		MSG_S2C_DELETEMAIL,
		MSG_S2C_GETMAILITEM,
		MSG_S2C_RECEIVEMAIL,
		MSG_S2C_UNREADMAIL,

		Msg_S2C_ReceiveStrangerMailFlag,	//获取陌生人邮件开关状态
		Msg_C2S_SetStrangerMailFlag,		//修改陌生人邮件开关状态
		Msg_S2C_SetStrangerMailFlag,		//修改陌生人邮件开关状态结果

		Mail_Msg_EndID,

		MSG_VIP_BEGIN = NetMsgTypeDef.MSG_TYPE_VIP,
		MSG_C2S_GetVIPRewardInfo,
		MSG_C2S_GetActivatedVIPRewardInfo,
		MSG_C2S_ActivateVIP,
		MSG_C2S_GetDailyVIPReward,
		MSG_C2S_GetYearlyVIPReward,

		MSG_S2C_GetVIPRewardInfoResult,
		MSG_S2C_GetActivatedVIPRewardInfoResult,
		MSG_S2C_ActivateVIPResult,
		MSG_S2C_GetDailyVIPRewardResult,
		MSG_S2C_GetYearlyVIPRewardResult,
		MSG_S2C_UpdateVIPInfo,

		MSG_S2C_LevelUpReward,
		MSG_C2S_GetVIPExpReward,
		MSG_S2C_GetVIPExpRewardSuc,

		MSG_C2S_RequestBuyVip,
		MSG_S2C_RequestBuyVipRes,

		MSG_C2S_SendOpenVipFrom,
		MSG_S2C_GetOpenVipFrom,

		MSG_VIP_END,

		MSG_CHECKIN_BEGIN = NetMsgTypeDef.MSG_TYPE_CHECKIN,
		MSG_C2S_GetCheckInInfo,
		MSG_C2S_Check,								// check

		MSG_S2C_GetCheckInInfoResult,	// Sign Info
		MSG_S2C_CheckInSuc,	// Signin success
		MSG_S2C_CheckInFail,	// Signin fail

		MSG_S2C_GetCheckInVipReissue,


		MSG_ACTIVITY_BEGIN = NetMsgTypeDef.MSG_TYPE_ACTIVITY,
		MSG_S2C_InTimeOnlineReward,

		MSG_C2S_GetOnlineReward,
		MSG_S2C_GetOnlineRewardSuc,
		MSG_S2C_GetOnlineRewardFail,

		MSG_S2C_GetLevelUpRewardInfoRes,
		MSG_S2C_UpgradeInstanller,
		MSG_S2C_UpdateExtraExpActivityState,

		MSG_S2C_SendGift,						// send gift to player direct
		MSG_S2C_UpdateGiftInfo,					// gift info
		MSG_C2S_GetGift,						// get the gift
		MSG_S2C_GetGiftResult,					// feedback of getting the gift

		MSG_S2C_UpdateOnlineRewardInfo,

		MSG_C2S_GetAllActivitiesInfo,
		MSG_S2C_GetAllActivitiesInfoSuc,
		MSG_S2C_GetAllActivitiesInfoFail,
		MSG_C2S_ReadActivity,

		MSG_C2S_GetCDKeyInfo,//激活码兑换
		MSG_S2C_GetCDKeyInfoSuc,
		MSG_C2S_GetBindActivationInfo,
		MSG_S2C_GetBindInfoSuc,

		MSG_S2C_UpdateCumulativeActivityState,
		MSG_S2C_UpdateCumulativeRechargeNum,
		MSG_C2S_GetCumulativeRechargeReward,
		MSG_S2C_GetCumulativeRechargeRewardSuc,
		MSG_S2C_GetCumulativeRechargeRewardFail,
		MSG_S2C_UpdateCumulativeSpendNum,

		MSG_C2S_GetProprietaryReward,
		MSG_S2C_GetProprietaryRewardResult,

		MSG_C2S_GetCumulativeSpendReward,
		MSG_S2C_GetCumulativeSpendRewardSuc,
		MSG_S2C_GetCumulativeSpendRewardFail,

		MSG_C2S_GetCumulativeSpendForMedalReward,
		MSG_S2C_GetCumulativeSpendForMedalRewardSuc,
		MSG_S2C_GetCumulativeSpendForMedalRewardFail,

		MSG_C2S_ExchangeItem,
		MSG_S2C_ExchangeItemSuc,
		MSG_S2C_ExchangeItemFail,

		MSG_C2S_GetFestivalFreeGiftInfo,
		MSG_S2C_GetFestivalFreeGiftInfoSuc,

		MSG_C2S_GetFestivalWishInfo,
		MSG_S2C_GetFestivalWishInfoSuc,

		MSG_C2S_GetFestivalBlessInfo,
		MSG_S2C_GetFestivalBlessInfoSuc,

		MSG_C2S_GetFestivalGift,
		MSG_S2C_GetFestivalGiftSuc,
		MSG_S2C_GetFestivalGiftFail,

		MSG_C2S_FestivalWish,
		MSG_S2C_FestivalWishSuc,
		MSG_S2C_FestivalWishFail,

		MSG_C2S_GetFestivalBlessReward,
		MSG_S2C_GetFestivalBlessRewardSuc,
		MSG_S2C_GetFestivalBlessRewardFail,

		MSG_C2S_BlessFriend,
		MSG_S2C_BlessFriendSuc,
		MSG_S2C_BlessFriendFail,

		MSG_S2C_GetBlessFromFriend,

		MSG_S2C_SyncPuzzleData,
		MSG_C2S_FillPuzzle,
		MSG_S2C_FillPuzzleSuc,
		MSG_S2C_FillPuzzleFail,

		MSG_C2S_BindInvitationCode,				//被邀请者绑定邀请码并领取奖励
		MSG_S2C_BindInvitationCodeSuc,
		MSG_S2C_BindInvitationCodeFail,
		MSG_C2S_GetInvitationInviterInfo,		//邀请者查询活动信息, 自己的邀请码和邀请人数
		MSG_S2C_GetInvitationInviterInfoSuc,
		MSG_S2C_GetInvitationInviterInfoFail,
		MSG_C2S_GetInvitationInviterReward,		//邀请者获取奖励
		MSG_S2C_GetInvitationInviterRewardSuc,
		MSG_S2C_GetInvitationInviterRewardFail,

		MSG_C2S_GetFresherReward,
		MSG_S2C_GetFresherRewardSuc,
		MSG_S2C_GetFresherRewardFail,
		MSG_S2C_GetFresherAllSignReward,

		MSG_C2S_GetExtraActivityInfo,
		MSG_S2C_GetExtraActivityInfoSuc,

		MSG_S2C_SyncRecompenstateData,
		MSG_C2S_GetRecompenstate,
		MSG_S2C_GetRecompenstateSuc,
		MSG_S2C_GetRecompenstateFail,

		MSG_C2S_SocialShare,
		MSG_S2C_SocialShare,

		//社交分享活动
		MSG_C2S_ActivitySocialShare,
		MSG_S2C_ActivitySocialShare,
		MSG_S2C_CanSocialShareFirst,	//能否首次分享

		MSG_C2S_RequestVipExtraReward,
		MSG_S2C_RequestVipExtraRewardRes,

		MSG_C2S_GetLongActingRechargeInfo,		// 发送获取长效累充配置表消息
		MSG_S2C_GetLongActingRechargeInfo,		// 收到长效累充配置表消息
		MSG_C2S_GetLongActingRechargeBroadcast,	// 发送获取领取奖励名单
		MSG_S2C_GetLongActingRechargeBroadcast,	// 收到领取奖励名单
		MSG_C2S_GetLongActingRechargeReward,	// 发送领取长效累充奖励请求
		MSG_S2C_GetLongActingRechargeFail,		// 领取长效累充奖励成功
		MSG_S2C_GetLongActingRechargeSucess,	// 领取长效累充奖励失败

		MSG_C2S_RequestBuyItemGotMedalReward,       //活动中心 勋章 购买礼包;
		MSG_S2C_RequestBuyItemGotMedalRewardSuc,
		MSG_S2C_RequestBuyItemGotMedalRewardFail,
		MSG_S2C_UpdateBuyItemGotMedalCurrentCount,  //活动中心 勋章 购买礼包 计数更新;

        MSG_C2S_GetCanGetRedEnevlopeNum,           // 请求可领取的红包个数
        MSG_S2C_UpdateCanGetRedEnevlopeNum,        // 返回可领取的红包个数
        MSG_C2S_GetRedEnvelopeList,                // 获取红包列表
        MSG_S2C_GetRedEnvelopeListResult,          // 返回红包列表
        MSG_C2S_GetRedEnvelopeDetail,              // 
        MSG_S2C_GetRedEnvelopeDetailFail,
        MSG_S2C_GetRedEnvelopeDetailResult,
        MSG_C2S_OpenRedEnvelope,
        MSG_S2C_OpenRedEnvelopeResult,
        MSG_C2S_SetAnonymity,
        MSG_S2C_UpdateAnonymity,
        MSG_S2C_NewRedEnvelopeNotice,

		MSG_C2S_GetDailyRechargeSpendReward,       //领取每日充值消费奖励请求
		MSG_S2C_GetDailyRechargeSpendRewardResult, //领取每日充值消费奖励结果
		MSG_S2C_SynDailyRechargeSpendRewardState,  //同步每日充值消费可领取状态

		MSG_DANCEGROUP_BEGIN = NetMsgTypeDef.MSG_TYPE_DANCEGROUP,
		MSG_C2S_RequestMyDanceGroup,//请求我的舞团信息
		MSG_C2S_CreateDanceGroup,
		MSG_C2S_RequestEnterDanceGroup,
		MSG_C2S_CancelRequestEnterDanceGroup,
		MSG_C2S_ReplyRequestEnterDanceGroup,
		MSG_C2S_ReplyRequestListEnterDanceGroup,
		MSG_C2S_GetDanceGroupInfo,
		MSG_C2S_GetDanceGroupInfoByName,	//按舞团名请求查找舞团信息
		MSG_C2S_GetDanceGroupInfoList,
		MSG_C2S_ExitDanceGroup,
		MSG_C2S_ChangeDanceGroupLeader,
		MSG_C2S_DismissDanceGroup,
		MSG_C2S_CancelDismissDanceGroup,
		MSG_C2S_ChangeDanceGroupTitle,
		MSG_C2S_KickOutDanceGroupMember,
		MSG_C2S_UpdateDanceGroupColor,
		MSG_C2S_UpdateDanceGroupBadge,
		MSG_C2S_UpdateDanceGroupTitleName,
		MSG_C2S_UpdateDanceGroupProfile,
		MSG_C2S_UpdateDanceGroupAnnouncement,


		//DanceGroup
		MSG_C2S_SelectDanceGroupTuanhui,
		MSG_C2S_UnlockDanceGroupTuanhui,
		MSG_C2S_SaveDanceGroupTuanhui,
		MSG_C2S_DanceGroupActivityStarGetInfo,
		MSG_C2S_DanceGroupActivityStarGetPowerInfo,
		MSG_C2S_DanceGroupActivityStarAddPower,
		MSG_C2S_DanceGroupActivityStarOpenPacket,
		MSG_C2S_DanceGroupShopGetInfo,
		MSG_C2S_DanceGroupShopExchange,
		MSG_C2S_DanceGroupGetRecords,


		MSG_S2C_CreateDanceGroupResult,
		MSG_S2C_RequestEnterDanceGroupResult,
		MSG_S2C_CancelRequestEnterDanceGroupResult,
		MSG_S2C_EnterDanceGroupSuccess,
		MSG_S2C_GetDanceGroupInfoResult,
		MSG_S2C_SendDanceGroupInfo,
		MSG_S2C_SendSimpleDanceGroupInfo,
		MSG_S2C_GetDanceGroupInfoByNameResult,//按舞团名请求查找舞团信息结果
		MSG_S2C_SendDanceGroupInfoList,
		MSG_S2C_ExitDanceGroupResult,
		MSG_S2C_ChangeDanceGroupLeaderResult,
		MSG_S2C_DismissDanceGroupResult,
		MSG_S2C_CancelDismissDanceGroupResult,
		MSG_S2C_ChangeDanceGroupTitleResult,
		MSG_S2C_KickOutDanceGroupMemberResult,
		MSG_S2C_KickedOutDanceGroup,
		MSG_S2C_UpdateDanceGroupColorResult,
		MSG_S2C_UpdateDanceGroupBadgeResult,
		MSG_S2C_UpdateDanceGroupTitleNameResult,
		MSG_S2C_UpdateDanceGroupProfileResult,
		MSG_S2C_UpdateDanceGroupAnnouncementResult,
		MSG_S2C_DanceGroupRemoved,
		MSG_S2C_UpdateDanceGroupBaseInfo,
		MSG_S2C_AddDanceGroupMember,
		MSG_S2C_UpdateDanceGroupMember,
		MSG_S2C_RemoveDanceGroupMember,
		MSG_S2C_NotifyRequestEnterDanceGroup,
		MSG_S2C_NotifyRemoveRequestEnter,
		MSG_S2C_ReplyRequestEnterDanceGroupResult,
		MSG_S2C_ReplyRequestListEnterDanceGroupResult,

		//DanceGroup
		MSG_S2C_SelectDanceGroupTuanhuiResult,
		MSG_S2C_UnlockDanceGroupTuanhuiResult,
		MSG_S2C_DanceGroupActivityStarGetInfoResult,
		MSG_S2C_DanceGroupActivityStarGetPowerInfoResult,
		MSG_S2C_DanceGroupActivityStarAddPowerResult,
		MSG_S2C_DanceGroupActivityStarAddPowerNotify,
		MSG_S2C_DanceGroupActivityStarOpenPacketResult,
		MSG_S2C_DanceGroupActivityReset,
		MSG_S2C_DanceGroupShopGetInfoResult,
		MSG_S2C_DanceGroupShopExchangeResult,
		MSG_S2C_DanceGroupGetRecrodsResult,
		MSG_S2C_DanceGroupAddRecordsNotify,
		MSG_S2C_DanceGroupChangeDayNotify,
		MSG_S2C_DanceGroupBeRefuseRefresh,

		MSG_C2S_RequestContribute,
		MSG_C2S_RequestUpgradeGroup,
		MSG_S2C_RequestContributeResult,
		MSG_S2C_RequestUpgradeGroupResult,
		MSG_C2S_RequestChangeDanceGroupName,
		MSG_S2C_RequestChangeDanceGroupNameResult,

		MSG_S2C_UpdateDanceGroupAnnounce,
		MSG_S2C_UpdateDanceGroupProfile,
		MSG_S2C_DanceGroupInfo,
		MSG_C2S_InviteDanceGroupMember,
		MSG_S2C_InviteDanceGroupMemberResult,
		MSG_S2C_BeInvitedAsDanceGroupMember,
		MSG_C2S_AcceptDanceGroupMemberInvitation,
		MSG_S2C_AcceptDanceGroupMemberInvitationResult,

		MSG_S2C_SyncCeremonyConfig,				//同步仪式配置
		MSG_S2C_SyncCermonyApplyTimes,			//同步入团仪式已经申请次数

		MSG_C2S_RequestApplyCeremony,			//申请入团仪式
		MSG_S2C_RequestApplyCeremonyRes,		//申请入团仪式结果
		MSG_S2C_SyncCeremonyState,				//同步入团仪式状态

		MSG_C2S_RequestEnterCeremony,			//请求进入仪式房间
		MSG_S2C_EnterCeremonyRes,				//进入仪式房间结果

		MSG_C2S_RequestQuitCeremony,			//请求退出仪式房间
		MSG_S2C_QuitCeremonyRes,				//退出仪式房间结果

		MSG_C2S_RequestRefreshCeremony,			//请求刷新仪式房间
		MSG_S2C_RefreshCeremonyRes,				//刷新仪式房间结果

		MSG_C2S_RequestStartCeremony,			//请求开始仪式
		MSG_S2C_StartCeremonyRes,				//开始仪式结果
		MSG_S2C_StartCeremonyFail,				//自动开始仪式失败

		MSG_S2C_OtherPlayerEnterCeremony,		//其他玩家进入仪式房间
		MSG_S2C_OtherPlayerQuitCeremony,		//其他玩家退出仪式房间

		MSG_C2S_SetCeremonyRookie,				//设为仪式新人
		MSG_S2C_SetCeremonyRookieRes,			//设为仪式新人结果
		MSG_C2S_CancelCeremonyRookie,			//取消仪式新人
		MSG_S2C_CancelCeremonyRookieRes,		//取消仪式新人结果
		MSG_S2C_SyncCeremonyRookie,				//同步仪式新人信息

		MSG_C2S_KickCeremonyPlayer,				//将玩家请出仪式房间
		MSG_S2C_KickCeremonyPlayerRes,			//将玩家请出仪式房间结果

		MSG_C2S_SaveCeremonyIntruduction,		//保存自我介绍
		MSG_S2C_SaveCeremonyIntruductionRes,	//保存自我介绍结果

		MSG_C2S_RequestCeremonyRecord,			//请求仪式日志
		MSG_S2C_RequestCeremonyRecordRes,		//请求仪式日志结果

		MSG_C2S_RequestCeremonyPraise,			//请求点赞信息
		MSG_S2C_RequestCeremonyPraiseRes,		//请求点赞信息结果

		MSG_C2S_RequestPraiseRookie,			//给新人点赞
		MSG_S2C_RequestPraiseRookieRes,			//点赞结果
		MSG_S2C_SyncPraiseInfo,					//同步点赞信息
		
		MSG_S2C_SyncBigMamaConfig,				//同步广场舞大妈活动状态
		
		MSG_C2S_RequestBigMamaStageInfo,		//请求广场舞大妈数据
		MSG_S2C_SyncBigMamaStageInfo,			//同步广场舞大妈数据
		
		MSG_C2S_RequestFightBigMama,			//请求挑战大妈
		MSG_S2C_RequestFightBigMamaRes,			//请求挑战结果
		
		MSG_C2S_RequestGetBigMamaReward,		//请求大妈奖励
		MSG_S2C_RequestGetBigMamaRewardRes,		//请求奖励结果
		
		MSG_C2S_CompleteBigMamaGuide,			//完成大妈引导
		MSG_C2S_RequestBigMamaGuideReward,		//请求大妈引导奖励
		
		MSG_S2C_NoticeBigMamaState,				//通知大妈活动开启或关闭
		MSG_S2C_BigMamaIntroduce,				//同步大妈活动规则

		MSG_C2S_OpenDanceGroupActivity,			// 请求打开舞团活动界面
		MSG_S2C_OpenDanceGroupActivity,			// 收到打开舞团活动界面

		MSG_COUPLE_BEGIN = NetMsgTypeDef.MSG_TYPE_COUPLE,
		MSG_C2S_SendCoupleApply,
		MSG_S2C_SendCoupleApplySuc,
		MSG_S2C_SendCoupleApplyFail,

		MSG_S2C_GetCoupleApply,

		MSG_C2S_AcceptCoupleApply,
		MSG_S2C_AcceptCoupleRequestSuc,
		MSG_S2C_AcceptCoupleRequestFail,
		MSG_C2S_RefuseCoupleApply,

		MSG_S2C_GetCoupleReply,

		MSG_C2S_RequestSeparate,
		MSG_S2C_RequestSeparateSuc,
		MSG_S2C_RequestSeparateFail,
		MSG_S2C_NotifyBeSeparated,

		MSG_C2S_RequestStartWedding,
		MSG_S2C_RequestStartWeddingFail,
		MSG_S2C_NotifyWeddingStart,

		MSG_C2S_RequestDivorce,
		MSG_S2C_RequestDivorceSuc,
		MSG_S2C_RequestDivorceFail,
		MSG_S2C_NotifyDivorced,

		MSG_S2C_UpdateCoupleIntimacy,
		MSG_S2C_UpdateCoupleTitle,
		MSG_S2C_UpdateCoupleName,

		MSG_C2S_ChangeLoveDeclaration,
		MSG_S2C_ChangeLoveDeclarationSuc,
		MSG_S2C_ChangeLoveDeclarationFail,
		MSG_S2C_NotifyChangeLoveDeclaration,

		MSG_C2S_RequestWeddingRoomList,
		MSG_S2C_RequestWeddingRoomListRes,

		MSG_C2S_RequestCreateWeddingRoom,
		MSG_S2C_RequestCreateWeddingRoomFail,

		MSG_C2S_TryEnterWeddingRoom,
		MSG_S2C_TryEnterWeddingRoomFail,

		MSG_C2S_RequestEnterWeddingRoom,
		MSG_S2C_EnterWeddingRoomSuc,
		MSG_S2C_EnterWeddingRoomFail,

		MSG_C2S_RequestEnterMyOwnWeddingRoom,
		MSG_S2C_EnterMyOwnWeddingRoomFail,

		MSG_C2S_QuitWeddingRoom,
		MSG_S2C_QuitWeddingRoomSuc,
		MSG_S2C_QuitWeddingRoomFail,

		MSG_S2C_EnterWeddingPlayer,
		MSG_S2C_QuitWeddingPlayer,

		MSG_C2S_ChangeWeddingRoomPwd,
		MSG_S2C_ChangeWeddingRoomPwdSuc,
		MSG_S2C_ChangeWeddingRoomPwdFail,

		MSG_C2S_SetWeddingPlayerType,
		MSG_S2C_SetWeddingPlayerTypeSuc,
		MSG_S2C_SetWeddingPlayerTypeFail,
		MSG_S2C_NotifyChangeWeddingPlayerType,

		MSG_S2C_NotifyWeddingPerpare,
		MSG_S2C_AutoStartWeddingFail,

		MSG_C2S_KickWeddingPlayer,
		MSG_S2C_KickWeddingPlayerSuc,
		MSG_S2C_KickWeddingPlayerFail,

		MSG_S2C_WeddingRoomStateChanged,

		MSG_S2C_BlessingNotify,
		MSG_C2S_GetBlessingRankList,
		MSG_S2C_GetBlessingRankListResult,
		MSG_C2S_GetWeddingRecordList,
		MSG_S2C_GetWeddingRecordListResult,
		MSG_C2S_GetCoupleInfo,
		MSG_S2C_GetCoupleInfoResult,
		MSG_C2S_GetBlessingValue,
		MSG_S2C_GetBlessingValueResult,
		MSG_C2S_BlessingPair,
		MSG_S2C_BlessingPairSuc,
		MSG_C2S_MoneyBlessingPair,
		MSG_S2C_MoneyBlessingPairSuc,
		MSG_S2C_BlessingError,
		MSG_C2S_GetPersonInfo,
		MSG_S2C_GetPersonInfoResult,
		MSG_C2S_GetCoupleRedEnvelopeGiftInfo,
		MSG_S2C_GetCoupleRedEnvelopeGiftInfoResult,
		MSG_C2S_ActivateBlessCardStyle,			//请求激活样式
		MSG_S2C_ActivateBlessCardStyleResult,	//激活样式结果
		MSG_S2C_ActivateBlessCardStyleSyn,		//激活样式同步
		MSG_C2S_SelectBlessCardStyle,			//选择甜蜜印证样式
		MSG_S2C_SelectBlessCardStyleResult,		//选择甜蜜印证样式结果
		MSG_S2C_SelectBlessCardStyleSyn,		//选择样式同步消息
		MSG_C2S_OpenPhotoWallStyle,
		MSG_S2C_OpenPhotoWallStyleResult,		//解锁照片墙样式
		MSG_S2C_SelectPhotoWallStyle,
		MSG_S2C_SelectPhotoWallStyleResult,
		MSG_C2S_OpenPhotoFrame,
		MSG_S2C_OpenPhotoFrameResult,
		MSG_C2S_SetPhotoWallJurisdiction,
		MSG_S2C_SetPhotoWallJurisdictionResult,
		MSG_C2S_UploadPhoto,
		MSG_S2C_UploadPhotoResult,
		MSG_S2C_SyncUploadPhoto,//上传照片广播消息
		MSG_C2S_ViewPhotoWall,
		MSG_S2C_ViewPhotoWallResult,
		MSG_C2S_InformUploadPhotoResult,

		MSG_AMUSEMENT_BEGIN = NetMsgTypeDef.MSG_TYPE_AMUSEMENT,
		MSG_C2S_GetAmuseRoomList,
		MSG_S2C_GetAmuseRoomListRes,

		MSG_C2S_CreateAmuseRoom,
		MSG_S2C_CreateAmuseRoomSuc,
		MSG_S2C_CreateAmuseRoomFail,

		MSG_C2S_TryEnterAmuseRoom,
		MSG_S2C_AmuseRoomRequirePwd,

		MSG_C2S_EnterAmuseRoom,
		MSG_S2C_EnterAmuseRoomFail,

		MSG_C2S_QuitAmuseRoom,
		MSG_S2C_QuitAmuseRoomSuc,
		MSG_S2C_QuitAmuseRoomFail,

		MSG_C2S_ChangeAmuseRoomInfo,
		MSG_S2C_ChangeAmuseRoomInfoSuc,
		MSG_S2C_ChangeAmuseRoomInfoFail,

		MSG_C2S_AmuseKickPlayer,
		MSG_S2C_AmuseKickPlayerFail,

		MSG_S2C_AmuseEnterPlayer,
		MSG_S2C_AmuseQuitPlayer,

		MSG_C2S_AmusePromoteHost,
		MSG_S2C_AmusePromoteHostFail,
		MSG_S2C_AmusePromoteHostSuc,

		MSG_C2S_AmuseInvitePlayer,
		MSG_S2C_AmuseInvitePlayerSuc,
		MSG_S2C_AmuseInvitePlayerFail,

		MSG_C2S_AmuseInviteeResponse,
		MSG_S2C_AmuseInviteeFail,
		MSG_S2C_AmuseInviteeNotice,

		MSG_C2S_DoAmusementAction,
		MSG_S2C_DoAmusementActionSuc,
		MSG_S2C_DoAmusementActionFail,

		MSG_S2C_AmusementEventNotic,
		MSG_C2S_AmuseApplyMatch,

		MSG_C2S_PlayMakerSync,                       //PlayMaker消息同步
		MSG_S2C_PlayMakerBroadcast,

		MSG_C2S_SwitchToRoom,
		MSG_S2C_SwitchToRoomResult,
		MSG_S2C_SwitchToRoomTimeBegin,

		MSG_NOTIFICATION_BEGIN = NetMsgTypeDef.MSG_TYPE_NOTIFICATION,
		MSG_C2S_NoPushRatingNotification,
		MSG_S2C_PushRatingNotification,

		Msg_S2C_InfoCenterNotification,//信息中心;
		Msg_C2S_InfoCenterGetIDRes,
		Msg_S2C_InfoCenterGetIDRes,
		Msg_C2S_InfoCenterMarkReaded,

		MSG_PLATFORMPRIVILEGE_BEGIN = NetMsgTypeDef.MSG_TYPE_PLATFORMPRIVILEGE,

		MSG_C2S_GetUCVIPRewardStatue,//获取UCVIP特权领取状态    level
		MSG_S2C_GetUCVIPRewardStatueSuc,
		MSG_S2C_GetUCVIPRewardStatueFail,

		MSG_C2S_GetUCVIPRewardDetail, //获取特权详细信息
		MSG_S2C_GetUCVIPRewardDetailSuc,
		MSG_S2C_GetUCVIPRewardDetailFail,

		MSG_C2S_GainUCVIPReward,  //领取UCVIP特权奖励
		MSG_S2C_GainUCVIPRewardSuc,
		MSG_S2C_GainUCVIPRewardFail,

		MSG_LANTERN_BEGIN = NetMsgTypeDef.MSG_TYPE_LANTERN,
		MSG_C2S_GetLanternBaseInfo,
		MSG_S2C_GetLanternBaseInfoResult,

		MSG_C2S_RequestLanternStory,
		MSG_S2C_RequestLanternStoryRes,
		MSG_S2C_RequestLanternStoryFail,

		MSG_C2S_RequestInstanceZoneInfo,
		MSG_S2C_SendInstanceZoneInfo,
		MSG_S2C_SendInstanceZoneInfoFail,

		MSG_C2S_RequestZoneStageInfo,
		MSG_S2C_SendZoneStageInfo,
		MSG_S2C_SendZoneStageInfoFail,

		MSG_C2S_RequestStarReward,
		MSG_S2C_GetStarRewardSuc,
		MSF_S2C_GetStarRewardFail,

		MSG_C2S_RequestRubLantern,
		MSG_S2C_RubLanternSuc,
		MSF_S2C_RubLanternFail,

		MSG_S2C_GetLanternRequest,
		MSG_C2S_AnswerLanternRequest,
		MSG_S2C_AnswerLanternRequestFail,

		MSG_C2S_RequestLanternRankList,
		MSG_S2C_RequestLanternRankListRes,
		MSG_S2C_RefreshMyLanternRank,


		MSG_C2S_GetVipBuyCrystalInfo,		// 请求获取VIP购买体力信息
		MSG_S2C_GetVipBuyCrystalInfoResult,	// 获取VIP购买体力信息
		MSG_C2S_RequestBuyCrystal,
		MSG_S2C_BuyCrystalSuc,
		MSG_S2C_BuyCrystalFail,
		MSG_S2C_CrystalRecover,

		MSG_C2S_BuyStage,
		MSG_S2C_BuyStageSuc,
		MSG_S2C_BuyStageFail,

		MSG_C2S_RequestEnterStage,
		MSG_S2C_EnterStageSuc,
		MSG_S2C_EnterStageFail,
		MSG_S2C_LanternStageEnd,

		MSG_C2S_RequestMainStageList,
		MSG_S2C_RequestMainStageListRes,

		MSG_C2S_RequestThemedStageList,
		MSG_S2C_RequestThemedStageListRes,

		MSG_C2S_ExchangePieceReward,
		MSG_S2C_ExchangePieceRewardSuc,
		MSG_S2C_ExchangePieceRewardFail,

		MSG_S2C_RefreshFriendInviteInfo,

		// 魔法神灯体力互赠
		MSG_C2S_GetFriendContactInfo,		// 获取好友交互信息
		MSG_S2C_GetFriendContactInfoResult,	// 获取好友交互信息(返回)
		MSG_C2S_GiveVitToFriend,			// 赠送好友体力
		MSG_S2C_GiveVitToFriendResult,		// 赠送好友体力(返回)
		MSG_S2C_GiveVitToFriendNotice,		// 收到好友赠送体力提示
		MSG_C2S_GetVitFromFrined,			// 收取体力
		MSG_S2C_GetVitFromFrinedResult,		// 收取体力(返回)

		// 魔法神灯扫荡相关
		MSG_C2S_QuickFinishParclose,		// 关卡扫荡
		MSG_S2C_QuickFinishParcloseSuc,		// 关卡扫荡(成功)
		MSG_S2C_QuickFinishParcloseFail,	// 关卡扫荡(失败)
		
		//神灯大厅
		MSG_C2S_GetLanternRoomList,				// 请求神灯房间列表
		MSG_S2C_GetLanternRoomListRes,		// 获取神灯房间列表
		
		MSG_C2S_CreateLanternRoom,				// 创建神灯房间
		MSG_S2C_CreateLanternRoomSuc,			// 创建成功
		MSG_S2C_CreateLanternRoomFail,			// 创建失败
		
		MSG_C2S_TryEnterLanternRoom,			// 尝试进入神灯房间
		MSG_S2C_LanternRoomRequirePwd,			// 要求密码
		MSG_C2S_EnterLanternRoom,				// 进入神灯房间
		MSG_S2C_EnterLanternRoomFail,			// 进入失败
		
		MSG_C2S_ApplyLanternMatch,				// 匹配神灯跳舞
		MSG_C2S_CancelLanternMatch,				// 取消匹配
		MSG_S2C_PrepareLanternMatch,			// 开始匹配
		
		//神灯房间
		MSG_C2S_QuitLanternRoom,				// 退出神灯房间
		MSG_S2C_QuitLanternRoomSuc,				// 退出成功
		MSG_S2C_QuitLanternRoomFail,			// 退出失败

		MSG_S2C_EnterLanternPlayer,				// 神灯玩家进入
		MSG_S2C_QuitLanternPlayer,				// 神灯玩家退出
		MSG_S2C_QuitLanternMany,				// 神灯玩家退出（多人）
		
		MSG_C2S_ChangeLanternStageInfo,			// 修改神灯关卡
		MSG_S2C_ChangeLanternStageInfoSuc,		// 修改成功
		MSG_S2C_ChangeLanternStageInfoFail,		// 修改失败
		
		MSG_C2S_ChangeLanternTenTimesState,		// 开关十连挑战
		MSG_S2C_ChangeLanternTenTimesStateSuc,	// 开关成功
		MSG_S2C_ChangeLanternTenTimesStateFail,	// 开关失败
		
		MSG_C2S_RequestBuyTenTimes,				// 购买十连次数
		MSG_S2C_RequestBuyTenTimesSuc,			// 购买成功
		MSG_S2C_RequestBuyTenTimesFail,			// 购买失败
		MSG_S2C_NoticeTenTimesFreeChance,		// 免费十连次数
		
		MSG_S2C_UpdateLanternTargetScore,		// 更新神灯目标分数

		MSG_S2C_GetLanternSimpleInfo,			// 获取神灯简易信息
		
		MSG_C2S_SwitchToNormalRoom,				// 切换到普通房间
		MSG_S2C_SwitchToNormalRoomResult,		// 切换成功
		MSG_S2C_SwitchToNormalRoomTimeBegin,	// 切换倒计时
		
		MSG_C2S_GetLanternRoomPlayerInfo,		// 请求神灯房间玩家信息
		MSG_S2C_GetLanternRoomPlayerInfoRes,	// 获取神灯房间玩家信息
		
		MSG_C2S_EnterLanternSingleGame,			// 进入神灯单人挑战
		MSG_C2S_QuitLanternSingleGame,			// 离开神灯单人挑战
		
		//时尚值
		MSG_S2C_RefreshFashionValue,			//刷新时尚值
		MSG_S2C_OldPlayerFashionTips,			//老玩家时尚值提示
		MSG_C2S_RequestPlayerFashion,			// 获取时尚值
		MSG_S2C_RequestPlayerFashionResult,		// 获取时尚值结果

		MSG_S2C_GetLanternShopInfoResult,		//获取数据结果

		MSG_C2S_ExchangeLanternItem,			//请求兑换神灯商品
		MSG_S2C_ExchangeLanternResult,			//请求兑换结果

		MSG_S2C_LanternStageMultiEnd,			//神灯多人跳舞结算

		MSG_C2S_LanternUpdateGuide,				// 神灯引导更新

		//monthCard
		MSG_MONTHCARD_BEGIN = NetMsgTypeDef.MSG_TYPE_MONTHCARD,

		MSG_C2S_RequestMonthCard,//请求打开月卡界面
		MSG_S2C_RequestMonthCardRes,

		MSG_C2S_GetMonthCardRewards,//领取月卡奖励
		MSG_S2C_GetMonthCardRewardsSuc,
		MSG_S2C_GetMonthCardRewardsFail,

		MSG_C2S_DredgeMonthCard,//开通月卡功能
		MSG_S2C_DredgeMonthCardSuc,
		MSG_S2C_DredgeMonthCardFail,

		MSG_C2S_RenewMonthCard,//续费月卡功能
		MSG_S2C_RenewMonthCardSuc,
		MSG_S2C_RenewMonthCardFail,

		MSG_MONTHCARD_END,

		// Ranking
		MSG_RANKING_BEGIN = NetMsgTypeDef.MSG_TYPE_RANKING,

		MSG_C2S_GetRankingTypes,		// 请求排行榜主界面类型数据
		MSG_S2C_GetRankingTypesSuc,		// 处理主界面返回的数据
		MSG_C2S_GetRankingDatas,		// 请求明细榜数据
		MSG_S2C_GetRankingDatasSuc,		// 处理返回的明细榜数据
        MSG_C2S_GetRankingRewardInfo,   // 请求奖励规则界面奖励数据
        MSG_S2C_GetRankingRewardInfoSuc,// 获取奖励数据成功

		MSG_RANKING_END,

		//Activeness
		MSG_ACTIVENESS_BEGIN = NetMsgTypeDef.MSG_TYPE_ACTIVENESS,

		MSG_C2S_GetActivenessData,			// 获取活跃度系统数据
		MSG_S2C_GetActivenessDataRes,		// 获取活跃度系统数据的返回
		MSG_C2S_GetActivenessReward,		// 领取活跃度系统宝箱
		MSG_S2C_GetActivenessRewardRes,		// 领取活跃度系统宝箱的结果

		MSG_ACTIVENESS_END,

		MSG_MEDAL_BEGIN = NetMsgTypeDef.MSG_TYPE_MEDAL,

		MSG_C2S_GET_MEDALINFO,                  //请求勋章详细数据;
		MSG_S2C_GET_MEDALINFO,                  //返回勋章详细;

		MSG_C2S_MEDAL_CONDITIONINFO,            //请求勋章条件;
		MSG_S2C_MEDAL_CONDITIONINFO,            //返回请求勋章条件;

		MSG_C2S_MEDAL_HANDBOOK,                 //请求勋章图鉴;
		MSG_S2C_MEDAL_HANDBOOK,                 //返回勋章图鉴;

		MSG_C2S_MEDAL_EXCHANGE_INFO,            //请求勋章兑换数据;
		MSG_S2C_MEDAL_EXCHANGE_INFO,            //返回勋章兑换数据;

		MSG_C2S_MEDAL_SHOW,                     //请求设置勋章显示;
		MSG_S2C_MEDAL_SHOW,                     //返回设置勋章展示;

		MSG_C2S_MEDAL_EXCHANGE,                 //请求勋章兑换;
		MSG_S2C_MEDAL_EXCHANGE,                 //返回勋章兑换;

		MSG_C2S_MEDAL_ACT_INFO,                 //请求激活勋章数据;
		MSG_S2C_MEDAL_ACT_INFO,                 //返回激活勋章数据;

		MSG_C2S_GET_MEDAL,                      //请求领取勋章;
		MSG_S2C_GET_MEDAL,                      //返回领取勋章;

		MSG_S2C_MEDAL_INIT,                     //登录获得已经装备的勋章信息;

        MSG_S2C_GetCompleteMedal,              //得到可领取勋章ID列表

		MSG_C2S_GetMedalScore,					// 请求勋章积分
		MSG_S2C_GetMedalScore,					// 返回勋章积分

		MSG_C2S_MEDALSCORE_DESCRIBE,			// 请求勋章积分描述
		MSG_S2C_MEDALSCORE_DESCRIBE,			// 返回勋章积分描述

		MSG_HANDBOOK_BEGIN = NetMsgTypeDef.MSG_TYPE_HANDBOOK,

		MSG_C2S_RequestHandbookCostumeInfo,   //请求图鉴服饰信息
		MSG_S2C_RequestHandbookCostumeInfo,   //返回图鉴服饰信息

		MSG_C2S_RequestMyCollectHandbook,     //请求我的收藏
		MSG_S2C_RequestMyCollectHandbook,     //返回我的收藏

		MSG_C2S_RequestCostumePersonalInfo,   //请求图鉴服饰个人信息
		MSG_S2C_RequestCostumePersonalInfo,   //返回图鉴服饰个人信息

		MSG_C2S_RequestSetCostumeState,       //请求更改图鉴服饰个人信息，收藏，点赞等
		MSG_S2C_RequestSetCostumeState,       //返回更改图鉴服饰个人信息结果

		MSG_C2S_RequestClothEffectPersonalInfo, //请求光效分离个人信息
		MSG_S2C_RequestClothEffectPersonalInfo, //返回光效分离个人信息

		MSG_C2S_RequestSetClothEffectState,    //请求更改光效分离个人信息
		MSG_S2C_RequestSetClothEffectState,    //返回更改光效分离个人信息
		
		MSG_VEHICLE_BEGIN = NetMsgTypeDef.MSG_TYPE_VEHICLE,
		
		MSG_C2S_RequestUseVehicle,				//请求使用座驾
		MSG_S2C_RequestUseVehicleRes,			//使用座驾结果
		
		MSG_C2S_RequestCancelVehicle,			//请求取消座驾
		MSG_S2C_RequestCancelVehicleRes,		//取消座驾结果
		
		MSG_C2S_InviteCarpoolVehicle,			//发送共乘座驾邀请
		MSG_S2C_InviteCarpoolVehicleRes,		//发送邀请结果
		
		MSG_S2C_ReceiveCarpoolVehicle,			//收到他人的共乘邀请
		MSG_C2S_ReplyCarpoolVehicle,			//回复他人的共乘邀请
		MSG_S2C_ReplyCarpoolVehicleRes,			//回复邀请结果

		// 舞团秘境相关消息
		MSG_FAIRYLAND_BEGIN = NetMsgTypeDef.MSG_TYPE_FAIRYLAND,

		MSG_C2S_FAIRYLAND_CHECKISINRANK,				// 检测是否在昨日排行榜奖励是否可领
		MSG_S2C_FAIRYLAND_CHECKISINRANK,				// 返回是否在昨日排行榜奖励是否可领

		MSG_C2S_FAIRYLAND_STORAGEBOX_INFO,				// 请求储物箱碎片数量
		MSG_S2C_FAIRYLAND_STORAGEBOX_INFO,				// 返回储物箱碎片数量

		MSG_C2S_FAIRYLAND_RANK_CONFIG,					// 申请排行榜配置数据
		MSG_S2C_FAIRYLAND_RANK_CONFIG,					// 返回排行榜配置数据

		MSG_C2S_FAIRYLAND_CONFIG,						// 申请配置数据
		MSG_S2C_FAIRYLAND_CONFIG,						// 返回配置数据

		MSG_S2C_FairyLandResetZero,						// 每日0点刷新数据
		MSG_C2S_FairyLandBuyChanllengeTimes,			// 请求购买挑战次数
		MSG_S2C_FairyLandBuyChanllengeTimes,			// 返回购买挑战次数

		MSG_C2S_FAIRYLAND_CHAPTER_INFO,					// 申请关卡数据
		MSG_S2C_FAIRYLAND_CHAPTER_INFO,					// 返回关卡数据

		MSG_S2C_FAIRYLAND_END_DANCE_INFO,				// 舞蹈结束数据

		MSG_S2C_FAIRYLAND_REWARD,						// 舞团秘境获得奖励(昨日排行榜,秘境结算)

		MSG_S2C_FAIRYLAND_END_RANDOMREWARD,				// 舞蹈结束随机奖励

		MSG_C2S_FAIRYLAND_RANK,							// 申请排行榜数据
		MSG_S2C_FAIRYLAND_RANK,							// 返回排行榜数据

		MSG_C2S_FAIRYLAND_GET_YESTERDAY_RANK_REWARD,	// 申请领取昨日排行榜奖励

		MSG_S2C_FAIRYLandChanllegeTimesNotify,			// 挑战次数更新
		MSG_S2C_FAIRYLandCurrentChapterNotify,			// 更新关卡数据
		MSG_S2C_FAIRYLandChapterScoreNotify,			// 关卡更新
		MSG_S2C_FAIRYLandStateNotify,					// 秘境状态更新

		MSG_C2S_GetFairyLandRoomList,					// 秘境房间列表
		MSG_S2C_GetFairyLandRoomListResult,				// 秘境房间列表返回

		/// <summary>
		/// 秘境房间消息
		/// </summary>
		MSG_NEWROOM_BEGIN = NetMsgTypeDef.MSG_TYPE_NEWROOM,
		MSG_C2S_NewCreateRoom,			// 创建房间
		MSG_C2S_CreateFairyLandGuideRoom,	// 创建引导房间
		MSG_C2S_NewTryEnterRoom,		// 进入房间
		MSG_C2S_NewQuitRoom,			// 退出房间
		MSG_C2S_NewPromoteRoomHost,		// 改变房主
		MSG_C2S_NewKickPlayer,			// 踢人
		MSG_C2S_NewChangeRoleRoomState,	// 房间内人物状态变更
		MSG_C2S_NewChangeRoomPosState,	// 房间位置编程
		MSG_S2C_NewCreateRoomSuc,		// 创建房间成功

		MSG_C2S_GroupAmuse_CreateRoom,
		MSG_S2C_GroupAmuse_CreateRoomSuc,
		MSG_S2C_GroupAmuse_CreateRoomFail,

		MSG_C2S_GroupAmuse_TryEnterRoom,
		MSG_S2C_GroupAmuse_RequirePwd,
		MSG_C2S_GroupAmuse_EnterRoom,
		MSG_S2C_GroupAmuse_EnterRoomFail,
		MSG_S2C_GroupAmuse_EnterPlayer,

		MSG_C2S_GroupAmuse_QuitRoom,
		MSG_S2C_GroupAmuse_QuitRoomSuc,
		MSG_S2C_GroupAmuse_QuitRoomFail,
		MSG_S2C_GroupAmuse_QuitPlayer,

		MSG_C2S_GroupAmuse_KickPlayer,
		MSG_S2C_GroupAmuse_KickPlayerFail,

		MSG_C2S_GroupAmuse_ChangeRoomInfo,
		MSG_S2C_GroupAmuse_ChangeRoomInfoSuc,
		MSG_S2C_GroupAmuse_ChangeRoomInfoFail,

		MSG_C2S_GroupAmuse_PromoteHost,
		MSG_S2C_GroupAmuse_PromoteHostSuc,
		MSG_S2C_GroupAmuse_PromoteHostFail,

		MSG_C2S_GroupAmuse_Posing,
		MSG_C2S_GroupAmuse_CancelPosing,
		MSG_S2C_GroupAmuse_PosedSuc,
		MSG_S2C_GroupAmuse_PosedFail,

		MSG_C2S_GroupAmuse_DoDevice,
		MSG_S2C_GroupAmuse_DoDeviceSuc,
		MSG_S2C_GroupAmuse_DoDeviceFail,

		MSG_C2S_TShowBackroom_GetList,
		MSG_S2C_TShowBackroom_GetListRes,

		MSG_C2S_TShowBackroom_GetTeamID,
		MSG_S2C_TShowBackroom_GetTeamIDRes,

		/// <summary>
		/// 玩家状态信息同步消息
		/// </summary>
		MSG_NEWROOMBROADCAST_BEGIN = NetMsgTypeDef.MSG_TYPE_NEWROOMBROADCAST,
		MSG_S2C_RefreshItemForOtherNew,					// 原始消息 MSG_S2C_RefreshItemForOther
		MSG_S2C_RefreshEffectChangeToOthersNew,			// 原始消息 MSG_S2C_RefreshEffectChangeToOther
		MSG_S2C_RefreshColorProgressToOthersNew,		// 原始消息 MSG_S2C_RefreshSevenColorInfoForOther
		MSG_S2C_ChangeRoleNameResultNew,				// 原始消息 MSG_S2C_ChangeRoleNameRes
		MSG_S2C_NotifyRoleVIPInfoChangedNew,			// 原始消息 MSG_S2C_NotifyRoleVIPInfoChanged
		MSG_S2C_UpdateDanceGroupNameNew,				// 原始消息 MSG_S2C_ChangeDanceGroupName
		MSG_S2C_UpdateDanceGroupTitleNew,				// 原始消息 MSG_S2C_ChangeDanceGroupPosition
		MSG_S2C_ChangeDanceGroupBadgeOrEffectResultNew,	// 原始消息 MSG_S2C_SaveDanceGroupTuanhuiResult
		MSG_S2C_SyncMedalInfoToOthersNew,				// 原始消息 MSG_S2C_SYNC_MEDAL_INFO_TO_OTHERS	//场景中公告,装备勋章;
		MSG_S2C_SYNC_HorseDataNew,						// 原始消息 MSG_S2C_SyncPlayerVehicleState		//同步玩家座驾状态
		MSG_S2C_ResetGeneNew,							// 原始消息 MSG_S2C_ResetGene
		MSG_S2C_DelGeneNew,								// 原始消息 MSG_S2C_DelGene
		MSG_S2C_AddGeneNew,								// 原始消息 MSG_S2C_AddGene

		#region TShow

		MSG_CATWALK_BEGIN = NetMsgTypeDef.MSG_TYPE_CATWALK,
		MSG_C2S_TShow_Enter,		// 进入T台秀主界面
		MSG_S2C_TShow_EnterRes,

		MSG_C2S_TShow_GetStageInfo,			// 获取当前阶段的信息;
		MSG_S2C_TShow_GetStageInfoRes,

		MSG_C2S_TShow_GetFightingTime,		// 获取比赛开始时间;
		MSG_S2C_TShow_GetFightingTimeRes,
		
		MSG_C2S_TShow_RequestPeriodReward,	//赛期奖励
		MSG_S2C_TShow_RequestPeriodRewardRes,

		MSG_C2S_TShow_RequestSeasonReward,	//赛季奖励
		MSG_S2C_TShow_RequestSeasonRewardRes,

		MSG_C2S_TShow_GetReward,		// 领奖
		MSG_S2C_TShow_GetRewardRes,

		MSG_C2S_TShow_GetRankList,		//排行榜列表
		MSG_S2C_TShow_GetRankListRes,

		MSG_C2S_TShow_SignUp,			//报名;
		MSG_S2C_TShow_SignUpRes,
		MSG_S2C_TShow_SignedUp,
		MSG_S2C_TShow_SignUpReminder,	// 开始报名的消息盒子;

		MSG_C2S_TShow_EnterBackroom,	// 进入后台房间;
		MSG_S2C_TShow_EnterBackroomFail,// 进入房间失败;

		//T台秀PK部分
		MSG_S2C_TShow_NotifyEnterBackroom,

		MSG_C2S_TShow_GetSavedCloth,
		MSG_S2C_TShow_GetSavedClothRes,

		MSG_C2S_TShow_SaveCloth,
		MSG_S2C_TShow_SaveClothRes,

		MSG_C2S_TShow_GetTopList,		// 前25名排名;
		MSG_S2C_TShow_GetTopListRes,

		MSG_C2S_TShow_ExitFighting,
		MSG_S2C_TShow_StartFighting,
		MSG_S2C_TShow_EndFighting,
		
		MSG_C2S_TShow_RequestFightData,
		MSG_S2C_TShow_RequestFightDataRes,

		MSG_C2S_TShow_SyncLoading,
		MSG_S2C_TShow_LoadingTimeout,

		MSG_S2C_TShow_StartRoleRound,		// 开始指定回合

		MSG_C2S_TShow_SyncRoleScore,		//同步戳屏幕的命中数量;
		MSG_S2C_TShow_NotifyRoleScore,

		MSG_S2C_TShow_NotifyRoundScore,
		MSG_S2C_TShow_NotifyFightEnd,	//通知舞团比赛结束;

		MSG_C2S_TShow_GetPhotoInfo,		// 进入照片场景;
		MSG_S2C_TShow_GetPhotoInfoRes,

		#endregion

		#region StarShow

		MSG_STARSHOW_BEGIN = NetMsgTypeDef.MSG_TYPE_STARSHOW,

		MSG_S2C_StarShow_SyncStaticData,
		MSG_S2C_StarShow_SyncShopData,
		MSG_S2C_StarShow_SyncRefresh,

		MSG_C2S_StarShow_GetAgentInfo,		// 请求经济人数据
		MSG_S2C_StarShow_GetAgentInfoRes,	// 返回经纪人数据

		MSG_C2S_StarShow_ModelUpgrade,
		MSG_S2C_StarShow_ModelUpgradeRes,

		MSG_C2S_StarShow_SendGift,
		MSG_S2C_StarShow_SendGiftRes,

		MSG_C2S_StarShow_GetHeadlineInfo,
		MSG_S2C_StarShow_GetHeadlineInfoRes,

		MSG_C2S_StarShow_GetFanRewards,			// 请求粉丝奖励信息
		MSG_S2C_StarShow_GetFanRewardsRes,		// 获得粉丝奖励信息

		MSG_C2S_StarShow_GetRankRewardInfo,		// 请求排行奖励信息
		MSG_S2C_StarShow_GetRankRewardInfoRes,	// 请求排行奖励信息

		MSG_C2S_StarShow_GetSignUpInfo,
		MSG_S2C_StarShow_GetSignUpInfoRes,

		MSG_C2S_StarShow_GetRoleSignUpInfo,
		MSG_S2C_StarShow_GetRoleSignUpInfoRes,

		MSG_C2S_StarShow_GetIdols,		// 获取明星秀场偶像信息
		MSG_S2C_StarShow_GetIdolsRes,

		MSG_C2S_StarShow_DeleteIdol,	//明星秀场删除偶像
		MSG_S2C_StarShow_DeleteIdolRes,

		MSG_C2S_StarShow_GetSuggestIdol,	//明星秀场推荐偶像
		MSG_S2C_StarShow_GetSuggestIdolRes,

		MSG_C2S_StarShow_GetFans,		//明星秀场获取粉丝信息
		MSG_S2C_StarShow_GetFansRes,

		MSG_C2S_StarShow_Support,		//点赞、送花
		MSG_S2C_StarShow_SupportRes,	//

		MSG_C2S_StarShow_SaveCloth,
		MSG_S2C_StarShow_SaveClothRes,

		MSG_S2C_StarShow_SyncRoleData,		// 同步明星秀场玩家基本信息

		MSG_S2C_StarShow_SyncAllFriendsData,
		MSG_S2C_StarShow_UpdateFriendData,

		MSG_C2S_StarShow_Guest,				// 请求客串
		MSG_S2C_StarShow_GuestRes,			// 请求客串结果

		MSG_C2S_StarShow_GuestAll,
		MSG_S2C_StarShow_GuestAllRes,

		MSG_C2S_StarShow_Receive,			// 请求接待
		MSG_S2C_StarShow_ReceiveRes,		// 请求接待结果

		MSG_C2S_StarShow_ReceiveAll,
		MSG_S2C_StarShow_ReceiveAllRes,

		MSG_C2S_StarShow_Invite,			// 邀请成为圈内伙伴
		MSG_S2C_StarShow_InviteRes,			// 邀请返回结果

		MSG_C2S_StarShow_InviteRefused,		// 拒绝邀请
		MSG_S2C_StarShow_InviteRefusedRes,	// 拒绝邀请结果

		MSG_C2S_StarShow_InviteAccepted,
		MSG_S2C_StarShow_InviteAcceptedRes,
		MSG_S2C_StarShow_NotifyAccepted,

		MSG_C2S_StarShow_Terminate,
		MSG_S2C_StarShow_TerminateRes,

		MSG_C2S_StarShow_GetAnnouncements,	// 获取通告
		MSG_S2C_StarShow_AnnouncementsList,	// 获取通告

		MSG_C2S_StarShow_Refresh,			// 刷新通告
		MSG_S2C_StarShow_RefreshFailed,		// 刷新通告失败

		MSG_C2S_StarShow_Execute,
		MSG_S2C_StarShow_ExecuteRes,

		MSG_C2S_StarShow_GetMyAnnouncement,
		MSG_S2C_StarShow_GetMyAnnouncementRes,

		MSG_C2S_StarShow_QuickFinish,
		MSG_S2C_StarShow_QuickFinishRes,

		MSG_C2S_StarShow_GetAnnRewards,
		MSG_S2C_StarShow_GetAnnRewardsRes,

		MSG_C2S_StarShow_CancelLockedAnn,		// 请求取消锁定通告
		MSG_S2C_StarShow_CancelLockedAnnRes,	// 取消锁定通告结果

		MSG_C2S_StarShow_ExchangeItem,
		MSG_S2C_StarShow_ExchangeItemRes,


		MSG_C2S_StarShow_GetPopularityInfos,	//获取人气指数奖励
		MSG_S2C_StarShow_GetPopularityInfosRes,	//

		MSG_C2S_StarShow_GetPopularityRewards, //领取人气奖励
		MSG_S2C_StarShow_GetPopularityRewardsRes,

		MSG_S2C_StarShow_SyncActions,

		MSG_C2S_StarShow_UnlockAction,//请求解锁动作
		MSG_S2C_StarShow_UnlockActionRes,

		MSG_C2S_StarShow_SelectActions,//选择的动作列表

		#endregion
	}
}