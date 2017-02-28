using UnityEngine;
using System.Collections;

public class UIRect : MonoBehaviour {

    protected GameObject mGo;
    protected Transform mTrans;

    // properties
    public GameObject cachedGameObject {get {if (mGo == null) mGo = gameObject; return mGo; }}
    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    // method
    protected void Start() {
        OnInit();
        OnStart();
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void OnStart() { }
    protected virtual void OnDisable() { }
}
