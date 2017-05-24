using UnityEngine;

public class TaiguDrumBall : MonoBehaviour
{
    public TaiguBallType BallType
    {
        get
        {
            return m_BallType;
        }
    }

    public bool IsKeyBall
    {
        get
        {
            return mKeyBall;
        }
        set
        {
            mKeyBall = value;
        }
    }

    public int RoundIndex
    {
        get
        {
            return mRoundIndex;
        }
        set
        {
            mRoundIndex = value;
        }
    }

    [SerializeField]
    TaiguBallType m_BallType = TaiguBallType.None;
    [SerializeField]
    Transform m_LineTrans = null;
    [SerializeField]
    Transform m_EndBall = null;

    bool mKeyBall = false;
    int mRoundIndex = 0;

    private float m_nWidth = 0;

    public Transform EndBall
    {
        get
        {
            return m_EndBall;
        }
    }

    public void ResizeHoldBall(float nWidth)
    {
        if (m_EndBall != null)
        {
            m_EndBall.localPosition = new Vector3(0 - nWidth, 0, 0);
        }
        else
        {
            m_EndBall = this.transform;
        }
        if (m_LineTrans != null)
        {
            m_LineTrans.localScale = new Vector3(nWidth, m_LineTrans.localScale.y, 1);
        }
    }
}
