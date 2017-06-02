using UnityEngine;
using LoveDance.Client.Common;
using LoveDance.Client.Logic;

public abstract class IRoomScene : IScenceType
{
    public abstract Transform GetPosTransForm(int nPos);

    public abstract void NewPlayerEnter(int nPos, PlayerBase player);

    public abstract void ChangeScene(int sceneID);

    public abstract void ChangeScene(int sceneID, bool needLoading);

    public abstract void ChangePosState(RoleRoomType type, int nPos, RoomPosState state, bool showAni);

    public abstract void ChangePlayerState(int pos, RoleRoomState state, bool host, bool hasPlayer);
    
    public abstract GameObject[] GetEventListeners();
}
