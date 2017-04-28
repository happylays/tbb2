using UnityEngine;
using LoveDance.Client.Common;

public class View : UIWnd
{
    //[HideInInspector]
    //public GameObject gameObject;

    public override UIFlag UIID
    {
        get { return UIFlag.none; }
    }

    [HideInInspector]
    public View parent;

    public override void Awake()
    {
        base.Awake();
        Init();
        
    }

    virtual public void Init()
    {
        //this.gameObject = go;
        addMsg();
    }

    virtual public void show() { }

    virtual public void hide() { }

    virtual public void update() { }

    virtual public void initView() { }

    virtual public void initData() { }

    virtual public void addEvent() { }

    virtual public void addMsg() { }

    virtual public void onEvent() { }

    virtual public void removeEvent() { }

    virtual public void onMsg() { }

    virtual public void Method() { }

    virtual public void destroy() { }

    
}