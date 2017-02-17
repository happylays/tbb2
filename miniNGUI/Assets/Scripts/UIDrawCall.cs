using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDrawCall : MonoBehaviour {

    // field::----------------------------
    static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();
    static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();

    public BetterList<Vector3> verts = new BetterList<Vector3>();
    public BetterList<Vector2> uvs = new BetterList<Vector2>();
    public BetterList<Color32> cols = new BetterList<Color32>();

    Material            mMaterial;
    public Texture             mTexture;
    Shader              mShader;
    Mesh                mMesh;
    MeshFilter          mFilter;
    MeshRenderer        mRenderer;
    Material            mDynamicMat;

    [HideInInspector]
    public int[] mIndices;
    bool mRebuildMat = true;
    float mRenderQueue = 3000;
    int mTriangles = 0;

    const int maxIndexBufferCache = 10;
    static List<int[]> mCache = new List<int[]>(maxIndexBufferCache);

    // properties::----------------------------
    Material baseMaterial
    {
        get 
        { 
            return mMaterial;
        }
        set
        {
            if (mMaterial != value)
            {
                mMaterial = value;
                mRebuildMat = true;
            }
        }
    }
    Material dynamicMaterial { get { return mDynamicMat; } }
    Texture mainTexture {
        get
        {
            return mTexture;
        }
        set
        {
            mTexture = value;
            if (mDynamicMat != null) mDynamicMat.mainTexture = value;
        }
    }

    public Shader shader
    {
        get { return mShader; }
        set
        {
            if (mShader != value)
            {
                mShader = value;
                mRebuildMat = true;
            }
        }
    }

    // method:----------------------------
    // set dc geometry
    public void UpdateGeometry() {
        int count = verts.size;

        if (count > 0 && count == uvs.size && count == cols.size)
        {
            // cache all components
            if (mFilter == null) mFilter = gameObject.GetComponent<MeshFilter>();

            // populate index buffer
            int indexCount = (count >> 1) * 3;
            bool setIndices = (mIndices == null || mIndices.Length != indexCount);

            // create mesh
            if (mMesh == null)
            {
                mMesh = new Mesh();
                mMesh.hideFlags = HideFlags.DontSave;
                mMesh.name = (mMaterial != null) ? mMaterial.name : "Mesh";
                mMesh.MarkDynamic();

                setIndices = true;
            }

            // mesh fill geometry(vert, uv, col, triangle, indices
            mTriangles = (verts.size >> 1);
            if (mMesh.vertexCount != verts.buffer.Length)
            {
                mMesh.Clear();
                setIndices = true;
            }

            mMesh.vertices = verts.buffer;
            mMesh.uv = uvs.buffer;
            mMesh.colors32 = cols.buffer;

            if (setIndices)
            {
                mIndices = GenerateChachedIndexBuffer(count, indexCount);
                mMesh.triangles = mIndices;
            }

            mMesh.RecalculateBounds();
            mFilter.mesh = mMesh;

            if (mRenderer == null) mRenderer = gameObject.GetComponent<MeshRenderer>();

            // update material
            UpdateMaterials();
        }

        verts.Clear();
        uvs.Clear();
        cols.Clear();
    }
    void UpdateMaterials() {
        if (mRebuildMat || mDynamicMat == null)
        {
            RebuildMaterial();
            mRebuildMat = false;
        }
    }
    Material RebuildMaterial() {
        // destroy old material
        NGUITools.DestroyImmediate(mDynamicMat);

        // create new material
        CreateMaterial();

        // assign main texture
        if (mTexture != null) mDynamicMat.mainTexture = mTexture;

        // update renderer
        if (mRenderer != null) mRenderer.sharedMaterials = new Material[] { mDynamicMat };
        return mDynamicMat;
    }
    void CreateMaterial() {
        // get shaderName
        string shaderName = (mShader != null) ? mShader.name :
            ((mMaterial != null) ? mMaterial.shader.name : "Unlit/Transparent Colored");

        // get shader by Name
        Shader shader;
        shader = Shader.Find(shaderName);
        
        // create Material by mat or shader
        if (mMaterial != null)
        {
            mDynamicMat = new Material(mMaterial);
            mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            mDynamicMat.CopyPropertiesFromMaterial(mMaterial);
        }
        else
        {
            mDynamicMat = new Material(shader);
            mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        if (shader != null)
        {
            mDynamicMat.shader = shader;
        }
    }
    
    // return existing dc
    static public UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader) {
        string name = null;
        if (tex != null) name = tex.name;
        else if (shader != null) name = shader.name;
        else if (mat != null) name = mat.name;
        return Create(name, panel, mat, tex, shader);
        return null;    
    }

    // create new dc, or reusing old
    static UIDrawCall Create(string name, UIPanel panel, Material mat, Texture tex, Shader shader)
    {
        UIDrawCall dc = Create(name);
        //dc.gameObject.layer = panel.cachedGameObject.layer;
        dc.baseMaterial = mat;
        dc.mainTexture = tex;
        dc.shader = shader;
        //dc.mRenderQueue = panel.startingRenderQueue;
        //dc.manager = panel;
        return dc;
                
    }

    static UIDrawCall Create(string name)
    {
        if (mInactiveList.size > 0)
        {
            UIDrawCall dc = mInactiveList.Pop();
            mActiveList.Add(dc);
            if (name != null) dc.name = name;
            NGUITools.SetActive(dc.gameObject, true);
            return dc;
        }

        GameObject go = new GameObject(name);
        DontDestroyOnLoad(go);
        UIDrawCall newDC = go.AddComponent<UIDrawCall>();
        
        mActiveList.Add(newDC);
        return newDC;
    }



    int[] GenerateChachedIndexBuffer(int vertexCount, int indexCount)
    {
        for (int i = 0, imax = mCache.Count; i < imax; ++i)
        {
            int[] ids = mCache[i];
            if (ids != null && ids.Length == indexCount)
                return ids;
        }

        int[] rv = new int[indexCount];
        int index = 0;

        for (int i = 0; i < vertexCount; i += 4)
        {
            rv[index++] = i;
            rv[index++] = i + 1;
            rv[index++] = i + 2;

            rv[index++] = i + 2;
            rv[index++] = i + 3;
            rv[index++] = i;
        }

        if (mCache.Count > maxIndexBufferCache) mCache.RemoveAt(0);
        mCache.Add(rv);
        return rv;
    }
}
