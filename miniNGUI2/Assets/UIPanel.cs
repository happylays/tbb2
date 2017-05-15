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

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

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
    void LateUpdate() {
        for (int i = 0; i < list.size; ++i)
        {
            list[i].UpdateSelf();
        }

        int rq = 3000;
        for (int i = 0; i < list.size; ++i)
        {
            UIPanel p = list.buffer[i];

            p.startingRenderQueue = rq;
            p.UpdateDrawCalls();
            rq += p.drawCalls.size;
        }
    
    }
    void UpdateSelf() {
        UpdateWidgets();

        if (mRebuild)
        {
            FillAllDrawCall();
        }
    }
    void UpdateWidgets { }

    // method-drawcall:
    void FillAllDrawCall() {
        for (int i = 0; i < drawCalls.size; ++i)
        {
            UIDrawCall.Destroy(drawCalls.buffer[i]);
        }
        drawCalls.Clear();

        Material mat = null;
        Texture tex = null;
        Shader sdr = null;
        UIDrawCall dc = null;

        for (int i = 0; i < widgets.size; ++i)
        {
            UIWidget w = widgets.buffer[i];

            if (w.isVisible)
            {
                Material mt = w.material;
                Texture tx = w.mainTexture;
                Shader sd = w.shader;

                if (mat != mt || tex != tx || sdr != sd)
                {
                    if (dc != null && dc.verts.size != 0)
                    {
                        drawCalls.Add(dc);
                        dc.UpdateGeometry();
                        dc = null;
                    }

                    mat = mt;
                    tex = tx;
                    sdr = sd;
                }

                if (mat != null || sdr != null || tex != null)
                {
                    if (dc == null)
                    {
                        dc = UIDrawCall.Create(this, mat, tex, sdr);
                        dc.depthStart = w.depth;
                        dc.depthEnd = c.depthStart;
                        dc.panel = this;
                    }
                    else
                    {
                        int rd = w.depth;
                        if (rd < dc.depthStart) dc.depthStart = rd;
                        if (rd > dc.depthEnd) dc.depthEnd = rd;
                    }

                    w.drawCall = dc;
                    w.WriteToBuffer(dc.verts, dc.uvs, dc.cols, null, null);
                }
            }
            else w.drawCall = null;
        }

        if (dc != null && dc.verts.size != 0)
        {
            drawCalls.Add(dc);
            dc.UpdateGeometry();
        }
    }
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

    // set all dc' transform/renderQueue
    void UpdateDrawCalls() {
        Transform trans = cachedTransform;
        Vector3 pos;

        bool isUI = true;
        if (isUI)
        {
            Transform parent = cachedTransform.parent;
            pos = cachedTransform.localPosition;

            if (parent != null)
            {
                float x = Mathf.Round(pos.x);
                float y = Mathf.Round(pos.y);

                pos.x = x;
                pos.y = y;
                pos = parent.TransformPoint(pos);
            }
        }
        else pos = trans.position;

        pos = trans.position;
        Quaternion rot = trans.rotation;
        Vector3 scale = transform.lossyScale;
        for (int i = 0; i < drawCalls.size; ++i) {
            UIDrawCall dc = drawCalls.buffer[i];

            Transform t = dc.cachedTransform;
            t.position = pos;
            t.rotation = rot;
            t.localScale = scale;

            dc.renderQueue = startingRenderQueue + i;
        }
    }
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