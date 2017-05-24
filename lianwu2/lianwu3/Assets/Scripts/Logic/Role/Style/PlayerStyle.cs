using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;
using LoveDance.Client.Logic;

public class PlayerStyle : MonoBehaviour
{    
    protected string Hello
    {
        get
        {
            return "Hello";
        }
    }

    public PlayerBase OwnerPlayer
    {
        get
        {
            return m_OwnerPlayer;
        }
    }

	public AnimationCell OwnerAni
	{
		get { return m_OwnerAni; }
	}

    public virtual PlayerStyleType StyleType
    {
        get;
        private set;
    }

    public virtual bool CanMove	////true-can move
    {
        get { return false; }
    }

    public virtual PlayerMoveType StyleMoveType
    {
        get { return PlayerMoveType.Fly; }
    }

    public virtual bool NeedChatBubble
    {
        get { return true; }
    }

    public PlayerMoveType RealMoveType
    {
        get { return m_RealMoveType; }
    }
	
	public uint CurVehicle
	{
		get { return m_nCurVehicleID; }
	}
	
	public bool HasVehicle
	{
		get { return (m_nCurVehicleID != 0); }
	}

	public bool HasAwaked
	{
		get
		{
			return m_HasAwaked;
		}
	}
    protected virtual bool IsExistAni
    {
        get { return false; }
    }
 

    PlayerBase m_OwnerPlayer = null;
	AnimationCell m_OwnerAni = null;

	//飞行
	AnimationCell m_WingAni = null; 
    List<string> m_DynamicFlyAniList = new List<string>();
    PlayerMoveType m_RealMoveType = PlayerMoveType.None;	//Current style move type. Real move type.
    
	// 替换当前人物的动画
    string m_strCurTransformAni = null;
	
	//座驾
	AnimationCell m_VehicleAni = null; 
	List<string> m_DynamicVehicleAniList = new List<string>();
	uint m_nCurVehicleOwner = 0;
	uint m_nCurVehicleID = 0;
	int m_nCurVehiclePos = 0;
	string m_strVehicleStandAniName = "";
	private bool m_HasAwaked = false;	// true 初始化完成

	protected List<string> m_ExtraclipName = new List<string>();	//添加闪亮饰品时添加，add by hanyingjun

    protected float m_Interval = 0f;
    protected float m_IntervalDuration = 0f;

    void Awake()
    {
        m_OwnerPlayer = gameObject.GetComponent<PlayerBase>();

        OnAwake();

		m_HasAwaked = true;
    }

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnBodyCreated()
    {
        m_OwnerAni = new AnimationCell(m_OwnerPlayer.RoleBody.gameObject);

        AddMovingScript();
        //ChangeMoveAnimation();
        //ChangeVehicleAnimation();

        InitAnimation();
    }

    public virtual void OnUpdate() 
    {
        if (OwnerPlayer != null && OwnerPlayer.BodyCreated)
        {
            if (HasVehicle)
            {
                //dateVehicleAnimation();
            }
            else
            {
                m_Interval += Time.deltaTime;
                if (m_Interval >= m_IntervalDuration)
                {
                    m_Interval -= m_IntervalDuration;

                    RandomStandAni();
                }
            }

        }
    }

    public virtual void RandomStandAni() { }

    protected virtual void InitAnimation() {}

    public virtual void OnAwake()
    {
        if (OwnerPlayer != null)// && OwnerPlayer.BodyCreated)
        {
            OnBodyCreated();
        }
        else
        {
            Debug.Log("Body is not create.");
        }

    }

    public virtual void ReAwake()
    {
    }

    public virtual void BeRemoved()
    {
        //RemoveVehicleAnimation();
        //RemoveFlyAnimation();
        RemoveWalkAnimation();
        RemoveTransformWalkAni();
        RemoveMovingScript();
		ReleaseAllExtraAni();
    }

    public virtual void RefreshVIPAnimation()
    {

    }

	public virtual void RefreshShiningJewelryAnimation()
	{
	}

    public virtual void AddWalkAnimation()
    {
    
    }

    public virtual void RemoveWalkAnimation()
    {        
        RemoveTransformWalkAni();
    }

    public virtual void AddMovingScript()
    {

    }

    public virtual void RemoveMovingScript()
    {

    }

	/// <summary>
	/// Add By hanyingjun
	/// Date:2017.1.20
	/// Des:添加单个动画，到额外的动画列表中
	/// </summary>
	/// <param name="clipName"></param>
	/// <returns></returns>
	public IEnumerator AddSingleAnimation(string clipName, WrapMode wrapMode, int layer)
	{
		IEnumerator itor = null;
		if (!string.IsNullOrEmpty(clipName))
		{
			if (!m_ExtraclipName.Contains(clipName))
			{
				m_ExtraclipName.Add(clipName);
				itor = AnimationLoader.LoadSingleAnimation(clipName);
				while (itor.MoveNext())
				{
					yield return null;
				}

				OwnerAni.AddClip(AnimationLoader.GetAnimationClip(clipName), clipName, wrapMode, layer);
			}
		}
	}

    protected virtual void CollectPlayMaker() { }

    /// <summary>
	/// 释放添加的额外动画
	/// </summary>
	private void ReleaseAllExtraAni()
	{
		if (m_ExtraclipName != null)
		{
			for (int i = m_ExtraclipName.Count - 1; i >= 0; i--)
			{
				ReleaseSingleExtraAni(m_ExtraclipName[i]);
			}
		}
	}

	public void ReleaseSingleExtraAni(string clipName)
	{
		OwnerAni.DestroyClip(clipName);
		if (!string.IsNullOrEmpty(clipName))
		{
			AnimationLoader.ReleaseSingleAnimation(clipName);
			m_ExtraclipName.Remove(clipName);
		}
	}

    private void RemoveTransformWalkAni()
    {
        if (!string.IsNullOrEmpty(m_strCurTransformAni))
        {
            if (m_OwnerAni != null)
            {
                m_OwnerAni.DestroyClip(AniMoveState.Stand.ToString());
            }

            StopCoroutine("AddTransformAni");
            AnimationLoader.ReleaseSingleAnimation(m_strCurTransformAni);

            m_strCurTransformAni = null;
        }
    }

    protected AnimationClip GetAniClipBySex(string clipName)
    {
        return AnimationLoader.GetAnimationClip(getLoaderClipNameBySex(clipName));
    }

	private string getLoaderClipNameBySex(string clipName)
    {
        if (OwnerPlayer.RoleAttr.IsBoy)
        {
            switch (clipName)
            {
                case "Hello":
                    if (StyleType == PlayerStyleType.Create)
                        return AnimationLoader.VIP_STAND_BOY_01;
                    else if (StyleType == PlayerStyleType.Handbook)
                        return AnimationLoader.HandbookPreviewPoseBoy;
                    else
                        return AnimationLoader.HelloBoy;
                case "Miss":
                    return AnimationLoader.MissBoy;
                case "DanceStart_HB":
                    return AnimationLoader.HB_StartDance_Boy;
                case "Win":                    
                    return AnimationLoader.WinBoy;                    
                case "Lose":                    
                    return AnimationLoader.LoseBoy;                    
                case "Dress":
                case "DressCurtain":
                    return AnimationLoader.DressBoy;
                case "DressBehind":
                    return AnimationLoader.DressBehind;
                case "Stand":
                    if (StyleType == PlayerStyleType.World)
                        return AnimationLoader.StandWorldBoy;
                    else 
                        return AnimationLoader.StandRoomBoy_A;
                case "Stand_b":
                    return AnimationLoader.StandRoomBoy_B;
                case "Stand_c":
                    return AnimationLoader.StandRoomBoy_C;
                case "Stand_d":
                    return AnimationLoader.StandRoomBoy_D;
                
                default:
                    return null;
            }
        }
        else
        {
            switch (clipName)
            {
                case "Hello":
                    if (StyleType == PlayerStyleType.Create)
                        return AnimationLoader.VIP_STAND_GIRL_01;
                    else if (StyleType == PlayerStyleType.Handbook)
                        return AnimationLoader.HandbookPreviewPoseGirl;
                    else
                        return AnimationLoader.HelloGirl;
                case "Miss":
                    return AnimationLoader.MissGirl;
                case "DanceStart_HB":
                    return AnimationLoader.HB_StartDance_Girl;
                case "Win":
                    return AnimationLoader.WinGirl;
                case "Lose":
                    return AnimationLoader.LoseGirl;  
                case "Dress":
                case "DressCurtain":
                    return AnimationLoader.DressGirl;
                case "DressBehind":
                    return AnimationLoader.DressBehind;
                case "Stand":
                    if (StyleType == PlayerStyleType.World)
                        return AnimationLoader.StandWorldGirl;
                    else
                        return AnimationLoader.StandRoomGirl_A;
                case "Stand_b":
                    return AnimationLoader.StandRoomGirl_B;
                case "Stand_c":
                    return AnimationLoader.StandRoomGirl_C;
                case "Stand_d":
                    return AnimationLoader.StandRoomGirl_D;

                default:
                    return null;
            }
        }

        return null;
    }

}
