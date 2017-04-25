using UnityEngine;
using System.Collections;

public class cMainApp : cSingleton<cMainApp>
{
    cSoundSystem mSoundSystem;
    cResourceManager mResourceManager;
    cCameraManager mCameraManager;
    cStageManager mStageManager;

    void Awake() 
    {
        Init();
    }

    void Init() 
    {
        mSoundSystem = new cSoundSystem();
        mSoundSystem.Init();

        mResourceManager = new cResourceManager();
        mResourceManager.Init();

        mCameraManager = new cCameraManager();
        mCameraManager.Init();

        mStageManager = new cStageManager();
        mStageManager.Init();
    }

    void Update() {

        mResourceManager.Process();

        mCameraManager.Process();

        mStageManager.Process();
  
    }

    void Exit() 
    {
        mStageManager.Exit();
    }
}
