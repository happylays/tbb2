using UnityEngine;
using System.Collections;

public class NGUIMath : MonoBehaviour {

    static public Vector2 GetPivotOffset(UIWidget.Pivot pv)
    {
        Vector2 v = Vector2.zero;

        if (pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Right) v.x = 0.5f;
        else if (pv == UIWidget.Pivot.TopRight || pv == UIWidget.Pivot.Right || pv == UIWidget.Pivot.BottomRight) v.x = 1f;
        else v.x = 0f;

        if (pv == UIWidget.Pivot.Left || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Right) v.y = 0.5f;
        else if (pv == UIWidget.Pivot.TopLeft || pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.TopRight) v.y = 1f;
        else v.y = 0f;

        return v;
    }
}
