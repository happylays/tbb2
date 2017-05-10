using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;

public abstract class SceneBehaviourBase : MonoBehaviour
{
    public abstract IScene CurScene { get; set; }

    public abstract IStandPlayerPosition[] StandPlayerPosition { get; }
    
    public abstract void RegisterSceneEventItem(ISceneEventItem item);

    public abstract ISceneEventItem GetSceneEventItemByID(byte eventId);

    public abstract void RegisterSceneItem(ISceneItem item);

    public abstract ISceneItem GetSceneObjByIndex(int index);

    public abstract Dictionary<int, ISceneItem> GetSceneItemMap();
}
