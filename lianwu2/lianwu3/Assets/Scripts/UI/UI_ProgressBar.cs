using UnityEngine;

public class UI_ProgressBar : MonoBehaviour
{
    [SerializeField] UISprite m_Sprite = null;
    [SerializeField] float m_nNormalTime = 0;

    private float m_nHideWaittingTime = 0;
    private bool m_isPlaying = false;//是否正在播放动画效果;
    private float m_nCurPct = 0;//当前百分比;
    private float m_nSpeed = 0;//每秒进度增长速度;
    private float m_nMaxScaleX = 0;//最大长度scale;

    private float m_nTargetPct = 0;//目标百分比;
    private float m_nEachPct = 0;//间隔百分比;

    /// <summary>
    /// 初始化进度条;
    /// </summary>
    public void InitProgressBar(float nHideWaittingTime)
    {
        m_nHideWaittingTime = nHideWaittingTime;

        m_nMaxScaleX = m_Sprite.transform.localScale.x;
        ResetPercent();
    }

    public void ResetPercent()
    {
        m_isPlaying = false;
        m_nCurPct = 0;
        m_nTargetPct = 0;
        m_nSpeed = 0;
        SetPercent(0);
    }

    /// <summary>
    /// 设置当前百分比的间隔;
    /// </summary>
    public void SetPercentCount(int nCount)
    {
        if (nCount == 0)
        {
            m_nEachPct = 0;
        }
        else
        {
            m_nEachPct = (float)1 / (float)nCount;
        }
    }

    /// <summary>
    /// 进度条增加到下一个阶段;
    /// </summary>
    public void MoveNextPercent()
    {
        m_nTargetPct += m_nEachPct;
        PlayAniToTarget();
    }
    
    /// <summary>
    /// 外部调用设置当前进度;
    /// </summary>
    /// <param name="nPct">100为max的进度</param>
    public void SetTargetPercent(int nPct)
    {
        m_nTargetPct = (float)nPct / (float)100;
        PlayAniToTarget();
    }

    private void PlayAniToTarget()
    {
        if (m_nTargetPct > 1)
        {
            return;
        }

        float nAniPct = m_nTargetPct - m_nCurPct;//差距;
        if (m_nTargetPct == 1)//如果设置的值等于100,进度max了;//快速结束进度效果;
        {
            if (m_nHideWaittingTime == 0)//进度100时候,并且没有结束延迟时间;
            {
                SetPercent(1);
                m_isPlaying = false;//直接塞满进度,不进行动画;
            }
            else
            {
                m_nSpeed = nAniPct / m_nHideWaittingTime;
            }
        }
        else
        {
            m_nSpeed = nAniPct / m_nNormalTime;//固定时间 算出当前速度;
        }

        m_isPlaying = true;//开始播放动画效果;
    }

    /// <summary>
    /// 内部设置显示;
    /// </summary>
    /// <param name="nPct">1为max的进度</param>
    private void SetPercent(float nPct)
    {
        if (nPct == 0)
        {
            nPct = 0.01f;
        }
        if(nPct > 1)
        {
            nPct = 1;
        }
        m_Sprite.transform.localScale = new Vector3((float)(m_nMaxScaleX * nPct), m_Sprite.transform.localScale.y, m_Sprite.transform.localScale.z);
    }
        
    void Update ()
    {
        if(!m_isPlaying)
        {
            return;
        }
        m_nCurPct += Time.deltaTime * m_nSpeed;
        if (m_nCurPct >= m_nTargetPct)
        {
            m_nCurPct = m_nTargetPct;
            m_isPlaying = false;//到达目标值停止动画;
        }
        SetPercent(m_nCurPct);
	}
}
