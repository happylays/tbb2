using UnityEngine;
using System.Collections;

public class UICamera : MonoBehaviour {

    // class-----------------------------
    // ambiguous mouse
    class MouseOrTouch 
    {
        public Vector2 pos;
        public Vector2 lastPos;
        public Vector2 delta;
        public Vector2 totalDelta;

        public GameObject last;
        public GameObject current;
        public GameObject pressed;
        public GameObject dragged;

        public bool touchBegan;
        public bool pressStarted;
        public bool dragStarted;
    }

    // field-----------------------------
    RaycastHit lastHit;
    MouseOrTouch currentTouch;
    MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch() }
    GameObject hoveredObject;
    Vector2 lastTouchPosition;

    // property-----------------------------

    // method-----------------------------
    // state
    void Update() 
    {
        ProcessMouse();
    }


    void ProcessMouse() 
    {
        // update pos
        lastTouchPosition = Input.mousePosition;
        mMouse[0].delta = lastTouchPosition - mMouse[0].pos;
        mMouse[0].pos = lastTouchPosition;


        // is btn pressed?
        bool justPressed = false;
        if (Input.GetMouseButtonDown(0)) 
        {
            justPressed = true;
        }

        // raycast
        Raycast(Input.mousePosition, out lastHit);


        // process mouse as touch
        bool pressed = Input.GetMouseButtonDown(0);
        bool unpressed = Input.GetMouseButtonUp(0);
        ProcessTouch(pressed, unpressed);

    }

    // process event of touch
    void ProcessTouch(bool pressed, bool unpressed) {}

    bool Raycast(Vector3 inPos, out RaycastHit hit) 
    {
        return false;
    }
    void Notify() {}


}
