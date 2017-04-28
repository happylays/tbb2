using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Role;

/// <summary>
/// NewPlayer 在DLL中的镜像文件;
/// 把NewPlayer的层级 拉到Dll层,使工程所有位置都可以使用到Player的接口.并在DLL外部实现接口内容;
/// 工程中原来所有NewPlayer对象用 PlayerBase代替;
/// 
/// 注：;
/// 如果需要对NewPlayer做修改;
/// 所有NewPlayer给外部调用的接口,需要在 本类做声明;
/// for DLL中的数据层,逻辑层 以及 unity中的UI层，都可以调用到NewPlayer的接口;
/// 
/// by 黄文叶 time 20151010;
/// </summary> 
/// <see cref="PlayerBase">详细可以联合NewPlayer注释</see>
public abstract class PlayerBase : NetMonoBehaviour
{
    public GameLayer m_CurrentPlayerLayer = GameLayer.Player;

	private Transform mTrans = null;
	private GameObject mGo = null;

	public Transform cachedTransform
	{
		get
		{
			if(mTrans == null)
			{
				mTrans = transform;
			}

			return mTrans;
		}
	}

	public GameObject cachedGameObject
	{
		get
		{
			if (mGo == null)
			{
				mGo = gameObject;
			}

			return mGo;
		}
	}

    public abstract GameObject UIRoleCamera
    {
        get;
        set;
    }

    public abstract bool IsToShow
    {
        get;
        set;
    }

    public abstract PlayerStyleType CurrentStyle
    {
        get;
        set;
    }

    public abstract bool BodyCreated
    {
        get;
    }


    public abstract PlayerStyle RoleStyle
    {
        get;
    }

    public abstract PlayerMoveType PlayerMoveType
    {
        get;
        set;
    }

    public abstract PlayerItem RoleItem
    {
        get;
        set;
    }

    public abstract PlayerPivot RolePivot
    {
        get;
    }

    public abstract PlayerGene RoleGene
    {
        get;
        set;
    }

	public abstract PlayerAttr RoleAttr
	{
		get;
	    set;
	}

	public abstract PlayerTransform RoleTransform
	{
		get;
		set;
	}

    public abstract GameObject RoleBody
    {
        get;
        set;
    }


    public abstract Vector3 RootBonePos { get; }

	public abstract bool IgnoreSync { get; set; }

    public abstract IEnumerator CreateMainPlayerPhysics(PlayerStyleType curStyle);

    public abstract IEnumerator CreatePhysics(bool bTitled, PhysicsType pType);

    public abstract IEnumerator CreatePhysics(bool bTitled, PhysicsType pType, byte npcID);

    public abstract IEnumerator CreatePhysics(bool bTitled, Dictionary<ItemCloth_Type, string> cloths);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer);

	public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer, float fieldOfView);

	public abstract void UpdateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer);

	public abstract void UpdateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer);

    public abstract void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca);

    public abstract void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer);

    public abstract void CreateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer);

	public abstract Camera CreateUIBustCamera(Camera ca);

	public abstract Camera CreateUIBustCamera(Camera ca, GameLayer targetLayer);

	public abstract Camera CreateUIBustCamera(Camera ca, CameraLevel camLevel, GameLayer targetLayer);

	public abstract void DestroyUIBustCamera();
	
	public abstract void CreateUICoupleFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, float angleOffset, bool needLight);

    public abstract void ShowClothEffect(bool show);

	public abstract void ShowTrailEffect(bool isActive);

	public abstract bool HasWristTrailEffect();

    public abstract void HideChatBubble();

    public abstract void InitBodySize(float scale);

    public abstract IEnumerator UnloadBodyPartAsync(ItemCloth_Type clothType, bool bReinit, bool bShowLoading);

    public abstract IEnumerator RevertBodyAsync(bool bShowLoading);

    public abstract void RevertBodySync(bool bShowLoading);

    public abstract void ChangeUIFaceCameraPosY();

    public abstract void SetRoleAttrVIPInfo(bool bIsVIP, ushort nVIPLevel);

    public abstract void ChangeClothEffect();

    public abstract void CreateCeremonyRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel);

    public abstract void DestroyCeremonyRoleCamera();

    public abstract void CreateUIHalfBodyCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, float angleOffset);

    public abstract void OnRefreshTransformID(int newTransformID);

    public abstract void RefreshVIPIcon();

    public abstract void ShowUIFaceCamera();

    public abstract void UpdateUIFaceCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca);

    public abstract void HideUIFaceCamera();

    public abstract void LoadVIPEnterRoomEffect();

    public abstract IEnumerator LoadVIPEnterRoomEffectAsync();

    public abstract void ChangeCurrentLayer(GameLayer targetLayer, GameLayer srcLayer);

    public abstract void EnableLightProb(bool bEnable);

    public abstract void SetBodyScale(float scale);

    public abstract void RefreshVIPAnimation();

	public abstract void RefreshShiningAnimation();

    public abstract void RevertBodyPartSync(ItemCloth_Type partType, bool bShowLoading);

    public abstract void AddEffectPartSync(ushort geneID, int effectID, int effectPriority, bool bShowLoading);

    public abstract IEnumerator RevertEffectAsync(bool bShowLoading);

    public abstract void AttachDefaultEffectForStage();

    public abstract IEnumerator LoadEffectPartAsync(ushort geneID, int effectID, int effectPriority, bool bShowLoading);

    public abstract IEnumerator LoadBodyPartAsync(uint itemType, ItemCloth_Type partType, string partResName, ItemCloth_Type[] arExcludeType, bool bShowLoading);

    public abstract IEnumerator LoadClothEffect(uint effectId, ItemCloth_Type partType, uint colorId);

    public abstract IEnumerator RevertClothEffectAsync();

    public abstract IEnumerator UnloadClothEffect(ItemCloth_Type partType);

    public abstract IEnumerator UnloadEffectPartAsync(ushort geneID, bool bShowLoading);

	public abstract IEnumerator LoadEffectPartByType(uint itemID, uint itemtype);

	public abstract void RemoveEffectPartSync(ushort geneID, bool bShowLoading);

    public abstract void ChangeClothToTransform(bool transform);

    public abstract IEnumerator ReverBodyToNormalImageAsync();

    public abstract void DettachDefaultEffectForStage();

    public abstract GameObject GetBodyExtra(ItemCloth_Type itemClothType);

    public abstract void UnloadVIPEnterRoomEffect();

    public abstract void DestroyUIRoleCamera();

    public abstract void DestroyPlayerImmediate();

    public abstract void DestroyUIHalfBodyCamera();

    public abstract void DestroyUIFaceCamera();
	
	public abstract void DestroyUICoupleFaceCamera();

    public abstract IEnumerator DestroyPlayer();

    public abstract IEnumerator DestroyPlayerDelay(float seconds);
	
	public abstract IEnumerator LoadVehicleAsync(uint vehicleID, bool bShowLoading);
	
	public abstract IEnumerator UnloadVehicleAsync(bool bShowLoading);
	
	public abstract IEnumerator RevertVehicleAsync(bool bShowLoading);
	
	public abstract GameObject GetVehicleMainGo();
	
	public abstract void ShowVehicle(bool show);
	
	public abstract void ChangeRoleCameraField(bool showVehicle);

    public abstract void SetBodyPartState(ItemCloth_Type clothType, bool show);
	
	public abstract void ChangeBodySizeScale(float offset);
	
	public abstract void PlayTransitionAni();

    public abstract void ShowTitle(bool show);

    public abstract void AddLookAt();

    public abstract void RemoveLookAt();

	public abstract void RefreshTransfromTitle();

	public abstract void RefreshPlayerTitle();

	public abstract void SetClothEffectIsVisible(bool state);
}
