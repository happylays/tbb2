﻿using UnityEngine;
using System.Collections;
using LoveDance.Client.Logic.Login;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;

public class cLoginStage : cBaseStage {

    bool mLoadComplete = false;
    cLoginUIManager mLoginUIManager;

    GameObject mLoginUI;

    protected override string Level
    {
        get { return "scene_login"; }
    }

    public override void InitStage() {
        
        //cResourceManager.Instance.InitLogin();
        ClientResourcesMgr.InitGameLoader();

        mLoginUIManager = new cLoginUIManager();
        mLoginUIManager.Init();

        mLoginUI = GameObject.FindObjectOfType<cLoginView>().gameObject;
        //cMainApp.Instance.InitGMTool();
        //UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_lobby, null));

        UICoroutine.InitUICoroutine();

        cLoginLogic.CreateLogic(NetObserver).RegistNetMessage();
    }

    public override void Open() {

        mLoadComplete = false;

        cWorldManager.Instance.Close();

        mLoginUIManager.Open();

        //cResourceManager.Instance.LoadInitSData();
                
        //cNetwork.Instance.Connect("172.168.6.36");
    }

    public override void Process() {

        //cResourceManager.eDATA_STAGE stage = cResourceManager.Instance.CheckInitData();
        //if (stage == cResourceManager.eDATA_STAGE.eEnd)
        //{
        //    mLoadComplete = true;            
        //}

        //if (mLoadComplete)
        //    cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);

        mLoginUIManager.Process();
    }

    public override void Close() {
        
        mLoginUIManager.Close();

        mLoginUI.SetActive(false);
    }

    public override void Exit() {
        //cResourceManager.Instance.LoadEnd();
        mLoginUIManager = null;
        mStageManager = null;
    }
}
