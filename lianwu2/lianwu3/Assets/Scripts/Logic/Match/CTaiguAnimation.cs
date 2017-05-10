using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Room;

public class CTaiguAnimation : MonoBehaviour
{

    [SerializeField]
    Camera cameraOpeartor = null;
    [SerializeField]
    CTaiguMatch m_Match = null;

    float mBallSpeed = 0f;

    public float BallSpeed
    {
        set
        {
            mBallSpeed = value;
        }
    }

    public void OnUpdate()
    {

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
        for (int i = 0; i < CommonDef.MAX_ROOM_PLAYER; i++)
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
        //GameObject roundBar = ObtainRoundBar();

        //float timeOffset = m_Match.CurrentTime - tle.m_Time;		// adjust pos caused by time offset
        //if (timeOffset > 0)
        //{
        //    Vector3 v = roundBar.transform.position;
        //    v.x += timeOffset * mBallSpeed;
        //    roundBar.transform.position = v;
        //}

        //mUsingRoundBar.Enqueue(roundBar);
    }

    void OnRoundBall(TimeLineElement tle)
    {
        //TaiguBallType ballType = (TaiguBallType)tle.GetParamByIndex(0);
        //bool isKeyBall = (bool)tle.GetParamByIndex(1);
        //float nKeyTimeSpace = (float)tle.GetParamByIndex(2);

        //TaiguDrumBall drumBall = ObtainDrumBall(ballType, nKeyTimeSpace);
        //drumBall.IsKeyBall = isKeyBall;

        //Vector3 v = drumBall.transform.localPosition;
        //v.x = -1 * tle.m_Time * mBallSpeed * 240.0f;
        //drumBall.transform.localPosition = v;

        //mUsingBalls.Enqueue(drumBall);
    }

    void OnMatchPrepare(bool isDancer)
    {
        //float checkRange = Mathf.Abs(m_CheckEdgeRight.position.x - m_CheckPos.position.x);
        //mBeatResult.InitResult(checkRange);

        //mRoundIndex = 1;
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

}