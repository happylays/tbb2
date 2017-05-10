
public class WrappedInt
{
    int m_nRandom = 0;
    int m_nNum1 = 0;
    int m_nNum2 = 1000;

    int m_nRank = 0;

    /// <summary>
    /// 命中等级 MISS,GOOD,PERFECT 等,暴露出来forT台秀记录命中率;
    /// </summary>
    public int nRank
    {
        get
        {
            return m_nRank;
        }
    }

    public int Value
    {
        set
        {
            m_nRank = value;

            m_nRandom = UnityEngine.Random.Range(2, 10);

            int temp = m_nRank + 1000;
            m_nNum1 = temp / m_nRandom;
            m_nNum2 = temp % m_nRandom;
        }

        get
        {
            int temp = m_nNum1 * m_nRandom + m_nNum2;
            return temp - 1000;
        }
    }
    public static WrappedInt Wrap(int i)
    {
        WrappedInt des = new WrappedInt();
        des.Value = i;

        return des;
    }

    public void Clear()
    {
        m_nRandom = 0;
        m_nNum1 = 0;
        m_nNum2 = 0;
    }
    public WrappedInt Clone()
    {
        WrappedInt r = new WrappedInt();
        r.Value = Value;
        return r;
    }
}