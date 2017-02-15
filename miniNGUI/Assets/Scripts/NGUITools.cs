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

}
