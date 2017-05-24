using UnityEngine;
using System.Collections.Generic;

public class CAniEventNoticer : MonoBehaviour
{
    bool mHasBind = false;

    Animation mAniTarget;
    GameObject mFireTarget;

    int mEventID = 1;
    Dictionary<int, AnimationEvent> mEventMap = new Dictionary<int, AnimationEvent>();

    public bool Bind(Animation ani, GameObject target)
    {
        if (!mHasBind)
        {
            mHasBind = true;
            mAniTarget = ani;
            mFireTarget = target;

            return true;
        }

        return false;
    }

    public void AddAniEvent(string clipName, AnimationEvent aniEvent)
    {
        AnimationClip aniClip = mAniTarget.GetClip(clipName);
        if (aniClip != null)
        {
            AnimationEvent newEvent = new AnimationEvent();
            newEvent.time = aniEvent.time;
            newEvent.functionName = "OnAniEvent";
            newEvent.intParameter = mEventID;
            newEvent.messageOptions = aniEvent.messageOptions;
            newEvent.messageOptions = aniEvent.messageOptions;

            mEventMap.Add(mEventID, aniEvent);
            aniClip.AddEvent(newEvent);

            ++mEventID;
        }
    }

    void OnAniEvent(AnimationEvent newEvent)
    {
        int eventID = newEvent.intParameter;
        if (mEventMap.ContainsKey(eventID))
        {
            AnimationEvent aniEvent = mEventMap[eventID];

            if (mFireTarget != null)
            {
                mFireTarget.SendMessage(aniEvent.functionName, aniEvent, aniEvent.messageOptions);
            }
            else
            {
                SendMessage(aniEvent.functionName, aniEvent, aniEvent.messageOptions);
            }
        }
    }
}
