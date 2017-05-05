using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;

public class cLobbyStage : cBaseStage
{
    bool mLoadComplete = false;

    cLobbyUIManager mLobbyUIManager;

    private PlayerBase m_Male = null;
    
    enum SkinType : byte
    {
        None,

        White,
        Yellow,
        Dark,
    }
    private SkinType m_SkinType = SkinType.White;

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

        //UICoroutine.uiCoroutine.StartCoroutine(Load());
        UICoroutine.uiCoroutine.StartCoroutine(LoadRole());

        // coroutine        
        //LoadMusic();
        //LoadBgTexture();
    }

    IEnumerator Load()
    {
        //1. cResourceManager.Instance.LoadClientResource();
        yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        //2. 
        UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_activity, null));
    }

    IEnumerator LoadRole()
    {
        yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        //yield return UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_activity, null));

        yield return UICoroutine.uiCoroutine.StartCoroutine(AnimationLoader.LoadRoleCreateAnimation());

        UICoroutine.uiCoroutine.StartCoroutine(PreparePlayerModel());
    }

    IEnumerator PreparePlayerModel()
    {
        IEnumerator itor = null;
        if (m_Male == null)
        {
            m_Male = CreateRoleModel("Boy", true, (byte)m_SkinType);
            itor = CreateRolePhysics(m_Male);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }
    }

    PlayerBase CreateRoleModel(string modelName, bool isBoy, byte skin)
    {
        BriefAttr attr = new BriefAttr();
        attr.m_strRoleName = modelName;
        attr.m_bIsBoy = isBoy;
        attr.m_nSkinColor = skin;

        PlayerBase player = PlayerManager.CreateLogic(attr, true, null, null);

        return player;
    }

    IEnumerator CreateRolePhysics(PlayerBase player)
    {
        if (player != null)
        {
            IEnumerator itor = player.CreatePhysics(false, PhysicsType.Player);
            while (itor.MoveNext())
            {
                yield return null;
            }

            player.CurrentStyle = PlayerStyleType.Create;
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
