using System.Collections;
using System.Collections.Generic;

public class TimeLineElement
{
    public float m_Time = 0;
    public string m_functionName;
    ArrayList m_Param = new ArrayList();
    public TimeLineElement()
    {
    }
    public void pushParam(object param)
    {
        m_Param.Add(param);
    }

    public void SetParam(int nIndex, object param)
    {
        if (m_Param.Count <= nIndex)
        {
            UnityEngine.Debug.Log("TimeLineElement SetParam,  m_Param.Count <= nIndex " + m_Param.Count + "  " + nIndex);
        }
        else
        {
            m_Param[nIndex] = param;
        }
    }

    public object GetParamByIndex(int Index)
    {
        if (Index >= 0 && Index < m_Param.Count)
            return m_Param[Index];
        return null;
    }
}

public class QueueTimeLine
{
    private Queue<TimeLineElement> mQueueTimeLine = new Queue<TimeLineElement>();
    private float mTime = 0;

    public float Time
    {
        get
        {
            return mTime;
        }
    }

    public int Count
    {
        get
        {
            return mQueueTimeLine.Count;
        }
    }

    public QueueTimeLine(float time)
    {
        mTime = time;
    }

    public TimeLineElement Dequeue()
    {
        return mQueueTimeLine.Dequeue();
    }

    public void AddTime(TimeLineElement tle)
    {
        mQueueTimeLine.Enqueue(tle);
    }

    public void Clear()
    {
        mQueueTimeLine.Clear();
    }
}

public class CTimeLineQueue
{
    // The class constructor is called when the class instance is created
    private List<QueueTimeLine> m_Data = new List<QueueTimeLine>();

    public void Enqueue(TimeLineElement tle)
    {
        QueueTimeLine q = GetQueueTimeLine(tle);
        if (q == null)
        {
            q = new QueueTimeLine(tle.m_Time);
            m_Data.Add(q);
        }

        q.AddTime(tle);
    }

    public void Sort()
    {
        m_Data.Sort(delegate(QueueTimeLine a, QueueTimeLine b)
        {
            return a.Time.CompareTo(b.Time);
        });
    }

    public TimeLineElement Dequeue()
    {
        TimeLineElement tle = null;
        if (m_Data.Count > 0)
        {
            QueueTimeLine q = m_Data[0];

            tle = q.Dequeue();

            if (q.Count == 0)
            {
                m_Data.Remove(q);
            }

        }
        return tle;
    }

    public float NextEventTime
    {
        get
        {
            //m_Data.
            float rResult = -1;
            if (m_Data.Count > 0)
            {
                rResult = m_Data[0].Time;
            }

            return rResult;
        }
    }

    private QueueTimeLine GetQueueTimeLine(TimeLineElement time)
    {
        QueueTimeLine q = null;
        for (int i = 0; i < m_Data.Count; ++i)
        {
            if (m_Data[i] != null && m_Data[i].Time == time.m_Time)
            {
                q = m_Data[i];
            }
        }

        return q;
    }

    public void ClearAll()
    {
        QueueTimeLine q = null;
        for (int i = 0; i < m_Data.Count; ++i)
        {
            q = m_Data[i];
            if (q != null)
            {
                q.Clear();
            }
        }
        m_Data.Clear();
    }
}
