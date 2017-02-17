using UnityEngine;
using System.Collections;

public class TestDrawCall : MonoBehaviour {

    void Awake()
    {
        Debug.Log(1);
    }

    void Start()
    {
        GameObject drawCallObj = GameObject.Find("DrawCallObject");
        UIDrawCall drawCall = drawCallObj.GetComponent<UIDrawCall>();

        drawCall.verts.buffer =
            new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 10, 0), new Vector3(10, 0, 0), new Vector3(10, 10, 0) };
        drawCall.verts.size = 4;

        drawCall.uvs.buffer =
            new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };
        drawCall.uvs.size = 4;

        drawCall.mIndices = new int[] { 0, 1, 2, 0,1,2 };

        drawCall.cols.buffer = new Color32[] { new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255) };
        drawCall.cols.size = 4;

        drawCall.UpdateGeometry();
    }
}
