using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIButton : UIButtonColor {

    // field:--------------------------
    static public UIButton current;
    public List<EventDelegate> onClick = new List<EventDelegate>();

    // properties::--------------------------
    public override bool isEnabled
    {
        get
        {
            if (!enabled) return false;
            Collider col = collider;
            if (col && col.enabled) return true;

            return false;
        }
    }


	// method:--------------------------

    // state
    protected override void OnInit() 
    {
        base.OnInit();
    }
    
    void OnEnable() 
    {
        if (isEnabled)
        {
            if (mInitDone)
            {
                OnHover();//UICamera.hoveredObject == gameObject);
            }
        }
        else SetState(State.Disabled);
    
    }
    
    // event
    void OnDragOver() { return; }
    void OnDragOut() { return; }
    void OnClick() {
        if (current == null && isEnabled)
        {
            current = this;
            EventDelegate.Execute(onClick);
            current = null;
        }
    }
    public override void SetState(State state) {
        base.SetState(state);

        switch (state)
        {
            //case State.Normal: SetSprite(mNormalSprite); break;
        }
    }

}
