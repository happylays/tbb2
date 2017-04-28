using System.Collections.Generic;
using UnityEngine;
using LoveDance.Client.Network.Login;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
//using LoveDance.Client.Network.SystemSetting;
//using LoveDance.Client.Logic.VIP;
//using LoveDance.Client.Logic.Role;
//using LoveDance.Client.Logic;
//using LoveDance.Client.Data.Setting;

public class PlayerManager
{
	//游戏中的玩家集合 ,暂时用于控制人物的特效显示与隐藏
	private static List<NewPlayer> m_PlayerList = new List<NewPlayer>();
	
    public static PlayerBase CreateMainPlayerLogic(GameMsg_S2C_CreateRoleSuc createMsg)
    {
        GameObject go = new GameObject("MainPlayer");
        GameObject.DontDestroyOnLoad(go);
        NewPlayer newPlayer = go.AddComponent<NewPlayer>();
        if (newPlayer != null)
        {
            //CommonLogicData.MainPlayer = newPlayer;
            
//             newPlayer.RoleAttr = new PlayerAttr();
//             newPlayer.RoleAttr.SerializeAttr(createMsg.GetPacketBuffer("Attribute"));

            //CommonLogicData.MainPlayerID = newPlayer.RoleAttr.RoleID;

            MainPlayerItem roleItem = new MainPlayerItem();
            roleItem.InitItemList(newPlayer.RoleAttr.BadgeGridNum);
            newPlayer.RoleItem = roleItem;

        }
        else
        {
            Debug.LogError("PlayerManager CreateMainPlayerLogic ,newPlayer can not be null.");
        }
        return newPlayer;
    }

    public static PlayerBase CreateLogic(BriefAttr briefAttr, bool bVisible, NetReadBuffer itemBuf, NetReadBuffer geneBuf)
    {
        return CreateLogic(briefAttr, bVisible, itemBuf, geneBuf, true);
    }

	public static PlayerBase CreateLogic(BriefAttr briefAttr, bool bVisible, NetReadBuffer itemBuf, NetReadBuffer geneBuf, bool dontDestroyOnLoad)
	{
		return CreateLogic(null, briefAttr, bVisible, itemBuf, geneBuf, dontDestroyOnLoad);
	}

    public static PlayerBase CreateLogic(GameObject targetObject, BriefAttr briefAttr, bool bVisible, NetReadBuffer itemBuf, NetReadBuffer geneBuf, bool dontDestroyOnLoad)
    {
		NewPlayer newPlayer = null;
		if (targetObject == null)
		{
			targetObject = new GameObject(briefAttr.m_strRoleName);
		}
		if (dontDestroyOnLoad)
		{
			GameObject.DontDestroyOnLoad(targetObject);
		}
		newPlayer = targetObject.AddComponent<NewPlayer>();
        if (newPlayer != null)
        {
			newPlayer.RoleBone = targetObject.GetComponentInChildren<PlayerBone>();
			if (newPlayer.RoleBone != null)
			{
				newPlayer.RoleBody = targetObject;
			}

            newPlayer.IsToShow = bVisible;
            //newPlayer.PlayerMoveType = SystemManager.SysMgr.m_DefPlayerMoveType;

        }
        else
        {
            Debug.LogError("PlayerManager CreateLogic ,newPlayer can not be null.");
        }
        return newPlayer;
    }

    public static PlayerBase CopyLogicDress(PlayerBase srcPlayer, bool includeEquip)
    {
        GameObject targetGO = new GameObject();
        NewPlayer newPlayer = targetGO.AddComponent<NewPlayer>();
        newPlayer.IsToShow = true;

        if (srcPlayer != null)
        {
			PlayerAttr attrClone = null;
			PlayerItem itemClone = null;
			PlayerGene geneClone = null;
			
			if (srcPlayer.RoleAttr != null)
			{
				attrClone = (PlayerAttr)srcPlayer.RoleAttr.Clone();
			}
			newPlayer.RoleAttr = attrClone;
			
			if (includeEquip)
			{
				if (srcPlayer.RoleItem != null)
				{
					itemClone = (PlayerItem)srcPlayer.RoleItem.Clone();
				}
				if (srcPlayer.RoleGene != null)
				{
					geneClone = (PlayerGene)srcPlayer.RoleGene.Clone();
				}
				newPlayer.RoleItem = itemClone;
				newPlayer.RoleGene = geneClone;
			}
		}

        newPlayer.PlayerMoveType = srcPlayer.PlayerMoveType;

        return newPlayer;
    }

	public static void AddPlayerToControl(NewPlayer newPlayer)
	{
		//不屏蔽本人和UI预览的
		if ((newPlayer.RoleAttr.RoleID == CommonLogicData.MainPlayerID) || (newPlayer.RoleAttr.RoleID == 0))
		{
			return;
		}

		if (!PlayerSetting.ShowLightEffect)
		{
			newPlayer.SetClothEffectIsVisible(false);
		}

		m_PlayerList.Add(newPlayer);
	}

	public static void RemovePlayerFromControl(NewPlayer newPlayer)
	{
		for (int i = 0; i < m_PlayerList.Count; i++)
		{
			if (m_PlayerList[i] == newPlayer)
			{
				m_PlayerList.Remove(newPlayer);
			}
		}
	}

	/// <summary>
	/// 设置玩家（NewPlayer）身上的光效石特效显示（隐藏）
	/// </summary>
	/// <param name="state"></param>
	public static void ShowClothEffect(bool state)
	{
		for (int i = 0; i < m_PlayerList.Count; i++)
		{
			if (m_PlayerList[i] != null)
			{
				m_PlayerList[i].SetClothEffectIsVisible(state);
			}
		}
	}

	/// <summary>
	/// 清除缓存的players数据
	/// </summary>
	public static void DestroyPlayersData()
	{
		m_PlayerList.Clear();
	}
}
