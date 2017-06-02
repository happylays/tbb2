using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;
using LoveDance.Client.Loader;
using LoveDance.Client.Logic.Room;
using LoveDance.Client.Network.Lantern;
using System.Collections;
using UnityEngine;

namespace LoveDance.Client.Logic.Scene
{
    /// <summary>
    /// 等待房间场景切换
    /// </summary>
    public class RoomSceneSwitch : SceneSwitchBase
    {
        object m_RoomWholeInfo = null;							// 房间
        CreateRoomType m_RoomType = CreateRoomType.Unknown;		// 创建房间类型

        public RoomSceneSwitch(CreateRoomType roomType, object roomWholeInfo)
        {
            m_RoomType = roomType;
            m_RoomWholeInfo = roomWholeInfo;
        }

        public override string SceneName
        {
            get
            {
                return RoomData.RoomSceneName();
            }
        }

        public override IEnumerator PrepareSwitch()
        {
            // 解析数据
            if (m_RoomType == CreateRoomType.Normal)
            {
                RoomData.DestroyRoom();
                RoomWholeInfo roomWholeInfo = m_RoomWholeInfo as RoomWholeInfo;
                RoomData.Serialize(roomWholeInfo);                
            }

            yield return null;
        }

        public override IEnumerator BeginSwitch()
        {
            PlayerBase mainPlayer = CommonLogicData.MainPlayer;
            if (mainPlayer != null)
            {
                mainPlayer.DestroyUIRoleCamera();
            }

            yield return null;
        }

        public override IEnumerator LoadSceneAnimation()
        {
            AnimationLoader.ClearStageSceneAnimation();

            IEnumerator itor = AnimationLoader.LoadRoomSceneAnimation();
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        public override IScenceType AddComponent()
        {
            IScenceType curScence = null;
            UnityLogic unityLogic = CommonLogicData.UnityLogic;
            if (unityLogic != null)
            {
                curScence = unityLogic.AddCompent(ScenceType.Room_Scene);
            }

            return curScence;
        }

        public override IEnumerator CreateScenePlayer()
        {
            IEnumerator itor = RoomData.CreateAllRoomPlayer();
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        public override IEnumerator PrepareUIAndData()
        {
            IEnumerator itor = CommonLogicData.UnityLogic.PrepareUIAsync(UIFlag.ui_room);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        public override IEnumerator FinishSwitch()
        {
            if (!false)
            {
                IEnumerator itor = CommonLogicData.UnityLogic.SwitchUIAsync(UIFlag.ui_room, null);
                while (itor.MoveNext())
                {
                    yield return null;
                }

                //CommonLogicData.UnityLogic.SetMainUI(UIFlag.ui_room);

            }
        }
    }
}
