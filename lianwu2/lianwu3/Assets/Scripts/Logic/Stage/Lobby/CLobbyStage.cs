using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;

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
        ClientResourcesMgr.InitGameLoader();

        mLobbyUIManager = new cLobbyUIManager();
        mLobbyUIManager.Init();

        UICoroutine.InitUICoroutine();

        
        //cMainApp.Instance.InitGMTool();
    }

    public override void Open()
    {

        mLoadComplete = false;

        cWorldManager.Instance.Close();

        mLobbyUIManager.Open();

        //cResourceManager.Instance.LoadInitSData();

        //1. cResourceManager.Instance.LoadClientResource();
        ClientResourcesMgr.LoadClientResource();

        //2. 
        UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_createrole, null));
        
        
        //3. AnimationLoader.LoadRoleCreateAnimation();
        //4. PreparePlayerModel();
        //5. LoadMusic();
        //6. LoadBgTexture();
    }

    IEnumerator ShowUI() {
        IEnumerator itor = UIMgr.ShowUIAsync(UIFlag.ui_createrole, null);
        while (itor.MoveNext())
        {
            yield return null;
        }
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
    }

    public override void Exit()
    {
        //cResourceManager.Instance.LoadEnd();
        mLobbyUIManager = null;
        mStageManager = null;
    }
}
