using UnityEngine;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Logic;
using System.Collections.Generic;
using System.Collections;
using LoveDance.Client.Data;

public class PlayerRoomStyle : PlayerStyle
{
    public override PlayerStyleType StyleType
    {
        get
        {
            return PlayerStyleType.Room;
        }
    }

    protected override bool IsExistAni
    {
        get { return AnimationLoader.RoomAniExist; }
    }

    private List<string> mStandAniList = new List<string>();

    private List<AnimationType> mAniTypeList = new List<AnimationType>();	//会随机的动画类型（现在是只有默认动画和闪亮饰品）

    private string NextPlayAniName = null;//下一个将要播放的动画
    private string lastLoadAniName = null;//上一个加载的动画


    public override void OnAwake()
    {
        base.OnAwake();

        m_IntervalDuration = 5;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void RemoveWalkAnimation()
    {
        base.RemoveWalkAnimation();

        if (OwnerAni != null)
        {
            OwnerAni.DestroyClip(AniMoveState.Stand);
        }
    }


    public override void OnBodyCreated()
    {
        base.OnBodyCreated();

        m_Interval = UnityEngine.Random.Range(0, 80000) / (float)10000;
    }

    protected override void InitAnimation()
    {
        if (AnimationLoader.RoomAniExist && OwnerAni != null)
        {
            

            // set animName
            List<string> animationNames = null;
            string[] standVIPs = new string[3];
            string[] standRooms = new string[3];
            if (true)//OwnerPlayer.RoleAttr.IsBoy)
            {
                standVIPs[0] = AnimationLoader.VIP_STAND_BOY_01;
                standVIPs[1] = AnimationLoader.VIP_STAND_BOY_02;
                standVIPs[2] = AnimationLoader.VIP_STAND_BOY_03;
                standRooms[0] = AnimationLoader.StandRoomBoy_B;
                standRooms[1] = AnimationLoader.StandRoomBoy_C;
                standRooms[2] = AnimationLoader.StandRoomBoy_D;
            }


            OwnerAni.AddClip(GetAniClipBySex(Hello), Hello, WrapMode.Once, 3);
            if (true)//IsNeedTransIdleAni)
            {
                animationNames = new List<string>() { standRooms[0], standRooms[1], standRooms[2] };
                AddStandAni(animationNames);
            }


            if (!CommonLogicData.IsMainPlayer(OwnerPlayer.RoleAttr.RoleID))
            {
                OwnerAni.CrossFadeQueued(Hello);
            }
        }
    }

    public override void RandomStandAni()
    {
        if (OwnerAni != null)
        {
            if (mStandAniList.Count > 0)
            {
                string willPlayAniName = GetWillPlayAniName();
                if (OwnerAni.GetClip(willPlayAniName))
                {
                    OwnerAni.CrossFade(willPlayAniName);

                }

                if (OwnerAni.GetClip(AniMoveState.Stand) != null)
                {
                    OwnerAni.Blend(AniMoveState.Stand, 1f);
                }
            }
        }
    }

    public override void BeRemoved()
    {
        base.BeRemoved();

        if (OwnerAni != null)
        {
            OwnerAni.DestroyClip(Hello);
            for (int i = 0; i < mStandAniList.Count; ++i)
            {
                OwnerAni.DestroyClip(mStandAniList[i]);
            }
        }
        mStandAniList.Clear();        
    }

    private void AddStandAni(List<string> aniList)
    {
        mStandAniList.Clear();
        AnimationClip clip = null;
        string clipName = null;
        for (int i = 0; i < aniList.Count; ++i)
        {
            clipName = aniList[i];
            clip = AnimationLoader.GetAnimationClip(clipName);
            if (clip != null)
            {
                mStandAniList.Add(clipName);
                OwnerAni.AddClip(clip, clipName, WrapMode.Once, 2, 0f);
            }
        }
    }

    /// <summary>
    /// 获取将要播放动画名
    /// </summary>
    /// <returns></returns>
    private string GetWillPlayAniName()
    {
        string willPlayAniName = string.Empty;

        if (mAniTypeList.Count > 0)
        {
            if (!string.IsNullOrEmpty(NextPlayAniName))
            {
                if (OwnerAni.GetClip(NextPlayAniName) != null)
                {
                    willPlayAniName = NextPlayAniName;
                    lastLoadAniName = NextPlayAniName;
                    NextPlayAniName = string.Empty;
                }
            }
            else
            {
                int random = UnityEngine.Random.Range(0, mAniTypeList.Count);
                if (mAniTypeList[random] == AnimationType.Default)
                {
                    random = UnityEngine.Random.Range(0, mStandAniList.Count);
                    willPlayAniName = mStandAniList[random];
                }
                
            }

            if (string.IsNullOrEmpty(willPlayAniName))
            {
                willPlayAniName = mStandAniList[0];
            }
        }

        return willPlayAniName;
    }
}
