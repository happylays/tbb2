using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Logic;
using LoveDance.Client.Logic.Scene;

public class UnityLogic
{
    public IScenceType AddCompent(ScenceType type)
    {
        IScenceType curScence = null;

        if (CSceneBehaviour.Current != null)
        {
            switch (type)
            {
                case ScenceType.Room_Scene:
                    curScence = CSceneBehaviour.Current.gameObject.AddComponent<RoomScene>();
                    break;
            }
        }

        return curScence;
    }
    public IEnumerator PrepareUIAsync(UIFlag showID)
    {
        return UIMgr.PrepareUIAsync(showID);
    }

    public IEnumerator SwitchUIAsync(UIFlag showID, object exData)
    {
        return UIMgr.SwitchUIAsync(showID, exData);
    }
}