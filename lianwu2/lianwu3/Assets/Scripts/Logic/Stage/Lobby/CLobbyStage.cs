using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;
using LoveDance.Client.Logic;

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
        //ClientResourcesMgr.InitGameLoader();

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

        UICoroutine.uiCoroutine.StartCoroutine(Load());
        //UICoroutine.uiCoroutine.StartCoroutine(LoadRole());

        // coroutine        
        //LoadMusic();
        //LoadBgTexture();
    }

    IEnumerator Load()
    {
        //1. cResourceManager.Instance.LoadClientResource();
        yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        //2. 
        UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_createrole, null));
    }
    
    public override void Process()
    {
        //cResourceManager.eDATA_STAGE stage = cResourceManager.Instance.CheckInitData();
        //if (stage == cResourceManager.eDATA_STAGE.eEnd)
        //{
        //    mLoadComplete = true;
        //}

        //if (mLoadComplete)
        //    cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Lobby);

        mLobbyUIManager.Process();
    }

    public override void Close()
    {
        mLobbyUIManager.Close();

        PlayerBase mainPlayer = CommonLogicData.MainPlayer;
        if (mainPlayer != null)
        {
            mainPlayer.DestroyUIRoleCamera();
        }
    }

    public override void Exit()
    {
        //cResourceManager.Instance.LoadEnd();
        mLobbyUIManager = null;
        mStageManager = null;
    }
}
