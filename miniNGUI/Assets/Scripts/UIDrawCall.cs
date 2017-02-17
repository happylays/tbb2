using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDrawCall : MonoBehaviour {

    // field::----------------------------
    List<Vector3> verts = new List<Vector3>();
    List<Vector3> uvs = new List<Vector3>();
    List<Vector3> cols = new List<Vector3>();

    Material mMaterial;
    Texture mTexture;
    Shader mShader;
    Mesh mMesh;
    MeshFilter mFilter;
    MeshRenderer mRenderer;
    Material mDynamicMat;
    int[] mIndices;

    bool mRebuildMat = true;
    bool mRenderQueue = 3000;
    int mTriangles = 0;

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

    // method:----------------------------
    // set dc geometry
    void UpdateGeometry() {
        int count = verts.size;

        if (count > 0 && count == uvs.size && count == cols.size)
        {
            // cache all components
            if (mFilter == null) mFilter = gameObject.GetComponent<MeshFilter>();

            // populate index buffer
            int indexCount = (count >> 1) * 3;

            // create mesh
            if (mMesh == null)
            {
                mMesh = new Mesh();
                mMesh.hideFlags = HideFlags.DontSave;
                mMesh.name = (mMaterial != null) ? mMaterial.name : "Mesh";
                mMesh.MarkDynamic();
            }

            // mesh fill geometry(vert, uv, col, triangle, indices
            mTriangles = (verts.size >> 1);
            if (mMesh.vertexCount != verts.buffer.Length)
            {
                mMesh.Clear();
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
    void UpdateMaterials() { }
    void RebuildMaterial() { }
    void CreateMaterial() { }
    void Create() { }
}
