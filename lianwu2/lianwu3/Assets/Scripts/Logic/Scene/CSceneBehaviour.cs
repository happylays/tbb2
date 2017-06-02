using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Logic;

[System.Serializable]
public class StandPlayerPosition : IStandPlayerPosition
{
    public Transform[] PlayerPosition
    {
        get { return m_PlayerPosition; }
    }

    public Transform[] m_PlayerPosition;
}

[ExecuteInEditMode]
public class CSceneBehaviour : SceneBehaviourBase
{
    public static CSceneBehaviour Current
    {
        get
        {
            return s_Current;
        }
    }

    public override ISceneCamera CameraControl
    {
        get
        {
            return m_SceneCamera;
        }
    }

    public override IStandPlayerPosition[] StandPlayerPosition
    {
        get
        {
            return m_StandPlayerPosition;
        }
    }

    public Color GlobalColor
    {
        get
        {
            return this.m_GlobalColor;
        }
        set
        {
            m_GlobalColor = value;
            Shader.SetGlobalColor("_DDL_Global_Add_Color", m_GlobalColor);
        }
    }

    public override IScene CurScene
    {
        get
        {
            return m_CurScene;
        }
        set
        {
            m_CurScene = value;
        }
    }


    [SerializeField] CSceneCamera m_SceneCamera = null;
    [SerializeField] StandPlayerPosition[] m_StandPlayerPosition = null;
    [SerializeField] Color m_GlobalColor = new Color(1f, 1f, 1f, 1f);    

    static CSceneBehaviour s_Current = null;

    Dictionary<int, ISceneItem> m_mapAllSceneItem = new Dictionary<int, ISceneItem>();
    Dictionary<byte, ISceneEventItem> m_mapAllSceneEventItem = new Dictionary<byte, ISceneEventItem>();
    IScene m_CurScene = null;

    void Awake()
    {
        s_Current = this;
        //if (m_SceneCamera != null)
        //{
        //    CommonLogicData.CurrentSceneCamera = m_SceneCamera.TargetCamera;
        //}

        CommonLogicData.CurrentSceneBehaviour = this;
    }

    void OnDestroy()
    {
        s_Current = null;
        //CommonLogicData.CurrentSceneCamera = null;
    }

    void Start()
    {
        int uiLayerScope = (1 << (int)GameLayer.UI);
        if (m_SceneCamera != null)
        {
            m_SceneCamera.TargetCamera.cullingMask &= ~uiLayerScope;
        }
        Shader.SetGlobalColor("_DDL_Global_Add_Color", m_GlobalColor);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
            Shader.SetGlobalColor("_DDL_Global_Add_Color", m_GlobalColor);
    }
#endif


    public override void RegisterSceneEventItem(ISceneEventItem item)
    {
        if (item != null)
        {
            if (!m_mapAllSceneEventItem.ContainsKey(item.EventID))
            {
                m_mapAllSceneEventItem.Add(item.EventID, item);
            }
        }
    }

    public override ISceneEventItem GetSceneEventItemByID(byte eventId)
    {
        ISceneEventItem item = null;

        if (m_mapAllSceneEventItem.ContainsKey(eventId))
        {
            item = m_mapAllSceneEventItem[eventId];
        }

        return item;
    }

    public override void RegisterSceneItem(ISceneItem item)
    {
        if (item != null)
        {
            if (!m_mapAllSceneItem.ContainsKey(item.ItemIndex))
            {
                m_mapAllSceneItem.Add(item.ItemIndex, item);
            }
        }
    }

    public override ISceneItem GetSceneObjByIndex(int index)
    {
        ISceneItem item = null;

        if (m_mapAllSceneItem.ContainsKey(index))
        {
            item = m_mapAllSceneItem[index];
        }

        return item;
    }

    public override Dictionary<int, ISceneItem> GetSceneItemMap()
    {
        return m_mapAllSceneItem;
    }

    void OnBeginShowTime()
    {
        //PlayMakerFSM fsm = GetComponent<PlayMakerFSM>();
        //if (fsm != null)
        //{
        //    fsm.SendEvent("OnShowTimeBegin");
        //}

        CameraShowTime();
    }

    void OnEndShowTime()
    {
        //PlayMakerFSM fsm = GetComponent<PlayMakerFSM>();
        //if (fsm != null)
        //{
        //    fsm.SendEvent("OnShowTimeEnd");
        //}

        CameraNoFocus();
    }

    public override void CameraFixed(bool bFix)
    {
        if (m_SceneCamera != null)
        {
            if (bFix)
            {
                m_SceneCamera.ToFixCamera();
            }
            else
            {
                m_SceneCamera.ToRandomCamera(true);
            }
        }
    }

    public override void CameraInFocus(Transform focusTo)
    {
        if (m_SceneCamera != null)
        {
            m_SceneCamera.ToInFocusCamera(focusTo);
        }
    }

    public override void CameraSequence(bool bAuto)
    {
        if (m_SceneCamera != null)
        {
            m_SceneCamera.ToSequenceCamera(bAuto);
        }
    }

    public override void CameraShowTime()
    {
        if (m_SceneCamera != null)
        {
            m_SceneCamera.ToShowTimeCamera();
        }
    }

    public override void CameraNoFocus()
    {
        if (m_SceneCamera != null)
        {
            m_SceneCamera.ToNoFocusCamera();
        }
    }

    public override void EmptyScene(bool bEmpty)
    {
        if (m_SceneCamera != null)
        {
            int sceneLayer = 1 << (int)GameLayer.Scene;

            if (bEmpty)
            {
                m_SceneCamera.TargetCamera.cullingMask &= ~sceneLayer;
            }
            else
            {
                m_SceneCamera.TargetCamera.cullingMask |= sceneLayer;
            }
        }
    }
}
