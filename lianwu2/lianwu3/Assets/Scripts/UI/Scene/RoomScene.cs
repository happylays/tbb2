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
        int nMaxPlayer = CommonDef.MAX_ROOM_PLAYER + CommonDef.MAX_ROOM_AUDIENCE;

        PlayerBase gtMainPlayer = CommonLogicData.MainPlayer;
        if (gtMainPlayer != null && roomType != CreateRoomType.Normal && RoomData.SelfRoomType == RoleRoomType.Audience)
        {
            gtMainPlayer.CurrentStyle = PlayerStyleType.Room;
        }

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

    IEnumerator UpdateScene(string newTexture, string oldTexture, bool needLoading)
    {
        if (newTexture != oldTexture)
        {
            if (m_RoomOperation != null && m_RoomOperation.SceneRenderer != null)
            {
                IEnumerator itor = null;
                if (needLoading)
                {
                    itor = SwitchingControl.ShowSwitching(true, 100);
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                }

                itor = ExtraLoader.LoadExtraTextureSync(newTexture);
                while (itor.MoveNext())
                {
                    yield return null;
                }

                Texture tex = ExtraLoader.GetExtraTexture(newTexture);
                if (tex != null)
                {
                    m_RoomOperation.SceneRenderer.material.SetTexture("_MainTex", tex);
                }

                if (needLoading)
                {
                    SwitchingControl.HideSwitching();
                }
            }

            if (oldTexture.Length > 0)
            {
                ExtraLoader.ReleaseExtraTexture(oldTexture, null);
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
                StartCoroutine(UpdateScene(newTexture, oldTexture, needLoading));
            }
        }
    }

    public override void ChangePosState(RoleRoomType type, int nPos, RoomPosState state, bool showAni)
    {
        if (m_RoomOperation != null)
        {
            if (type == RoleRoomType.Dancer)
            {
                if (m_RoomOperation.m_DancerPos != null && m_RoomOperation.m_DancerPos.Length > nPos
                    && m_RoomOperation.m_DancerPos[nPos] != null)
                {
                    m_RoomOperation.m_DancerPos[nPos].SetPosState(state, showAni);
                }
            }
            else
            {
                if (m_RoomOperation.m_AudiencePos != null && m_RoomOperation.m_AudiencePos.Length > nPos
                    && m_RoomOperation.m_AudiencePos[nPos] != null)
                {
                    m_RoomOperation.m_AudiencePos[nPos].SetPosState(state, showAni);
                }
            }
        }

    }

    public void ClosePos(int nPos)
    {
        if (m_RoomOperation.m_DancerPos != null && m_RoomOperation.m_DancerPos.Length > nPos
    && m_RoomOperation.m_DancerPos[nPos] != null)
        {
            m_RoomOperation.m_DancerPos[nPos].ClosePos();
        }
    }

    public override void ChangePlayerState(int pos, RoleRoomState state, bool host, bool hasPlayer)
    {
        if (m_RoomOperation.m_DancerPos != null && m_RoomOperation.m_DancerPos.Length > pos
   && m_RoomOperation.m_DancerPos[pos] != null)
        {
            m_RoomOperation.m_DancerPos[pos].SetPlayerState(hasPlayer, host, state);
        }
    }

    public override void SetLightCurtainState(int pos, RoomTeamColor teamColor)
    {
        bool isShow = true;
        if (teamColor == RoomTeamColor.NONE || teamColor == RoomTeamColor.MAX)
            isShow = false;

        if (m_RoomOperation.m_DancerPos != null && m_RoomOperation.m_DancerPos.Length > pos
&& m_RoomOperation.m_DancerPos[pos] != null)
        {
            m_RoomOperation.m_DancerPos[pos].SetLightCurtainState(isShow, teamColor);
        }
    }

    public override GameObject[] GetEventListeners()
    {
        if (m_RoomOperation != null)
        {
            return m_RoomOperation.m_EventListeners;
        }

        return null;
    }

    public override IEnumerator IEPlayerEnterScene(bool bNewStyle)
    {
        PlayerEnterScene(bNewStyle);

        yield return null;
    }
}
