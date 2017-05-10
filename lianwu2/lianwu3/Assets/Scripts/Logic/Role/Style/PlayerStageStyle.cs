using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Data.Setting;

public class PlayerStageStyle : PlayerStyle
{
    public override PlayerStyleType StyleType
    {
        get
        {
            return PlayerStyleType.Stage;
        }
    }
    
    int MissLayer
    {
        get
        {
            return 3;
        }
    }
    int StandLayer
    {
        get
        {
            return 2;
        }
    }

    public static int DefaultBpm
    {
        get
        {
            return 120;
        }
    }

    string Miss
    {
        get
        {
            return "Miss";
        }
    }

    string Lose
    {
        get
        {
            return "Lose";
        }
    }

    string Win
    {
        get
        {
            return "Win";
        }
    }

    string DancePrepare
    {
        get
        {
            return "Prepare";
        }
    }

    string DanceStart
    {
        get
        {
            return "Start";
        }
    }

    string DanceStart_HB
    {
        get
        {
            return "DanceStart_HB";
        }
    }

    string Stand_A
    {
        get
        {
            return "Stand_a";
        }
    }

    string Stand_B
    {
        get
        {
            return "Stand_b";
        }
    }

    string Select_A
    {
        get
        {
            return "Select_a";
        }
    }

    string Select_B
    {
        get
        {
            return "Select_b";
        }
    }

    float DefaultAnimationTime
    {
        get
        {
            return 2f;
        }
    }

    bool m_bIsDancer = false;
    float m_fAniScale = 0f;

    AudioSource m_MatchAudio = null;
    
    bool m_IsAfterSelectTime = false;
    bool m_IsSelectTime = false;

    Vector3 m_LastPlayerPos = Vector3.zero;


    public override void OnAwake()
    {
        base.OnAwake();
        m_IntervalDuration = 5.0f;
    }

    public override void OnUpdate()
    {
        if (m_bIsDancer && m_IsSelectTime)
        {
            base.OnUpdate();
        }
    }
    
    public override void OnBodyCreated()
    {
        base.OnBodyCreated();                
    }

    protected override void InitAnimation()
    {
        m_bIsDancer = true;

        if (!m_bIsDancer)
        {
            OwnerPlayer.IsToShow = false;
            m_LastPlayerPos = OwnerPlayer.cachedTransform.position;
            OwnerPlayer.cachedTransform.position = CommonDef.RoleHidePos;
        }
        else
        {
            m_fAniScale = 1;// RoomData.PlaySongBPM / (float)DefaultBpm;

            if (AnimationLoader.StageAniExist && OwnerAni != null)
            {
                string strPreName = getPreNameBySex();
                WrapMode mode = getWrapModeBySongMode(RoomData.PlaySongMode);

                OwnerAni.AddClip(AnimationLoader.GetAnimationClip(AnimationLoader.StartDance), DanceStart, WrapMode.Loop, 2, 1, m_fAniScale);
                OwnerAni.AddClip(GetAniClipBySex(DanceStart_HB), DanceStart_HB, WrapMode.Loop, 2, 1f, m_fAniScale);
                OwnerAni.AddClip(GetAniClipBySex(Miss), Miss, WrapMode.Once, 3, 1f, m_fAniScale);
                OwnerAni.AddClip(GetAniClipBySex(Lose), Lose, WrapMode.Once, 3, 1f);
                OwnerAni.AddClip(GetAniClipBySex(Win), Win, mode, 3, 1f);

                if (AnimationLoader.s_AniStates.ContainsKey(strPreName))
                {
                    AniState preState = AnimationLoader.s_AniStates[strPreName];
                    if (preState != null)
                    {
                        OwnerAni.AddClip(AnimationLoader.GetAnimationClip(preState.Motion), DancePrepare, WrapMode.Once, 3, 0f, preState.Speed * m_fAniScale);
                    }
                    else
                    {
                        Debug.LogError("Add animation failed. AniState can not be null.Name=" + strPreName);
                    }
                }
                else
                {
                    Debug.LogError("Add animation failed. Animation can not find.Name=" + strPreName);
                }

                OwnerAni.CrossFade(DanceStart);
            }
        }
    }

    void OnPrepareDance()
    {
        if (m_bIsDancer && OwnerAni != null)
        {
            OwnerAni.Blend(DancePrepare, 1f, 0.3f);
        }
    }

    void OnStartDance()
    {
        if (m_bIsDancer && OwnerAni != null)
        {
            OwnerAni.Blend(DancePrepare, 0f);
            OwnerAni.Blend(DanceStart, 0f);
        }
    }

    void OnRankAction(BeatResultRank rank)
    {
        if (m_bIsDancer && OwnerAni != null)
        {
            if (rank == BeatResultRank.Miss)
            {
                OwnerAni.CrossFade(Miss);
                OwnerAni.Blend(DanceStart, 1f);
            }
            else
            {
                OwnerAni.Blend(DanceStart, 0f);
            }
        }
    }


    void OnMatchBegin()
    {
        if (m_bIsDancer && OwnerAni != null)
        {
            OwnerAni.Play(DanceStart);
        }
    }

    void OnPlayAnimation(TimeLineElement tle)
    {
        string stateName = (string)tle.GetParamByIndex(0);
        float speed = (float)tle.GetParamByIndex(1);
        bool isBoy = (bool)tle.GetParamByIndex(2);

        if (OwnerPlayer.RoleAttr.DanceSexIsBoy == isBoy && OwnerAni != null)
        {
            AnimationState state = GetAnimationStat(stateName);
            if (state != null)
            {
                state.speed = speed;
                state.wrapMode = WrapMode.Once;
                state.weight = 1;
                state.layer = 0;
                OwnerAni.Play(stateName, PlayMode.StopSameLayer);
            }
        }
    }

    void OnRemoveAnimation(TimeLineElement tle)
    {
        string stateName = (string)tle.GetParamByIndex(0);
        bool isBoy = (bool)tle.GetParamByIndex(1);

        if (OwnerPlayer.RoleAttr.DanceSexIsBoy == isBoy)
        {
            if (OwnerAni != null)
            {
                AnimationState aniState = OwnerAni[stateName];
                if (aniState != null && !OwnerAni.IsPlaying(stateName))
                {
                    OwnerAni.DestroyClip(aniState.clip.name);
                }
            }
        }
    }

    AnimationState GetAnimationStat(string strAnimation)
    {
        AnimationState state = null;

        if (OwnerAni != null)
        {
            state = OwnerAni[strAnimation];
            if (state == null)
            {
                AnimationClip animClip = AnimationLoader.GetAnimationClip(strAnimation);
                if (animClip != null)
                {
                    state = OwnerAni.AddClip(animClip, strAnimation);
                }
            }
        }

        return state;
    }

    void OnBlendAnimation(TimeLineElement tle)
    {
        string stateName = (string)tle.GetParamByIndex(0);
        float duration = (float)tle.GetParamByIndex(1);
        float s = (float)tle.GetParamByIndex(2);
        float speed = (float)tle.GetParamByIndex(3);
        bool isBoy = (bool)tle.GetParamByIndex(4);

        if (OwnerPlayer.RoleAttr.DanceSexIsBoy == isBoy && OwnerAni != null)
        {
            AnimationState state = GetAnimationStat(stateName);
            if (state != null)
            {
                state.speed = speed;
                state.weight = 1;
                state.layer = 0;
                OwnerAni[stateName].time = s;
                OwnerAni.CrossFade(stateName, duration);
            }
        }
    }

    string getPreNameBySex()
    {
        if (OwnerPlayer.RoleAttr.IsBoy)
        {
            return "boy start";
        }
        else
        {
            return "girl start";
        }

    }

    WrapMode getWrapModeBySongMode(SongMode mode)
    {
        switch (mode)
        {
            default:
                return WrapMode.Once;
        }
        return WrapMode.Default;
    }
}
