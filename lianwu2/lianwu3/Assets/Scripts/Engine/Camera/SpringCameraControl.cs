using System;
using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;

public class SpringCameraControl : MonoBehaviour, ISpringCameraControl
{
    public Transform PlayerTran
    {
        set
        {
            mPlayerTran = value;
        }
    }

    [SerializeField]
    Vector2 m_TouchArea = new Vector2(400f, 240f); //Default value:means rotate 180 degrees in x axis,rotate 180 degrees in y axis.
    [SerializeField]
    float m_RotationUpMin = -15f;					//Rotation up min angle
    [SerializeField]
    float m_RotationUpMax = 70f;					//Rotation up max angle
    [SerializeField]
    BoxCollider m_Collider = null;
    [SerializeField]
    bool m_CanPullCamera = false;
    [SerializeField]
    float m_CameraNear = 0;
    [SerializeField]
    float m_CameraFar = 0;
    [SerializeField]
    float m_NearUpMin = 0;
    [SerializeField]
    float m_FarUpMin = 0;

    Vector2 mRotationAngle = new Vector2(180f, 180f);

    float mCamRotationSide = 0f;		//Y axis
    float mCamRotationUp = 0f;			//X axis

    Transform mPlayerTran = null;
    Transform mCameraTran = null;

    Vector2 mPosition = Vector2.zero;
    float mScale = 0;

    float mMultiTouchDistance = 0;

    private ISceneCamera mSceneCamera = null;

    private ISceneCamera SceneCamera
    {
        get
        {
            if (mSceneCamera == null)
            {
                if (CSceneBehaviour.Current != null)
                {
                    mSceneCamera = CSceneBehaviour.Current.CameraControl;
                }
            }

            return mSceneCamera;
        }
    }

    void Start()
    {
        if (m_Collider != null)
        {
            Vector3 center = new Vector3(0, 0, 20);
            m_Collider.center = center;
            Vector3 size = new Vector3(Screen.width, Screen.height, 0);
            m_Collider.size = size;
        }

        if (m_CanPullCamera && SceneCamera != null && SceneCamera.TargetCamera != null)
        {
            float z = SceneCamera.TargetCamera.transform.localPosition.z;

            SceneCamera.TargetCamera.transform.localPosition = new Vector3(0, 0, m_CameraNear);
            m_RotationUpMin = m_NearUpMin;

            SetCameraScale(m_CameraNear - z);
            SpringCamera(Vector2.zero);
        }
    }

# if UNITY_EDITOR || UNITY_STANDALONE_WIN
    void Update()
    {
        float scale = Input.GetAxis("Mouse ScrollWheel");
        if (m_CanPullCamera && scale != 0)
        {
            SetCameraScale(scale);
            SpringCamera(Vector2.zero);
        }
    }
#endif

    void OnPress(bool bPress)
    {
        if (bPress)
        {
            if (m_CanPullCamera && Input.touchCount > 1)
            {
                mMultiTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            }
        }
    }

    void OnDrag(Vector2 delta)
    {
        if (m_CanPullCamera && Input.touchCount > 1)
        {
            float distance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            mScale = (mMultiTouchDistance - distance) / 1000;

            mPosition = Vector2.zero;

            SetCameraScale(mScale);
        }
        else
        {
            mPosition.x = delta.x / m_TouchArea.x;
            mPosition.y = delta.y / m_TouchArea.y;
        }

        SpringCamera(mPosition);
    }

    void SetCameraScale(float scale)
    {
        if (mPlayerTran != null && SceneCamera != null)
        {
            float z = SceneCamera.TargetCamera.transform.localPosition.z;
            z -= scale;
            if (z > m_CameraNear)
            {
                z = m_CameraNear;
            }
            else if (z < m_CameraFar)
            {
                z = m_CameraFar;
            }
            SceneCamera.TargetCamera.transform.localPosition = new Vector3(0, 0, z);

            m_RotationUpMin += (4 * scale);
            if (m_RotationUpMin > m_FarUpMin)
            {
                m_RotationUpMin = m_FarUpMin;
            }
            else if (m_RotationUpMin < m_NearUpMin)
            {
                m_RotationUpMin = m_NearUpMin;
            }
        }
    }

    void SpringCamera(Vector2 pos)
    {
        //Record initial data
        if (mPlayerTran != null)
        {
            if (mCameraTran == null && SceneCamera != null)
            {
                mCameraTran = SceneCamera.Trans;
                mCamRotationSide = mCameraTran.eulerAngles.y;
                mCamRotationUp = mCameraTran.eulerAngles.x;
            }

            //Rechange camera rotation
            mCamRotationSide += pos.x * mRotationAngle.x;
            mCamRotationUp -= pos.y * mRotationAngle.y;

            //Limited camera up rotation
            if (mCamRotationUp >= m_RotationUpMax)
            {
                mCamRotationUp = m_RotationUpMax;
            }
            else if (mCamRotationUp <= m_RotationUpMin)
            {
                mCamRotationUp = m_RotationUpMin;
            }

            mCameraTran.rotation = Quaternion.Euler(mCamRotationUp, mCamRotationSide, 0);
        }
    }
}
