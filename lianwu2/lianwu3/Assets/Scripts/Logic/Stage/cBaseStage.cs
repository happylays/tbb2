using UnityEngine;
using System.Collections;

public class cBaseStage : NetMonoBehaviour
{

    public enum eSTAGE
    {
        eStage_Login = 0,
        //eStage_Select,
        eStage_Lobby,        
        eStage_Room,
        eStage_Game,

        eStage_Max
    };

    protected cStageManager mStageManager;

    protected virtual string Level
    {
        get { return string.Empty; }
    }

    public virtual void Init(cStageManager StageManager) {
        mStageManager = StageManager;
    }

    public virtual void InitStage() { }

    public virtual void Open() { }

    public virtual void Process() { }

    public virtual void Close() { }

    public virtual void Exit() { }

    public virtual void LoadLevel() {

        cSceneManager.Instance.LoadLevel(Level);

    }

}
