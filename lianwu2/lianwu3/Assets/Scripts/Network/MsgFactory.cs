using System.Collections;
using UnityEngine;

namespace LoveDance.Client.Network
{
	public class MsgFactory
	{
		private static Hashtable s_MsgObjMap = new Hashtable();
		public delegate GameMsgBase CreateMsg();


		public static INetWorkMessage CreateMsgByType(GameMsgType nType)
		{
			if (s_MsgObjMap.ContainsKey(nType))
			{
				CreateMsg cm = (CreateMsg)s_MsgObjMap[nType];
				return cm();
			}
			else
			{
				Debug.LogError("register none message create func, msg type: " + nType);
				return new GameMsgBase(nType);
			}
		}

		public static void AddProductLine(CreateMsg cm, GameMsgType nType)
		{
			if (s_MsgObjMap.ContainsKey(nType))
			{
				Debug.LogError("register repeat message create func, msg type: " + nType);
			}

			s_MsgObjMap[nType] = cm;
		}
	}
}