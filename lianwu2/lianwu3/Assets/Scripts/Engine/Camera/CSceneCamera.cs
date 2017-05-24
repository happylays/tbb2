using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;


[System.Serializable]
class CameraPath
{
    public Transform m_Pivot = null;
    public Transform m_Target = null;
}

public class CSceneCamera : MonoBehaviour, ISceneCamera
{
    public Camera TargetCamera
    {
        get
        {
            return m_TargetCamera;
        }
    }

    public SpringCameraControl SpringControl
    {
        get
        {
            return m_SprindCon;
        }
    }

    public Animation CameraAni
    {
        get
        {
            return m_CameraAni;
        }
    }

    public Transform Trans
    {
        get { return transform; }
    }

    [SerializeField]
    Camera m_TargetCamera = null;
    [SerializeField]
    SpringCameraControl m_SprindCon = null;
    [SerializeField]
    Animation m_CameraAni = null;

    [SerializeField]
    Transform m_FixedPath = null;
    [SerializeField]
    Vector3 m_FixedAngle = Vector3.zero;

    [SerializeField]
    CameraPath[] m_SwitchPath = null;
    [SerializeField]
    Transform m_FixedCameraForModel3Position = null;
    [SerializeField]
    CameraPath m_ShowTimePath = null;

    bool mHasInit = false;
    bool mStartGo = false;

    bool mCameraFixed = false;
    bool mChangePath = true;

    bool mPathRandom = true;
    bool mPathAuto = true;

    int mPathIndex = -1;
    XQLookAt mLookAt = null;

    void Start()
    {
        InitSceneCamera();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mCameraFixed)
            {
                ToRandomCamera(mPathAuto);
            }
            else
            {
                ToFixCamera();
            }
        }
    }
#endif

    Camera CreatePlayerCamera(Camera source)
    {
        GameObject go = new GameObject("PlayerCamera");
        Camera c = go.AddComponent<Camera>();
        c.farClipPlane = source.farClipPlane;
        c.nearClipPlane = source.nearClipPlane;
        c.depth = source.depth + 1;
        c.orthographic = source.orthographic;
        c.rect = source.rect;
        c.renderingPath = source.renderingPath;
        c.clearFlags = CameraClearFlags.Nothing;
        c.fieldOfView = source.fieldOfView;
        return c;
    }

    void InitSceneCamera()
    {
        if (m_CameraAni != null && m_CameraAni.clip != null)
        {
            CAniEventNoticer aniNotice = m_CameraAni.gameObject.GetComponent<CAniEventNoticer>();
            if (aniNotice == null)
            {
                aniNotice = m_CameraAni.gameObject.AddComponent<CAniEventNoticer>();
                aniNotice.Bind(m_CameraAni, gameObject);

                AnimationEvent aniEvent = new AnimationEvent();
                aniEvent.time = m_CameraAni.clip.length;
                aniEvent.functionName = "OnAniLoopEnd";
                aniNotice.AddAniEvent(m_CameraAni.clip.name, aniEvent);

                m_CameraAni[m_CameraAni.clip.name].wrapMode = (mPathAuto ? WrapMode.Loop : WrapMode.Once);
            }
        }

        if (m_TargetCamera != null)
        {
            mLookAt = m_TargetCamera.gameObject.GetComponent<XQLookAt>();
            if (mLookAt == null)
            {
                mLookAt = m_TargetCamera.gameObject.AddComponent<XQLookAt>();
            }
        }

        mHasInit = true;

        if (mStartGo)
        {
            ToNoFocusCamera();
        }
    }

    void OnAniLoopEnd(AnimationEvent aniEvent)
    {
        if (mPathAuto)
        {
            SwitchCameraToPath();
        }
    }

    void FixCameraToPath()
    {
        if (mChangePath && m_TargetCamera != null && m_FixedPath != null)
        {
            m_TargetCamera.transform.parent = m_FixedPath;
            m_TargetCamera.transform.localPosition = Vector3.zero;
            m_TargetCamera.transform.localRotation = Quaternion.Euler(m_FixedAngle);
        }
    }

    void SwitchCameraToPath()
    {
        if (mPathRandom)
        {
            if (m_SwitchPath.Length > 0)
            {
                List<int> newPathList = new List<int>();
                for (int i = 0; i < m_SwitchPath.Length; ++i)
                {
                    if (i != mPathIndex)
                    {
                        newPathList.Add(i);
                    }
                }

                if (newPathList.Count > 0)
                {
                    int index = Random.Range(0, newPathList.Count);
                    mPathIndex = newPathList[index];
                }

                MoveCamera(mPathIndex);
            }
        }
        else
        {
            if (m_SwitchPath.Length > 0)
            {
                ++mPathIndex;

                if (mPathIndex >= m_SwitchPath.Length)
                {
                    mPathIndex -= m_SwitchPath.Length;
                }

                MoveCamera(mPathIndex);
            }
        }
    }

    void MoveCamera(int pathIndex)
    {
        if (mChangePath && m_TargetCamera != null
            && mPathIndex >= 0 && mPathIndex < m_SwitchPath.Length && m_SwitchPath[mPathIndex] != null)
        {
            if (m_SwitchPath[mPathIndex].m_Pivot != null)
            {
                m_TargetCamera.transform.parent = m_SwitchPath[mPathIndex].m_Pivot;
                m_TargetCamera.transform.localPosition = Vector3.zero;

                if (mLookAt != null)
                {
                    mLookAt.LookTarget = m_SwitchPath[mPathIndex].m_Target;
                }
            }
        }
    }

    void ChangeCameraPath()
    {
        if (mHasInit)
        {
            if (mCameraFixed)
            {
                if (m_CameraAni != null)
                {
                    m_CameraAni.Stop();
                }

                m_TargetCamera.nearClipPlane = 0.3f;
                mLookAt.Looking = false;

                FixCameraToPath();
            }
            else
            {
                if (m_CameraAni != null && m_CameraAni.clip != null)
                {
                    m_CameraAni.Stop();

                    m_CameraAni.clip.wrapMode = (mPathAuto ? WrapMode.Loop : WrapMode.Once);
                    m_CameraAni.Play(m_CameraAni.clip.name);
                }

                m_TargetCamera.nearClipPlane = 0.3f;
                mLookAt.Looking = true;

                SwitchCameraToPath();
            }
        }
        else
        {
            mStartGo = true;
        }
    }

    public void ToFixCamera()
    {
        mCameraFixed = true;
        ChangeCameraPath();
    }

    public void ToRandomCamera(bool bAutoChange)
    {
        mCameraFixed = false;
        mPathRandom = true;
        mPathAuto = bAutoChange;
        ChangeCameraPath();
    }

    public void ToSequenceCamera(bool bAutoChange)
    {
        mCameraFixed = false;
        mPathRandom = false;
        mPathAuto = bAutoChange;
        ChangeCameraPath();
    }

    public void ToInFocusCamera(Transform focusTo)
    {
        mChangePath = false;

        m_TargetCamera.transform.parent = focusTo;
        if (m_FixedCameraForModel3Position != null)
        {
            m_TargetCamera.transform.localPosition = m_FixedCameraForModel3Position.localPosition;
            m_TargetCamera.transform.localRotation = m_FixedCameraForModel3Position.localRotation;
        }

        m_TargetCamera.nearClipPlane = 0.6f;
        mLookAt.Looking = false;
    }

    public void ToShowTimeCamera()
    {
        mChangePath = false;

        if (m_ShowTimePath != null)
        {
            m_TargetCamera.transform.parent = m_ShowTimePath.m_Pivot;
            m_TargetCamera.transform.localPosition = Vector3.zero;

            if (mLookAt != null)
            {
                mLookAt.LookTarget = m_ShowTimePath.m_Target;
                mLookAt.Looking = true;
            }
        }
    }

    public void ToNoFocusCamera()
    {
        mChangePath = true;
        ChangeCameraPath();
    }
}
