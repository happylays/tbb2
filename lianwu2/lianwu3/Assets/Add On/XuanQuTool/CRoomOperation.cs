using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;

public class CRoomOperation : MonoBehaviour
{
	public MeshRenderer SceneRenderer
	{
		get
		{
			return m_SceneRenderer;
		}
	}

    [System.Serializable]
    public class AudiencePos
    {
        [SerializeField] GameObject m_PosClose = null;
        [SerializeField] GameObject m_Light = null;

        private Animation mCloseAni = null;

        private Animation CloseAni
        {
            get
            {
                if (mCloseAni == null)
                {
                    mCloseAni = m_PosClose.animation;
                }

                return mCloseAni;
            }
        }

        public void SetPosState(RoomPosState state, bool showAni)
        {
            bool show = false;
            if (state == RoomPosState.Open)
            {
                show = true;
            }

            if (m_Light != null)
            {
                m_Light.SetActive(show);
            }

            PlayAnimation(CloseAni, show, showAni);
        }

        private void PlayAnimation(Animation ani, bool isReverse, bool showAni)
        {
            if (ani != null)
            {
                AnimationState aniState = ani[ani.clip.name];
                if (!isReverse)
                {
                    aniState.speed = 1;
                    if (showAni)
                    {
                        aniState.time = 0;
                    }
                    else
                    {
                        aniState.time = ani.clip.length;
                    }
                }
                else
                {
                    aniState.speed = -1;
                    if (showAni)
                    {
                        aniState.time = ani.clip.length;
                    }
                    else
                    {
                        aniState.time = 0;
                    }
                }

                ani.Play();
            }
        }
    }

	[SerializeField] MeshRenderer m_SceneRenderer = null;

    public RoomDancePos[] m_DancerPos = null;
	public AudiencePos[] m_AudiencePos = null;
	public GameObject[] m_EventListeners = null;

    public void Init()
    {
        for (int i = 0; i < m_DancerPos.Length; ++i)
        {
            m_DancerPos[i].Init();
        }
    }
}
