using LoveDance.Client.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoveDance.Client.Logic.Scene
{
    /// <summary>
    /// 切换场景参数
    /// </summary>
    public class SceneSwitchInfo
    {
        public CallbackReturn<IEnumerator, object> m_ItorCb = null;	// 切换Coroutine
        public object m_Data = null;								// 切换时需要的数据
    }

    public class SceneSwitchMgr
    {
        static Queue<SceneSwitchInfo> m_SwitchQueue = null;	// 场景切换队列
        static bool m_IsSwitch = false;						// 是否在切换场景中 true 表示切换中

        /// <summary>
        /// 切换场景
        /// </summary>
        public static IEnumerator SwitchToScence(object obj)
        {
            SceneSwitchBase sceneSwitcher = obj as SceneSwitchBase;
            IEnumerator itor = null;
            if (sceneSwitcher != null)
            {
                if (sceneSwitcher != null)
                {
                    if (sceneSwitcher.CanSwitch)
                    {                        
                        
                            itor = sceneSwitcher.OnReleaseCachedData();
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

                            //itor = sceneSwitcher.PrepareSwitch();
                            //while (itor.MoveNext())
                            //{
                            //    yield return null;
                            //}

                            sceneSwitcher.IsCreatingScene = true;
                                                    
                            itor = SwitchingControl.ShowSwitching(true, 100);
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

                            //itor = sceneSwitcher.BeginSwitch();
                            //while (itor.MoveNext())
                            //{
                            //    yield return null;
                            //}

                            //itor = unityLogic.DestoryAllUIAsync();
                            //while (itor.MoveNext())
                            //{
                            //    yield return null;
                            //}

                            itor = sceneSwitcher.LoadSceneAnimation();
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

                            if (!string.IsNullOrEmpty(sceneSwitcher.SceneName))
                            {
                                itor = SceneLoader.LoadStageScene(sceneSwitcher.SceneName);
                                while (itor.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            else
                            {
                                Debug.LogError("SceneSwitchBase.SwitchToScence SceneName can not be null");
                            }

                            IScenceType curScene = sceneSwitcher.AddComponent();

                            //itor = sceneSwitcher.CreateScenePlayer();
                            //while (itor.MoveNext())
                            //{
                            //    yield return null;
                            //}

                            if (curScene != null)
                            {
                                SceneBehaviourBase sceneBehaviourBase = CommonLogicData.CurrentSceneBehaviour;
                                if (sceneBehaviourBase != null)
                                {
                                    sceneBehaviourBase.CurScene = curScene;
                                }
                                itor = curScene.IEPlayerEnterScene(true);
                                while (itor.MoveNext())
                                {
                                    yield return null;
                                }
                            }

                            itor = sceneSwitcher.PrepareUIAndData();
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

                            sceneSwitcher.IsCreatingScene = false;

                            itor = sceneSwitcher.FinishSwitch();
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

                            SwitchingControl.ShowSwitching();
                            //unityLogic.HideSwitching();
                        }
                        else
                        {
                            Debug.LogError("SceneSwitchMgr.SwitchToScence unityLogic can not be null");
                        }
                    
                }
                else
                {
                    Debug.LogError("SceneSwitchMgr.SwitchToScence sceneSwitcher can not be null");
                }
            }
        }

        /// <summary>
        /// 尝试切换场景
        /// </summary>
        /// <param name="sceneSwitcher">切换实例</param>
        public static void TrySwitch(SceneSwitchBase sceneSwitcher)
        {
            TrySwitch(SwitchToScence, sceneSwitcher);
        }

        /// <summary>
        /// 尝试切换场景
        /// </summary>
        /// <param name="itorCb">切换Coroutine</param>
        /// <param name="data">切换时需要的数据</param>
        public static void TrySwitch(CallbackReturn<IEnumerator, object> itorCb, object data)
        {
            if (m_SwitchQueue == null)
            {
                m_SwitchQueue = new Queue<SceneSwitchInfo>();
            }

            SceneSwitchInfo info = new SceneSwitchInfo();
            info.m_ItorCb = itorCb;
            info.m_Data = data;
            m_SwitchQueue.Enqueue(info);

            if (!m_IsSwitch)
            {
                MainCoroutine.mainCoroutine.StartCoroutine(StartSwicth());
            }
        }

        /// <summary>
        /// 开始切换场景
        /// </summary>
        static IEnumerator StartSwicth()
        {
            m_IsSwitch = true;

            SceneSwitchInfo info = null;
            CallbackReturn<IEnumerator, object> itorCb = null;
            IEnumerator itor = null;
            while (m_SwitchQueue.Count > 0)
            {
                info = m_SwitchQueue.Dequeue();

                itorCb = info.m_ItorCb;
                if (itorCb != null)
                {
                    itor = itorCb(info.m_Data);
                    while (itor.MoveNext())
                    {
                        yield return null;
                    }
                }
            }

            m_IsSwitch = false;
        }
    }
}