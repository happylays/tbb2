
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Data.Setting;
using LoveDance.Client.Common;

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

    bool m_bIsShowTime = false;

    int m_nPlayernCount = 0;
    


    public override void OnUpdate()
    {
#if UNITY_EDITOR
        ProcessInput();
#endif

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
        AddAdjustTime(offset - musicRoundTime);
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
                else
                {
                    if (nEmptyLineCount >= SystemSetting.AdjustRound)
                    {
                        int nOffsetCount = nEmptyLineCount - SystemSetting.AdjustRound;
                        float nHideUIBeginTime = shootTime - ((float)nOffsetCount * musicRoundTime);

                        if (!InShowTime(round, mStageInfo.ShowTimeList))
                        {
                            // Hide UI 
                            AddHideUIBegin(nHideUIBeginTime + fHideUIOffsetTime);
                            // Show UI 
                            AddHideUIEnd(shootTime - fHideUIOffsetTime);
                        }

                        // Adjust time 
                        AddAdjustTime(shootTime - fCountTime);
                    }
                    nEmptyLineCount = 0;

                    AddRoundBar(shootTime);

                    if (roundInfo != null && roundInfo.BallList.Count > 0)
                    {
                        int ballCount = 0;
                        foreach (TaiguBallType ballType in roundInfo.BallList)
                        {
                            if (ballType > TaiguBallType.None && ballType < TaiguBallType.Max)
                            {
                                ballCount++;
                            }
                        }

                        int curCount = 0;
                        float nKeyBeginTime = 0;// hold 的开始时间;
                        float ballInterval = musicRoundTime / roundInfo.BallList.Count;
                        foreach (TaiguBallType ballType in roundInfo.BallList)
                        {
                            if (ballType > TaiguBallType.None && ballType < TaiguBallType.Max)
                            {
                                curCount++;
                            }
                            bool keyBall = (curCount == ballCount ? true : false);
                            switch (ballType)
                            {
                                case TaiguBallType.HoldBegin:
                                    nKeyBeginTime = shootTime;
                                    break;
                                case TaiguBallType.Holding:
                                    break;
                                case TaiguBallType.HoldEnd:
                                    AddRoundBall(nKeyBeginTime, TaiguBallType.HoldBegin, keyBall, shootTime - nKeyBeginTime);
                                    break;
                                default:
                                    if (ballType != TaiguBallType.None)
                                    {
                                        AddRoundBall(shootTime, ballType, keyBall, 0);
                                    }
                                    break;
                            }

                            shootTime += ballInterval;
                        }
                    }
                }
                round++;
            }

            float roundOverTime = shootTime + actualTime + 0.5f;
            AddRoundOver(roundOverTime);
        }

        // showtime 
        foreach (CTaiguShowTime showTime in mStageInfo.ShowTimeList)
        {
            int nStopMissTime = showTime.BeginTime - 2;
            if (nStopMissTime < 0)
            {
                nStopMissTime = 0;
            }
            //AddStopMissForShowTime(offset + musicRoundTime * nStopMissTime);
            AddShowTimeBegin(offset + musicRoundTime * (showTime.BeginTime - 1));
            AddShowTimeEnd(offset + musicRoundTime * (showTime.EndTime - 1));
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

    private bool InShowTime(int round, List<CTaiguShowTime> showTimeList)
    {
        bool inShowTime = false;
        CTaiguShowTime showTime = null;

        for (int i = 0; i < showTimeList.Count; ++i)
        {
            showTime = showTimeList[i];
            if (showTime != null)
            {
                if (round >= showTime.BeginTime && round <= showTime.EndTime)
                {
                    inShowTime = true;
                    break;
                }
            }
        }

        return inShowTime;
    }
    
    void AddHideUIBegin(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnHideUIBegin";
        tle.m_Time = ftime;
        AddEvent(tle);
    }

    void AddHideUIEnd(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "OnHideUIEnd";
        tle.m_Time = ftime;
        AddEvent(tle);
    }

    public void AddAdjustTime(float ftime)
    {
        TimeLineElement tle = new TimeLineElement();
        tle.m_functionName = "AdjustTimeFromAudioSourceTimeNextFrame";
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

    public static ContinuousBeatBonus CalcuTaiguComboBonus(int comboCount)
    {
        ContinuousBeatBonus comboBonus = ContinuousBeatBonus.None;
        if (comboCount >= 80)
        {
            comboBonus = ContinuousBeatBonus.Lv5;
        }
        else if (comboCount >= 50)
        {
            comboBonus = ContinuousBeatBonus.Lv4;
        }
        else if (comboCount >= 30)
        {
            comboBonus = ContinuousBeatBonus.Lv3;
        }
        else if (comboCount >= 10)
        {
            comboBonus = ContinuousBeatBonus.Lv2;
        }
        else if (comboCount >= 5)
        {
            comboBonus = ContinuousBeatBonus.Lv1;
        }

        return comboBonus;
    }


#if UNITY_EDITOR
    public void ProcessInput()
    {
        TaiguBeatType downType = TaiguBeatType.Nothing;
        if (Input.GetKeyDown(KeyCode.F) && Input.GetKeyDown(KeyCode.J))
        {
            downType = TaiguBeatType.LRMix;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                downType = TaiguBeatType.Left;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                downType = TaiguBeatType.Right;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                
            }
        }

        if (downType != TaiguBeatType.Nothing)
        {
            SendMessage("OnDrumDown", downType, SendMessageOptions.DontRequireReceiver);
        }

        TaiguBeatType upType = TaiguBeatType.Nothing;
        if (Input.GetKeyUp(KeyCode.F) && Input.GetKeyUp(KeyCode.J))
        {
            upType = TaiguBeatType.LRMix;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                upType = TaiguBeatType.Left;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                upType = TaiguBeatType.Right;
            }
        }

        if (upType != TaiguBeatType.Nothing)
        {
            SendMessage("OnDrumUp", upType, SendMessageOptions.DontRequireReceiver);
        }
    }
#endif

    void OnShowTimeBegin(TimeLineElement tle)
    {
        m_bIsShowTime = true;
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                ///player.gameObject.SendMessage("OnShowTimeBegin", m_MarkBoard.IsTopMark(i), SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnShowTimeEnd(TimeLineElement tle)
    {
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnEndShowTime", SendMessageOptions.RequireReceiver);
            }
        }
        m_bIsShowTime = false;
        //m_bStopMissForShowTime = false;
    }
}
