using System.Collections;
using UnityEngine;

namespace LoveDance.Client.Network
{
	public delegate void NetMsgProcessor(GameMsgBase Msg);
	public class NetMsgMap
	{
		private static Hashtable s_MsgMap = new Hashtable();

		public static void DispatchNetMsg(GameMsgBase Msg)
		{
			//Debug.Log("DispatchNetMsg Msg:" + Msg.getMsgType() + "s_MsgMap count" + s_MsgMap.Count);
			GameMsgType nMsgType = Msg.getMsgType();
			if (s_MsgMap.Contains(nMsgType))
			{
				//Debug.Log("DispatchNetMsg 2");
				NetMsgProcessor MsgEntry = (NetMsgProcessor)s_MsgMap[nMsgType];
				if (MsgEntry != null)
				{
					MsgEntry(Msg);
				}
				else
				{
					Debug.LogError("MsgEntry is null ,ID:" + Msg.getMsgType());
				}
			}
		}

		public static void RegistMsgProcessor(GameMsgType nMsgType, NetMsgProcessor Msgprocessor)
		{
			//Debug.Log("RegistMsgProcessor,nMsgType:" + nMsgType);
			if (s_MsgMap.Contains(nMsgType))
			{
				NetMsgProcessor OldHandler = (NetMsgProcessor)s_MsgMap[nMsgType];
				OldHandler += Msgprocessor;
				s_MsgMap[nMsgType] = OldHandler;
			}
			else
			{
				s_MsgMap[nMsgType] = Msgprocessor;
			}
		}

		public static void UnRegistMsgProcessor(GameMsgType nMsgType, NetMsgProcessor Msgprocessor)
		{
			if (s_MsgMap.Contains(nMsgType))
			{
				NetMsgProcessor OldHandler = (NetMsgProcessor)s_MsgMap[nMsgType];
				OldHandler -= Msgprocessor;
				if (OldHandler != null)
				{
					s_MsgMap[nMsgType] = OldHandler;
				}
				else
				{
					s_MsgMap.Remove(nMsgType);
				}
			}
		}

	}
}