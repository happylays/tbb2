using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICamera : MonoBehaviour {

    // class-----------------------------
    // ambiguous mouse
    class MouseOrTouch 
    {
        public Vector2 pos;
        public Vector2 lastPos;
        public Vector2 delta;       // 每帧偏移或总偏移
        public Vector2 totalDelta;

        public GameObject last;
        public GameObject current;
        public GameObject pressed;
        public GameObject dragged;

        public float clickTime = 0f;

        public bool touchBegan;
        public bool pressStarted;
        public bool dragStarted;
    }

    // field-----------------------------
    RaycastHit lastHit;
    static MouseOrTouch currentTouch;
    static MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch() };
    static GameObject hoveredObject;    // 当前只会有一个
    Vector2 lastTouchPosition;
    static bool isDragging = false;

    static List<UICamera> list = new List<UICamera>();
    static Camera currentCamera = null;
    
    Camera mCam = null;
    LayerMask eventReceiverMask = -1;

    bool mNotifying = false;
    float mouseDragThreshold = 4f;
    static RaycastHit mEmpty = new RaycastHit();


    Camera cachedCamera { get { if (mCam == null) mCam = camera; return mCam; } }

    // property-----------------------------

    // method-----------------------------
    // state
    void OnEnable() { list.Add(this); }
    void OnDisable() { list.Remove(this); }

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
        mMouse[0].current = hoveredObject; 

        // process mouse as touch
        bool pressed = Input.GetMouseButtonDown(0);
        bool unpressed = Input.GetMouseButtonUp(0);
        currentTouch = mMouse[0];
        ProcessTouch(pressed, unpressed);

    }

    // process event of touch
    void ProcessTouch(bool pressed, bool unpressed) 
    {
        float drag = mouseDragThreshold;

        if (pressed)    // 按下时
        {
            currentTouch.pressStarted = true;
            Notify(currentTouch.pressed, "OnPress", false); // buttonColor 颜色设置
            currentTouch.pressed = currentTouch.current;
            currentTouch.dragged = currentTouch.current;
            currentTouch.totalDelta = Vector2.zero;
            currentTouch.dragStarted = false;
            Notify(currentTouch.pressed, "OnPress", true);
        }
        else if (currentTouch.pressed != null && (currentTouch.delta.sqrMagnitude != 0f))
        {   // 之前有键按下，且按下后拖动，drag事件

            // 追踪总movement
            currentTouch.totalDelta += currentTouch.delta;
            float mag = currentTouch.totalDelta.sqrMagnitude;
            bool justStarted = false;

            // 。检测是否开始drag
            // 还没开始drag，但目标object已不一样
            // 还没开始drag，拖动一段了
            if (!currentTouch.dragStarted && currentTouch.last != currentTouch.current)
            {

            }
            else if (!currentTouch.dragStarted && drag < mag)
            {
                justStarted = true;
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
            }

            // 。drag处理
            // 如果正在drag，发送drag事件
            if (currentTouch.dragStarted)
            {
                isDragging = true;

                //如果justStarted
                //!justStarted, 目标不同了
                if (justStarted)
                {
                    Notify(currentTouch.dragged, "OnDragStart", null);
                    // drag对象悬浮于drag区域，在drag区域内移动多次触发
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                else if (currentTouch.last != currentTouch.current)
                {   // drag出原Object，到另一个object上
                    Notify(currentTouch.last, "OnDragOut", currentTouch.dragged);
                    
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                // process drag
                Notify(currentTouch.dragged, "OnDrag", currentTouch.delta);

                //更新currentTouch最新object
                currentTouch.last = currentTouch.current;
                isDragging = false;
            }
        }

        if (unpressed)
        {
            // if pressed存在
            if (currentTouch.pressed != null)
            {

                // if btn 在同一个object上release
                if (currentTouch.dragged == currentTouch.current)
                {
                    float time = Time.time;

                    // if pressed对象和当前对象一致
                    Notify(currentTouch.pressed, "OnClick", null);

                    currentTouch.clickTime = time;
                }
                else if (currentTouch.dragStarted)
                {   // if btn 不在同一个object上，表示是一个drag事件结束

                    Notify(currentTouch.current, "OnDrop", currentTouch.dragged);
                }
            }

            // 按起后，清除state
            currentTouch.dragStarted = false;
            currentTouch.pressed = null;
            currentTouch.dragged = null;
        }
    }

    // return object under pos（不考虑点击）
    static bool Raycast(Vector3 inPos, out RaycastHit hit) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            UICamera cam = list[0];

            // skip inactive script
            if (!cam.enabled || !NGUITools.GetActive(cam.gameObject)) continue;

            // convert to viewSpace
            currentCamera = cam.cachedCamera;
            Vector3 pos = currentCamera.ScreenToViewportPoint(inPos);
            if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f) continue;

            // raycast into screen
            Ray ray = currentCamera.ScreenPointToRay(inPos);
            int mask = currentCamera.cullingMask & (int)cam.eventReceiverMask;
            float dist = 100;// debugXX

            // ui
            RaycastHit[] hits = Physics.RaycastAll(ray, dist, mask);
            if (hits.Length == 1)
            {
                hit = hits[0];
                hoveredObject = hit.collider.gameObject;
                return true;
            }
        }

        hit = mEmpty;
        return false;
    }
    void Notify(GameObject go, string funcName, object obj) 
    {
        if (mNotifying) return;
        mNotifying = true;

        if (NGUITools.GetActive(go))
        {
            go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
        }
        mNotifying = false;
    }




}
