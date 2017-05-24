using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XQLookAt : MonoBehaviour
{
    public bool Looking
    {
        set
        {
            mIsLooking = value;
        }
    }

    public Transform LookTarget
    {
        set
        {
            mLookTarget = value;
        }
    }

    bool mIsLooking = true;
    [SerializeField]
    Transform mLookTarget;

    private Transform mTrans = null;

    void Awake()
    {
        mTrans = transform;
    }

    void LateUpdate()
    {
        if (mIsLooking && mLookTarget != null && mTrans != null)
        {
            if (mLookTarget.position != mTrans.position)
            {
                mTrans.rotation = Quaternion.LookRotation(mLookTarget.position - mTrans.position);
            }
        }
    }
}
