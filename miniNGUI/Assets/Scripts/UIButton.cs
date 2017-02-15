using UnityEngine;
using System.Collections;

public class UIButton : UIButtonColor {

    // field:--------------------------


    // properties::--------------------------
    override bool isEnabled
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
    override void OnInit() 
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
    void OnDragOver() { }
    void OnDragOut() { }
    void OnClick() {
        if (current == null && isEnabled)
        {
            current = this;
            EventDelegate.Execute(onClick);
            current = null;
        }
    }
    override void SetState(State state) {
        base.SetState(state);

        switch (state)
        {
            //case State.Normal: SetSprite(mNormalSprite); break;
        }
    }

}
