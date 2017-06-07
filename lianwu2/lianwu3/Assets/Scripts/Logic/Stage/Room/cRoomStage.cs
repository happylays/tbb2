using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Data.Scene;
using LoveDance.Client.Data;
using LoveDance.Client.Logic.Scene;

public class cRoomStage : cBaseStage
{
    bool mLoadComplete = false;

    protected override string Level
    {
        get { return "scene_room"; }
    }

    public override void InitStage()
    {
        //ClientResourcesMgr.InitGameLoader();

        //UICoroutine.InitUICoroutine();

        //cMainApp.Instance.InitGMTool();
    }

    public override void Open()
    {

        mLoadComplete = false;

        cWorldManager.Instance.Close();

        SceneSwitchMgr.TrySwitch(new RoomSceneSwitch(CreateRoomType.Normal, null));

        //GameMsg_S2C_PrepareRoom msg = new GameMsg_S2C_PrepareRoom();
        //res.m_nScene = 27;
        //res.m_nMusic = 2246;
        //res.m_nMode = 1;
        //res.m_nLevel = 1;
        //res.m_strCheckKey = ;
        //res.m_szStage = null;
        //res.m_CountDownTime = 0;

        //UICoroutine.uiCoroutine.StartCoroutine(DownLoadMatch());

        // coroutine        
        //LoadMusic();
        //LoadBgTexture();
    }

    IEnumerator Load()
    {
        yield return null;

        //1. cResourceManager.Instance.LoadClientResource();
        ///yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        // LoadMatch

        //IEnumerator itor = AnimationLoader.LoadStageSceneAnimation(RoomData.PlayMusciInfo.m_strMusicSource, RoomData.PlaySongMode);
    }

    public static IEnumerator DownLoadMatch()
    {
        yield return null;

        SwitchingControl.HideSwitching();
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
