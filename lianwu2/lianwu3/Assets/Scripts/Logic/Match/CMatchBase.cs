using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Room;


public class CMatchBase : NetMonoBehaviour
{
    static CMatchBase sCurrentMatch = null;
    bool mIsStart = false;
    float mCurrentTime = 0f;
    float mDeltaTime = 0f;

    public static CMatchBase CurrentMatch
    {
        get
        {
            return sCurrentMatch;
        }
    }

    public virtual CMatchStageInfo StageInfo
    {
        get;
        private set;
    }

    public bool IsStart
    {
        get
        {
            return mIsStart;
        }
    }

    public float CurrentTime
    {
        get
        {
            return mCurrentTime;
        }
    }

    public float DeltaTime
    {
        get
        {
            return mDeltaTime;
        }
        set
        {
            mDeltaTime = value;
        }
    }

    CTimeLineQueue m_TimeLineEventQueue = new CTimeLineQueue();

    private int audioTime = 0;
    private int min = 0;
    private int sec = 0;

    void Awake()
    {
        sCurrentMatch = this;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        sCurrentMatch = null;
    }

    void Update()
    {

        if (mIsStart)
        {
            mDeltaTime = Time.deltaTime;
            mCurrentTime += mDeltaTime;
            
            OnUpdate();
        }
    }

    public virtual void OnUpdate()
    {
        UpdateEvent();
    }

    void UpdateEvent()
    {
        while (mCurrentTime > m_TimeLineEventQueue.NextEventTime && m_TimeLineEventQueue.NextEventTime != -1)
        {
            TimeLineElement tle = m_TimeLineEventQueue.Dequeue();
            if (tle != null)
            {
                gameObject.SendMessage(tle.m_functionName, tle, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
    public void AddEvent(TimeLineElement tle)
    {
        m_TimeLineEventQueue.Enqueue(tle);
    }

    public void SortEvent()
    {
        m_TimeLineEventQueue.Sort();
    }

    public void ClearEvent()
    {
        m_TimeLineEventQueue.ClearAll();
    }
    
    public void AddPrepareDance(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnPrepareDance";
        tle.m_Time = ftime;
        AddEvent(tle);
    }

    public void AddStartDance(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnStartDance";
        tle.m_Time = ftime;
        AddEvent(tle);
    }
    
    public void AddRoundOver(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnRoundOver";
        tle.m_Time = ftime;
        AddEvent(tle);
    }
    
    public void AddMatchEnd(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnMatchEnd";
        tle.m_Time = ftime;
        AddEvent(tle);
    }

    public void AddAnimationPlayEvent(float ftime, string strAniName, float speed, bool isBoy)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnPlayAnimation";
        tle.m_Time = ftime;
        tle.pushParam(strAniName);
        tle.pushParam(speed);
        tle.pushParam(isBoy);
        AddEvent(tle);
    }
    public void AddAnimationRemoveEvent(float ftime, string strAniName, bool isBoy)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnRemoveAnimation";
        tle.m_Time = ftime;
        tle.pushParam(strAniName);
        tle.pushParam(isBoy);
        AddEvent(tle);
    }

    public void AddAnimationBlendEvent(float ftime, string strAniName, float fDuration, float s, float speed, bool isBoy)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnBlendAnimation";
        tle.m_Time = ftime;
        tle.pushParam(strAniName);
        tle.pushParam(fDuration);
        tle.pushParam(s);
        tle.pushParam(speed);
        tle.pushParam(isBoy);
        AddEvent(tle);
    }

    public void PrepareMatch(bool isDancer, bool isAutoWithAudioTime, byte[] stageInfo)
    {
        if (StageInfo != null)
        {            
            StageInfo.LoadStageInfo(stageInfo);
        }

        SendMessage("OnMatchPrepare", isDancer, SendMessageOptions.DontRequireReceiver);
    }



    
    public void BeginMatch()
    {
        mIsStart = true;
        SendMessage("OnMatchBegin", SendMessageOptions.DontRequireReceiver);
    }

    protected void OnQueueState(float fStartTime, float BPMScale)
    {
        foreach (AniState ss in AnimationLoader.s_AniStates.Values)
        {
            ss.Speed *= BPMScale;
        }

        //Girl
        QueueAddState(fStartTime, BPMScale, AnimationLoader.DanceAniSequence, false);
        //Boy
        QueueAddState(fStartTime, BPMScale, AnimationLoader.DanceAniSequenceBoy, true);
    }

    void QueueAddState(float fStartTime, float BPMScale, List<string> danceAniSeq, bool isBoy)
    {
        float startStateTime = fStartTime;
        float startDuration = 0;
        float fdurtion = 0;
        float startFrame = 0;

        for (int i = 0; i < danceAniSeq.Count; i++)
        {
            string stateName = danceAniSeq[i];
            if (!AnimationLoader.s_AniStates.ContainsKey(stateName)) continue;
            AniState state = AnimationLoader.s_AniStates[stateName];
            string clipName = state.Motion;
            AnimationClip animClip = AnimationLoader.GetAnimationClip(clipName);

            if (animClip == null)
            {
                Debug.LogError("Dance error, animation can not be null." + clipName);
                continue;
            }

            //AddAnimationPlayEvent(startStateTime, stateName, nLayer);
            if (startDuration != 0)
            {
                if (CheckIfAvailableAni(stateName))
                {
                    AddAnimationBlendEvent(startDuration, clipName, fdurtion, startFrame, state.Speed, isBoy);
                }
                if (i > 0)
                {
                    int tmpIndex = i - 1;
                    AniState revState = AnimationLoader.s_AniStates[AnimationLoader.DanceAniSequence[tmpIndex]];
                    if (CheckIfAvailableAni(revState.Name))
                    {
                        AddAnimationRemoveEvent(startDuration + animClip.length, revState.Motion, isBoy);
                    }
                }
            }
            else
            {
                if (CheckIfAvailableAni(stateName))
                {
                    AddAnimationPlayEvent(startStateTime, clipName, state.Speed, isBoy);
                }
                if (i > 0)
                {
                    int tmpIndex = i - 1;
                    AniState revState = AnimationLoader.s_AniStates[AnimationLoader.DanceAniSequence[tmpIndex]];
                    if (CheckIfAvailableAni(revState.Name))
                    {
                        AddAnimationRemoveEvent(startDuration + animClip.length, revState.Motion, isBoy);
                    }
                }
            }

            AniState nextState = null;
            TranState nextTran = null;
            List<TranState> trans = state.Trans;
            foreach (TranState tran in trans)
            {
                if (tran != null && isBoy == tran.IsBoy && AnimationLoader.s_AniStates.ContainsKey(tran.DestState))
                {
                    nextState = AnimationLoader.s_AniStates[tran.DestState];
                    nextTran = tran;
                    break;
                }
            }

            if (nextState == null)
            {
                foreach (TranState tran in trans)
                {
                    if (tran != null && AnimationLoader.s_AniStates.ContainsKey(tran.DestState))
                    {
                        nextState = AnimationLoader.s_AniStates[tran.DestState];
                        nextTran = tran;
                        break;
                    }
                }
            }

            if (nextState != null && nextTran != null)
            {
                AnimationClip nextAnimClip = AnimationLoader.GetAnimationClip(nextState.Motion);
                if (nextAnimClip != null)
                {
                    if (state.Speed != 0 && nextTran.ExitTime != 0)
                    {
                        startDuration = startStateTime + animClip.length / state.Speed * nextTran.ExitTime;
                        startStateTime = startDuration - nextAnimClip.length / AnimationLoader.s_AniStates[nextTran.DestState].Speed * nextTran.TranOffset;
                        fdurtion = animClip.length * nextTran.TranDuration / state.Speed;

                        startFrame = nextAnimClip.length * nextTran.TranOffset;
                    }
                    else
                    {
                        Debug.LogError("Animation  speed or next ExitTime is zero! Please Check it, aniName is : " + clipName);
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    bool CheckIfAvailableAni(string stateName)
    {
        if (string.IsNullOrEmpty(stateName)) return false;

        if (stateName.ToUpper().Contains("NODANCE"))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 初始化比赛
    /// </summary>
    /// <param name="curUI"></param>
    public IEnumerator InitMatch(UIFlag uiFlag)
    {
        yield return null;
    }
}

