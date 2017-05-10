
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Logic.Room;

public class CTaiguMatch : CMatchBase
{
    public override CMatchStageInfo StageInfo
    {
        get
        {
            return mStageInfo;
        }
    }

    [SerializeField]
    CTaiguAnimation m_Ani = null;

    [SerializeField]
    Camera m_OperatorCam = null;

    [SerializeField]
    Transform m_BeginPos = null;
    [SerializeField]
    Transform m_CheckPos = null;

    CTaiguStageInfo mStageInfo = new CTaiguStageInfo();

    int m_nPlayernCount = 0;
    protected bool m_bIsDancer = false;


    public override void OnUpdate()
    {
        if (IsStart)
        {
            m_Ani.OnUpdate();
            ///mLeadDance.OnUpdate(DeltaTime);
        }

        base.OnUpdate();
    }

    void SetMatchEvent()
    {
        float musicRoundTime = mStageInfo.RoundTime;

        // count down
        float offset = mStageInfo.mOffset;
        //AddCountDown(offset - musicRoundTime, MatchCountDown.Ready);
        //AddAdjustTime(offset - musicRoundTime);
        //AddCountDown(offset - 0.3f, MatchCountDown.Go);

        float prepareTime = musicRoundTime * 4;
        if (offset >= prepareTime)
        {
            AddPrepareDance(offset - prepareTime);
        }
        else
        {
            AddPrepareDance(offset - musicRoundTime);
        }

        AddStartDance(mStageInfo.mDanceTime);
        AddMatchEnd(mStageInfo.mMatchTime);
        OnQueueState(mStageInfo.mDanceTime, 1);//RoomData.PlaySongBPM / PlayerStageStyle.DefaultBpm);

        if (m_bIsDancer)
        {
            // ball speed
            float lineLength = Mathf.Abs(m_CheckPos.position.x - m_BeginPos.position.x);
            float actualTime = musicRoundTime / mStageInfo.mKSpeed;
            float actualSpeed = lineLength / actualTime;
            m_Ani.BallSpeed = actualSpeed;

            float shootTime = offset - actualTime;

            // round bar
            float barTime = shootTime - musicRoundTime;
            while (barTime > musicRoundTime)
            {
                AddRoundBar(barTime);
                barTime -= musicRoundTime;
            }

            float fCountTime = 0.5f;
            float fHideUIOffsetTime = 0.1f;
            int nEmptyLineCount = 0;
            int round = 0;
            foreach (TaiguRoundInfo roundInfo in mStageInfo.RoundInfoList)
            {
                if (roundInfo.m_bAllEmpty)
                {
                    shootTime += musicRoundTime; // Directly skip this round 
                    nEmptyLineCount++;
                }
            }

            float roundOverTime = shootTime + actualTime + 0.5f;
            AddRoundOver(roundOverTime);
        }

        SortEvent();
    }

    void AddRoundBall(float ftime, TaiguBallType ballType, bool keyBall, float nTimeSpace)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnRoundBall";
        tle.m_Time = ftime;
        tle.pushParam(ballType);
        tle.pushParam(keyBall);
        tle.pushParam(nTimeSpace);
        AddEvent(tle);
    }

    void AddRoundBar(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnRoundBar";
        tle.m_Time = ftime;
        AddEvent(tle);
    }

    void OnRoundOver()
    {
        m_OperatorCam.gameObject.SetActive(false);
    }

    void OnMatchPrepare(bool isDancer)
    {
        m_bIsDancer = isDancer;

        ///m_MarkBoard.InitBoard();
        m_OperatorCam.gameObject.SetActive(isDancer);

        ///m_btnQuitRoom.SetActive(!m_bIsDancer);

        SetMatchEvent();
    }

    void OnMatchBegin()
    {
        m_nPlayernCount = RoomData.DancerCount();
    }

}
