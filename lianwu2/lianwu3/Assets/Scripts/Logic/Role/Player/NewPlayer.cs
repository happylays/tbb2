using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
//using LoveDance.Client.Data.Item;
//using LoveDance.Client.Network.VIP;
//using LoveDance.Client.Network.Player;
//using LoveDance.Client.Network.Gene;
//using LoveDance.Client.Data.Lantern;
//using LoveDance.Client.Data.Cloth;
//using LoveDance.Client.Data.GeneEffect;
//using LoveDance.Client.Data;
//using LoveDance.Client.Data.Setting;
//using LoveDance.Client.Network.Item;
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



	/// <summary>
	/// 根据变身ID刷新人物
	/// </summary>
	IEnumerator RefreshTransform(int trasformID)
	{
		while (!ClientResourcesMgr.IsInitRemainRes)
		{
			yield return null;
		}

		OnRefreshTransformID(trasformID);
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


	public override void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca)
	{
		CreateUIRoleCamera(topLeft, bottomRight, ca, new CameraLevel(CameraLevel.Default));
	}

	public override void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel)
	{
		CreateUIRoleCamera(topLeft, bottomRight, ca, camLevel, GameLayer.Player_UI);
	}

	public override void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer)
	{
		CreateUIRoleCamera(topLeft, bottomRight, ca, new CameraLevel(CameraLevel.Default), targetLayer);
	}

	public override void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer)
	{
		CreateUIRoleCamera(topLeft, bottomRight, ca, camLevel, targetLayer, 15);
	}

	public override void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer, float fieldOfView)
	{
		if (m_UIRoleCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIRoleCamera = new GameObject("UIRoleCamera");

			mTempTrans = m_UIRoleCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(172.5f, 0f, 180f);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.9f, 7.6f);
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIRoleCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.Nothing;
			cr.depth = ca.depth;
			cr.cullingMask = 1 << (int)targetLayer;
			cr.fieldOfView = fieldOfView;
			cr.nearClipPlane = camLevel.Level.x;
			cr.farClipPlane = camLevel.Level.y;

			Vector3 tl = ca.WorldToScreenPoint(topLeft);
			Vector3 br = ca.WorldToScreenPoint(bottomRight);
			Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
			if (rect != cr.rect)
			{
				cr.rect = rect;
			}

			m_LastStyle = CurrentStyle;
			CurrentStyle = PlayerStyleType.World;

			CommonFunc.SetLayer(cachedGameObject, targetLayer, true, GameLayer.Player);
			m_CurrentPlayerLayer = targetLayer;
			//create light;
			GameObject Playerlight = new GameObject("Player_UI_LIGHT");
			Light l = Playerlight.AddComponent<Light>();
			l.type = LightType.Directional;
			l.color = Color.white;
			l.intensity = 0.35f;
			l.renderMode = LightRenderMode.ForcePixel;
			l.cullingMask = 1 << (int)targetLayer;
			mTempTrans = l.transform;
			mTempTrans.parent = m_UIRoleCamera.transform;
			mTempTrans.localRotation = Quaternion.Euler(13.7f, 327.7f, 334.3f);

			//Disable LightProb because we have a light;
			EnableLightProb(false);
		}
	}

	public override void UpdateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca,  GameLayer targetLayer)
	{
		UpdateUIRoleCamera(topLeft, bottomRight, ca, new CameraLevel(CameraLevel.Default), targetLayer);
	}

	public override void UpdateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer)
	{
		if (m_UIRoleCamera != null)
		{
			Camera cr = m_UIRoleCamera.GetComponent<Camera>();
			if (cr != null && ca != null)
			{
				Vector3 tl = ca.WorldToScreenPoint(topLeft);
				Vector3 br = ca.WorldToScreenPoint(bottomRight);

				Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
				if (rect != cr.rect)
				{
					cr.rect = rect;
				}
			}
		}
		else
		{
			CreateUIRoleCamera(topLeft, bottomRight, ca, camLevel, targetLayer);
		}
	}

	public override void EnableLightProb(bool bEnable)
	{
		SkinnedMeshRenderer[] ss = cachedGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		int skinLength = ss.Length;
		for (int i = 0; i < skinLength; ++i)
		{
			SkinnedMeshRenderer s = ss[i];
			if (s != null)
			{
				s.useLightProbes = bEnable;
			}
			else
			{
				Debug.LogError("NewPlayer EnableLightProb failed.SkinnedMeshRenderer can not be null.");
			}
		}
		m_buseLightProb = bEnable;

	}

	public override void ChangeCurrentLayer(GameLayer targetLayer, GameLayer srcLayer)
	{
		m_CurrentPlayerLayer = targetLayer;

		if (!m_bBodyCreated)
			return;

		CommonFunc.SetLayer(cachedGameObject, m_CurrentPlayerLayer, true, srcLayer);
	}

	public override void DestroyUIRoleCamera()
	{
		if (m_UIRoleCamera != null)
		{
			Destroy(m_UIRoleCamera);
			m_UIRoleCamera = null;

			CurrentStyle = m_LastStyle;
			m_LastStyle = PlayerStyleType.None;
			m_CurrentPlayerLayer = GameLayer.Player;
			CommonFunc.SetLayer(cachedGameObject, GameLayer.Player, true, GameLayer.Player_UI);
			EnableLightProb(true);
		}
	}

	public override void CreateCeremonyRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel)
	{
		if (m_UIRoleCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIRoleCamera = new GameObject("UIRoleCeremonyCamera");

			mTempTrans = m_UIRoleCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(172.5f, 180f, 180f);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.8f, -7.6f);
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIRoleCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.Nothing;
			cr.depth = ca.depth;
			cr.cullingMask = 1 << (int)GameLayer.Player_UI;
			cr.fieldOfView = 15;
			cr.nearClipPlane = camLevel.Level.x;
			cr.farClipPlane = camLevel.Level.y;

			Vector3 tl = ca.WorldToScreenPoint(topLeft);
			Vector3 br = ca.WorldToScreenPoint(bottomRight);
			Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
			if (rect != cr.rect)
			{
				cr.rect = rect;
			}

			CommonFunc.SetLayer(cachedGameObject, GameLayer.Player_UI, true, GameLayer.Player);
			m_CurrentPlayerLayer = GameLayer.Player_UI;
			//create light;
			GameObject Playerlight = new GameObject("Player_UI_LIGHT");
			Light l = Playerlight.AddComponent<Light>();
			l.type = LightType.Directional;
			l.color = Color.white;
			l.intensity = 0.35f;
			l.renderMode = LightRenderMode.ForcePixel;
			l.cullingMask = 1 << (int)GameLayer.Player_UI;
			mTempTrans = l.transform;
			mTempTrans.parent = m_UIRoleCamera.transform;
			mTempTrans.localRotation = Quaternion.Euler(13.7f, 327.7f, 334.3f);

			//Disable LightProb because we have a light;
			EnableLightProb(false);
		}
	}

	public override void DestroyCeremonyRoleCamera()
	{
		if (m_UIRoleCamera != null)
		{
			Destroy(m_UIRoleCamera);
			m_UIRoleCamera = null;

			m_CurrentPlayerLayer = GameLayer.Player;
			CommonFunc.SetLayer(cachedGameObject, GameLayer.Player, true, GameLayer.Player_UI);

			EnableLightProb(true);
		}
	}

	/// <summary>
	/// 半身像;
	/// </summary>
	public override Camera CreateUIBustCamera(Camera ca)
	{
		return CreateUIBustCamera( ca, new CameraLevel(CameraLevel.Default), m_CurrentPlayerLayer);
	}

	/// <summary>
	/// 半身像;
	/// </summary>
	public override Camera CreateUIBustCamera( Camera ca, GameLayer targetLayer)
	{
		return CreateUIBustCamera( ca, new CameraLevel(CameraLevel.Default), targetLayer);
	}

	/// <summary>
	/// 半身像;
	/// </summary>
	public override Camera CreateUIBustCamera(Camera ca, CameraLevel camLevel, GameLayer targetLayer)
	{
		if (m_UIBustCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIBustCamera = new GameObject("UIBustCamera");

			mTempTrans = m_UIBustCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(3f, 180, 0);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.55f, 4f);
			ChangeUIFaceCameraPosY();
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIBustCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.SolidColor;
			cr.depth = -2;
			cr.cullingMask = 1 << (int)targetLayer;
			cr.fieldOfView = 11;
			cr.nearClipPlane = camLevel.Level.x;
			cr.farClipPlane = camLevel.Level.y;
			cr.farClipPlane = 5;

			CommonFunc.SetLayer(cachedGameObject, targetLayer, true, GameLayer.Player);
			m_CurrentPlayerLayer = targetLayer;
			
			//create light;
			GameObject Playerlight = new GameObject("Player_UI_LIGHT");
			Light l = Playerlight.AddComponent<Light>();
			l.type = LightType.Directional;
			l.color = Color.white;
			l.intensity = 0.35f;
			l.renderMode = LightRenderMode.ForcePixel;
			l.cullingMask = 1 << (int)targetLayer;
			mTempTrans = l.transform;
			mTempTrans.parent = m_UIBustCamera.transform;
			mTempTrans.localRotation = Quaternion.Euler(13.7f, 327.7f, 334.3f);

			//Disable LightProb because we have a light;
			EnableLightProb(false);

			return cr;
		}
		return m_UIBustCamera.GetComponent<Camera>();
	}

	public override void DestroyUIBustCamera()
	{
		if (m_UIBustCamera != null)
		{
			Destroy(m_UIBustCamera);
			m_UIBustCamera = null;

			EnableLightProb(true);
		}
	}

	public override void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca)
	{
		CreateUIFaceCamera(topLeft, bottomRight, ca, new CameraLevel(CameraLevel.Default), m_CurrentPlayerLayer);
	}

	public override void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer)
	{
		CreateUIFaceCamera(topLeft, bottomRight, ca, new CameraLevel(CameraLevel.Default), targetLayer);
	}

	public override void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer)
	{
		if (m_UIFaceCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIFaceCamera = new GameObject("UIFaceCamera");

			mTempTrans =  m_UIFaceCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(180f, 0f, 180f);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.55f, 1.8f);
			ChangeUIFaceCameraPosY();
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIFaceCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.Nothing;
			cr.depth = ca.depth;
			cr.cullingMask = 1 << (int)targetLayer;
			cr.fieldOfView = 15;
			cr.nearClipPlane = camLevel.Level.x;
			cr.farClipPlane = camLevel.Level.y;

			CommonFunc.SetLayer(cachedGameObject, targetLayer, true, GameLayer.Player);
			m_CurrentPlayerLayer = targetLayer;

			Vector3 tl = ca.WorldToScreenPoint(topLeft);
			Vector3 br = ca.WorldToScreenPoint(bottomRight);
			Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
			if (rect != cr.rect)
			{
				cr.rect = rect;
			}
		}
	}

	public override void CreateUICoupleFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, float angleOffset, bool needLight)
	{
		if (m_UIFaceCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIFaceCamera = new GameObject("UIFaceCamera");

			mTempTrans = m_UIFaceCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(180f, angleOffset, 180f);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.55f, 2.5f);
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIFaceCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.Nothing;
			cr.depth = ca.depth;
			cr.cullingMask = 1 << (int)GameLayer.Player_UI_Secnod;
			cr.fieldOfView = 15;
			CameraLevel tempCamLev = new CameraLevel(CameraLevel.UIPlayer);
			cr.nearClipPlane = tempCamLev.Level.x;
			cr.farClipPlane = tempCamLev.Level.y;

			CommonFunc.SetLayer(cachedGameObject, GameLayer.Player_UI_Secnod, true, GameLayer.Player);
			m_CurrentPlayerLayer = GameLayer.Player_UI_Secnod;

			Vector3 tl = ca.WorldToScreenPoint(topLeft);
			Vector3 br = ca.WorldToScreenPoint(bottomRight);
			Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
			if (rect != cr.rect)
			{
				cr.rect = rect;
			}

			if (needLight)
			{
				//create light;
				GameObject Playerlight = new GameObject("Player_UI_LIGHT");
				Light l = Playerlight.AddComponent<Light>();
				l.type = LightType.Directional;
				l.color = Color.white;
				l.intensity = 0.35f;
				l.renderMode = LightRenderMode.ForcePixel;
				l.cullingMask = 1 << (int)GameLayer.Player_UI_Secnod;
				mTempTrans = l.transform;
				mTempTrans.parent = m_UIFaceCamera.transform;
				mTempTrans.localRotation = Quaternion.Euler(13.7f, 327.7f, 334.3f);
			}
			//Disable LightProb because we have a light;
			EnableLightProb(false);
		}
	}

	public override void DestroyUICoupleFaceCamera()
	{
		if (m_UIFaceCamera != null)
		{
			Destroy(m_UIFaceCamera);
			m_UIFaceCamera = null;

			EnableLightProb(true);
		}
	}

	public override void ShowUIFaceCamera()
	{
		if (m_UIFaceCamera != null)
		{
			m_UIFaceCamera.gameObject.SetActive(true);
		}
	}

	public override void HideUIFaceCamera()
	{
		if (m_UIFaceCamera != null)
		{
			m_UIFaceCamera.gameObject.SetActive(false);
		}
	}

	public override void CreateUIHalfBodyCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, float angleOffset)
	{
		if (m_UIHalfBodyCamera == null)
		{
			if (ca == null)
			{
				ca = Camera.main;
			}

			IsToShow = true;

			m_UIHalfBodyCamera = new GameObject("UIHalfBodyCamera");

			mTempTrans = m_UIHalfBodyCamera.transform;
			Quaternion q = mTempTrans.localRotation;
			q.eulerAngles = new Vector3(180f, angleOffset, 180f);

			mTempTrans.parent = cachedTransform;
			mTempTrans.localPosition = new Vector3(0f, 1.4f, 4f);
			ChangeUIHalfBodyCameraPosY();
			mTempTrans.localRotation = q;
			mTempTrans.localScale = Vector3.zero;

			Camera cr = m_UIHalfBodyCamera.AddComponent<Camera>();
			cr.clearFlags = CameraClearFlags.Nothing;
			cr.depth = ca.depth;
			cr.cullingMask = 1 << (int)m_CurrentPlayerLayer;
			cr.fieldOfView = 15;
			CameraLevel camLevel = new CameraLevel(CameraLevel.UIPlayer);
			cr.nearClipPlane = camLevel.Level.x;
			cr.farClipPlane = camLevel.Level.y;

			Vector3 tl = ca.WorldToScreenPoint(topLeft);
			Vector3 br = ca.WorldToScreenPoint(bottomRight);
			Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
			if (rect != cr.rect)
			{
				cr.rect = rect;
			}
		}
	}

	public override void DestroyUIFaceCamera()
	{
		if (m_UIFaceCamera != null)
		{
			Destroy(m_UIFaceCamera);
			m_UIFaceCamera = null;
		}
	}

	public override void DestroyUIHalfBodyCamera()
	{
		if (m_UIHalfBodyCamera != null)
		{
			Destroy(m_UIHalfBodyCamera);
			m_UIHalfBodyCamera = null;
		}
	}

	public override void UpdateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca)
	{
		if (m_UIFaceCamera != null)
		{
			Camera cr = m_UIFaceCamera.GetComponent<Camera>();
			if (cr != null && ca != null)
			{
				Vector3 tl = ca.WorldToScreenPoint(topLeft);
				Vector3 br = ca.WorldToScreenPoint(bottomRight);

				Rect rect = new Rect(tl.x / Screen.width, br.y / Screen.height, (br.x - tl.x) / Screen.width, (tl.y - br.y) / Screen.height);
				if (rect != cr.rect)
				{
					cr.rect = rect;
				}
			}
		}
	}

	private void ShowHandEffect(bool bShow)
	{
		if (m_RoleBone != null)
		{
			TrailRenderer leftHandRen = m_RoleBone.m_LeftHandBone.GetComponent<TrailRenderer>();
			TrailRenderer rightHandRen = m_RoleBone.m_RightHandBone.GetComponent<TrailRenderer>();

			if (leftHandRen != null)
			{
				leftHandRen.enabled = bShow;
			}

			if (rightHandRen != null)
			{
				rightHandRen.enabled = bShow;
			}
		}
	}

	private void ReplaceBodyPartSync(uint itemType, ItemCloth_Type partType, string partResName, ItemCloth_Type[] arExcludeType, bool bShowLoading)
	{
		StartCoroutine(LoadBodyPartAsync(itemType, partType, partResName, arExcludeType, bShowLoading));
	}

	public override void RevertBodyPartSync(ItemCloth_Type partType, bool bShowLoading)
	{
		StartCoroutine(UnloadBodyPartAsync(partType, false, bShowLoading));
	}

	public override void RevertBodySync(bool bShowLoading)
	{
		StartCoroutine(RevertBodyAsync(bShowLoading));
		StartCoroutine(RevertClothEffectAsync());
	}

	public override void AddEffectPartSync(ushort geneID, int effectID, int effectPriority, bool bShowLoading)
	{
		StartCoroutine(LoadEffectPartAsync(geneID, effectID, effectPriority, bShowLoading));
	}

	public override void RemoveEffectPartSync(ushort geneID, bool bShowLoading)
	{
		StartCoroutine(UnloadEffectPartAsync(geneID, bShowLoading));
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

		itor = RevertClothEffectAsync();
		while (itor.MoveNext())
		{
			yield return null;
		}
		
		StartDownLoadCloth();

		EndHandling();
	}

	/// <summary>
	/// 创建座驾
	/// </summary>
	IEnumerator CreateVehicle()
	{
		IEnumerator itor = RevertVehicleAsync(false);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	IEnumerator CreateEffect()
	{
		IEnumerator itor = RevertEffectAsync(false);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}


	/// <summary>
	/// 将当前的player添加的本地player控制器
	/// </summary>
	void AddPlayerToPlayerControl()
	{
		PlayerManager.AddPlayerToControl(this);
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

	void AddNpcGuideBody(ref RoleBodyLoader[] arrAttachLoader)
	{
		string equipStr = SystemSetting.GetNpcEquip("headwear_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Cap,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Cap, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("hair_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Hair, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("face_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Face, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("shoulders_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Shoulders,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Shoulders, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("gloves_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Gloves, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("righthand_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_RightHand,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_RightHand, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("cloth_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Suit,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Suit, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNpcEquip("shoes_npc");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Feet, null),
				ref arrAttachLoader);
		}
	}

	void AddNpcMinisterBody(ref RoleBodyLoader[] arrAttachLoader)
	{
		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("hair_minister"), ItemCloth_Type.ItemCloth_Type_Hair, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("face_minister"), ItemCloth_Type.ItemCloth_Type_Face, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Body,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("coat_minister"), ItemCloth_Type.ItemCloth_Type_Body, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("shoes_minister"), ItemCloth_Type.ItemCloth_Type_Feet, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Leg,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("leg_minister"), ItemCloth_Type.ItemCloth_Type_Leg, null),
			ref arrAttachLoader);

		AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves,
			GetRoleBodyLoader(SystemSetting.GetWeddingProp("gloves_minister"), ItemCloth_Type.ItemCloth_Type_Gloves, null),
			ref arrAttachLoader);
	}

	void AddNpcDancePartnerBody(ref RoleBodyLoader[] arrAttachLoader)
	{
		string equipStr = SystemSetting.GetNoviceProp("hair_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Hair, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNoviceProp("face_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Face, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNoviceProp("coat_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Body,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Body, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNoviceProp("shoes_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Feet, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNoviceProp("leg_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Leg,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Feet, null),
				ref arrAttachLoader);
		}

		equipStr = SystemSetting.GetNoviceProp("gloves_minister");
		if (!string.IsNullOrEmpty(equipStr))
		{
			AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves,
				GetRoleBodyLoader(equipStr, ItemCloth_Type.ItemCloth_Type_Gloves, null),
				ref arrAttachLoader);
		}
	}

	void AddNpcLanternBody(ref RoleBodyLoader[] arrAttachLoader)
	{
		LanternNpcData npcInfo = StaticData.LanternMgr.GetLanternNpcInfoByID(m_NpcID);
		if (npcInfo != null)
		{
			if (npcInfo.m_Trans != 0)
			{
				AddRoleBody(ItemCloth_Type.ItemCloth_Type_Transform, GetRoleBodyLoader(npcInfo.m_Trans), ref arrAttachLoader);
			}
			else
			{
				if (npcInfo.m_Hair != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair, GetRoleBodyLoader(npcInfo.m_Hair), ref arrAttachLoader);
				}

				if (npcInfo.m_Face != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face, GetRoleBodyLoader(npcInfo.m_Face), ref arrAttachLoader);
				}

				if (npcInfo.m_Body != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Body, GetRoleBodyLoader(npcInfo.m_Body), ref arrAttachLoader);
				}

				if (npcInfo.m_Gloves != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves, GetRoleBodyLoader(npcInfo.m_Gloves), ref arrAttachLoader);
				}

				if (npcInfo.m_Leg != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Leg, GetRoleBodyLoader(npcInfo.m_Leg), ref arrAttachLoader);
				}

				if (npcInfo.m_Cap != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Cap, GetRoleBodyLoader(npcInfo.m_Cap), ref arrAttachLoader);
				}

				if (npcInfo.m_Facial != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Facial_Content, GetRoleBodyLoader(npcInfo.m_Facial), ref arrAttachLoader);
				}

				if (npcInfo.m_Shoulders != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Shoulders, GetRoleBodyLoader(npcInfo.m_Shoulders), ref arrAttachLoader);
				}

				if (npcInfo.m_Wing != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Wing, GetRoleBodyLoader(npcInfo.m_Wing), ref arrAttachLoader);
				}

				if (npcInfo.m_LeftHand != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_LeftHand, GetRoleBodyLoader(npcInfo.m_LeftHand), ref arrAttachLoader);
				}

				if (npcInfo.m_RightHand != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_RightHand, GetRoleBodyLoader(npcInfo.m_RightHand), ref arrAttachLoader);
				}

				if (npcInfo.m_Wrist != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Wrist, GetRoleBodyLoader(npcInfo.m_Wrist), ref arrAttachLoader);
				}

				if (npcInfo.m_Hip != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hip, GetRoleBodyLoader(npcInfo.m_Hip), ref arrAttachLoader);
				}

				if (npcInfo.m_Socks != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Socks, GetRoleBodyLoader(npcInfo.m_Socks), ref arrAttachLoader);
				}

				if (npcInfo.m_Feet != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet, GetRoleBodyLoader(npcInfo.m_Feet), ref arrAttachLoader);
				}

				if (npcInfo.m_TwoHandTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooArm, GetRoleBodyLoader(npcInfo.m_TwoHandTatto), ref arrAttachLoader);
				}

				if (npcInfo.m_TwoLegTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooLeg, GetRoleBodyLoader(npcInfo.m_TwoLegTatto), ref arrAttachLoader);
				}

				if (npcInfo.m_BodyTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooBody, GetRoleBodyLoader(npcInfo.m_BodyTatto), ref arrAttachLoader);
				}
			}

			m_nSkinItem = npcInfo.m_NpcSkin;
		}
		else
		{
			AddDefaultBody(false, ref arrAttachLoader);
		}
	}

	private void AddNpcBigMamaBody(ref RoleBodyLoader[] arrAttachLoader)
	{
		BigMamaNpcData npcInfo = StaticData.DanceGroupMgr.GetBigMamaNpcByID(m_NpcID);
		if (npcInfo != null)
		{
			if (npcInfo.m_nTrans != 0)
			{
				AddRoleBody(ItemCloth_Type.ItemCloth_Type_Transform, GetRoleBodyLoader(npcInfo.m_nTrans), ref arrAttachLoader);
			}
			else
			{
				if (npcInfo.m_nHair != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hair, GetRoleBodyLoader(npcInfo.m_nHair), ref arrAttachLoader);
				}

				if (npcInfo.m_nFace != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Face, GetRoleBodyLoader(npcInfo.m_nFace), ref arrAttachLoader);
				}

				if (npcInfo.m_nBody != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Body, GetRoleBodyLoader(npcInfo.m_nBody), ref arrAttachLoader);
				}

				if (npcInfo.m_nGloves != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Gloves, GetRoleBodyLoader(npcInfo.m_nGloves), ref arrAttachLoader);
				}

				if (npcInfo.m_nLeg != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Leg, GetRoleBodyLoader(npcInfo.m_nLeg), ref arrAttachLoader);
				}

				if (npcInfo.m_nCap != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Cap, GetRoleBodyLoader(npcInfo.m_nCap), ref arrAttachLoader);
				}

				if (npcInfo.m_nFacial != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Facial_Content, GetRoleBodyLoader(npcInfo.m_nFacial), ref arrAttachLoader);
				}

				if (npcInfo.m_nShoulders != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Shoulders, GetRoleBodyLoader(npcInfo.m_nShoulders), ref arrAttachLoader);
				}

				if (npcInfo.m_nWing != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Wing, GetRoleBodyLoader(npcInfo.m_nWing), ref arrAttachLoader);
				}

				if (npcInfo.m_nLeftHand != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_LeftHand, GetRoleBodyLoader(npcInfo.m_nLeftHand), ref arrAttachLoader);
				}

				if (npcInfo.m_nRightHand != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_RightHand, GetRoleBodyLoader(npcInfo.m_nRightHand), ref arrAttachLoader);
				}

				if (npcInfo.m_nWrist != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Wrist, GetRoleBodyLoader(npcInfo.m_nWrist), ref arrAttachLoader);
				}

				if (npcInfo.m_nHip != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Hip, GetRoleBodyLoader(npcInfo.m_nHip), ref arrAttachLoader);
				}

				if (npcInfo.m_nSocks != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Socks, GetRoleBodyLoader(npcInfo.m_nSocks), ref arrAttachLoader);
				}

				if (npcInfo.m_nFeet != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_Feet, GetRoleBodyLoader(npcInfo.m_nFeet), ref arrAttachLoader);
				}

				if (npcInfo.m_TwoHandTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooArm, GetRoleBodyLoader(npcInfo.m_TwoHandTatto), ref arrAttachLoader);
				}

				if (npcInfo.m_TwoLegTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooLeg, GetRoleBodyLoader(npcInfo.m_TwoLegTatto), ref arrAttachLoader);
				}

				if (npcInfo.m_BodyTatto != 0)
				{
					AddRoleBody(ItemCloth_Type.ItemCloth_Type_TattooBody, GetRoleBodyLoader(npcInfo.m_BodyTatto), ref arrAttachLoader);
				}
			}

			m_nSkinItem = npcInfo.m_nNpcSkin;
		}
		else
		{
			AddDefaultBody(false, ref arrAttachLoader);
		}
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

	public override IEnumerator LoadBodyPartAsync(uint itemType, ItemCloth_Type partType, string partResName, ItemCloth_Type[] arExcludeType, bool bShowLoading)
	{
		if (ItemCloth_Type.ItemCloth_Type_Skin == partType)
		{
			_LoadBodySkinSync(itemType);
		}
		else
		{
			IEnumerator itor = _LoadBodyPartAsync(partType, partResName, arExcludeType, bShowLoading);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		if (m_RoleStyle != null && m_RoleStyle.OwnerPlayer != null)
		{
			m_RoleStyle.ChangeMoveAnimation(itemType, partType);
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
		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(true);
		}

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
					if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
					{
						Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
					}
					else
					{
						//可能出现 网络服饰切换到本地服饰.帘子应该关掉;
						Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
					}

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

		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(false);
		}
	}

	public override IEnumerator UnloadBodyPartAsync(ItemCloth_Type clothType, bool bReinit, bool bShowLoading)
	{
		if (ItemCloth_Type.ItemCloth_Type_Skin == clothType)
		{
			_UnloadBodySkinSync(bReinit);
		}
		else
		{
			IEnumerator itor = _UnloadBodyPartAsync(clothType, bReinit, bShowLoading);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		if (m_RoleStyle != null && m_RoleStyle.OwnerPlayer != null && m_RoleStyle.OwnerPlayer.RoleItem != null)
		{
			CEquipItem itemInfo = m_RoleStyle.OwnerPlayer.RoleItem.GetCurrentEquip(clothType);
			m_RoleStyle.ChangeMoveAnimation(clothType, itemInfo);
		}
	}

	private IEnumerator _UnloadBodyPartAsync(ItemCloth_Type clothType, bool bReinit, bool bShowLoading)
	{
		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(true);
		}

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
				if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
				{
					Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
				}
				else
				{
					//可能出现 网络服饰切换到本地服饰.帘子应该关掉;
					Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
				}

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

		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(false);
		}
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

	public override IEnumerator RevertBodyAsync(bool bShowLoading)
	{
		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(true);
		}

		while (!BeginHandling())
		{
			yield return null;
		}

		for (int i = 0; i < (int)ItemCloth_Type.ItemCloth_Type_MaxNumber; i++)
		{
			ReleaseAsyncBodyLoader((ItemCloth_Type)i);
		}

		List<ItemCloth_Type> releaseTypeList = new List<ItemCloth_Type>();
		List<RoleBodyLoader> attachLoaderList = new List<RoleBodyLoader>();

		for (ItemCloth_Type clothType = ItemCloth_Type.ItemCloth_Type_Hair; clothType < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++clothType)
		{
			if (ItemCloth_Type.ItemCloth_Type_Skin == clothType)
			{
				m_nSkinItem = 0;
				_LoadBodySkinSync(m_nSkinItem);
			}
			else
			{
				//qiangxing charu daima
				if (clothType != ItemCloth_Type.ItemCloth_Type_Transform)
				{
					RoleBodyLoader relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Transform);
					if (relatedLoader != null)
					{
						releaseTypeList.Add(clothType);
						continue;
					}
				}

				if (clothType == ItemCloth_Type.ItemCloth_Type_Body || clothType == ItemCloth_Type.ItemCloth_Type_Leg)
				{
					RoleBodyLoader relatedLoader = GetRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Suit);
					if (relatedLoader != null)
					{
						releaseTypeList.Add(clothType);
						continue;
					}
				}

				RoleBodyLoader bodyLoader = GetRelatedBodyLoader(clothType);
				if (bodyLoader != null)
				{
					if (LoaderNeedProc(bodyLoader))
					{
						attachLoaderList.Add(bodyLoader);
					}
				}
				else
				{
					releaseTypeList.Add(clothType);
				}
			}
		}

		//区分 是加载or下载 队列;
		attachLoaderList = GetAttachRoleBodyLoaderList(attachLoaderList);

		//开始本地加载流程;
		if (attachLoaderList.Count != 0 || CheckHasBodyChange(releaseTypeList))
		{
			if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
			{
				Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
			}
			else
			{
				//可能出现 网络服饰切换到本地服饰.帘子应该关掉;
				Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
			}

			ReleaseAsyncBodyLoader(releaseTypeList);

			IEnumerator itor = LoadAttachRoleBody(attachLoaderList);
			while (itor.MoveNext())
			{
				yield return null;
			}
			ProcessBody(attachLoaderList, releaseTypeList);

			ChangeBodySkin(m_nSkinItem);
		}

		StartDownLoadCloth();

		if (m_RoleStyle != null)
		{
			m_RoleStyle.ChangeMoveAnimation();
		}

		EndHandling();

		if (bShowLoading)
		{
			LoadingMgr.ShowLoading(false);
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

		DestroyUIRoleCamera();
		DestroyUIFaceCamera();

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

	public override void AttachDefaultEffectForStage()
	{
		//m_DefaultFootEffectm_arBodyEffect
		if (m_arBodyEffect[(int)EffectPart.Foot] == null)
		{
			if (m_DefaultFootEffectGo == null)
			{
				GameObject effectAsset = EffectLoader.DefaultFootEffect;
				if (effectAsset != null)
				{
					m_DefaultFootEffectGo = (GameObject)Instantiate(effectAsset);
				}
			}
			if (m_DefaultFootEffectGo != null)
			{
				mTempTrans = m_DefaultFootEffectGo.transform;
				mTempTrans.parent = RolePivot.PivotFixed;
				mTempTrans.localPosition = Vector3.zero;
				CommonFunc.SetLayer(m_DefaultFootEffectGo, m_CurrentPlayerLayer, true, GameLayer.Player);
			}
		}
	}
	public override void DettachDefaultEffectForStage()
	{
		if (m_DefaultFootEffectGo != null && m_arBodyEffect[(int)EffectPart.Foot] == null)
		{
			m_DefaultFootEffectGo.SetActive(false);
			m_DefaultFootEffectGo.transform.parent = null;
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

	public override void LoadVIPEnterRoomEffect()
	{
		StartCoroutine(LoadVIPEnterRoomEffectAsync());
	}

	public override IEnumerator LoadVIPEnterRoomEffectAsync()
	{
		if (RoleAttr != null)
		{
			if (RoleAttr.IsVIP)
			{
				CVIPPrivilegeInfo infoPrivilege = VIPData.Instance.GetVIPPrivilegeInfoByLevel(RoleAttr.VIPLevel);
				if (infoPrivilege != null)
				{
					// Add enter room effect 
					if (!string.IsNullOrEmpty(infoPrivilege.m_strEnterRoomViewEffe))
					{
						IEnumerator itor = ReplaceVIPEffect(infoPrivilege.m_strEnterRoomViewEffe, EffectPart.Foot);
						while (itor.MoveNext())
						{
							yield return null;
						}
					}
				}
			}
		}
		else
		{
			Debug.Log("Load VIP Enter Room Effect Error: RoleAttr is null");
		}
	}

	public override void UnloadVIPEnterRoomEffect()
	{
		// Remove the enter room effect 
		StartCoroutine(ReplaceVIPEffect(null, EffectPart.None));
	}

	private void UnloadPlayerTitleEffect()
	{
		if (RoleTitle != null)
		{
			RoleTitle.UnloadPlayerTitleEffect();
		}
	}

	public override void RefreshVIPAnimation()
	{
		PlayerStyle style = cachedGameObject.GetComponent<PlayerStyle>();
		if (style != null)
		{
			style.RefreshVIPAnimation();
		}
	}

	public override void RefreshShiningAnimation()
	{
		if (RoleStyle != null)
		{
			RoleStyle.RefreshShiningJewelryAnimation();
		}
	}

	public override void SetBodyScale(float scale)
	{
		cachedTransform.localScale = Vector3.one * m_InitScale * scale;
	}

	public override void ChangeUIFaceCameraPosY()
	{
		if (m_UIFaceCamera != null && m_RoleStyle != null)
		{
			Vector3 pos = m_UIFaceCamera.transform.localPosition;
			if (m_RoleStyle.RealMoveType == PlayerMoveType.Fly)
			{
				pos.y = 1.7f;
			}
			else
			{
				pos.y = 1.55f;
			}
			m_UIFaceCamera.transform.localPosition = pos;
		}
	}

	private void ChangeUIHalfBodyCameraPosY()
	{
		if (m_UIHalfBodyCamera != null && m_RoleStyle != null)
		{
			Vector3 pos = m_UIHalfBodyCamera.transform.localPosition;
			if (m_RoleStyle.RealMoveType == PlayerMoveType.Fly)
			{
				pos.y = 1.55f;
			}
			else
			{
				pos.y = 1.4f;
			}
			m_UIHalfBodyCamera.transform.localPosition = pos;
		}
	}

	public override void OnRefreshTransformID(int newTransformID)
	{
		if (RoleAttr != null)
		{
			int oldTransformID = RoleAttr.TransformID;

			RoleAttr.OnRefreshTransformID(newTransformID);

			if (oldTransformID != newTransformID && !UIMgr.IsUIShowing(UIFlag.ui_mall))
			{
				RevertBodySync(false);
			}

			if (CommonLogicData.IsMainPlayer(RoleAttr.RoleID))
			{
				SyncTransformId();
			}

			RefreshVIPAnimation();
		}
	}

	public override void ChangeClothToTransform(bool transform)
	{
		int transformID = 0;
		if (RoleAttr != null)
		{
			transformID = RoleAttr.TransformID;
		}

		if (transform)
		{
			if (transformID != 0)
			{
				OnRefreshTransformID(0);
			}
		}
		else
		{
			if (transformID != 0)
			{
				StartCoroutine(ReverBodyToNormalImageAsync());
			}
		}
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

	public override IEnumerator ReverBodyToNormalImageAsync()
	{
		while (!BeginHandling())
		{
			yield return null;
		}

		List<ItemCloth_Type> releaseTypeList = new List<ItemCloth_Type>();
		List<RoleBodyLoader> attachLoaderList = new List<RoleBodyLoader>();

		for (ItemCloth_Type clothType = ItemCloth_Type.ItemCloth_Type_Hair; clothType < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++clothType)
		{
			if (ItemCloth_Type.ItemCloth_Type_Skin == clothType)
			{
				m_nSkinItem = 0;
				_LoadBodySkinSync(m_nSkinItem);
			}
			else
			{
				if (clothType != ItemCloth_Type.ItemCloth_Type_Transform)
				{
					RoleBodyLoader relatedLoader = GetNormalRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Transform);
					if (relatedLoader != null)
					{
						releaseTypeList.Add(clothType);
						continue;
					}
				}

				if (clothType == ItemCloth_Type.ItemCloth_Type_Body || clothType == ItemCloth_Type.ItemCloth_Type_Leg)
				{
					RoleBodyLoader relatedLoader = GetNormalRelatedBodyLoader(ItemCloth_Type.ItemCloth_Type_Suit);
					if (relatedLoader != null)
					{
						releaseTypeList.Add(clothType);
						continue;
					}
				}

				RoleBodyLoader bodyLoader = GetNormalRelatedBodyLoader(clothType);
				if (bodyLoader != null)
				{
					if (LoaderNeedProc(bodyLoader))
					{
						attachLoaderList.Add(bodyLoader);
					}
				}
				else
				{
					releaseTypeList.Add(clothType);
				}
			}
		}

		IEnumerator itor = null;

		//区分 是加载or下载 队列;
		attachLoaderList = GetAttachRoleBodyLoaderList(attachLoaderList);

		//开始本地加载流程;
		if (attachLoaderList.Count != 0 || CheckHasBodyChange(releaseTypeList))
		{
			if (CheckDownLoadNeedBrocastDown())//是否需要帘子;
			{
				Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_START, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
			}
			else
			{
				//可能出现 网络服饰切换到本地服饰.帘子应该关掉;
				Messenger<object>.Broadcast(MessangerEventDef.DOWNROLERES_END, RoleAttr.RoleID, MessengerMode.DONT_REQUIRE_LISTENER);
			}

			ReleaseAsyncBodyLoader(releaseTypeList);

			itor = LoadAttachRoleBody(attachLoaderList);
			while (itor.MoveNext())
			{
				yield return null;
			}
			ProcessBody(attachLoaderList, releaseTypeList);

			ChangeBodySkin(m_nSkinItem);
		}
		
		StartDownLoadCloth();

		if (RoleItem != null)
		{
			for (ItemCloth_Type clothType = ItemCloth_Type.ItemCloth_Type_Hair; clothType < ItemCloth_Type.ItemCloth_Type_MaxNumber; ++clothType)
			{
				CEquipItem equipItem = RoleItem.GetNormalRelatedEquip(clothType);
				if (equipItem != null)
				{
					itor = LoadClothEffect(equipItem.m_EffectId, clothType, equipItem.ItemInfo.m_ClothColor);
					while (itor.MoveNext())
					{
						yield return null;
					}
				}
			}
		}

		if (m_RoleStyle != null)
		{
			m_RoleStyle.ChangeMoveAnimation();
		}

		EndHandling();
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

	private void SyncTransformId()
	{
		GameMsg_C2S_PlayerMotion msg = new GameMsg_C2S_PlayerMotion();
		if (RoleAttr != null)
		{
			msg.TransformId = (ushort)RoleAttr.TransformID;
		}
		else
		{
			Debug.Log("Player Motion Error: RoleAttr is null");
		}
		NetworkMgr.SendMsg(msg);
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
			
			if (RoleTitle != null)
			{
				RoleTitle.RefreshPos_TransformTitle();
			}
		}
		else
		{
			Debug.Log("Change Body Size Error: RoleAttr is null");
		}
	}

	public override void PlayTransitionAni()
	{
		GameObject extra = null;
		for (int i = 0; i < m_arBodyExtra.Length; ++i)
		{
			if (CheckTransitionPart((ItemCloth_Type)i))
			{
				extra = m_arBodyExtra[i];
				if (extra != null)
				{
					Vector3 scale = m_arBodyExtraScale[i];
					//Vector3 scale = extra.transform.localScale;
					extra.transform.localScale = new Vector3(0.001f, scale.y, scale.z);

					Hashtable hash = new Hashtable();
					hash.Add("time", 1.67f);
					hash.Add("scale", scale);

					iTween.ScaleTo(extra, hash);
				}
			}
		}
	}

	private bool CheckTransitionPart(ItemCloth_Type clothType)
	{
		if (clothType == ItemCloth_Type.ItemCloth_Type_Wing
			|| clothType == ItemCloth_Type.ItemCloth_Type_Hip)
			return true;

		return false;
	}


	public override void AddLookAt()
	{
		if (mLookAt == null)
		{
			mLookAt = cachedGameObject.AddComponent<XQLookAtForward>();
		}

		if (CSceneBehaviour.Current != null)
		{
			if (mLookAt != null)
			{
				mLookAt.LookTarget = CSceneBehaviour.Current.CameraControl.TargetCamera.transform;
			}
		}
	}

	public override void RemoveLookAt()
	{
		if (mLookAt != null)
		{
			Component.Destroy(mLookAt);
		}

		mLookAt = null;
	}

	private bool CheckDownLoadNeedBrocastDown()
	{
		foreach (KeyValuePair<ItemCloth_Type, RoleBodyLoader> kv in m_dicAsyncBodyLoader)
		{
			if (kv.Value != null)
			{
				if (CheckNeedBrocastDown(kv.Value.ClothType))
				{
					return true;
				}
			}
		}

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