using UnityEngine;
using System.Collections;
using LoveDance.Client.Network;

public class cStageManager : cSingleton<cStageManager> {

    cBaseStage[] mStages = new cBaseStage[(int)cBaseStage.eSTAGE.eStage_Max];
    cBaseStage.eSTAGE mCurStage;

    //NetworkMgr mNetworkMgr;
    //cGameResourceManager mGameResourceManager;
    //cSceneManager mSceneManager;
    UIMgr mUIMgr;
    PlayerManager mPlayerManager;

    public void Init() {        

        NetworkMgr.InitNetwork();
        UIMgr.Init();

        mCurStage = cBaseStage.eSTAGE.eStage_Login;
        //mStages = new cBaseStage[(int)cBaseStage.eSTAGE.eStage_Max];
        mStages[(int)cBaseStage.eSTAGE.eStage_Login] = new cLoginStage();
        //mStages[(int)cBaseStage.eSTAGE.eStage_Select] = new cSelectStage();
        mStages[(int)cBaseStage.eSTAGE.eStage_Lobby] = new cLobbyStage();
        mStages[(int)cBaseStage.eSTAGE.eStage_Room] = new cRoomStage();
        mStages[(int)cBaseStage.eSTAGE.eStage_Game] = new cGameStage();

        for (int i = 0; i < (int)cBaseStage.eSTAGE.eStage_Max; ++i)
        {
            mStages[i].Init(this);
        }
        /// stage open
        mStages[(int)mCurStage].InitStage();
        mStages[(int)mCurStage].Open();
    }

    public void Process() {  }

    public void Exit() { }

    public void ChangeStage(cBaseStage.eSTAGE stage) {

        if (mCurStage == stage) return;

        mStages[(int)mCurStage].Close();
        
        mStages[(int)stage].InitStage();
        mStages[(int)stage].Open();

        ManagedStageMap(mCurStage, stage);

        mCurStage = stage;
    
    }

    public void ManagedStageMap(cBaseStage.eSTAGE curStage, cBaseStage.eSTAGE newStage)
    {
        switch (newStage)
        {
            case cBaseStage.eSTAGE.eStage_Login:
            case cBaseStage.eSTAGE.eStage_Lobby:
            case cBaseStage.eSTAGE.eStage_Room:
            case cBaseStage.eSTAGE.eStage_Game:
                cCameraManager.Instance.ChangeCamera();
                mStages[(int)newStage].LoadLevel();
                break;
        }
    }
}
