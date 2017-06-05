using UnityEngine;
using System.Collections;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Data.Scene;
using LoveDance.Client.Data;
using LoveDance.Client.Data.Setting;
using LoveDance.Client.Logic;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Common.Messengers;

public class RoomScene : IRoomScene
{
    IStandPlayerPosition[] m_StandPlayerPosition = null;

    int m_CurScene = -1;
    
    bool[] m_FsmOpen = new bool[CommonDef.MAX_ROOM_PLAYER];

    void Awake()
    {
        Messenger<bool>.AddListener("Room_Player_Position_Change", SyncPosition);

        CSceneBehaviour scene = GetComponent<CSceneBehaviour>();
        m_StandPlayerPosition = scene.StandPlayerPosition;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Messenger<bool>.RemoveListener("Room_Player_Position_Change", SyncPosition);
    }

    void SyncPosition(bool bNewStyle)
    {
        int nMaxPlayer = CommonDef.MAX_ROOM_PLAYER;

        for (int nPos = 0; nPos < nMaxPlayer; nPos++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(nPos);
            if (player != null)
            {
                Transform t = GetPosTransForm(nPos);
                if (t != null)
                {
                    player.cachedTransform.position = t.position;
                }
                else
                {
                    player.cachedTransform.position = new Vector3(1000, 1000, 1000);
                }
            }
        }
    }

    public void PlayerEnterScene(bool bNewStyle)
    {
        ///RoomData.CreateCacheRoomPlayer();

        CreateRoomType roomType = RoomData.RoomType;
        int nMaxPlayer = 1;// CommonDef.MAX_ROOM_PLAYER + CommonDef.MAX_ROOM_AUDIENCE;

        //PlayerBase gtMainPlayer = CommonLogicData.MainPlayer;
        //if (gtMainPlayer != null && roomType != CreateRoomType.Normal)
        //{
        //    gtMainPlayer.CurrentStyle = PlayerStyleType.Room;
        //}

        for (int nPos = 0; nPos < nMaxPlayer; nPos++)
        {
            PlayerBase player = RoomData.GetRoomPlayerByPos(nPos);
            if (player != null)
            {
                if (bNewStyle)
                {
                    player.CurrentStyle = PlayerStyleType.Room;
                }

                Transform t = GetPosTransForm(nPos);
                if (t != null)
                {
                    player.cachedTransform.position = t.position;
                    player.cachedTransform.rotation = t.rotation;
                }
                else
                {
                    player.cachedTransform.position = new Vector3(1000, 1000, 1000);
                }
            }
        }
    }


    public override Transform GetPosTransForm(int nPos)
    {
        if (nPos < m_StandPlayerPosition[0].PlayerPosition.Length)
        {
            return m_StandPlayerPosition[0].PlayerPosition[nPos];
        }
        return null;
    }

    public override void NewPlayerEnter(int nPos, PlayerBase player)
    {
        if (player != null)
        {
            player.CurrentStyle = PlayerStyleType.Room;

            Transform t = GetPosTransForm(nPos);
            if (t != null)
            {
                player.cachedTransform.position = t.position;
                if (nPos >= CommonDef.MAX_ROOM_PLAYER)
                {
                    player.cachedTransform.rotation = t.rotation;
                }
            }
            else
            {
                player.cachedTransform.position = new Vector3(1000, 1000, 1000);
            }
        }
    }

    public override void ChangeScene(int sceneID)
    {
        ChangeScene(sceneID, false);
    }

    public override void ChangeScene(int sceneID, bool needLoading)
    {
        if (sceneID != m_CurScene)
        {
            string newTexture = "";
            string oldTexture = "";

            CSceneInfo sceneInfo = StaticData.SceneDataMgr.GetSceneByID((byte)sceneID);
            if (sceneInfo != null)
            {
                newTexture = sceneInfo.m_strRoomSceneTexture;
            }
            sceneInfo = StaticData.SceneDataMgr.GetSceneByID((byte)m_CurScene);
            if (sceneInfo != null)
            {
                oldTexture = sceneInfo.m_strRoomSceneTexture;
            }

            if (newTexture.Length > 0)
            {
                m_CurScene = sceneID;
                ///StartCoroutine(UpdateScene(newTexture, oldTexture, needLoading));
            }
        }
    }


    public override IEnumerator IEPlayerEnterScene(bool bNewStyle)
    {
        PlayerEnterScene(bNewStyle);

        yield return null;
    }
}
