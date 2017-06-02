using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Room;

public class CTaiguAnimation : MonoBehaviour
{
    public float BallSpeed
    {
        set
        {
            mBallSpeed = value;
        }
    }

    [SerializeField]
    Camera cameraOpeartor = null;
    [SerializeField]
    CTaiguMatch m_Match = null;

    [SerializeField] float m_ThresholdTime = 0.1f;

    [SerializeField] Transform m_BeginPos = null;
    [SerializeField] Transform m_CheckPos = null;
    [SerializeField] Transform m_CheckEdgeRight = null;

    [SerializeField] GameObject m_BallCaptain = null;
    [SerializeField] GameObject m_BallCache = null;

    [SerializeField] TaiguCombo m_BeatCombo = null;

    [SerializeField] GameObject m_RedDrumUp = null;
    [SerializeField] GameObject m_RedDrumDown = null;
    [SerializeField] ParticleSystem m_RedEffect = null;

    [SerializeField] GameObject m_BlueDrumUp = null;
    [SerializeField] GameObject m_BlueDrumDown = null;
    [SerializeField] ParticleSystem m_BlueEffect = null;

    [SerializeField] GameObject m_RoundBarPreb = null;
    [SerializeField] GameObject m_RedBallPreb = null;
    [SerializeField] GameObject m_BlueBallPreb = null;
    [SerializeField] GameObject m_MixBallPreb = null;
    [SerializeField] GameObject m_HoldBallPreb = null;

    [SerializeField] ParticleSystem m_psRed = null;
    [SerializeField] ParticleSystem m_psBlue = null;
    [SerializeField] ParticleSystem m_psMix = null;
    [SerializeField] ParticleSystem m_psYellow = null;
    [SerializeField] ParticleSystem m_psMiss = null;

    [SerializeField] Animation m_HitRedAni = null;
    [SerializeField] Animation m_HitBlueAni = null;
    [SerializeField] Animation m_HitMixAni = null;

    [SerializeField] Animation[] m_BeatRankAni = null;

    [SerializeField] Animation m_ReadyAni = null;
    [SerializeField] Animation m_GoAni = null;
    
    [SerializeField] GameObject m_SpecialParticleNormal = null;
    [SerializeField] GameObject m_SpecialParticle1 = null;
    [SerializeField] GameObject m_SpecialParticle2 = null;

    int mRoundIndex = 1;

    float mBallSpeed = 0f;
    int mBallDepth = 0;

    bool m_bIsInShowTime = false;
    float mLastBeatTime = 0f;
    TaiguBeatType mLastBeatType = TaiguBeatType.Nothing;

    CTaiguBeatResult mBeatResult = new CTaiguBeatResult();

    Animation mPlayingDrumAni = null;
    Animation mPlayingRankAni = null;
    private BeatResultRank m_headRank = BeatResultRank.None;

    Queue<GameObject> mUnuseRoundBar = new Queue<GameObject>();
    Queue<GameObject> mUsingRoundBar = new Queue<GameObject>();

    Queue<TaiguDrumBall> mUnuseRedBalls = new Queue<TaiguDrumBall>();
    Queue<TaiguDrumBall> mUnuseBlueBalls = new Queue<TaiguDrumBall>();
    Queue<TaiguDrumBall> mUnuseMixBalls = new Queue<TaiguDrumBall>();
    Queue<TaiguDrumBall> mUnuseHoldBalls = new Queue<TaiguDrumBall>();
    Queue<TaiguDrumBall> mUsingBalls = new Queue<TaiguDrumBall>();

    private int nHoldIndex = 0;

    public void OnUpdate()
    {
        Vector3 pos = m_BallCaptain.transform.position;
        pos.x = m_BeginPos.position.x + mBallSpeed * m_Match.CurrentTime;
        m_BallCaptain.transform.position = pos;

        CheckExceed();

        if (!Mathf.Approximately(mLastBeatTime, 0f))
        {
            if (mLastBeatTime + m_ThresholdTime < Time.realtimeSinceStartup)
            {
                mLastBeatTime = 0f;
                mLastBeatType = TaiguBeatType.Nothing;
            }
        }
    }

    void OnStartCountDown(TimeLineElement tle)
    {
        //MatchCountDown type = (MatchCountDown)tle.GetParamByIndex(0);
        //if (type == MatchCountDown.Ready)
        //{
        //    m_ReadyAni.gameObject.SetActive(true);
        //    m_ReadyAni.Play();
        //}
        //else if (type == MatchCountDown.Go)
        //{
        //    m_ReadyAni.gameObject.SetActive(false);
        //    m_GoAni.gameObject.SetActive(true);
        //    m_GoAni.Play();
        //}
    }

    void OnPrepareDance(TimeLineElement tle)
    {
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnPrepareDance", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnStartDance(TimeLineElement tle)
    {
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnStartDance", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void OnPlayAnimation(TimeLineElement tle)
    {
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnPlayAnimation", tle, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void OnBlendAnimation(TimeLineElement tle)
    {        
        for (int i = 0; i < 1; i++) //CommonDef.MAX_ROOM_PLAYER
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnBlendAnimation", tle, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnRoundBar(TimeLineElement tle)
    {
        GameObject roundBar = ObtainRoundBar();

        float timeOffset = m_Match.CurrentTime - tle.m_Time;		// adjust pos caused by time offset
        if (timeOffset > 0)
        {
            Vector3 v = roundBar.transform.position;
            v.x += timeOffset * mBallSpeed;
            roundBar.transform.position = v;
        }

        mUsingRoundBar.Enqueue(roundBar);
    }

    void OnRoundBall(TimeLineElement tle)
    {
        TaiguBallType ballType = (TaiguBallType)tle.GetParamByIndex(0);
        bool isKeyBall = (bool)tle.GetParamByIndex(1);
        float nKeyTimeSpace = (float)tle.GetParamByIndex(2);

        TaiguDrumBall drumBall = ObtainDrumBall(ballType, nKeyTimeSpace);
        drumBall.IsKeyBall = isKeyBall;

        Vector3 v = drumBall.transform.localPosition;
        v.x = -1 * tle.m_Time * mBallSpeed * 240.0f;
        drumBall.transform.localPosition = v;

        mUsingBalls.Enqueue(drumBall);
    }

    void OnMatchPrepare(bool isDancer)
    {
        float checkRange = Mathf.Abs(m_CheckEdgeRight.position.x - m_CheckPos.position.x);
        mBeatResult.InitResult(checkRange);

        mRoundIndex = 1;
    }

    void OnMatchBegin()
    {
        //if (m_HitRedAni != null)
        //{
        //    m_HitRedAni["taigu_1_jiantou"].speed = 1f / m_Match.StageInfo.BeatTime;
        //}
        //if (m_HitBlueAni != null)
        //{
        //    m_HitBlueAni["taigu_1_jiantou"].speed = 1f / m_Match.StageInfo.BeatTime;
        //}
        //if (m_HitMixAni != null)
        //{
        //    m_HitMixAni["taigu_1_jiantou"].speed = 1f / m_Match.StageInfo.BeatTime;
        //}

        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; ++i)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnMatchBegin", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnHideUIBegin(TimeLineElement tle)
    {
        if (!m_bIsInShowTime)
        {
            cameraOpeartor.gameObject.SetActive(false);
        }
    }

    void OnHideUIEnd(TimeLineElement tle)
    {
        if (!m_bIsInShowTime)
        {
            if (m_Match.MatchOpen && m_Match.IsDancer)
            {
                cameraOpeartor.gameObject.SetActive(true);
            }
        }
    }

    GameObject ObtainRoundBar()
    {
        GameObject roundBar = null;

        if (mUnuseRoundBar.Count > 0)
        {
            roundBar = mUnuseRoundBar.Dequeue();
            roundBar.SetActive(true);
        }
        else
        {
            roundBar = (GameObject)Instantiate(m_RoundBarPreb, m_RoundBarPreb.transform.position, m_RoundBarPreb.transform.rotation);
            roundBar.layer = (int)GameLayer.UI;
            roundBar.name = "RoundBar";
        }

        Vector3 pos = m_BallCaptain.transform.position;
        pos.x = m_BeginPos.position.x;
        pos.z += 1f;
        roundBar.transform.parent = m_BallCaptain.transform;
        roundBar.transform.localScale = Vector3.one;
        roundBar.transform.position = pos;

        return roundBar;
    }

    void RecycleRoundBar(GameObject roundBar)
    {
        roundBar.transform.parent = m_BallCache.transform;
        roundBar.transform.localPosition = Vector3.zero;
        roundBar.SetActive(false);

        mUnuseRoundBar.Enqueue(roundBar);
    }

    TaiguDrumBall ObtainDrumBall(TaiguBallType ballType, float nKeyTimeSpace)
    {
        TaiguDrumBall drumBall = null;

        switch (ballType)
        {
            case TaiguBallType.Red:
                if (mUnuseRedBalls.Count > 0)
                {
                    drumBall = mUnuseRedBalls.Dequeue();
                    drumBall.gameObject.SetActive(true);
                }
                else
                {
                    drumBall = CreateRedBall();
                }
                break;
            case TaiguBallType.Blue:
                if (mUnuseBlueBalls.Count > 0)
                {
                    drumBall = mUnuseBlueBalls.Dequeue();
                    drumBall.gameObject.SetActive(true);
                }
                else
                {
                    drumBall = CreateBlueBall();
                }
                break;
            case TaiguBallType.RBMix:
                if (mUnuseMixBalls.Count > 0)
                {
                    drumBall = mUnuseMixBalls.Dequeue();
                    drumBall.gameObject.SetActive(true);
                }
                else
                {
                    drumBall = CreateMixBall();
                }
                break;
            case TaiguBallType.HoldBegin:
                if (mUnuseHoldBalls.Count > 0)
                {
                    drumBall = mUnuseHoldBalls.Dequeue();
                    drumBall.ResizeHoldBall(nKeyTimeSpace * mBallSpeed * 240.0f);
                    drumBall.gameObject.SetActive(true);
                }
                else
                {
                    drumBall = CreateHoldBall(nKeyTimeSpace);
                }
                break;
        }

        Vector3 pos = m_BallCaptain.transform.position;
        pos.x = m_BeginPos.position.x;
        pos.z += mBallDepth * 0.001f;
        drumBall.transform.parent = m_BallCaptain.transform;
        drumBall.transform.localScale = Vector3.one;
        drumBall.transform.position = pos;
        mBallDepth++;

        return drumBall;
    }


    TaiguDrumBall CreateRedBall()
    {
        GameObject go = null;
        go = (GameObject)Instantiate(m_RedBallPreb, m_RedBallPreb.transform.position, m_RedBallPreb.transform.rotation);
        go.layer = (int)GameLayer.UI;
        go.name = "Red";

        TaiguDrumBall drumBall = go.GetComponent<TaiguDrumBall>();
        drumBall.ResizeHoldBall(0);
        return drumBall;
    }

    TaiguDrumBall CreateBlueBall()
    {
        GameObject go = null;
        go = (GameObject)Instantiate(m_BlueBallPreb, m_BlueBallPreb.transform.position, m_BlueBallPreb.transform.rotation);
        go.layer = (int)GameLayer.UI;
        go.name = "Blue";

        TaiguDrumBall drumBall = go.GetComponent<TaiguDrumBall>();
        drumBall.ResizeHoldBall(0);
        return drumBall;
    }

    TaiguDrumBall CreateMixBall()
    {
        GameObject go = null;
        go = (GameObject)Instantiate(m_MixBallPreb, m_MixBallPreb.transform.position, m_MixBallPreb.transform.rotation);
        go.layer = (int)GameLayer.UI;
        go.name = "Mix";

        TaiguDrumBall drumBall = go.GetComponent<TaiguDrumBall>();
        drumBall.ResizeHoldBall(0);
        return drumBall;
    }

    TaiguDrumBall CreateHoldBall(float nKeyTimeSpace)
    {
        GameObject go = null;
        go = (GameObject)Instantiate(m_HoldBallPreb, m_HoldBallPreb.transform.position, m_HoldBallPreb.transform.rotation);
        go.layer = (int)GameLayer.UI;
        go.name = "Hold" + nHoldIndex.ToString();	//	给实例命名方便调试;
        nHoldIndex++;

        TaiguDrumBall drumBall = go.GetComponent<TaiguDrumBall>();
        drumBall.ResizeHoldBall(nKeyTimeSpace * mBallSpeed * 240.0f);
        return drumBall;
    }

    // Beat---------------------------------------------------------
    void CheckExceed()
    {
        GameObject roundBar = CurrentLeadRoundBar();
        if (roundBar != null)
        {
            if (roundBar.transform.position.x > m_CheckEdgeRight.position.x)
            {
                mUsingRoundBar.Dequeue();
                RecycleRoundBar(roundBar);
            }
        }

        TaiguDrumBall drumBall = CurrentLeadBall();
        if (drumBall != null)
        {
            if (drumBall.EndBall.position.x > m_CheckEdgeRight.position.x)
            {
                float checkOffset = drumBall.transform.position.x - m_CheckPos.position.x;
                if (drumBall.BallType != TaiguBallType.HoldBegin)
                {
                    HandleBeat(true, checkOffset, m_headRank, true);
                }
                else
                {
                    if (m_headRank == BeatResultRank.Miss || m_headRank == BeatResultRank.Bad || m_headRank == BeatResultRank.None)
                    {
                        mBeatResult.AddBeatCheck(0, BeatResultRank.Miss);
                        SendMessage("OnBeatResult", BeatResultRank.Miss, SendMessageOptions.RequireReceiver);
                        SendMessage("OnBeatCombo", mBeatResult.ComnoCount.Value, SendMessageOptions.DontRequireReceiver);
                        PlayHitAniSuper(TaiguBallType.HoldBegin, BeatResultRank.Miss);
                    }
                    drumBall = mUsingBalls.Dequeue();

                    if (drumBall.IsKeyBall)
                    {
                        ///m_Match.HandleRoundMark(mRoundIndex, mBeatResult.LatestRoundMark, mBeatResult.LatestBeatRank, mBeatResult.LatestRoundRank);

                        ++mRoundIndex;
                        mBeatResult.StartNewRound();
                    }

                    RecycleDrumBall(drumBall);
                }
                m_headRank = BeatResultRank.None;
            }
        }
    }

    GameObject CurrentLeadRoundBar()
    {
        if (mUsingRoundBar.Count > 0)
        {
            return mUsingRoundBar.Peek();
        }

        return null;
    }

    TaiguDrumBall CurrentLeadBall()
    {
        if (mUsingBalls.Count > 0)
        {
            return mUsingBalls.Peek();
        }

        return null;
    }

    TaiguBeatType FixBeatType(TaiguBeatType beatType)
    {
        if (beatType == TaiguBeatType.Left || beatType == TaiguBeatType.Right)
        {
            TaiguDrumBall drumBall = CurrentLeadBall();
            if (drumBall != null && drumBall.BallType == TaiguBallType.RBMix)
            {
                float checkOffset = drumBall.transform.position.x - m_CheckPos.position.x;
                if (mBeatResult.InCheckRange(checkOffset))
                {
                    if (mLastBeatType == TaiguBeatType.Nothing)
                    {
                        mLastBeatTime = Time.realtimeSinceStartup;
                        mLastBeatType = beatType;

                        beatType = TaiguBeatType.Ignore;
                    }
                    else if (mLastBeatType != beatType)
                    {
                        mLastBeatTime = 0f;
                        mLastBeatType = TaiguBeatType.Nothing;

                        beatType = TaiguBeatType.LRMix;
                    }
                }
            }
        }

        return beatType;
    }
        

    bool IsBeatValid(TaiguBallType ballType, TaiguBeatType beatType)
    {
        if (ballType == TaiguBallType.Red && beatType == TaiguBeatType.Left)
        {
            return true;
        }

        if (ballType == TaiguBallType.Blue && beatType == TaiguBeatType.Right)
        {
            return true;
        }

        if (ballType == TaiguBallType.RBMix && beatType == TaiguBeatType.LRMix)
        {
            return true;
        }

        if (ballType == TaiguBallType.HoldBegin
            && (beatType == TaiguBeatType.Left || beatType == TaiguBeatType.Right || beatType == TaiguBeatType.LRMix)
            && m_headRank != BeatResultRank.Miss)
        {
            return true;
        }

        return false;
    }

    private BeatResultRank HandleBeat(bool isValid, float checkOffset, BeatResultRank headRank, bool isEnd)
    {
        BeatResultRank curHeadRank = BeatResultRank.None;
        TaiguDrumBall drumBall = null;
        if (isEnd)
        {
            drumBall = mUsingBalls.Dequeue();
        }
        else
        {
            drumBall = mUsingBalls.Peek();
        }

        bool isKeyBall = drumBall.IsKeyBall;
        TaiguBallType ballType = drumBall.BallType;
        Vector3 ballPos = drumBall.transform.position;

#if !MODE_ALLCOMBO
        if (isValid)
        {
            if (isEnd)
            {
                headRank = BeatResultRank.None;
            }
            curHeadRank = mBeatResult.AddBeatCheck(checkOffset, headRank);
        }
        else
        {
            mBeatResult.AddBeatMiss();
            curHeadRank = BeatResultRank.Miss;
        }

        PlayHitAniNormal(ballType, ballPos);
        
#else
		if (RoomData.IsPlaySuperMode)
		{
			PlayHitAniSuper(ballType, curHeadRank);
		}
		else
		{
			PlayHitAniNormal(ballType, ballPos);
		}
		mBeatResult.AddBeatCheck(0,BeatResultRank.None);
#endif

        ///m_Match.HandleBeatMark(mRoundIndex, mBeatResult.LatestBeatMark, mBeatResult.ComnoCount);

        SendMessage("OnBeatResult", (BeatResultRank)mBeatResult.LatestBeatRank.Value, SendMessageOptions.RequireReceiver);
        SendMessage("OnBeatCombo", mBeatResult.ComnoCount.Value, SendMessageOptions.DontRequireReceiver);
        if (isEnd)
        {
            if (isKeyBall)
            {
                ///m_Match.HandleRoundMark(mRoundIndex, mBeatResult.LatestRoundMark, mBeatResult.LatestBeatRank, mBeatResult.LatestRoundRank);

                ++mRoundIndex;
                mBeatResult.StartNewRound();
            }

            RecycleDrumBall(drumBall);
        }
        return curHeadRank;
    }

    void PlayHitAniSuper(TaiguBallType ballType, BeatResultRank beatRank)
    {
        ParticleSystem curPs = null;

        if (beatRank == BeatResultRank.Miss || beatRank == BeatResultRank.None)
        {
            curPs = m_psMiss;
        }
        else
        {
            if (ballType == TaiguBallType.Red)
            {
                curPs = m_psRed;
            }
            else if (ballType == TaiguBallType.Blue)
            {
                curPs = m_psBlue;
            }
            else if (ballType == TaiguBallType.RBMix)
            {
                curPs = m_psMix;
            }
            else if (ballType == TaiguBallType.HoldBegin)
            {
                curPs = m_psYellow;
            }
        }

        if (curPs != null)
        {
            curPs.gameObject.SetActive(true);
            curPs.Stop();
            curPs.Play();
        }
    }

    void PlayHitAniNormal(TaiguBallType ballType, Vector3 hitPos)
    {
        _HideHitAniNormal();

        if (ballType == TaiguBallType.Red)
        {
            mPlayingDrumAni = m_HitRedAni;
        }
        else if (ballType == TaiguBallType.Blue)
        {
            mPlayingDrumAni = m_HitBlueAni;
        }
        else if (ballType == TaiguBallType.RBMix)
        {
            mPlayingDrumAni = m_HitMixAni;
        }

        mPlayingDrumAni.gameObject.SetActive(true);
        mPlayingDrumAni.transform.position = hitPos;
        mPlayingDrumAni.Play();
    }

    void RecycleDrumBall(TaiguDrumBall drumBall)
    {
        drumBall.IsKeyBall = false;
        drumBall.transform.parent = m_BallCache.transform;
        drumBall.transform.localPosition = Vector3.zero;
        drumBall.gameObject.SetActive(false);

        TaiguBallType ballType = drumBall.BallType;
        if (ballType == TaiguBallType.Red)
        {
            mUnuseRedBalls.Enqueue(drumBall);
        }
        else if (ballType == TaiguBallType.Blue)
        {
            mUnuseBlueBalls.Enqueue(drumBall);
        }
        else if (ballType == TaiguBallType.RBMix)
        {
            mUnuseMixBalls.Enqueue(drumBall);
        }
        else if (ballType == TaiguBallType.HoldBegin)
        {
            mUnuseHoldBalls.Enqueue(drumBall);
        }
        else
        {
            if (drumBall != null)
            {
                Destroy(drumBall.gameObject);
            }
        }
    }


    void _HideHitAniSuper()
    {
        m_psRed.gameObject.SetActive(false);
        m_psBlue.gameObject.SetActive(false);
        m_psMix.gameObject.SetActive(false);
        m_psYellow.gameObject.SetActive(false);
    }

    void _HideHitAniNormal()
    {
        if (mPlayingDrumAni != null)
        {
            mPlayingDrumAni.Stop();
            mPlayingDrumAni.gameObject.SetActive(false);
        }
    }

    void OnBeatResult(BeatResultRank rank)
    {
        int aniIndex = (int)rank - 1;
        if (aniIndex < m_BeatRankAni.Length)
        {
            _HidePlayingRankAni();

            mPlayingRankAni = m_BeatRankAni[aniIndex];
            mPlayingRankAni.gameObject.SetActive(true);
            mPlayingRankAni.Play();
        }
    }

    void OnBeatCombo(int comboCount)
    {
        return;

        bool showSpecial = false;
        ContinuousBeatBonus beatBonus = CTaiguMatch.CalcuTaiguComboBonus(comboCount);
        if (beatBonus > ContinuousBeatBonus.None)
        {
            m_BeatCombo.gameObject.SetActive(true);
            m_BeatCombo.ChangeCount(comboCount);
            m_BeatCombo.PlayEffect();
            
            if (beatBonus >= ContinuousBeatBonus.Lv4)
            {
                showSpecial = true;
            }
        }
        else
        {
            m_BeatCombo.gameObject.SetActive(false);
            if (m_SpecialParticle1 != null)
            {
                m_SpecialParticle1.gameObject.SetActive(false);
            }
            if (m_SpecialParticle1 != null)
            {
                m_SpecialParticle2.gameObject.SetActive(false);
            }
        }
        if (m_SpecialParticleNormal != null)
        {
            m_SpecialParticleNormal.gameObject.SetActive(showSpecial);
        }
    }

    void _HidePlayingRankAni()
    {
        if (mPlayingRankAni != null)
        {
            mPlayingRankAni.Stop();
            mPlayingRankAni.gameObject.SetActive(false);
        }
    }


    void OnDrumDown(TaiguBeatType beatType)
    {
        if (beatType == TaiguBeatType.Left)
        {
            m_RedDrumUp.SetActive(false);
            m_RedDrumDown.SetActive(true);
            if (m_RedEffect != null)
            {
                m_RedEffect.Stop();
                m_RedEffect.Play();
            }
        }
        else if (beatType == TaiguBeatType.Right)
        {
            m_BlueDrumUp.SetActive(false);
            m_BlueDrumDown.SetActive(true);
            if (m_BlueEffect != null)
            {
                m_BlueEffect.Stop();
                m_BlueEffect.Play();
            }
        }
        else if (beatType == TaiguBeatType.LRMix)
        {
            m_RedDrumUp.SetActive(false);
            m_BlueDrumUp.SetActive(false);
            m_RedDrumDown.SetActive(true);
            m_BlueDrumDown.SetActive(true);

            if (m_BlueEffect != null)
            {
                m_BlueEffect.Stop();
                m_BlueEffect.Play();
            }
            if (m_RedEffect != null)
            {
                m_RedEffect.Stop();
                m_RedEffect.Play();
            }
        }

        TaiguBeatType actualType = FixBeatType(beatType);
        if (actualType != TaiguBeatType.Ignore)
        {
            CheckBeat(actualType);
        }
    }

    void OnDrumUp(TaiguBeatType beatType)
    {
        if (beatType == TaiguBeatType.Left)
        {
            m_RedDrumDown.SetActive(false);
            m_RedDrumUp.SetActive(true);
        }
        else if (beatType == TaiguBeatType.Right)
        {
            m_BlueDrumDown.SetActive(false);
            m_BlueDrumUp.SetActive(true);
        }
        else if (beatType == TaiguBeatType.LRMix)
        {
            m_RedDrumDown.SetActive(false);
            m_BlueDrumDown.SetActive(false);
            m_RedDrumUp.SetActive(true);
            m_BlueDrumUp.SetActive(true);
        }
    }
    
    void CheckBeat(TaiguBeatType beatType)
    {
        TaiguDrumBall drumBall = CurrentLeadBall();
        if (drumBall != null)
        {
            float checkOffset = drumBall.transform.position.x - m_CheckPos.position.x;

            bool isCheck = false;
            switch (drumBall.BallType)
            {
                case TaiguBallType.HoldBegin:
                    if (drumBall.transform.position.x >= m_CheckPos.position.x && drumBall.EndBall.position.x <= m_CheckPos.position.x)
                    {
                        isCheck = true;
                    }
                    break;
                default:
                    isCheck = mBeatResult.InCheckRange(checkOffset);
                    break;
            }

            if (isCheck)
            {
                bool isValid = IsBeatValid(drumBall.BallType, beatType);

                bool isEnd = true;
                if (drumBall.BallType == TaiguBallType.HoldBegin)
                {
                    isEnd = false;
                }

                BeatResultRank headRank = HandleBeat(isValid, checkOffset, m_headRank, isEnd);
                if (m_headRank == BeatResultRank.None)
                {
                    m_headRank = headRank;
                }

                mLastBeatTime = 0f;
                mLastBeatType = TaiguBeatType.Nothing;
            }
        }
    }

    void OnShowTimeBegin(TimeLineElement tle)
    {
        m_bIsInShowTime = true;
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(i);
            if (player != null)
            {
                player.gameObject.SendMessage("OnRankAction", BeatResultRank.None, SendMessageOptions.DontRequireReceiver);
            }
        }

        //if (m_ShowTimeFx != null)
        //{
        //    m_ShowTimeFx.Play();

        //}
        if (CSceneBehaviour.Current != null)
        {
            CSceneBehaviour.Current.SendMessage("OnBeginShowTime");
        }

        cameraOpeartor.gameObject.SetActive(false);

        _HidePlayingRankAni();

        _HideHitAniNormal();
        
    }

    void OnShowTimeEnd(TimeLineElement tle)
    {
        m_bIsInShowTime = false;
        if (CSceneBehaviour.Current != null)
        {
            CSceneBehaviour.Current.SendMessage("OnEndShowTime");
        }

        if (m_Match.IsDancer)
        {
            cameraOpeartor.gameObject.SetActive(true);
        }

    }
}