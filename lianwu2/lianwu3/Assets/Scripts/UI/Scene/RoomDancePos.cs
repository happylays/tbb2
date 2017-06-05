using UnityEngine;
using LoveDance.Client.Common;

public class RoomDancePos : MonoBehaviour
{
    [SerializeField]
    GameObject m_RoomHost = null;
    [SerializeField]
    GameObject m_Prepare = null;
    [SerializeField]
    GameObject m_Equip = null;
    [SerializeField]
    MeshRenderer m_RoomHostRender = null;
    [SerializeField]
    MeshRenderer m_RoomDancerRender = null;
    
    public void Init()
    {
        //if (m_PosState != null)
        //{
        //    m_PosState.Init();
        //}
    }

    public void SetPosState(RoomPosState state, bool showAni)
    {
        //if (m_PosState != null)
        //{
        //    m_PosState.SetPosState(state, showAni);
        //}
    }

    public void ClosePos()
    {
    //    if (m_PosState != null)
    //    {
    //        m_PosState.ClosePos();
    //    }
    }

    public void SetPlayerState(bool hasPlayer, bool host, RoleRoomState state)
    {
        host &= hasPlayer;

        bool isPrepare = hasPlayer & (state == RoleRoomState.Ready) && !host;
        bool isEquip = hasPlayer & (state == RoleRoomState.Equip) && !host;

        if (m_RoomHost != null)
        {
            m_RoomHost.SetActive(host);
        }

        if (m_RoomHostRender != null)
        {
            m_RoomHostRender.enabled = host;
        }

        if (m_RoomDancerRender != null)
        {
            m_RoomDancerRender.enabled = !host;
        }

        if (m_Prepare != null)
        {
            m_Prepare.SetActive(isPrepare);
        }

        if (m_Equip != null)
        {
            m_Equip.SetActive(isEquip);
        }
    }

}
