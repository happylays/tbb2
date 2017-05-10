using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Data.Setting;
using LoveDance.Client.Logic;
using LoveDance.Client.Logic.Room;
using System.Collections;

public class StageScene : IScenceType
{
    public int DancerCount
    {
        get
        {
            return m_DancerCount;
        }
    }

    int m_DancerCount = 0;
    int[] m_DancerStagePos = null;

    CSceneBehaviour m_SceneBehaviour = null;
    ISceneCamera m_SceneCamera = null;
    IStandPlayerPosition[] m_StandPlayerPosition = null;

    void Awake()
    {
        m_DancerCount = RoomData.DancerCount();
        m_DancerStagePos = new int[CommonDef.MAX_ROOM_PLAYER] { -1, -1, -1, -1 };

        m_SceneBehaviour = GetComponent<CSceneBehaviour>();
        //m_SceneCamera = m_SceneBehaviour.CameraControl;
        m_StandPlayerPosition = m_SceneBehaviour.StandPlayerPosition;
    }

    void Start()
    {
        //m_SceneBehaviour.CameraFixed(PlayerSetting.FixedCamera);
        //m_SceneBehaviour.EmptyScene(PlayerSetting.EmptyScene);
    }

    public void PlayerEnterScene(bool bNewStyle)
    {

        RoomDanceMode danceMode = RoomData.DanceMode;

        if (danceMode == RoomDanceMode.SINGLE)
        {
            if (m_DancerCount > 0 && m_DancerCount <= m_StandPlayerPosition.Length)
            {
                int tmpIndex = m_DancerCount - 1;
                IStandPlayerPosition standPosition = m_StandPlayerPosition[tmpIndex];

                int curCount = 0;
                for (int dancerPos = 0; dancerPos < CommonDef.MAX_ROOM_PLAYER; ++dancerPos)
                {
                    PlayerBase dancerPlayer = RoomData.GetRoomPlayerByPos(dancerPos);
                    if (dancerPlayer != null)
                    {
                        m_DancerStagePos[dancerPos] = curCount;
                        dancerPlayer.transform.position = standPosition.PlayerPosition[curCount].position;

                        if (bNewStyle)
                        {
                            dancerPlayer.CurrentStyle = PlayerStyleType.Stage;
                        }

                        ++curCount;
                    }
                }                
            }
        }
    }

    public override IEnumerator IEPlayerEnterScene(bool bNewStyle)
    {
        PlayerEnterScene(bNewStyle);

        yield return null;
    }
}