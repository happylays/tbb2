using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;

public abstract class SceneBehaviourBase : MonoBehaviour
{
    public abstract IScene CurScene { get; set; }

    public abstract ISceneCamera CameraControl { get; }

    public abstract IStandPlayerPosition[] StandPlayerPosition { get; }

    public abstract void CameraFixed(bool bFix);

    public abstract void CameraInFocus(Transform focusTo);

    public abstract void CameraSequence(bool bAuto);

    public abstract void CameraShowTime();

    public abstract void CameraNoFocus();

    public abstract void EmptyScene(bool bEmpty);

    public abstract void RegisterSceneEventItem(ISceneEventItem item);

    public abstract ISceneEventItem GetSceneEventItemByID(byte eventId);

    public abstract void RegisterSceneItem(ISceneItem item);

    public abstract ISceneItem GetSceneObjByIndex(int index);

    public abstract Dictionary<int, ISceneItem> GetSceneItemMap();
}
