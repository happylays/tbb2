using UnityEngine;
using System.Collections;

public class cResourceManager : cSingleton<cResourceManager>
{
    public enum eDATA_STAGE
    {
        eStart = 0,
        eEnd,
        eError
    }

    cItemSData mItemSData;
    cAmusementSData mAmusementSData;

    protected eDATA_STAGE mDataLoadFlag;

    public void Init() {
        mDataLoadFlag = eDATA_STAGE.eStart;
    }

    public void InitLogin() {
        mDataLoadFlag = eDATA_STAGE.eStart;
    }

    public void LoadInitSData() 
    {
        mDataLoadFlag = eDATA_STAGE.eStart;

        mItemSData = new cItemSData();
        mItemSData.Init();

        mAmusementSData = new cAmusementSData();
        mAmusementSData.Init();

        mDataLoadFlag = eDATA_STAGE.eEnd;
    }
    
    public void Process() { }

    public eDATA_STAGE CheckInitData()
    {
        return mDataLoadFlag;
    }
}
