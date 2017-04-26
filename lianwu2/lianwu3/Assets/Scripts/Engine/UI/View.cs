using UnityEngine;
public class View : NetMonoBehaviour
{
    [HideInInspector]
    public GameObject gameObject;

    [HideInInspector]
    public View parent;

    virtual public void Init(GameObject go)
    {
        this.gameObject = go;
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