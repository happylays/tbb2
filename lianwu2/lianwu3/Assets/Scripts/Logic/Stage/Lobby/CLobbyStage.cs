using UnityEngine;
using System.Collections;

public class cLobbyStage : cBaseStage
{
    bool mLoadComplete = false;

    cLobbyUIManager mLobbyUIManager;

    protected override string Level
    {
        get { return "scene_lobby"; }
    }

    public override void InitStage()
    {
        cResourceManager.Instance.InitLogin();

        mLobbyUIManager = new cLobbyUIManager();
        mLobbyUIManager.Init();

        //cMainApp.Instance.InitGMTool();
    }

    public override void Open()
    {

        mLoadComplete = false;

        cWorldManager.Instance.Close();

        mLobbyUIManager.Open();

        //cResourceManager.Instance.LoadInitSData();
    }

    public override void Process()
    {
        cResourceManager.eDATA_STAGE stage = cResourceManager.Instance.CheckInitData();
        if (stage == cResourceManager.eDATA_STAGE.eEnd)
        {
            mLoadComplete = true;
        }

        //if (mLoadComplete)
        //    cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);

        mLobbyUIManager.Process();
    }

    public override void Close()
    {
        mLobbyUIManager.Close();
    }

    public override void Exit()
    {
        //cResourceManager.Instance.LoadEnd();
        mLobbyUIManager = null;
        mStageManager = null;
    }
}
