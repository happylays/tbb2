using UnityEngine;

namespace LoveDance.Client.Logic
{
    /// <summary>
    /// 用于存放游戏运行过程中产生的公共数据
    /// </summary>
    public class CommonLogicData
    {
        //由CommonLogicData清除缓存
        private static uint m_nMainPlayerID = 0;
        private static PlayerBase m_pMainPlayer = null;
        private static PlayerBase m_NpcPlayer = null;	//NPC

        //由外部清除缓存
        private static Camera s_CurrentSceneCamera = null;		//当前场景的camera;
        private static SceneBehaviourBase s_CurrentSceneBehaviour = null;
        private static bool s_IsAppPause = false;	//游戏后台暂停，仅WindowPhone用

        public static PlayerBase MainPlayer
        {
            get
            {
                return m_pMainPlayer;
            }
            set
            {
                m_pMainPlayer = value;
            }
        }

        public static uint MainPlayerID
        {
            get
            {
                return m_nMainPlayerID;
            }
            set
            {
                m_nMainPlayerID = value;
            }
        }
    }
}