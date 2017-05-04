using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Data.Item;
//using LoveDance.Client.Network.VIP;
//using LoveDance.Client.Network.Player;
//using LoveDance.Client.Network.Gene;
//using LoveDance.Client.Data.Lantern;
//using LoveDance.Client.Data.Cloth;
//using LoveDance.Client.Data.GeneEffect;
using LoveDance.Client.Data;
//using LoveDance.Client.Data.Setting;
using LoveDance.Client.Network.Item;
//using LoveDance.Client.Logic.VIP;
using LoveDance.Client.Logic.Role;
//using LoveDance.Client.Network.Medal;
//using LoveDance.Client.Network.DanceGroup;
//using LoveDance.Client.Logic.Medal;
using LoveDance.Client.Common.Messengers;
//using LoveDance.Client.Logic.Tips;
//using LoveDance.Client.Data.Tips;
using LoveDance.Client.Logic;
//using LoveDance.Client.Data.Quest;
//using LoveDance.Client.Data.Vehicle;
//using LoveDance.Client.Logic.Room;
using LoveDance.Client.Logic.Ress;
//using LoveDance.Client.Logic.Itembag;
//using LoveDance.Client.Data.DanceGroup;
//using LoveDance.Client.Logic.Loading;
//using LoveDance.Client.Network.Room;
//using LoveDance.Client.Network.Wedding;
//using LoveDance.Client.Logic.Wedding;
using LoveDance.Client.Data.Setting;

/// <summary>
/// 玩家对象实体类;
/// 
/// change by 黄文叶 time 20151010;
/// 增加NewPlayer的基类PlayerBase. NewPlayer作为PlayerBase的接口实现类.;
/// for解耦需要把NewPlayer的层级降到DLL层;
/// 采用中转类的方法进行解耦。在DLL中存在一个全局可用的对象PlayerBase作为基类。在DLL外部实现类接口内容;
/// 创建玩家对象时, 创建NewPlayer的对象,保存为PlayerBase。因为Base类中含有所有给外部调用的接口。多态方式实现解耦; 
/// 注:;
/// RoleBone较为特殊,是绑定脚本,外部会直接使用Bone的tranform.;
/// 使用时,需要获取PlayerBase对象转换成NewPlayer后,获取成员RoleBone;
/// 其余全都使用PlayerBase使用函数进行操作;
/// </summary>
/// <see cref="PlayerBase">详细可以联合PlayerBase注释</see>
public class NewPlayer : PlayerBase
{
	struct SClothEffect
	{
		public uint m_EffectId;
		public uint m_ColorId;
	}

	public override bool IsToShow
	{
		get
		{
			return m_bToShow;
		}
		set
		{
			m_bToShow = value;

			if(!m_bToShow)
			{
				Animation[] ani = RoleBody.GetComponentsInChildren<Animation>(true);
				for (int i = 0; i < ani.Length; ++i)
				{
					if (ani[i].gameObject != RoleBody)
					{
						ani[i].cullingType = AnimationCullingType.AlwaysAnimate;
					}
				}
			}

			RoleBody.SetActive(m_bToShow);

			for (int i = 0; i < m_arBodyExtraSpEffect.Length; ++i)
			{
				if (m_arBodyExtraSpEffect[i] != null)
				{
					m_arBodyExtraSpEffect[i].SetActive(m_bToShow);
				}
			}
		}
	}

	public override bool BodyCreated
	{
		get
		{
			return m_bBodyCreated;
		}
	}

	public override GameObject RoleBody
	{
		get
		{
			if (m_RoleBody == null)
			{
				m_RoleBody = (GameObject)Instantiate(BoneLoader.GetPlayerBone());
				m_RoleBody.transform.parent = cachedTransform;
				m_RoleBody.transform.localScale = Vector3.one;
				m_RoleBody.transform.localRotation = Quaternion.Euler(Vector3.zero);
				m_RoleBody.transform.localPosition = Vector3.zero;

				m_RoleBone = m_RoleBody.GetComponentInChildren<PlayerBone>();
				mRoleBodyTrans = m_RoleBody.transform;
			}

			return m_RoleBody;
		}
		set
		{
			m_RoleBody = value;
		}
	}

	public PlayerBone RoleBone
	{
		get
		{
			return m_RoleBone;
		}
		set
		{
			m_RoleBone = value;
		}
	}

	public override PlayerPivot RolePivot
	{
		get
		{
			return m_RolePivot;
		}
	}

	public override PlayerAttr RoleAttr
	{
		get
		{
			return m_RoleAttr;
		}
	    set
		{
			m_RoleAttr = value;
		}
	}

	public override PlayerTransform RoleTransform
	{
		get
		{
			if (m_RoleTransfrom == null)
			{
				m_RoleTransfrom = new PlayerTransform();
			}
			return m_RoleTransfrom;
		}
		set
		{
			m_RoleTransfrom = value;
		}
	}

	public override PlayerItem RoleItem
	{
		get
		{
			return m_RoleItem;
		}
		set
		{
			m_RoleItem = value;
		}
	}

	public override PlayerGene RoleGene
	{
		get
		{
			return m_RoleGene;
		}
		set
		{
			m_RoleGene = value;
		}
	}

	public override PlayerStyle RoleStyle
	{
		get
		{
			return m_RoleStyle;
		}
	}

	public override PlayerMoveType PlayerMoveType
	{
		get
		{
			return m_PlayerMoveType;
		}
		set
		{
			m_PlayerMoveType = value;
		}
	}
    
	public override PlayerStyleType CurrentStyle
	{
		get
		{
			return this.m_CurStyle;
		}
		set
		{
			if (m_CurStyle != value)
			{

				m_CurStyle = value;
				switch (m_CurStyle)
				{
					
					case PlayerStyleType.Create:
						m_RoleStyle = cachedGameObject.AddComponent<PlayerCreateStyle>();
						break;
					
				}

                if (m_RoleStyle != null)
				{
					m_RoleStyle.ReAwake();
				}
				
			}
		}
	}

	public override Vector3 RootBonePos
	{
		get { return m_RoleBone.m_RootBone.localPosition; }
	}

	public override bool IgnoreSync
	{
		get
		{
			return m_IgnoreSync;
		}
		set
		{
			m_IgnoreSync = value;
		}
	}

	bool m_bToShow = false;
	bool m_bBodyCreated = false;
	bool m_bHandling = false;
	bool m_buseLightProb = true;
	byte m_NpcID = 0;

	GameObject m_RoleBody = null;
	PlayerBone m_RoleBone = null;
	PlayerPivot m_RolePivot = null;
    //PlayerTitleBase m_RoleTitle = null;
    //ChatBubble m_RoleChatBubble = null;
	PlayerAttr m_RoleAttr = null;
	PlayerItem m_RoleItem = null;
	PlayerGene m_RoleGene = null;
	PlayerStyle m_RoleStyle = null;
	PlayerTransform m_RoleTransfrom = null;
	PlayerMoveType m_PlayerMoveType = PlayerMoveType.None;
	private Transform mRoleBodyTrans = null;
	//private PlayerTitleSystem m_RoleTitleSystem=null;

	private Transform RoleBodyTrans
	{
		get
		{
			if(mRoleBodyTrans == null)
			{
				if (RoleBody != null)
				{
					mRoleBodyTrans = RoleBody.transform;
				}

			}

			return mRoleBodyTrans;
		}
	}

	//MovementMotor m_MovementMotor = null;

	GameObject m_UIRoleCamera = null;

	public override GameObject UIRoleCamera
	{
		get
		{
			return this.m_UIRoleCamera;
		}
		set
		{
			m_UIRoleCamera = value;
		}
	}

	Material m_LegMaterial = null;
	GameObject m_UIFaceCamera = null;
	GameObject m_UIHalfBodyCamera = null;
	GameObject m_UIBustCamera = null;

	PlayerStyleType m_CurStyle = PlayerStyleType.None;
	PlayerStyleType m_LastStyle = PlayerStyleType.None;

	RoleBodyLoader[] m_arBodyLoader = new RoleBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
	List<GameObject>[] m_arBodyRenderList = new List<GameObject>[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];

	uint m_nSkinItem = 0;

	GameObject[] m_arBodyExtra = new GameObject[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
	Vector3[] m_arBodyExtraScale = new Vector3[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
	GameObject[] m_arBodyExtraSpEffect = new GameObject[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
	Dictionary<ParticleSystem, float>[] m_dicClothParticleEffect = new Dictionary<ParticleSystem, float>[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];

	//EffectElem[] m_arBodyEffect = new EffectElem[(int)EffectPart.Max];//身体部位特效
	GameObject m_DefaultFootEffectGo = null;
	//EffectBase m_effectElemVIP = new EffectBase();

	float m_InitScale = 1.0f;

	//EnchantLoader[] m_arEnchantLoader = new EnchantLoader[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];

	Dictionary<string, List<GameObject>> m_DicEnchantGO = new Dictionary<string, List<GameObject>>();
	SClothEffect[] m_arClothEffect = new SClothEffect[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];

	//EnchantLoader m_GroupEffectLoader = null;
	uint m_GroupEffecId = 0;
	bool m_bClothEffectHandling = false;
	bool m_bClothEffectProcess = false;

	//座驾
	//VehicleElem m_VehicleElem = new VehicleElem();

	private Dictionary<ItemCloth_Type, RoleBodyLoader> m_dicAsyncBodyLoader = new Dictionary<ItemCloth_Type, RoleBodyLoader>();// 下载中的RoleBodyLoader;

	//private XQLookAtForward mLookAt = null;

	private bool m_IgnoreSync = false;

	private Transform mTempTrans = null;

	void Awake()
	{

		//NetObserver.AddNetMsgProcessor(GameMsgType.MSG_S2C_SyncTitleToOthers, OnSyncTitleToOther);
	}

	void Update()
	{
        //if (RoleItem != null)
        //{
        //    RoleItem.UpdateEx();
        //}
        //if (RoleGene != null)
        //{
        //    RoleGene.UpdateEX();
        //}
	}


	private bool CheckHasBodyChange(List<ItemCloth_Type> releaseTypeList)
	{
		bool hasChange = false;
		if (releaseTypeList != null)
		{
			for (int i = 0; i < releaseTypeList.Count; i++)
			{
				if (m_arBodyLoader[(int)releaseTypeList[i]] != null)
				{
					hasChange = true;
					break;
				}
			}
		}
		return hasChange;
	}

	/// <summary>
	/// 开始下载 需要显示的服饰资源;
	/// </summary>
	private void StartDownLoadCloth()
	{
		if (m_dicAsyncBodyLoader.Count > 0)
		{
			List<RoleBodyLoader> listRoleBodyLoader = new List<RoleBodyLoader>();

			foreach (KeyValuePair<ItemCloth_Type, RoleBodyLoader> kv in m_dicAsyncBodyLoader)
			{
				listRoleBodyLoader.Add(kv.Value);
			}

			for (int i = 0; i < listRoleBodyLoader.Count; i++)
			{
				if (listRoleBodyLoader[i] != null)
				{
					listRoleBodyLoader[i].LoadBodyAsync(OnLoadRoleBodyFinish);
				}
				else
				{
					Debug.Log("NewPlayer StartDownLoadCloth, RoleBodyLoader is null.");
				}
			}
			listRoleBodyLoader.Clear();
		}
	}

	/// <summary>
	/// 释放下载中的RoleBodyLoader;
	/// </summary>
	private void ReleaseAsyncBodyLoader(List<ItemCloth_Type> clothTypeList)
	{
		for (int i = 0; i < clothTypeList.Count; i++)
		{
			ReleaseAsyncBodyLoader(clothTypeList[i]);
		}
	}

	private void ReleaseAsyncBodyLoader(ItemCloth_Type clothTypeList)
	{
		if (m_dicAsyncBodyLoader.ContainsKey(clothTypeList))
		{
			m_dicAsyncBodyLoader[clothTypeList].Release();
			m_dicAsyncBodyLoader.Remove(clothTypeList);
		}
	}

	/// <summary>
	/// 换装无加载过程 for回调;
	/// </summary>
	private void ChangeNetCloth(RoleBodyLoader bodyLoader)
	{
		List<ItemCloth_Type> releaseTypeList = null;
		if (bodyLoader != null && bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_Transform)
		{
			//如果新的服饰为变身服饰 需要卸载其他所有部位
			releaseTypeList = new List<ItemCloth_Type>();
			for (ItemCloth_Type clothType = ItemCloth_Type.ItemCloth_Type_Hair; clothType < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++clothType)
			{
				if (clothType != ItemCloth_Type.ItemCloth_Type_Transform)
				{
					releaseTypeList.Add(clothType);
				}
			}
		}
		
		ProcessBody(new List<RoleBodyLoader>() { bodyLoader });
		
	}


	public override IEnumerator CreateMainPlayerPhysics(PlayerStyleType curStyle)
	{
		IsToShow = true;

        IEnumerator itor = CreateBody(PhysicsType.Player);
		while (itor.MoveNext())
		{
			yield return null;
		}
        
		CurrentStyle = curStyle;
	}

	public override IEnumerator CreatePhysics(bool bTitled, PhysicsType pType)
	{

        IEnumerator itor = CreateBody(pType);
		while (itor.MoveNext())
		{
			yield return null;
		}

	}

	public override IEnumerator CreatePhysics(bool bTitled, PhysicsType pType, byte npcID)
	{
		
		IEnumerator itor = null;
		itor = CreateBody(pType);
		while (itor.MoveNext())
		{
			yield return null;
		}


	}

	public override IEnumerator CreatePhysics(bool bTitled, Dictionary<ItemCloth_Type, string> cloths)
	{

		RoleBodyLoader[] arrAttachLoader = new RoleBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
		if (cloths != null && cloths.Count > 0)
		{
			foreach (ItemCloth_Type itemType in cloths.Keys)
			{
				string clothName = cloths[itemType];
				AddRoleBody(itemType, GetRoleBodyLoader(clothName, itemType, null), ref arrAttachLoader);
			}
		}
		else
		{
			bool bEuipSuit = AddEquipBody(ref arrAttachLoader);
			AddDefaultBody(bEuipSuit, ref arrAttachLoader);
		}

		//区分 是加载or下载 队列;
		List<RoleBodyLoader> attachLoaderList = GetAttachRoleBodyLoaderList(arrAttachLoader);

		IEnumerator itor = LoadAttachRoleBody(attachLoaderList);
		while (itor.MoveNext())
		{
			yield return null;
		}
		
		
		ProcessBody(attachLoaderList);

		m_bBodyCreated = true;
		SendMessage("OnBodyCreated", null, SendMessageOptions.DontRequireReceiver);

		StartDownLoadCloth();
	}

    
	public override void ChangeCurrentLayer(GameLayer targetLayer, GameLayer srcLayer)
	{
		m_CurrentPlayerLayer = targetLayer;

		if (!m_bBodyCreated)
			return;

		CommonFunc.SetLayer(cachedGameObject, m_CurrentPlayerLayer, true, srcLayer);
	}
        
	public override IEnumerator DestroyPlayer()
	{
		IsToShow = false;
		while (m_bHandling)
		{
			yield return null;
		}

		DestroyPlayerImmediate();
	}

	public override IEnumerator DestroyPlayerDelay(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		DestroyPlayerImmediate();
	}

	/// <summary>
	/// 强制销毁PlayerPlayer，不需要判断m_bHandling
	/// 解决切换界面时，Coroutine未结束意外中断无法销毁
	/// </summary>
	public override void DestroyPlayerImmediate()
	{
		StopAllCoroutines();
		m_RoleStyle = null;
		ReleaseLoadingBodyLoader();


		GameObject.Destroy(gameObject);

		for (int i = 0; i < (int)ItemCloth_Type.ItemCloth_Type_MaxNumber; i++)
		{
			ReleaseBodyLoader((ItemCloth_Type)i);
			ReleaseAsyncBodyLoader((ItemCloth_Type)i);
		}


		RemovePlayerFromControl();
	}

	bool BeginHandling()
	{
		if (m_bHandling)
		{
			return false;
		}

		m_bHandling = true;
		return true;
	}

	void EndHandling()
	{
		m_bHandling = false; ;
	}

    bool AddEquipBody(ref RoleBodyLoader[] arrAttachLoader)
    {
        bool bEquipSuit = false;

        if (RoleItem != null)
        {
            for (ItemCloth_Type pos = ItemCloth_Type.ItemCloth_Type_Hair; pos < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++pos)
            {
                CEquipItem equitItem = RoleItem.GetCurrentEquip(pos);
                if (equitItem != null)
                {
                    AddRoleBody((ItemCloth_Type)equitItem.GetClothPos(),
                        GetRoleBodyLoader(equitItem.ClothResource, (ItemCloth_Type)equitItem.GetClothPos(), equitItem.ExcludeItems),
                        ref arrAttachLoader);

                    if (equitItem.IsItemCloth_Type_Suit())
                    {
                        bEquipSuit = true;
                    }
                }
            }
        }

        return bEquipSuit;
    }


	private IEnumerator CreateBody(PhysicsType pType)
	{
        RoleBodyLoader[] arrAttachLoader = new RoleBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];
		
		if (pType == PhysicsType.Player)
		{
			bool bEuipSuit = AddEquipBody(ref arrAttachLoader);
			AddDefaultBody(bEuipSuit, ref arrAttachLoader);
		}
		

		//区分 是加载or下载 队列;
		List<RoleBodyLoader> attachLoaderList = GetAttachRoleBodyLoaderList(arrAttachLoader);

		IEnumerator itor = LoadAttachRoleBody(attachLoaderList);
		while (itor.MoveNext())
		{
			yield return null;
		}
		
		bool tempHasTransform = false;
		for (int i = 0; i < attachLoaderList.Count; ++i)
		{
			RoleBodyLoader loader = attachLoaderList[i];
			if (loader != null && loader.ClothType == ItemCloth_Type.ItemCloth_Type_Transform)
			{
				tempHasTransform = true;
				break;
			}
		}
		
		List<ItemCloth_Type> releaseTypeList = null;
		if (tempHasTransform)
		{
			//如果玩家在变身状态 需要卸载其他所有部位
			releaseTypeList = new List<ItemCloth_Type>();
			for (ItemCloth_Type clothType = ItemCloth_Type.ItemCloth_Type_Hair; clothType < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++clothType)
			{
				if (clothType != ItemCloth_Type.ItemCloth_Type_Transform)
				{
					releaseTypeList.Add(clothType);
				}
			}
		}
		
		ProcessBody(attachLoaderList);

		ChangeBodySkin(m_nSkinItem);
		ChangeBodySize();

		m_bBodyCreated = true;
		SendMessage("OnBodyCreated", null, SendMessageOptions.DontRequireReceiver);
        		
		StartDownLoadCloth();

		EndHandling();
	}

	/// <summary>
	/// 将当前的player从本地player控制器移除
	/// </summary>
	void RemovePlayerFromControl()
	{
		PlayerManager.RemovePlayerFromControl(this);
	}

	/// <summary>
	/// 获取一个默认服饰的loader;
	/// </summary>
	RoleBodyLoader GetDefaultRoleBodyLoader(ItemCloth_Type Type)
	{
		string strFileName = GetDefaultRes(Type);//使用默认服饰;
		if (string.IsNullOrEmpty(strFileName) || !CheckHasLocalRes(strFileName))
		{
			return null;//本地必须存在默认服饰;
		}
		

		return RoleBodyLoader.GetRoleBodyLoader(strFileName, Type, null, "");
	}

	/// <summary>
	/// 下载服饰完成;
	/// </summary>
	private void OnLoadRoleBodyFinish(RoleBodyLoader roleBodyLoader,bool isLoadSuc)
	{
		if (isLoadSuc)//下载是成功的;
		{
			ChangeNetCloth(roleBodyLoader);
		}

		bool isSuc = m_dicAsyncBodyLoader.Remove(roleBodyLoader.ClothType);
		if (!isSuc)
		{
			Debug.Log("NewPlayer OnLoadRoleBodyFinish, Remove key Error, Check it");
		}

		if (m_dicAsyncBodyLoader.Count == 0)//没有等待中的内容,发出完成通知;
		{
			Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
		}
	}

	/// <summary>
	/// 调用接口,区分加载or下载;
	/// </summary>
	RoleBodyLoader GetRoleBodyLoader(string strFileName, ItemCloth_Type Type, ItemCloth_Type[] excludeTypes)
	{
		return RoleBodyLoader.GetRoleBodyLoader(strFileName, Type, excludeTypes, "");
	}

    //RoleBodyLoader GetRoleBodyLoader(uint itemType)
    //{
    //    CItemInfo itemInfo = StaticData.ItemDataMgr.GetByID(itemType);
    //    if (itemInfo != null)
    //    {
    //        string strFileName = itemInfo.m_strAtlas + ".clh";
    //        ItemCloth_Type itemClothType = (ItemCloth_Type)itemInfo.GetClothPos();

    //        return GetRoleBodyLoader(strFileName, itemClothType, itemInfo.ExcludeItems);
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

	public void ReleaseLoadingBodyLoader()
	{
		foreach (KeyValuePair<ItemCloth_Type, RoleBodyLoader> kv in m_dicAsyncBodyLoader)
		{
			if (kv.Value != null)
			{
				kv.Value.Release();
			}
		}
		m_dicAsyncBodyLoader.Clear();
	}

	/// <summary>
	/// 获取部位的默认资源
	/// </summary>
	string GetDefaultRes(ItemCloth_Type Type)
	{
		if (ItemCloth_Type.ItemCloth_Type_Hair == Type)
		{
			return SystemSetting.GetDefaultResEquip("hair", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Face == Type)
		{
			return SystemSetting.GetDefaultResEquip("face", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Body == Type)
		{
			return SystemSetting.GetDefaultResEquip("cloth", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Gloves == Type)
		{
			return SystemSetting.GetDefaultResEquip("gloves", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Leg == Type)
		{
			return SystemSetting.GetDefaultResEquip("leg", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Cap == Type)
		{
			return SystemSetting.GetDefaultResEquip("cap", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Facial_Content == Type)
		{
			return SystemSetting.GetDefaultResEquip("facial_content", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Shoulders == Type)
		{
			return SystemSetting.GetDefaultResEquip("shoulders", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Wing == Type)
		{
			return SystemSetting.GetDefaultResEquip("wing", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_LeftHand == Type)
		{
			return SystemSetting.GetDefaultResEquip("lefthand", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_RightHand == Type)
		{
			return SystemSetting.GetDefaultResEquip("righthand", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Wrist == Type)
		{
			return SystemSetting.GetDefaultResEquip("wrist", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Hip == Type)
		{
			return SystemSetting.GetDefaultResEquip("hip", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Socks == Type)
		{
			return SystemSetting.GetDefaultResEquip("socks", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Feet == Type)
		{
			return SystemSetting.GetDefaultResEquip("foot", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Skin == Type)
		{
			return SystemSetting.GetDefaultResEquip("skin", RoleAttr.IsBoy);
		}
		else if (ItemCloth_Type.ItemCloth_Type_Suit == Type)
		{
			return SystemSetting.GetDefaultResEquip("suit", RoleAttr.IsBoy);
		}
		else
		{
			return "";
		}
	}

	void AddDefaultBody(bool bEquipSuit, ref RoleBodyLoader[] arrAttachLoader)
	{
		RoleBodyLoader loader = arrAttachLoader[(int)ItemCloth_Type.ItemCloth_Type_Transform];
		if (loader != null)
		{
			return;
		}

		if (RoleAttr == null)
		{
			Debug.Log("Add Default Body Error: RoleAttr is null");
			return;
		}

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair,
			GetRoleBodyLoader(SystemSetting.GetInitEquip("hair", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Hair, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face,
			GetRoleBodyLoader(SystemSetting.GetInitEquip("face", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Face, null),
			ref arrAttachLoader);

		if (!bEquipSuit)
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Body,
				GetRoleBodyLoader(SystemSetting.GetInitEquip("cloth", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Body, null),
				ref arrAttachLoader);

			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Leg,
				GetRoleBodyLoader(SystemSetting.GetInitEquip("leg", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Leg, null),
				ref arrAttachLoader);
		}

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet,
			GetRoleBodyLoader(SystemSetting.GetInitEquip("foot", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Feet, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves,
			GetRoleBodyLoader(SystemSetting.GetInitEquip("gloves", RoleAttr.IsBoy), ItemCloth_Type.ItemCloth_Type_Gloves, null),
			ref arrAttachLoader);
	}

	bool AddRoleBody(ItemCloth_Type Part, RoleBodyLoader bodyLoader, ref RoleBodyLoader[] attachList)
	{
		if (bodyLoader != null && Part > ItemCloth_Type.ItemCloth_Type_Begin && Part < ItemCloth_Type.ItemCloth_Type_MaxNumber)
		{
			int tempIndex = (int)Part;
			if (tempIndex < attachList.Length && attachList[tempIndex] == null)
			{
				attachList[tempIndex] = bodyLoader;
				return true;
			}
		}

		return false;
	}

	IEnumerator LoadAttachRoleBody(List<RoleBodyLoader> attachList)
	{
		IEnumerator itor = null;

		for (int i = 0; i < attachList.Count; ++i)
		{
			RoleBodyLoader bodyLoader = attachList[i];
			if (bodyLoader != null)
			{
				itor = bodyLoader.LoadBodySync();
				while (itor.MoveNext())
				{
					yield return null;
				}
			}
			else
			{
				Debug.LogError("NewPlayer ProcessBody failed.RoleBodyLoader can not be null.");
			}
		}
	}

	void ProcessBody(List<RoleBodyLoader> attachList)
	{


		if (attachList != null)
		{
			int listCount = attachList.Count;
			for (int i = 0; i < listCount; ++i)
			{
				RoleBodyLoader bodyLoader = attachList[i];
				if (bodyLoader != null)
				{
					DettachFromBone(bodyLoader.ClothType);
					ReleaseBodyLoader(bodyLoader.ClothType);

					AttachToBone(bodyLoader);
					m_arBodyLoader[(int)bodyLoader.ClothType] = bodyLoader;
				}
				else
				{
					Debug.LogError("NewPlayer ProcessBody failed.RoleBodyLoader can not be null.");
				}
			}
		}
	}

	void AttachToBone(RoleBodyLoader bodyLoader)
	{
		
		if (RoleBone == null)
		{
			Debug.Log("Attach to Bone Error: RoleBone is null");
			return;
		}

		if (bodyLoader != null)
		{
			string resName = bodyLoader.ResuceName;
			if (resName.StartsWith("wing_"))
			{
				AttachExtraToBone(bodyLoader, RoleBone.m_BackBone, RoleBone.WingOffsetPos, RoleBone.WingOffsetRot);
			}
			else if (resName.StartsWith("hip_"))
			{
				AttachExtraToBone(bodyLoader, RoleBone.m_TailBone, RoleBone.HipOffsetPos, RoleBone.HipOffsetRot);
			}
			else if (resName.StartsWith("lefthand_"))
			{
				AttachExtraToBone(bodyLoader, RoleBone.m_LeftHandBone, RoleBone.LeftHandOffsetPos, RoleBone.LeftHandOffsetRot);
			}
			else if (resName.StartsWith("righthand_"))
			{
				AttachExtraToBone(bodyLoader, RoleBone.m_RightHandBone, RoleBone.RightHandOffsetPos, RoleBone.RightHandOffsetRot);
			}
			else if (resName.StartsWith("shoulders_"))
			{
				int n = 0;
				int tmpIndex = resName.Length - 2;
				if (resName[tmpIndex] == 'r')
				{
					n = 2;
				}
				
				tmpIndex = resName.Length - 1;
				if (resName[tmpIndex] == '2')
				{
					n++;
				}
				AttachExtraToBone(bodyLoader, RoleBone.m_ShoulderBone[n]
					, RoleBone.ShoulderOffsetPos[n], RoleBone.ShoulderOffsetRot[n]);
			}
			else
			{
				AttachBodyToBone(bodyLoader);
			}

			
		}
	}

	void DettachFromBone(ItemCloth_Type clothType)
	{
		RoleBodyLoader bodyLoader = m_arBodyLoader[(int)clothType];
		if (bodyLoader != null)
		{
			if (bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_Socks
				|| bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_TattooLeg)
			{
				ReplaceDefaultLegMat(null);
			}
			else
			{
				ReplaceDefaultLegMatForNewPart(bodyLoader.ClothType);
			}

			string resName = bodyLoader.ResuceName;

			if (resName.StartsWith("wing_"))
			{
				DettachExtraFromBone(clothType);
			}
			else if (resName.StartsWith("hip_"))
			{
				DettachExtraFromBone(clothType);
			}
			else if (resName.StartsWith("lefthand_"))
			{
				DettachExtraFromBone(clothType);
			}
			else if (resName.StartsWith("righthand_"))
			{
				DettachExtraFromBone(clothType);
			}
			else if (resName.StartsWith("shoulders_"))
			{
				DettachExtraFromBone(clothType);
			}

			m_dicClothParticleEffect[(int)clothType] = null;
		}
	}

	void ReplaceLegMatToRender(Material mat, ItemCloth_Type etype)
	{
		if (mat == null)
		{
			mat = SkinnLoader.GetDefaultLegSkinMat(RoleAttr.IsBoy);
			ChangeMaterialColor(m_nSkinItem, mat);
		}
		if (m_LegMaterial == null)
		{
			m_LegMaterial = SkinnLoader.GetDefaultLegSkinMat(RoleAttr.IsBoy);
			ChangeMaterialColor(m_nSkinItem, m_LegMaterial);
		}

		_ReplaceMatToRender(etype, m_LegMaterial, mat);
	}

	/// <summary>
	/// 将服饰中与m_LegMaterial同名的Materials替换成指定的Materials
	/// </summary>
	void ReplaceDefaultLegMat(Material mat)
	{
		if (m_LegMaterial != mat)
		{
			ReplaceLegMatToRender(mat, ItemCloth_Type.ItemCloth_Type_Suit);
			ReplaceLegMatToRender(mat, ItemCloth_Type.ItemCloth_Type_Leg);
			ReplaceLegMatToRender(mat, ItemCloth_Type.ItemCloth_Type_Feet);
		}
		m_LegMaterial = mat;
	}

	/// <summary>
	/// 将服饰Materials还原成m_LegMaterial
	/// </summary>
	/// <param name="itemtype"></param>
	void ReplaceDefaultLegMatForNewPart(ItemCloth_Type itemtype)
	{
		if (itemtype == ItemCloth_Type.ItemCloth_Type_Suit
			|| itemtype == ItemCloth_Type.ItemCloth_Type_Leg
			|| itemtype == ItemCloth_Type.ItemCloth_Type_Feet)
		{
			if (RoleAttr != null)
			{
				Material mat = SkinnLoader.GetDefaultLegSkinMat(RoleAttr.IsBoy);
				if (m_LegMaterial != null && mat != m_LegMaterial)
				{
					_ReplaceMatToRender(itemtype, mat, m_LegMaterial);
				}
			}
			else
			{
				Debug.Log("Replace Default Leg Mat Error: RoleAttr is null");
			}
		}
	}

	/// <summary>
	/// 替换材质
	/// </summary>
	void _ReplaceMatToRender(ItemCloth_Type itemType, Material scr, Material des)
	{
		List<GameObject> ls = m_arBodyRenderList[(int)itemType];
		if (ls != null && scr != null)
		{
			for (int i = 0; i < ls.Count; ++i)
			{
				GameObject go = ls[i];
				if (go != null)
				{
					Renderer smr = go.renderer;
					if (smr != null)
					{
						Material[] mats = smr.materials;
						for (int j = 0; j < mats.Length; j++)
						{
							Material mat = mats[j];
							if (mat != null && mat.name.Contains(scr.name))
							{
								mats[j] = des;
								Destroy(mat);	//销毁Render上旧材质
							}
						}

						smr.materials = mats;
					}
				}
				else
				{
					Debug.LogError("NewPlayer _ReplaceMatToRender failed.GameObject can not be null.");
				}
			}
		}
	}

	void AttachBodyToBone(RoleBodyLoader bodyLoader)
	{
		if (bodyLoader != null)
		{
			Quaternion q = new Quaternion(0f, 0f, 0f, 0f);

			List<string[]> boneNamesList = new List<string[]>();
			List<object> holderList = bodyLoader.GetBoneNames;
			if (holderList != null)
			{
				foreach (object obj in holderList)
				{
					if (obj != null)
					{
						StringHolder holder = (StringHolder)obj;
						boneNamesList.Add(holder.content);
					}
				}
			}

			List<GameObject> newRenderList = bodyLoader.GetMainGameObject;
			m_arBodyRenderList[(int)bodyLoader.ClothType] = newRenderList;
			m_dicClothParticleEffect[(int)bodyLoader.ClothType] = bodyLoader.GetClothParticleEffect;

			int n = 0;
			for (int i = 0; i < newRenderList.Count; ++i)
			{
				GameObject go = newRenderList[i];
				if (go.renderer != null)
				{
					SkinnedMeshRenderer meshRender = go.renderer as  SkinnedMeshRenderer;
					if(meshRender != null)
					{
						if (meshRender.sharedMesh != null)
						{
							mTempTrans = meshRender.transform;
							mTempTrans.parent = RoleBodyTrans;
							mTempTrans.localPosition = Vector3.zero;
							mTempTrans.rotation = q;
							meshRender.updateWhenOffscreen = true;
							meshRender.useLightProbes = m_buseLightProb;
							CommonFunc.SetLayer(go, m_CurrentPlayerLayer, true, GameLayer.NONE);

							if (i < boneNamesList.Count)
							{
								if (boneNamesList[i] != null)
								{
									List<Transform> boneList = new List<Transform>();

									string[] boneNameArr = boneNamesList[n];
									int nameLength = boneNameArr.Length;
									for (int j = 0; j < nameLength; ++j)
									{
										Transform bone = RoleBodyTrans.Find(boneNameArr[j]);
										if (bone != null)
										{
											boneList.Add(bone);
										}
									}

									if (boneList.Count > 0)
									{
										meshRender.bones = boneList.ToArray();
									}
								}
							}
						}
						else
						{
							meshRender.transform.parent = RoleBodyTrans;
							meshRender.gameObject.SetActive(false);
						}
					}
					n++;
				}
				else
				{
					Transform bone = RoleBodyTrans.Find(go.name);
					if (bone != null)
					{
						mTempTrans = go.transform;
						mTempTrans.parent = bone;
						mTempTrans.localPosition = Vector3.zero;
						mTempTrans.localRotation = Quaternion.Euler(Vector3.zero);
						mTempTrans.localScale = Vector3.one;
						CommonFunc.SetLayer(go, m_CurrentPlayerLayer, true, GameLayer.NONE);
					}
					else
					{
						string resName = go.name;

						SkinnedMeshRenderer[] arMeshRender = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
						int meshLength = arMeshRender.Length;
						for (int j = 0; j < meshLength; ++j)
						{
							SkinnedMeshRenderer renderer = arMeshRender[j];
							if (renderer != null)
							{
								renderer.useLightProbes = m_buseLightProb;
								renderer.updateWhenOffscreen = true;
							}
							else
							{
								Debug.LogError("NewPlayer AttachExtraToBone failed.SkinnedMeshRenderer can not be null.");
							}
						}

						if (resName.Contains("_separate-effect"))
						{
							mTempTrans = go.transform;
							mTempTrans.parent = RoleBodyTrans;
							mTempTrans.localPosition = Vector3.zero;
							mTempTrans.localRotation = Quaternion.Euler(Vector3.zero);
							mTempTrans.localScale = Vector3.one;
							CommonFunc.SetLayer(go, m_CurrentPlayerLayer, true, GameLayer.NONE);
						}
						else
						{

							if (resName.StartsWith("headwear_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_HeadWear, RoleBone.HeadWearOffsetPos, RoleBone.HeadWearOffsetRot);
							}
							else if (resName.StartsWith("pant_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_Pant, RoleBone.PantOffsetPos, RoleBone.PantOffsetRot);
							}
							else if (resName.StartsWith("coat_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_Coat, RoleBone.CoatOffsetPos, RoleBone.CoatOffsetRot);
							}
							else if (resName.StartsWith("facial_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_HeadWear, RoleBone.HeadWearOffsetPos, RoleBone.HeadWearOffsetRot);
							}
							else if (resName.StartsWith("wrist_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_Wrist, RoleBone.WristOffsetPos, RoleBone.WristOffsetRot);
							}
							else if (resName.StartsWith("wing_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_BackBone, RoleBone.WingOffsetPos, RoleBone.WingOffsetRot);
							}
							else if (resName.StartsWith("hip_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_TailBone, RoleBone.HipOffsetPos, RoleBone.HipOffsetRot);
							}
							else if(resName.StartsWith("legwearleft_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_CalfLeft, RoleBone.CalfLeftOffsetPos, RoleBone.CalfLeftOffsetRot);
							}
							else if (resName.StartsWith("legwearright_"))
							{
								AttachSkinExtraToBone(go, RoleBone.m_CalfRight, RoleBone.CalfRightOffsetPos, RoleBone.CalfRightOffsetRot);
							}
							else
							{
								mTempTrans = go.transform;
								mTempTrans.parent = RoleBodyTrans;
								mTempTrans.localPosition = Vector3.zero;
								mTempTrans.localRotation = Quaternion.Euler(Vector3.zero);
								mTempTrans.localScale = Vector3.one;
								CommonFunc.SetLayer(go, m_CurrentPlayerLayer, true, GameLayer.NONE);
							}
						}
					}
				}
			}

			if (bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_Socks
				|| bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_TattooLeg)
			{
				if (newRenderList.Count > 0)
				{
					GameObject socksRenderObject = newRenderList[0];
					if (socksRenderObject != null && socksRenderObject.renderer != null)
					{
						ReplaceDefaultLegMat(socksRenderObject.renderer.material);
					}
				}
			}
			else
			{
				ReplaceDefaultLegMatForNewPart(bodyLoader.ClothType);
			}
		}
	}

	/// <summary>
	/// 添加服饰附加物体到骨骼节点
	/// </summary>
	/// <param name="newExtra"></param>
	/// <param name="bone"></param>
	/// <param name="offsetPos"></param>
	/// <param name="offsetRot"></param>
	void AttachSkinExtraToBone(GameObject newExtra, Transform bone, Vector3 offsetPos, Vector3 offsetRot)
	{
		if (newExtra == null)
		{
			return;
		}

		if (newExtra.animation != null)
		{
			newExtra.animation.enabled = true;
			newExtra.animation.wrapMode = WrapMode.Loop;
			newExtra.animation.Play();
		}
		if (bone != null)
		{
			Vector3 oriPos = cachedTransform.position;
			Quaternion oriRot = cachedTransform.rotation;
			cachedTransform.position = Vector3.zero;
			cachedTransform.rotation = Quaternion.identity;

			mTempTrans = newExtra.transform;

			Vector3 bonePos = bone.position;
			Quaternion bonerote = bone.rotation;

			bone.position = offsetPos + cachedTransform.position;
			bone.rotation = Quaternion.Euler(Vector3.zero);

			mTempTrans.parent = bone;
			CommonFunc.SetLayer(newExtra, m_CurrentPlayerLayer, true, GameLayer.NONE);

			mTempTrans.parent = null;

			bone.position = offsetPos;
			bone.rotation = Quaternion.Euler(offsetRot);

			mTempTrans.parent = bone;

			bone.position = bonePos;
			bone.rotation = bonerote;

			cachedTransform.position = oriPos;
			cachedTransform.rotation = oriRot;
		}
		else
		{
			Debug.LogWarning("NewPlayer AttachSkinExtraToBone, bone can not be null.");
		}
	}

	void AttachExtraToBone(RoleBodyLoader bodyLoader, Transform bone, Vector3 offsetPos, Vector3 offsetRot)
	{
		if (bodyLoader != null)
		{
			DettachExtraFromBone(bodyLoader.ClothType);

			GameObject newExtra = bodyLoader.GetExtraGameObject;
			if (newExtra == null)
			{
				return;
			}

			mTempTrans = newExtra.transform;

			newExtra.name = bodyLoader.ResuceName.Replace(".clh", "");
			int tempIndex = (int)bodyLoader.ClothType;
			if (tempIndex < m_arBodyExtra.Length)
			{
				m_arBodyExtra[tempIndex] = newExtra;
				m_arBodyExtraScale[tempIndex] = mTempTrans.localScale;
			}
			if (tempIndex < m_dicClothParticleEffect.Length)
			{
				m_dicClothParticleEffect[tempIndex] = bodyLoader.GetExtraParticleEffect;
			}

			if (newExtra.animation != null)
			{
				newExtra.animation.wrapMode = WrapMode.Loop;
				newExtra.animation.Play();
			}

			if (bone != null)
			{
				Vector3 bonePos = bone.position;
				Quaternion bonerote = bone.rotation;

				bone.position = offsetPos + cachedTransform.position;
				bone.rotation = Quaternion.Euler(Vector3.zero);

				mTempTrans.parent = bone;
				m_arBodyExtraScale[tempIndex] = mTempTrans.localScale;
				CommonFunc.SetLayer(newExtra, m_CurrentPlayerLayer, true, GameLayer.NONE);

				SkinnedMeshRenderer[] arMeshRender = newExtra.GetComponentsInChildren<SkinnedMeshRenderer>(true);
				int meshLength = arMeshRender.Length;
				for (int i = 0; i < meshLength; ++i)
				{
					SkinnedMeshRenderer renderer = arMeshRender[i];
					if (renderer != null)
					{
						renderer.useLightProbes = m_buseLightProb;
						renderer.updateWhenOffscreen = true;
					}
					else
					{
						Debug.LogError("NewPlayer AttachExtraToBone failed.SkinnedMeshRenderer can not be null.");
					}
				}

				GameObject spEffect = null;
				foreach (Transform t in mTempTrans)
				{
					if (t.name.Contains("_separate-effect"))
					{
						spEffect = t.gameObject;
						break;
					}
				}

				if (spEffect != null)
				{
					if (tempIndex < m_arBodyExtraSpEffect.Length)
					{
						m_arBodyExtraSpEffect[tempIndex] = spEffect;
					}
					spEffect.transform.parent = cachedTransform;
				}

				mTempTrans.parent = null;

				bone.position = offsetPos;
				bone.rotation = Quaternion.Euler(offsetRot);

				mTempTrans.parent = bone;

				bone.position = bonePos;
				bone.rotation = bonerote;

				//if (CheckTransitionPart(bodyLoader.ClothType))
				//{
				//    newExtra.SetActive(InCurtain);
				//}
			}
			else
			{
				Debug.LogWarning("NewPlayer AttachExtraToBone,bone can not be null.");
			}
		}
	}

	void DettachExtraFromBone(ItemCloth_Type clothType)
	{
		int tempIndex = (int)clothType;
		if (tempIndex < m_arBodyExtra.Length && m_arBodyExtra[tempIndex] != null)
		{
			GameObject oldExtra = m_arBodyExtra[tempIndex];
			m_arBodyExtra[tempIndex] = null;
			
			if (oldExtra != null)
			{
				GameObject.Destroy(oldExtra);
			}

			if (tempIndex < m_arBodyExtraSpEffect.Length)
			{
				GameObject oldSpEffect = m_arBodyExtraSpEffect[tempIndex];
				m_arBodyExtraSpEffect[tempIndex] = null;

				if (oldSpEffect != null)
				{
					GameObject.Destroy(oldSpEffect);
				}
			}
		}
	}
    
	void _LoadBodySkinSync(uint itemType)
	{
		m_nSkinItem = itemType;

		ChangeBodySkin(m_nSkinItem);
		cachedGameObject.SendMessage("OnDressUp", ItemCloth_Type.ItemCloth_Type_Skin, SendMessageOptions.DontRequireReceiver);
	}

	private List<RoleBodyLoader> GetRoleBodyAttachList(ItemCloth_Type partType)
	{
		List<RoleBodyLoader> attachLoaderList = new List<RoleBodyLoader>();
		if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Transform] != null && partType != ItemCloth_Type.ItemCloth_Type_Transform)
		{
			for (int i = 0; i < (int)ItemCloth_Type.ItemCloth_Type_MaxNumber; ++i)
			{
				RoleBodyLoader relatedLoader = null;
				ItemCloth_Type part = (ItemCloth_Type)i;

				if (part == ItemCloth_Type.ItemCloth_Type_Transform
					|| part == ItemCloth_Type.ItemCloth_Type_Skin
					|| part == partType)
				{
					continue;
				}

				if (partType == ItemCloth_Type.ItemCloth_Type_Body || partType == ItemCloth_Type.ItemCloth_Type_Leg)
				{
					if (part == ItemCloth_Type.ItemCloth_Type_Suit)
						continue;
				}

				if (partType == ItemCloth_Type.ItemCloth_Type_Suit)
				{
					if (part == ItemCloth_Type.ItemCloth_Type_Body || part == ItemCloth_Type.ItemCloth_Type_Leg)
					{
						relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Suit);
						if (relatedLoader != null)
						{
							continue;
						}
					}
				}

				relatedLoader = GetRelatedBodyLoader(part);

				if (relatedLoader != null && LoaderNeedProc(relatedLoader))
				{
					attachLoaderList.Add(relatedLoader);
				}
			}
		}

		if (partType == ItemCloth_Type.ItemCloth_Type_Body || partType == ItemCloth_Type.ItemCloth_Type_Leg)
		{
			if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Suit] != null)
			{
				RoleBodyLoader relatedLoader = null;
				if (partType == ItemCloth_Type.ItemCloth_Type_Body)
				{
					relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Leg);
				}
				else
				{
					relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Body);
				}

				if (relatedLoader != null && LoaderNeedProc(relatedLoader))
				{
					attachLoaderList.Add(relatedLoader);
				}
			}
		}
		return attachLoaderList;
	}

	private List<ItemCloth_Type> GetRoleBodyReleaseList(ItemCloth_Type partType)
	{
		List<ItemCloth_Type> releaseTypeList = new List<ItemCloth_Type>();

		if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Transform] != null && partType != ItemCloth_Type.ItemCloth_Type_Transform)
		{
			releaseTypeList.Add(ItemCloth_Type.ItemCloth_Type_Transform);
		}

		if (partType == ItemCloth_Type.ItemCloth_Type_Body || partType == ItemCloth_Type.ItemCloth_Type_Leg)
		{
			if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Suit] != null)
			{
				releaseTypeList.Add(ItemCloth_Type.ItemCloth_Type_Suit);
			}
		}
		else if (partType == ItemCloth_Type.ItemCloth_Type_Suit)
		{
			if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Body] != null)
			{
				releaseTypeList.Add(ItemCloth_Type.ItemCloth_Type_Body);
			}

			if (m_arBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_Leg] != null)
			{
				releaseTypeList.Add(ItemCloth_Type.ItemCloth_Type_Leg);
			}
		}
		else if (partType == ItemCloth_Type.ItemCloth_Type_Transform)
		{
			for (int i = 0; i < (int)ItemCloth_Type.ItemCloth_Type_MaxNumber; ++i)
			{
				if (m_arBodyLoader[i] != null && i != (int)ItemCloth_Type.ItemCloth_Type_Transform)
				{
					releaseTypeList.Add((ItemCloth_Type)i);
				}
			}
		}
		return releaseTypeList;
	}

	IEnumerator _LoadBodyPartAsync(ItemCloth_Type partType, string partResName, ItemCloth_Type[] arExcludeType, bool bShowLoading)
	{
        //if (bShowLoading)
        //{
        //    LoadingMgr.ShowLoading(true);
        //}

		while (!BeginHandling())
		{
			yield return null;
		}

		RoleBodyLoader gtBodyLoader = GetRoleBodyLoader(partResName, partType, arExcludeType);
		if (gtBodyLoader != null)
		{
			if (LoaderNeedProc(gtBodyLoader))
			{
				List<ItemCloth_Type> releaseTypeList = GetRoleBodyReleaseList(partType);
				List<RoleBodyLoader> attachLoaderList = GetRoleBodyAttachList(partType);
				attachLoaderList.Add(gtBodyLoader);//当前换装的服饰;

				//区分 是加载or下载 队列;
				attachLoaderList = GetAttachRoleBodyLoaderList(attachLoaderList);

				//开始本地加载流程;
				if (attachLoaderList.Count != 0 || CheckHasBodyChange(releaseTypeList))
				{
                    //if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
                    //{
                    //    Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
                    //}
                    //else
                    //{
                    //    //可能出现 网络服饰切换到本地服饰.帘子应该关掉;
                    //    Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
                    //}

					ReleaseAsyncBodyLoader(releaseTypeList);//下载队列正好有删除的部位,立即停止下载;

					IEnumerator itor = LoadAttachRoleBody(attachLoaderList);
					while (itor.MoveNext())
					{
						yield return null;
					}
					ProcessBody(attachLoaderList, releaseTypeList);

					ChangeBodySkin(m_nSkinItem);
					cachedGameObject.SendMessage("OnDressUp", partType, SendMessageOptions.DontRequireReceiver);
				}
				
				//下载流程;
				StartDownLoadCloth();
			}
		}
		else
		{
			Debug.LogError("NewPlayer _LoadBodyPartAsync ,RoleBodyLoader can not be null. name: " + partResName + " ,type: " + partType);
		}

		EndHandling();

        //if (bShowLoading)
        //{
        //    LoadingMgr.ShowLoading(false);
        //}
	}


	private IEnumerator _UnloadBodyPartAsync(ItemCloth_Type clothType, bool bReinit, bool bShowLoading)
	{
        //if (bShowLoading)
        //{
        //    LoadingMgr.ShowLoading(true);
        //}

		while (!BeginHandling())
		{
			yield return null;
		}

		RoleBodyLoader bodyLoader = m_arBodyLoader[(int)clothType];
		if (bodyLoader != null)
		{
			if (bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_Socks
				|| bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_TattooLeg)
			{
				ReplaceDefaultLegMat(null);
			}
			else
			{
				ReplaceDefaultLegMatForNewPart(bodyLoader.ClothType);
			}

			List<ItemCloth_Type> releaseTypeList = new List<ItemCloth_Type>();
			RoleBodyLoader[] arrAttachLoader = new RoleBodyLoader[(int)ItemCloth_Type.ItemCloth_Type_MaxNumber];

			RoleBodyLoader newLoader = (bReinit ? GetInitBodyLoader(clothType) : GetRelatedBodyLoader(clothType));
			if (newLoader != null)
			{
				if (LoaderNeedProc(newLoader))
				{
					int index = (int)clothType;
					if (index <= arrAttachLoader.Length)
					{
						arrAttachLoader[index] = newLoader;
					}
				}
			}
			else
			{
				releaseTypeList.Add(clothType);

				if (clothType == ItemCloth_Type.ItemCloth_Type_Suit)
				{
					RoleBodyLoader relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Body);
					if (relatedLoader != null && LoaderNeedProc(relatedLoader))
					{
						int index = (int)ItemCloth_Type.ItemCloth_Type_Body;
						if (index <= arrAttachLoader.Length)
						{
							arrAttachLoader[index] = relatedLoader;
						}
					}

					relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Leg);
					if (relatedLoader != null && LoaderNeedProc(relatedLoader))
					{
						int index = (int)ItemCloth_Type.ItemCloth_Type_Leg;
						if (index <= arrAttachLoader.Length)
						{
							arrAttachLoader[index] = relatedLoader;
						}
					}
				}
				else if (bodyLoader.ClothType == ItemCloth_Type.ItemCloth_Type_Transform)
				{
					bool bEuipSuit = AddEquipBody(ref arrAttachLoader);
					AddDefaultBody(bEuipSuit, ref arrAttachLoader);
				}
			}

			//区分 是加载or下载 队列;
			List<RoleBodyLoader> attachLoaderList = GetAttachRoleBodyLoaderList(arrAttachLoader);

			//开始本地加载流程;
			if (attachLoaderList.Count != 0 || CheckHasBodyChange(releaseTypeList))
			{
                //if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
                //{
                //    Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
                //}
                //else
                //{
                //    //可能出现 网络服饰切换到本地服饰.帘子应该关掉;
                //    Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
                //}

				IEnumerator itor = LoadAttachRoleBody(attachLoaderList);
				while (itor.MoveNext())
				{
					yield return null;
				}
				ProcessBody(attachLoaderList, releaseTypeList);

				ChangeBodySkin(m_nSkinItem);
			}


			StartDownLoadCloth();
		}

		EndHandling();
	}

	void _UnloadBodySkinSync(bool bReinit)
	{
		m_nSkinItem = 0;

		uint skinItem = 0;
		if (bReinit)
		{
			string strID = GetInitEquipName(ItemCloth_Type.ItemCloth_Type_Skin);
			uint.TryParse(strID, out skinItem);
		}

		ChangeBodySkin(skinItem);
	}


    void ProcessBody(List<RoleBodyLoader> attachList, List<ItemCloth_Type> releaseList)
    {
        if (releaseList != null)
        {
            int releaseCount = releaseList.Count;
            for (int i = 0; i < releaseCount; ++i)
            {
                ItemCloth_Type clothType = releaseList[i];
                DettachFromBone(clothType);
                ReleaseBodyLoader(clothType);
            }
        }

        if (attachList != null)
        {
            int listCount = attachList.Count;
            for (int i = 0; i < listCount; ++i)
            {
                RoleBodyLoader bodyLoader = attachList[i];
                if (bodyLoader != null)
                {
                    DettachFromBone(bodyLoader.ClothType);
                    ReleaseBodyLoader(bodyLoader.ClothType);

                    AttachToBone(bodyLoader);
                    m_arBodyLoader[(int)bodyLoader.ClothType] = bodyLoader;
                }
                else
                {
                    Debug.LogError("NewPlayer ProcessBody failed.RoleBodyLoader can not be null.");
                }
            }
        }
    }

	public override void SetBodyPartState(ItemCloth_Type clothType, bool show)
	{
		GameObject extraGo = m_arBodyExtra[(int)clothType];
		if (extraGo != null)
		{
			extraGo.SetActive(show);
		}

		List<GameObject> listRenderGo = m_arBodyRenderList[(int)clothType];
		if (listRenderGo != null && listRenderGo.Count > 0)
		{
			for (int i = 0; i < listRenderGo.Count; ++i)
			{
				listRenderGo[i].SetActive(show);
			}
		}
	}

	void ReleaseBodyLoader(ItemCloth_Type partType)
	{
		int nPartType = (int)partType;
		if (nPartType < m_arBodyRenderList.Length)
		{
			List<GameObject> bodyRenderList = m_arBodyRenderList[nPartType];
			if (bodyRenderList != null)
			{
				if (partType == ItemCloth_Type.ItemCloth_Type_Socks
					|| partType == ItemCloth_Type.ItemCloth_Type_TattooLeg)
				{
					ReplaceDefaultLegMat(null);
				}

				//销毁Render上的材质，防止内存泄漏;
				for (int i = 0; i < bodyRenderList.Count; ++i)
				{
					GameObject go = bodyRenderList[i];
					if (go != null && go.renderer != null)
					{
						SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.renderer;
						Material[] matArr = smr.materials;
						for (int j = 0; j < matArr.Length; ++j)
						{
							Material mat = matArr[j];
							if (mat != null)
							{
								Destroy(mat);
							}
						}
					}
				}

				m_arBodyRenderList[nPartType] = null;
			}
		}

		if (nPartType < m_arBodyLoader.Length && m_arBodyLoader[nPartType] != null)
		{
			m_arBodyLoader[nPartType].Release();
			m_arBodyLoader[nPartType] = null;
		}
	}

	RoleBodyLoader GetRelatedBodyLoader(ItemCloth_Type clothType)
	{
		RoleBodyLoader bodyLoader = null;

		if (RoleItem != null)
		{
			CEquipItem equipItem = RoleItem.GetRelatedEquip(clothType);
			if (equipItem != null)
			{
				bodyLoader = GetRoleBodyLoader(equipItem.ClothResource, clothType, null);
			}
			else
			{
				bodyLoader = GetInitBodyLoader(clothType);
			}
		}
		else
		{
			bodyLoader = GetInitBodyLoader(clothType);
		}

		return bodyLoader;
	}

	RoleBodyLoader GetInitBodyLoader(ItemCloth_Type clothType)
	{
		RoleBodyLoader bodyLoader = null;

		string strEquipName = GetInitEquipName(clothType);
		if (strEquipName.Length > 0)
		{
			bodyLoader = GetRoleBodyLoader(strEquipName, clothType, null);
		}

		return bodyLoader;
	}

	bool LoaderNeedProc(RoleBodyLoader bodyLoader)
	{
		int tempIndex = (int)bodyLoader.ClothType;
		if (tempIndex < m_arBodyLoader.Length && m_arBodyLoader[tempIndex] != null
			&& bodyLoader != null && m_arBodyLoader[tempIndex].ResuceName == bodyLoader.ResuceName)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// 修改材质颜色，不会造成内存泄漏
	/// </summary>
	/// <param name="skinItem">肤色ID</param>
	void ChangeMaterialColor(uint skinItem, Material mat)
	{
		Color skinColor = (skinItem > 0 ? GetSkinColorByItem(skinItem) : GetEquipedSkinColor());
		if (mat != null && mat.shader.name.Contains("Skin"))
		{
			mat.SetColor("_Color", skinColor);
		}
	}

	/// <summary>
	/// 修改肤色颜色
	/// </summary>
	void ChangeBodySkin(uint skinItem)
	{
		Color skinColor = (skinItem > 0 ? GetSkinColorByItem(skinItem) : GetEquipedSkinColor());

		for (int i = 0; i < m_arBodyRenderList.Length; ++i)
		{
			List<GameObject> ls = m_arBodyRenderList[i];
			if (ls != null)
			{
				for (int j = 0; j < ls.Count; ++j)
				{
					GameObject go = ls[j];
					if (go != null)
					{
						Renderer renderer = go.renderer;
						if (renderer != null)
						{
							MaterialPropertyBlock matBolck = new MaterialPropertyBlock();
							matBolck.Clear();
							matBolck.AddColor("_Color", skinColor);
							renderer.SetPropertyBlock(matBolck);
						}
					}
					else
					{
						Debug.LogError("NewPlayer ChangeBodySkin failed.SkinnedMeshRenderer can not be null.");
					}
				}
			}
		}
	}

	Color GetSkinColorByInitEquip()
	{
		Color skinColor = new Color(1f, 1f, 1f, 1f);

		uint itemType = 0;
		string strID = GetInitEquipName(ItemCloth_Type.ItemCloth_Type_Skin);
		itemType = uint.Parse(strID);

		if (0 != itemType)
		{
			skinColor = GetSkinColorByItem(itemType);
		}

		return skinColor;
	}

	Color GetSkinColorByItem(uint itemType)
	{
		Color skinColor = new Color(1f, 1f, 1f, 1f);

		CSkinInfo skinInfo = StaticData.ItemDataMgr.GetSkinInfoByID(itemType);
		if (null != skinInfo)
		{
			skinColor = new Color((float)skinInfo.m_nR / 255f, (float)skinInfo.m_nG / 255f, (float)skinInfo.m_nB / 255f, (float)skinInfo.m_nA / 255f);
		}

		return skinColor;
	}

	Color GetEquipedSkinColor()
	{
		// get the related skin
		uint itemType = 0;
		if (null != RoleItem)
		{
			CEquipItem skinEquip = RoleItem.GetRelatedEquip(ItemCloth_Type.ItemCloth_Type_Skin);
			if (null != skinEquip)
			{
				itemType = skinEquip.ItemInfo.m_nType;
			}
		}
		else
		{
			string strID = GetInitEquipName(ItemCloth_Type.ItemCloth_Type_Skin);
			uint.TryParse(strID,out itemType);
		}

		// get the color from item
		Color skinColor = GetSkinColorByInitEquip();
		if (0 != itemType)
		{
			skinColor = GetSkinColorByItem(itemType);
		}

		return skinColor;
	}

	public override void InitBodySize(float scale)
	{
		m_InitScale = scale;
		cachedTransform.localScale = Vector3.one * scale;
	}

	string GetInitEquipName(ItemCloth_Type clothType)
	{
		string strEquipName = "";
		if (RoleAttr != null)
		{
			switch (clothType)
			{
				case ItemCloth_Type.ItemCloth_Type_Hair:
					strEquipName = SystemSetting.GetInitEquip("hair", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Face:
					strEquipName = SystemSetting.GetInitEquip("face", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Body:
					strEquipName = SystemSetting.GetInitEquip("cloth", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Leg:
					strEquipName = SystemSetting.GetInitEquip("leg", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Feet:
					strEquipName = SystemSetting.GetInitEquip("foot", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Gloves:
					strEquipName = SystemSetting.GetInitEquip("gloves", RoleAttr.IsBoy);
					break;
				case ItemCloth_Type.ItemCloth_Type_Skin:
					strEquipName = SystemSetting.GetInitEquip("skin", RoleAttr.IsBoy);
					break;
			}
		}
		else
		{
			Debug.Log("Get Init Equip Name Error: RoleAttr is null");
		}

		return strEquipName;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_LastStyle = PlayerStyleType.None;

        //DestroyUIRoleCamera();
        //DestroyUIFaceCamera();

		PlayerStyle[] playerStyles = cachedGameObject.GetComponents<PlayerStyle>();
		if (playerStyles != null)
		{
			for (int i = 0; i < playerStyles.Length; ++i)
			{
				if (playerStyles[i] != null)
				{
					playerStyles[i].BeRemoved();
					Destroy(playerStyles[i]);
				}
			}
		}

		for (int i = 0; i < (int)ItemCloth_Type.ItemCloth_Type_MaxNumber; i++)
		{
			DettachFromBone((ItemCloth_Type)i);
		}
	}


	public override GameObject GetBodyExtra(ItemCloth_Type itemClothType)
	{
		if (itemClothType < ItemCloth_Type.ItemCloth_Type_MaxNumber && itemClothType > ItemCloth_Type.ItemCloth_Type_Begin)
		{
			return m_arBodyExtra[(int)itemClothType];
		}
		return null;
	}

    
	public override void SetBodyScale(float scale)
	{
		cachedTransform.localScale = Vector3.one * m_InitScale * scale;
	}


	private List<RoleBodyLoader> GetAttachRoleBodyLoaderList(RoleBodyLoader[] tempLoaderList)
	{
		return GetAttachRoleBodyLoaderList(new List<RoleBodyLoader>(tempLoaderList));
	}

	private List<RoleBodyLoader> GetAttachRoleBodyLoaderList(List<RoleBodyLoader> tempLoaderList)
	{
		List<RoleBodyLoader> attachLoaderList = new List<RoleBodyLoader>();
		RoleBodyLoader tempTransformLoader = null;
		
		//区分 是加载or下载 队列;
		for (int i = 0; i < tempLoaderList.Count; i++)
		{
			if (tempLoaderList[i] == null)
			{
				continue;
			}
			ItemCloth_Type partType = tempLoaderList[i].ClothType;

			List<ItemCloth_Type> gtReleaseType = GetRoleBodyReleaseList(partType);
			gtReleaseType.Add(partType);
			for ( int j = 0 ; j < gtReleaseType.Count ; j++ )
			{
				if (m_dicAsyncBodyLoader.ContainsKey(gtReleaseType[j]))
				{
					m_dicAsyncBodyLoader[gtReleaseType[j]].Release();
					m_dicAsyncBodyLoader.Remove(gtReleaseType[j]);
				}
			}

			if (tempLoaderList[i].IsNetLoad)
			{
				m_dicAsyncBodyLoader.Add(partType, tempLoaderList[i]);//网络下载;

				RoleBodyLoader gtBodyLoader = GetDefaultRoleBodyLoader(partType);
				if (gtBodyLoader != null)
				{
					attachLoaderList.Add(gtBodyLoader);
				}
			}
			else
			{
				if (!CheckHasLocalRes(tempLoaderList[i].ResuceName))//不开启动态下载，小包，不存在服饰的情况，使用默认服饰;
				{
					RoleBodyLoader gtBodyLoader = GetDefaultRoleBodyLoader(partType);
					if (gtBodyLoader != null)
					{
						attachLoaderList.Add(gtBodyLoader);
					}
				}
				else
				{
					if (partType == ItemCloth_Type.ItemCloth_Type_Transform)
					{
						tempTransformLoader = tempLoaderList[i];
					}
					else
					{
						attachLoaderList.Add(tempLoaderList[i]);//本地加载;
					}
				}
			}
		}
		
		if (tempTransformLoader != null)
		{
			attachLoaderList.Clear();
			attachLoaderList.Add(tempTransformLoader);
		}
		
		return attachLoaderList;
	}


	RoleBodyLoader GetNormalRelatedBodyLoader(ItemCloth_Type clothType)
	{
		RoleBodyLoader bodyLoader = null;

		if (RoleItem != null)
		{
			CEquipItem equipItem = RoleItem.GetNormalRelatedEquip(clothType);
			if (equipItem != null)
			{
				bodyLoader = GetRoleBodyLoader(equipItem.ClothResource, clothType, null);
			}
			else
			{
				bodyLoader = GetInitBodyLoader(clothType);
			}
		}
		else
		{
			bodyLoader = GetInitBodyLoader(clothType);
		}

		return bodyLoader;
	}

	void ChangeBodySize()
	{
		if (RoleAttr != null)
		{
			if (RoleAttr.IsBoy)
			{
				Vector3 scale = new Vector3(CommonDef.ROLE_MANSCALE, CommonDef.ROLE_MANSCALE, CommonDef.ROLE_MANSCALE);
				cachedTransform.localScale = scale;
			}
			
            //if (RoleTitle != null)
            //{
            //    RoleTitle.RefreshPos_TransformTitle();
            //}
		}
		else
		{
			Debug.Log("Change Body Size Error: RoleAttr is null");
		}
	}
    
	private bool CheckTransitionPart(ItemCloth_Type clothType)
	{
		if (clothType == ItemCloth_Type.ItemCloth_Type_Wing
			|| clothType == ItemCloth_Type.ItemCloth_Type_Hip)
			return true;

		return false;
	}
    
	private bool CheckNeedBrocastDown(List<RoleBodyLoader> listLoader)
	{
		RoleBodyLoader bodyLoader = null;
		for (int i = 0; i < listLoader.Count; ++i)
		{
			bodyLoader = listLoader[i];
			if (bodyLoader != null)
			{
				if (CheckNeedBrocastDown(bodyLoader.ClothType))
				{
					return true;
				}
			}
		}

		return false;
	}

	/// <summary>
	/// check本地是不是有资源;
	/// </summary>
	private bool CheckHasLocalRes(string strFileName)
	{
		bool hasLocalRes = true;
		if (!System.IO.File.Exists(CommonValue.MaterialDir + strFileName))
		{
			if (!GlobalFunc.ExistInFile(CommonValue.InMaterialDir, strFileName))
			{
				//内外包都不包含;
				hasLocalRes = false;
			}
		}
		return hasLocalRes;
	}

	private bool CheckNeedBrocastDown(ItemCloth_Type clothType)
	{
		if (clothType == ItemCloth_Type.ItemCloth_Type_Hair
		|| clothType == ItemCloth_Type.ItemCloth_Type_Body
		|| clothType == ItemCloth_Type.ItemCloth_Type_Suit
		|| clothType == ItemCloth_Type.ItemCloth_Type_Leg
		|| clothType == ItemCloth_Type.ItemCloth_Type_Feet)
		{
			return true;
		}

		return false;
	}
}