using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;
using LoveDance.Client.Loader;

using LoveDance.Client.Logic.Room;
using LoveDance.Client.Logic.Scene;
using LoveDance.Client.Network;

using LoveDance.Client.Network.Lantern;
using LoveDance.Client.Network.Room;
using System.Collections;
using UnityEngine;

namespace LoveDance.Client.Logic
{
    public class RoomBaseLogic : BaseLogic
    {
        public RoomBaseLogic(NetMsgObserver netObserver)
            : base(netObserver)
        {
        }

    }
}