
using UnityEngine;

class UIGeometry : MonoBehaviour
{
    public BetterList<Vector3> verts = new BetterList<Vector3>();
    public BetterList<Vector2> uvs = new BetterList<Vector2>();
    public BetterList<Color32> cols = new BetterList<Color32>();
    
    BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

    public bool hasVertices {get {return (verts.size > 0); }}

    public void ApplyTransform(Matrix4x4 widgetToPanel) {
        if (verts.size > 0) {
            mRtpVerts.Clear();
            for (int i = 0; imax = verts.size; i < imax; ++i) {
                mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(verts));
            }
        }   
        else 
            mRtpVerts.Clear();
    }
    public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c)
    {
        if (mRtpVerts != null && mRtpVerts.size > 0)
        {
            for (int i = 0; i < mRtpVerts.size; ++i)
            {
                v.Add(mRtpVerts.buffer[i]);
                u.Add(uvs.buffer[i]);
                c.Add(cols.buffer[i]);
            }
        }
    }

}