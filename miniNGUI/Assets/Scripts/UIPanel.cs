using UnityEngine;
using System.Collections;

public class UIPanel : UIRect {

    // filed:--------------------------------
    static public BetterList<UIPanel> list = new BetterList<UIPanel>();
    
    BetterList<UIWidget> widgets = new BetterList<UIWidget>();
    BetterList<UIDrawCall> drawCalls = new BetterList<UIDrawCall>();

    public Matrix4x4 worldToLocal = Matrix4x4.identity;

    bool mRebuild = false;

    public int startingRenderQueue = 3000;


    // properties:--------------------------------
    

    // method:--------------------------------
    // state
    void Awake() {
        mGo = gameObject;
        mTrans = transform;
    }
    void OnEnable() {
        //base.OnEnable();
        
    }
    protected override void OnStart() { }
    protected override void OnInit() {
        base.OnInit();

        mRebuild = true;
        list.Add(this);
        //list.Sort(CompareFunc);
    }
    protected override void OnDisable() {
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
        base.OnDisable();


    }


    // update
    void LateUpdate()
    {
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

    // update panel, all widgets and dcs
    void UpdateSelf() {
        UpdateWidgets();

        if (mRebuild)
        {
            //mRebuild = false;
            FillAllDrawCall();
        }        
    
    }
    // update all widgets
    void UpdateWidgets() {
        bool changed = false;

        for (int i = 0, imax = widgets.size; i < imax; ++i)
        {
            UIWidget w = widgets.buffer[i];

            if (w.panel == this && w.enabled)
            {
                int frame = Time.frameCount;

                if (w.UpdateTransform(frame))
                {

                }

                if (w.UpdateGeometry(frame))
                {
                    changed = true;

                    if (!mRebuild)
                    {
                        if (w.drawCall != null)
                        {
                            w.drawCall.isDirty = true;
                        }
                        else
                        {
                            FindDrawCall(w);
                        }
                    }
                }
            }
        }
    }
    // fill geo fully, process all widgets and re-creat dcs
    void FillAllDrawCall() {
        // destroy dcs
        for (int i = 0; i < drawCalls.size; ++i)
            UIDrawCall.Destroy(drawCalls.buffer[i]);
        drawCalls.Clear();

        Material mat = null;
        Texture tex = null;
        Shader sdr = null;
        UIDrawCall dc = null;

        // sort widgets
        SortWidgets();

        // loop widgets
        for (int i = 0; i < widgets.size; ++i)
        {
            // 1.get component
            UIWidget w = widgets.buffer[i];

            if (w.isVisible) // && w.hasVertices)
            {
                Material mt = w.material;
                Texture tx = w.mainTexture;
                Shader sd = w.shader;

                // 2.判定dc：ABA, 相同component，连续depth，才一个dc
                if (mat != mt || tex != tx || sdr != sd)
                {
                    // 5.update dc' geo
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

                // 3.create dc
                if (mat != null || sdr != null || tex != null)
                {
                    if (dc == null)
                    {
                        dc = UIDrawCall.Create(this, mat, tex, sdr);
                        dc.depthStart = w.depth;
                        dc.depthEnd = dc.depthStart;
                        dc.panel = this;
                    }
                    else
                    {
                        int rd = w.depth;
                        if (rd < dc.depthStart) dc.depthStart = rd;
                        if (rd > dc.depthEnd) dc.depthEnd = rd;
                    }

                    // 4.fill dc（多个连续widget‘ geo到一个dc）
                    w.drawCall = dc;
                    w.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);

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
    // fill geo for dc
    void FillDrawCall() { }

    UIDrawCall FindDrawCall(UIWidget w)
    {
        // 机制：通过depth, mat/tex, visible的优先级判断; 
        //      决定获取dc还是rebuild

        Material mat = w.material;
        Texture tex = w.mainTexture;
        int depth = w.depth;

        for (int i = 0; i < drawCalls.size; ++i)
        {
            UIDrawCall dc = drawCalls.buffer[i];
            int dcStart = (i == 0) ? int.MinValue : drawCalls.buffer[i - 1].depthEnd + 1;
            int dcEnd = (i + 1 == drawCalls.size) ? int.MaxValue : drawCalls.buffer[i + 1].depthStart - 1;

            // depth范围内；相同mat和tex；不同则rebuild
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

    public void SortWidgets()
    {
        widgets.Sort(UIWidget.PanelCompareFunc);
    }

    // add to panel & find dc
    public void AddWidget(UIWidget w)
    {
        if (widgets.size == 0)
        {
            widgets.Add(w);
        }
        //else if (mSortWidgets)
        //{
        //    widgets.Add(w);
        //    SortWidgets();
        //}
        else if (UIWidget.PanelCompareFunc(w, widgets[0]) == -1)
        {
            widgets.Insert(0, w);
        }
        else
        {
            for (int i = widgets.size; i > 0; )
            {
                if (UIWidget.PanelCompareFunc(w, widgets[--i]) == -1) continue;
                widgets.Insert(i + 1, w);
                break;
            }
        }
        FindDrawCall(w);
    }

    public void RemoveWidget(UIWidget w)
    {
        if (widgets.Remove(w) && w.drawCall != null)
        {
            int depth = w.depth;
            if (depth == w.drawCall.depthStart || depth == w.drawCall.depthEnd)
                mRebuild = true;    // 正好在开头或结尾，就需要Rebuild dc

            w.drawCall.isDirty = true;  // 重新fill数据，remove 这个widget数据
            w.drawCall = null;
        }
    }

    // update all dc' transform/renderQueue
    void UpdateDrawCalls()
    {
        Transform trans = cachedTransform;
        Vector3 pos;

        // panel' world pos
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
        for (int i = 0; i < drawCalls.size; ++i)
        {
            UIDrawCall dc = drawCalls.buffer[i];

            Transform t = dc.cachedTransform;
            t.position = pos;
            t.rotation = rot;
            t.localScale = scale;

            dc.renderQueue = startingRenderQueue + i;
        }

    }

    static public UIPanel Find(Transform trans, bool createIfMissing, int layer)
    {
        UIPanel panel = null;

        while (panel == null && trans != null)
        {
            panel = trans.GetComponent<UIPanel>();
            if (panel != null) return panel;
            if (trans.parent == null) break;
            trans = trans.parent;
        }
        return null;
    }

    static public UIPanel Find(Transform trans, bool createIfMissiing, int layer)
    {
        UIPanel panel = null;

        while (panel == null && trans != null)
        {
            panel = trans.GetComponent<UIPanel>();
            if (panel != null) return panel;
            if (trans.parent == null) break;
            trans = trans.parent;
        }

        return null;
    }
}
