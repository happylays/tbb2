using UnityEngine;
using System;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Network.Login;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;

public class cTaiguView : View
{
    public override UIFlag UIID
    {
        get { return UIFlag.ui_taigu; }
    }

    //mvc
    private cTaiguClip _uiClip;

    void Start()
    {
        OnShowWnd();
    }

    public void show()
    {
        //initView();
        //addEvent();
        //initData();
    }

    public void initView()
    {
        if (_uiClip == null)
        {
            _uiClip = new cTaiguClip(this.gameObject);
        }
    }

    public void initData() { }

    public void hide()
    {
        //view.hide();
        //view.hide();
        removeEvent();

    }

    public void update() { }

    public void addEvent()
    {
        //UIEventListener.Get(_uiClip.btnOk).onClick = onEvent;

        //UIEventListener.Get(_uiClip.m_btnOk.gameObject).onClick = OnClickOk;
    }

    public void removeEvent()
    {
        //UIEventListener.Get(_uiClip.m_btnOk.gameObject).onClick = null;

    }

    public void onEvent(GameObject go)
    {
        //data = DynamicData.Get(id);
        //_uiClip.SetMethod(data);
    }

    public void OnClickOk(GameObject go)
    {
        //GameMsg_C2S_Login msg = new GameMsg_C2S_Login();        
        //NetworkMgr.SendMsg(msg);

        //NetworkMgr.DoMessage(GameMsgType.MSG_ACCOUNT_LoginResult);
    }

    // receive msg
    public void onMsg()//GameMsgBase msg) 
    {
        //baseMsg msg = msg;
    }

    public void staticMethod()
    {

    }

    // send msg
    public void dynamicMethod()
    {
        //Network.send(msg);
    }

    public void destroy()
    {
        //base.destroy();

        if (_uiClip != null)
        {
            _uiClip.destroy();
        }
        _uiClip = null;
    }

    //old
    public void OnShowWnd()
    {
        initView();
        addEvent();
        initData();

    }

    public void OnHideWnd()
    {
        removeEvent();
        //base.OnHideWnd();
    }


}