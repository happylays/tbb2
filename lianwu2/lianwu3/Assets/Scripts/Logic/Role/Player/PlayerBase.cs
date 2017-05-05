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

    public abstract void InitBodySize(float scale);

    public abstract void ChangeCurrentLayer(GameLayer targetLayer, GameLayer srcLayer);

    public abstract void SetBodyScale(float scale);

        
    public abstract GameObject GetBodyExtra(ItemCloth_Type itemClothType);

    public abstract void DestroyPlayerImmediate();

    public abstract IEnumerator DestroyPlayer();

    public abstract IEnumerator DestroyPlayerDelay(float seconds);
	
    public abstract void SetBodyPartState(ItemCloth_Type clothType, bool show);
    
    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, GameLayer targetLayer);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer);

    public abstract void CreateUIRoleCamera(Vector3 topLeft, Vector3 bottomRight, Camera ca, CameraLevel camLevel, GameLayer targetLayer, float fieldOfView);

}
