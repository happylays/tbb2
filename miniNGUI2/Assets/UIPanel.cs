using UnityEngine;


class UIPanel : MonoBehaviour
{
    // filed:
    static public BetterList<UIPanel> list = new BetterList<UIPanel>();
    BetterList<UIWidget> widgets = new BetterList<UIWidget>();
    BetterList<UIDrawCall> drawCalls = new BetterList<UIDrawCall>();

    public Matrix4x4 worldToLocal = Matrix4x4.identity;
    bool mRebuild = false;
    public int startingRenderQueue = 3000;

    GameObject mGo;
    Transform mTrans;

    // method:

    // method-state:
    void Awake()
    {
        mGo = gameObject;
        mTrans = transform;
    }
    void OnInit()
    {
        mRebuild = true;
        list.Add(this);
    }
    void OnStart() { }
    void OnEnable() { }
    void OnDisable()
    {
        for (int i = 0; i < drawCalls.size; ++i)
        {
            UIDrawCall dc = drawCalls.buffer[i];
            if (dc != null) UIDrawCall.Destroy(dc);
        }
        drawCalls.Clear();

        list.Remove(this);

        if (list.size == 0)
        {
            UIDrawCall.ReleaseAll();
        }
    }

    // method-update:
    void LateUpdate() { }
    void UpdateSelf() { }
    void UpdateWidgets { }

    // method-drawcall:
    void FillAllDrawCall() { }
    void FillDrawCall() { }
    UIDrawCall FindDrawCall(UIWidget w)
    {
        Material mat = w.material;
        Texture tex = w.mainTexture;
        int depth = w.depth;

        for (int i = 0; i < drawCalls.size; ++i)
        {
            UIDrawCall dc = drawCalls.buffer[i];
            int dcStart = (i == 0) ? int.MinValue : drawCalls.buffer[i - 1].depthEnd + 1;
            int dcEnd = (i + 1 == drawCalls.size) ? int.MaxValue : drawCalls.buffer[i + 1].depthEnd - 1;

            if (dcStart <= depth && dcEnd >= depth)
            {
                if (dc.baseMaterial == mat && dc.mainTexture == tex)
                {
                    w.drawCall = dc;
                    if (w.hasVertices) dc.isDirty = true;
                    return dc;
                }
                else mRebuild = true;
                return null;
            }
        }

        mRebuild = true;
        return null;
    }
    void UpdateDrawCalls() { }
    // widget:
    void SortWidgets() { }
    void AddWidget(UIWidget w) { 
        // add to list by depth
        if (widgets.size == 0)
            widgets.Add(w);
        else if (UIWidget.PanelCompareFunc(w, widgets[0]) == -1)
        {
            widgets.Insert(0, w);
        }
        else
        {
            for (int i = widgets.size; i > 0; )
            {
                if (UIWidget.PanelCompareFunc(w, widgets[--i]) == -1) continue
                widgets.Insert(i+1, w);
                break;
            }
        }


        // set dc        
        FindDrawCall(w);
    }
    void RemoveWidget(UIWidget w) { }

    // other
    UIPanel Find(Transform trans) { }
}