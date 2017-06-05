using UnityEngine;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Logic;
using LoveDance.Client.Logic.Room;
using System.Collections.Generic;
using System.Collections;
using LoveDance.Client.Data;

public class PlayerRoomStyle : PlayerStyle
{
    string Hello
    {
        get
        {
            return "Hello";
        }
    }

    public override PlayerStyleType StyleType
    {
        get
        {
            return PlayerStyleType.Room;
        }
    }

    float m_Interval = 0f;


    private CapsuleCollider mCapsuleCollider = null;
    private List<string> mStandAniList = new List<string>();

    public override void OnAwake()
    {
        //AddDragScript();
        if (OwnerPlayer != null && OwnerPlayer.BodyCreated)
        {
            OnBodyCreated();
        }
    }

    public override void AddWalkAnimation()
    {
        if (AnimationLoader.RoomAniExist && OwnerAni != null)
        {
            if (OwnerPlayer.RoleAttr.IsBoy)
            {
                OwnerAni.AddClip(AnimationLoader.GetAnimationClip(AnimationLoader.StandRoomBoy_A), AniMoveState.Stand.ToString(), WrapMode.Loop, 1, 1f);

            }            
        }
    }

    public override void RemoveWalkAnimation()
    {
        base.RemoveWalkAnimation();

        if (OwnerAni != null)
        {
            OwnerAni.DestroyClip(AniMoveState.Stand.ToString());
        }
    }

    void Update()
    {
        if (OwnerPlayer != null && OwnerPlayer.BodyCreated)
        {
            //在座驾上时不播放房间待机动画
            if (!false)
            {
                m_Interval += Time.deltaTime;
                if (m_Interval >= 10f)
                {
                    m_Interval -= 10f;

                    RandomStandAni();
                }
            }
        }
    }

    bool HasTransformAni()
    {
        bool hasTransformAni = false;
        if (OwnerPlayer != null && OwnerPlayer.RoleTransform.TransformID != 0)
        {
            hasTransformAni = true;
        }

        return hasTransformAni;
    }

    public override void OnBodyCreated()
    {
        base.OnBodyCreated();

        AddLookAt();

        m_Interval = UnityEngine.Random.Range(0, 80000) / (float)10000;

        InitAnimation();
    }

    void InitAnimation()
    {
        if (AnimationLoader.RoomAniExist && OwnerAni != null)
        {
            bool hasTransformAni = HasTransformAni();

            if (OwnerPlayer.RoleAttr.IsBoy)
            {
                OwnerAni.AddClip(AnimationLoader.GetAnimationClip(AnimationLoader.HelloBoy), Hello, WrapMode.Once, 3);
                if (!hasTransformAni)
                {
                    
                        AddStandAni(new List<string>() { AnimationLoader.StandRoomBoy_B,
													AnimationLoader.StandRoomBoy_C,
													AnimationLoader.StandRoomBoy_D});
                    
                }
            }
            

            if (!CommonLogicData.IsMainPlayer(OwnerPlayer.RoleAttr.RoleID) && !HasVehicle)
            {
                OwnerAni.CrossFadeQueued(Hello);
            }
        }
    }
    void RandomStandAni()
    {
        if (OwnerAni != null)
        {
            if (mStandAniList.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, mStandAniList.Count);
                OwnerAni.CrossFade(mStandAniList[random]);
                if (OwnerAni.GetClip(AniMoveState.Stand.ToString()) != null)
                {
                    OwnerAni.Blend(AniMoveState.Stand.ToString(), 1f);
                }
            }
        }
    }

    public override void BeRemoved()
    {
        base.BeRemoved();

        //if (OwnerPlayer != null)
        //{
        //    OwnerPlayer.RemoveLookAt();
        //}

        if (OwnerAni != null)
        {
            OwnerAni.DestroyClip(Hello);
            for (int i = 0; i < mStandAniList.Count; ++i)
            {
                OwnerAni.DestroyClip(mStandAniList[i]);
            }
        }
        mStandAniList.Clear();
        //RemoveDragScript();
        //ReleaseTransformAni();
    }

    void AddLookAt()
    {
        //if (OwnerPlayer != null)
        //{
        //    OwnerPlayer.AddLookAt();
        //}
    }
    

    private IRoomScene GetRoomScene()
    {
        IRoomScene roomScene = null;
        if (CommonLogicData.CurrentSceneBehaviour != null)
        {
            roomScene = CommonLogicData.CurrentSceneBehaviour.gameObject.GetComponent<IRoomScene>();
        }

        return roomScene;
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
}
