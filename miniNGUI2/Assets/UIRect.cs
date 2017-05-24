
using UnityEngine;
using System.Collections;

class UIRect : MonoBehaviour
{
    protected GameObject mGo;
    protected Transform mTrans;

    public GameObject cachedGameObject { get {if (mGo == null) mGo = gameObject; return mGo; }}
    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; }}

    public virtual Vector3 worldCorners { get; set; }

    protected void Start() {
        OnInit();
        OnStart();
    }
    protected virtual void OnInit() {

    }
    protected virtual void OnStart() {}
    protected virtual void OnDisable() { }
}