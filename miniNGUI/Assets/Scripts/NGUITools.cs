using UnityEngine;
using System.Collections;

static public class NGUITools
{
    static public bool GetActive(GameObject go)
    {
#if UNITY_3_5
        return go && go.active;
#else 
        return go && go.activeInHierarchy;
#endif
    }

    static public void SetActive(GameObject go, bool state) { SetActive(go, state, true); }

    static public void SetActive(GameObject go, bool state, bool compatibilityMode)
    {
        if (go)
        {
            if (state)
            {
                Activate(go.transform, compatibilityMode);
            }
            else Deactivate(go.transform);
        }
    }

    static void Activate(Transform t, bool compatibilityMode)
    {
        SetActiveSelf(t.gameObject, true);

        if (compatibilityMode)
        {
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                if (child.gameObject.activeSelf) return;
            }

            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                Activate(child, true);
            }
        }
    }

    static public void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    static void Deactivate(Transform t)
    {
        SetActiveSelf(t.gameObject, false);
    }

    static public void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor) UnityEngine.Object.DestroyImmediate(obj);
            else UnityEngine.Object.Destroy(obj);
        }
    } 

}
