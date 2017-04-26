using UnityEngine;
using System.Collections;
using LoveDance.Client.Network;

public class cMainApp : cSingleton<cMainApp>
{
    cSoundSystem mSoundSystem;
    cResourceManager mResourceManager;
    cCameraManager mCameraManager;
    //cStageManager mStageManager;

    public void Init() 
    {
        mSoundSystem = new cSoundSystem();
        mSoundSystem.Init();

        mResourceManager = new cResourceManager();
        mResourceManager.Init();

        mCameraManager = new cCameraManager();
        mCameraManager.Init();

        cStageManager.Instance.Init();
    }

    public void Process() {

        //mResourceManager.Process();

        //mCameraManager.Process();

        cStageManager.Instance.Process();

        NetworkMgr.ProcessNetwork();
  
    }

    void Exit() 
    {
        cStageManager.Instance.Exit();
    }
}
