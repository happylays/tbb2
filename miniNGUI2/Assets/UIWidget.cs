using UnityEngine;

class UIWidget : UIRect
{
    public enum Pivot {
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

    // field:
    protected int mWidget = 100;
    protected int mHeight = 100;
    protected Vector4 mDrawRegion = new Vector4(0,0,1,1);
    protected Vector3[] mCorners = new Vector3[4];

    public UIPanel panel;
    UIGeometry geometry = new UIGeometry();
    public UIDrawCall drawCall;

    protected int mDepth = 0;

    Matrix4x4 mLocalToPanel;
    public bool hasVertices {get {return geometry != null; && geometry.hasVertices;}}

    public virtual Material material 
    {
        get {return null;}
        set {throw new System.NotImplementedException(GetType() + "has no material setter");}
    }

    public virtual Texture mainTexture 
    {
        get 
        {
            Material mat = material;
            return (mat != null) ? mat.mainTexture : null;
        }
        set {
            throw new System.NotImplementedException(GetType() + "has no texture setter");
        }
    }
    
    public virtual Shader shader 
    {
        get {
            Material mat = material;
            return (mat != null) ? mat.shader : null;
        }
    }

    public override Vector3[] worldCorners {
        get {
            Vector2 offset = pivotOffset;

            float x0 = -offset.x * mWidget;
            float y0 = -offset.y * mHeight;
            float x1 = x0 + mWidget;
            float y1 = y0 + mHeight;

            Transform wt = cachedTransform;

            mCorners[0] = wt.TransformPoint(x0,y0,0f);
            mCorners[1] = wt.TransformPoint(x0,y1,0f);
            mCorners[2] = wt.TransformPoint(x1,y1,0f);
            mCorners[3] = wt.TransformPoint(x1,y0,0f);

            return mCorners;
        }
    }

    public Pivot pivot {

    }

    public Vector2 pivotOffset { get { return NGUIMath.GetPivotOffset(pivot); }}

    // method
    protected override void OnStart() {
        CreatePanel();
    }

    public bool UpdateTransform() { return false; }
    public bool UpdateGeometry(int frame) {
        geometry.Clear();

        OnFill(geometry.verts, geometry.uvs, geometry.cols);
        mLocalToPanel = panel.worldToLocal * cachedTransform.localToWorldMatrix;
        geometry.ApplyTransform(mLocalToPanel);
    }
    public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
    {
        geometry.WriteToBuffers(v,u,c);
    }
    public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols) {}
    public UIPanel CreatePanel() {
        panel = UIPanel.Find(cachedTransform, true, cachedGameObject.layer);
        if (panel != null) {
            panel.AddWidget(this);
        }

        return panel;
    }
    protected void RemoveFromPanel() {
        if (panel != null) {
            panel.RemoveWidget(this);
            panel = null;
        }
    }
    static public int PanelCompareFund(UIWidget left, UIWidget right) {
        if (left.mDepth < right.mDepth) return -1;
        if (left.mDepth > right.mDepth) return 1;

        Material leftMat = left.material;
        Material rightMat = right.material;

        if (leftMat == rightMat) return 0;
        if (leftMat != null) return -1;
        if (rightMat != null) return 1;

        return (leftMat.GetInstanceID() < rightMat.GetInstanceID()) ? -1 : 1;
    }
    protected void MarkAsChanged() { }
    void SetDirty() { }
}