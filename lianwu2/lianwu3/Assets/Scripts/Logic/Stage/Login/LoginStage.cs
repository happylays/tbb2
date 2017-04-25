using UnityEngine;
using System.Collections;

public class cLoginStage : cBaseStage {

    bool mLoadComplete = false;
    cLoginUIManager mLoginUIManager;

    public override void InitStage() {
        
        cResourceManager.Instance.InitLogin();
        
        mLoginUIManager = new cLoginUIManager();
        mLoginUIManager.Init();

        //cMainApp.Instance.InitGMTool();
    }

    public override void Open() {

        cWorldManager.Instance.Close();

        mLoginUIManager.Open();

        cResourceManager.Instance.LoadInitSData();
                
        cNetwork.Instance.Connect("172.168.6.36");
    }

    public override void Process() {

        cResourceManager.eDATA_STAGE stage = cResourceManager.Instance.CheckInitData();
        if (stage == cResourceManager.eDATA_STAGE.eEnd)
        {
            mLoadComplete = true;            
        }

        if (mLoadComplete)
            cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);

        mLoginUIManager.Process();
    }

    public override void Close() {
        mLoginUIManager.Close();
    }

    public override void Exit() {
        //cResourceManager.Instance.LoadEnd();
        mLoginUIManager = null;
        mStageManager = null;
    }
}
