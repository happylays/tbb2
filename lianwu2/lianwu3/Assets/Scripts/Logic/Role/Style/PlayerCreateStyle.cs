/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:PlayerCreateStyle.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.02.27
					Modify: 
******************************************************************************/

using UnityEngine;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;

public class PlayerCreateStyle : PlayerStyle
{
    public override PlayerStyleType StyleType
    {
        get
        {
            return PlayerStyleType.Create;
        }
    }

    public override bool NeedChatBubble
    {
        get
        {
            return false;
        }
    }
       
	string Stand
	{
		get
		{
			return "Stand";
		}
	}

    string Stand_B
    {
        get
        {
            return "Stand_b";
        }
    }

    string Stand_C
    {
        get
        {
            return "Stand_c";
        }
    }

    string Stand_D
    {
        get
        {
            return "Stand_d";
        }
    }

    public override void OnAwake()
    {
        base.OnAwake();
        m_IntervalDuration = 10;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnBodyCreated()
    {
        base.OnBodyCreated();

        m_Interval = UnityEngine.Random.Range(0, 80000) / (float)10000;
    }

    protected override void InitAnimation()
    {
        if (AnimationLoader.RoleCreateAniExist && OwnerAni != null)
        {
			OwnerAni.AddClip(GetAniClipBySex(Hello), Hello, WrapMode.Once, 3);
			OwnerAni.AddClip(GetAniClipBySex(AniMoveState.Stand.ToString()), AniMoveState.Stand.ToString(), WrapMode.Loop, 1, 1f);
			OwnerAni.AddClip(GetAniClipBySex(Stand_B), Stand_B, WrapMode.Once, 2, 0f);
			OwnerAni.AddClip(GetAniClipBySex(Stand_C), Stand_C, WrapMode.Once, 2, 0f);
			OwnerAni.AddClip(GetAniClipBySex(Stand_D), Stand_D, WrapMode.Once, 2, 0f);            
        }
    }

	void PlaySelectAnimation()
	{
		if (OwnerAni != null)
		{
			OwnerAni.Stop();
			OwnerAni.CrossFade(Hello);
		}

		m_Interval = UnityEngine.Random.Range(0, 80000) / (float)10000;
	}

    void StopAnimation()
    {
        m_Interval = 10f;
    }

    public override void RandomStandAni()
    {
		if (OwnerAni != null)
		{
			int random = UnityEngine.Random.Range(0, 3);
			if (random == 0)
			{
				OwnerAni.CrossFade(Stand_B);
				OwnerAni.Blend(Stand, 1f);
			}
			else if (random == 1)
			{
				OwnerAni.CrossFade(Stand_C);
				OwnerAni.Blend(Stand, 1f);
			}
			else
			{
				OwnerAni.CrossFade(Stand_D);
				OwnerAni.Blend(Stand, 1f);
			}
		}
    }

	public override void BeRemoved()
	{
		base.BeRemoved();

		if (OwnerAni != null)
		{
			OwnerAni.DestroyClip(Hello);
			OwnerAni.DestroyClip(Stand_B);
			OwnerAni.DestroyClip(Stand_C);
			OwnerAni.DestroyClip(Stand_D);
		}
	}

    public void OnSelectPlayer(bool selected)
    {
        if (selected)
        {
            PlaySelectAnimation();
        }
        else
        {
            StopAnimation();
        }
    }

    public override void AddWalkAnimation() { }
}