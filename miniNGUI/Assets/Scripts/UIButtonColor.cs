using UnityEngine;
using System.Collections;

public class UIButtonColor : UIWidgetContainer {

    // field:--------------------------
    public enum State 
    {
        Normal,
        Hover,
        Pressed,
        Disabled,
    }

    GameObject tweenTarget;

    protected bool mInitDone = false;
    protected State mState = State.Normal;
    public Color hover = new Color(225f / 255f, 200f / 255f, 150f / 255f, 1f);

    // properties:----------------------
    public virtual bool isEnabled { get { return enabled; } set { enabled = value; } }


    // method:--------------------------

    // state
    void Awake() { }
    void Start() { }
    protected virtual void OnInit()
    {
        mInitDone = true;
        if (tweenTarget == null) tweenTarget = gameObject;

    }

    void OnEnable() { }
    void OnDisable() { }

    // event
    protected virtual void OnHover() 
    {
        if (isEnabled)
        {
            if (!mInitDone) OnInit();
            if (tweenTarget != null) SetState(State.Hover);
        }
        else SetState(State.Disabled);
    }
    void OnPress() { }

    void OnDragOver() { }
    void OnDragOut() { }
    void OnClick() { }
    virtual void SetState(State state) {
        if (!mInitDone)
        {
            mInitDone = true;
            OnInit();
        }

        if (mState != state)
        {
            mState = state;

            //TweenColor tc;

            switch (mState)
            {
                //case State.Hover: tc = TweenColor.Begin(tweenTarget, duration, hover); break;
            }
        }
    
    }
}
