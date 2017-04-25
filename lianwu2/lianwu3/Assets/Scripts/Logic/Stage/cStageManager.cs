using UnityEngine;
using System.Collections;

public class cStageManager : cSingleton<cStageManager> {

    cBaseStage[] mStages = new cBaseStage[(int)cBaseStage.eSTAGE.eStage_Max];
    cBaseStage.eSTAGE mCurStage;

    ServerConnect mServerConnect;
    //cGameResourceManager mGameResourceManager;
    //cSceneManager mSceneManager;
    UIMgr mUIMgr;
    PlayerManager mPlayerManager;

    public void Init() {
        ServerConnect.Instance.Init();
        UIMgr.Instance.Init();

        mCurStage = cBaseStage.eSTAGE.eStage_Login;
        mStages[(int)cBaseStage.eSTAGE.eStage_Login] = new cLoginStage();
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

        }
    }
}
