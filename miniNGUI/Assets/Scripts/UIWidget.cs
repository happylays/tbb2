using UnityEngine;
using System.Collections;

public class UIWidget : UIRect {

    public enum Pivot
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
    }

    // filed:-----------------------------
    protected Pivot mPivot = Pivot.Center;
    protected int mWidth = 100;
    protected int mHeight = 100;
    protected Vector4 mDrawRegion = new Vector4(0, 0, 1, 1);

    public UIPanel panel;
    UIGeometry geometry = new UIGeometry();
    public UIDrawCall drawCall;
    protected int mDepth = 0;

    public bool hasVertices { get { return geometry != null && geometry.hasVertices; } }

    Matrix4x4 mLocalToPanel;

    protected Vector3[] mCorners = new Vector3[4];

    // properties:----------------------------

    public virtual Material material
    {
        get { return null; }
        set { throw new System.NotImplementedException(GetType() + "has no material setter"); }
    }

    public virtual Texture mainTexture
    {
        get
        {
            Material mat = material;
            return (mat != null) ? mat.mainTexture : null;
        }
        set
        {
            throw new System.NotImplementedException(GetType() + "has no texture setter");
        }
    }

    public virtual Shader shader
    {
        get
        {
            Material mat = material;
            return (mat != null) ? mat.shader : null;
        }
    }

    public int depth
    {
        get
        {
            return mDepth;
        }
        set
        {
            if (mDepth != value)
            {
                if (panel != null) panel.RemoveWidget(this);
                mDepth = value;

                if (panel != null)
                {
                    panel.AddWidget(this);

                    if (!Application.isPlaying)
                    {
                        //panel.SortWidgets();
                        //panel.RebuildAllDrawCalls();
                    }
                }
            }
        }
    }

    public bool isVisible { get { return true; } }

    public int width
    {
        get
        {
            return mWidth;
        }
        set
        {
            int min = 100;
            if (value < min) value = min;

            if (mWidth != value)
            {                
                SetDimensions(value, mHeight);
            }
        }
    }

    public int height
    {
        get
        {
            return mHeight;
        }
        set
        {
            int min = 100;
            if (value < min) value = min;

            if (mHeight != value)
            {
               SetDimensions(mWidth, value);
            }
        }
    }

    public override Vector3[] worldCorners
    {
        get
        {
            Vector2 offset = pivotOffset;

            float x0 = -offset.x * mWidth;
            float y0 = -offset.y * mHeight;
            float x1 = x0 + mWidth;
            float y1 = y0 + mHeight;

            Transform wt = cachedTransform;

            mCorners[0] = wt.TransformPoint(x0, y0, 0f);
            mCorners[1] = wt.TransformPoint(x0, y1, 0f);
            mCorners[2] = wt.TransformPoint(x1, y1, 0f);
            mCorners[3] = wt.TransformPoint(x1, y0, 0f);

            return mCorners;
        }
    }

    // change pivot, change corners, change pos
    public Pivot pivot
    {
        get
        {
            return mPivot;
        }
        set
        {
            if (mPivot != value) {
                Vector3 before = worldCorners[0];

                mPivot = value;
                //mChanged = true;

                Vector3 after = worldCorners[0];

                Transform t = cachedTransform;
                Vector3 pos = t.position;
                float z = t.localPosition.z;
                pos.x += (before.x - after.x);
                pos.y += (before.y - after.y);
                cachedTransform.position = pos;

                pos = cachedTransform.localPosition;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                pos.z = z;
                cachedTransform.localPosition = pos;
            
            }
        }
    }

    public Vector2 pivotOffset { get { return NGUIMath.GetPivotOffset(pivot); } }

    protected override void OnStart() { CreatePanel(); }

    // method:-----------------------------
    // mark widget changed, so geo can rebuilt
    void SetDirty() { }

    protected void MarkAsChanged() { }

    // check if widget moved to panel
    public bool UpdateTransform(int frame) {
        return false;
    }
    // update widget and fill geo
    public bool UpdateGeometry(int frame) {

        geometry.Clear();
        OnFill(geometry.verts, geometry.uvs, geometry.cols);

        mLocalToPanel = panel.worldToLocal * cachedTransform.localToWorldMatrix;

        // 最终verts = verts实际在world中的位置
        //          = widget一开始是在world中心点的一块矩形
        //              * 变换为Panel的局部坐标
        //              * 再从Panel坐标变换为World坐标
        // worldVerts = widget dimension verts(vert as world) * worldVertToPanel * PanelToworld
        geometry.ApplyTransform(mLocalToPanel);
        //mMoved = false;        
        return false;
    }
    public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t) {

        geometry.WriteToBuffers(v, u, c);
    }
    virtual public void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols) { }

    static public int PanelCompareFunc(UIWidget left, UIWidget right)
    {
        if (left.mDepth < right.mDepth) return -1;
        if (left.mDepth > right.mDepth) return 1;

        Material leftMat = left.material;
        Material rightMat = right.material;

        if (leftMat == rightMat) return 0;
        if (leftMat != null) return -1;
        if (rightMat != null) return 1;

        return (leftMat.GetInstanceID() < rightMat.GetInstanceID()) ? -1 : 1;
    }

    protected void RemoveFromPanel()
    {
        if (panel != null)
        {
            panel.RemoveWidget(this);
            panel = null;
        }

    }


    public void SetDimensions(int w, int h) // 新w或者h，通过长宽比调整长宽
    {
        if (mWidth != w || mHeight != h)
        {
            mWidth = w;
            mHeight = h;

            
            MarkAsChanged();
        }
    }

    public UIPanel CreatePanel()
    {
        //if (panel == null && enabled && NGUITools.GetActive(gameObject))
        //{
            panel = UIPanel.Find(cachedTransform, true, cachedGameObject.layer);

            if (panel != null)
            {
                //mParentFound = false;
                panel.AddWidget(this);
                //CheckLayer();
                //Invalidate(true);
            }
        //}
        return panel;
    }

    public UIPanel CreatePanel()
    {
        panel = UIPanel.Find(cachedTransform, true, cachedGameObject.layer);

        if (panel != null)
        {
            panel.AddWidget(this);
        }

        return panel;
    }
}
