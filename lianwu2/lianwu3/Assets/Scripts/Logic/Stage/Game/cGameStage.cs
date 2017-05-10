using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Data.Scene;
using LoveDance.Client.Data;

public class cGameStage : cBaseStage
{
    bool mLoadComplete = false;    

    protected override string Level
    {
        get { return "scene_game"; }
    }

    public override void InitStage()
    {
        ClientResourcesMgr.InitGameLoader();
        
        UICoroutine.InitUICoroutine();
        
        //cMainApp.Instance.InitGMTool();
    }

    public override void Open()
    {

        mLoadComplete = false;

        cWorldManager.Instance.Close();

        UICoroutine.uiCoroutine.StartCoroutine(Load());        

        // coroutine        
        //LoadMusic();
        //LoadBgTexture();
    }

    IEnumerator Load()
    {
        //1. cResourceManager.Instance.LoadClientResource();
        yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        // LoadMatch

        //IEnumerator itor = AnimationLoader.LoadStageSceneAnimation(RoomData.PlayMusciInfo.m_strMusicSource, RoomData.PlaySongMode);
    }

    public static IEnumerator DownLoadMatch()
    {
        IEnumerator itor = null;
        itor = AnimationLoader.DownLoadStageSceneAnimation(RoomData.PlayMusciInfo.m_strMusicSource, RoomData.PlaySongMode);
        while (itor.MoveNext())
        {
            yield return null;
        }

        CSceneInfo sceneInfo = StaticData.SceneDataMgr.GetSceneByID((byte)RoomData.PlayScene);
        if (sceneInfo != null)
        {
            itor = SceneLoader.DownLoadStageScene(sceneInfo.m_strSceneStage);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        UIFlag targetFlag = UIFlag.ui_taigu;
        itor = UIMgr.DownLoadUISync(targetFlag);
        while (itor.MoveNext())
        {
            yield return null;
        }

    }

    public static IEnumerator LoadMatch(byte[] stageInfo)
    {
        IEnumerator itor = AnimationLoader.LoadStageSceneAnimation(RoomData.PlayMusciInfo.m_strMusicSource, RoomData.PlaySongMode);
        while (itor.MoveNext())
        {
            yield return null;
        }

        CSceneInfo sceneInfo = StaticData.SceneDataMgr.GetSceneByID((byte)RoomData.PlayScene);
        if (sceneInfo != null)
        {
            itor = SceneLoader.LoadStageScene(sceneInfo.m_strSceneStage);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        StageScene stageScene = null;
        if (CSceneBehaviour.Current != null)
        {
            stageScene = CSceneBehaviour.Current.gameObject.AddComponent<StageScene>();
            CSceneBehaviour.Current.CurScene = stageScene;
        }

        itor = UIMgr.ShowUIAsync(UIFlag.ui_taigu, null);
        while (itor.MoveNext())
        {
            yield return null;
        }

        CMatchBase.CurrentMatch.PrepareMatch(true, true, stageInfo);

        if (stageScene != null)
        {
            stageScene.gameObject.SendMessage("PlayerEnterScene", true, SendMessageOptions.RequireReceiver);
        }

        CMatchBase.CurrentMatch.BeginMatch();
        
    }

    public override void Process()
    {
    }

    public override void Close()
    {
    }

    public override void Exit()
    {
        //cResourceManager.Instance.LoadEnd();
        mStageManager = null;
    }
}
