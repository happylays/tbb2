using GameFramework;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    public static class GameEntry
    {
        private const string UnityGameFrameworkVersion = "3.1.0";
        private static readonly LinkedList<GameFrameworkComponent> s_GameFrameworkComponents = new LinkedList<GameFrameworkComponent>();

        internal const int GameFrameSceneId = 0;

        public static string Version
        {
            get
            {
                return UnityGameFrameworkVersion;
            }
        }

        public static T GetComponent<T>() where T : GameFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }
        public static void Shutdown(ShutdownType shutdownType)
        {
            BaseComponent baseComponent = GetComponent<BaseComponent>();
            if (baseComponent != null)
            {
                baseComponent.Shutdown();
                baseComponent = null;

            }

            s_GameFrameworkComponents.Clear();
            if (shutdownType == shutdownType.Quit)
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif 
                return;
            }
        }

        internal static void RegisterComponent(GameFrameworkComponent gameFrameworkComponent)
        {
            Type type = gameFrameworkComponent.GetType();

            LinkedListNode<GameFrameworkComponent> current = s_GameFrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error();
                    return;
                }
                current = current.Next;
            }
            s_GameFrameworkComponents.AddLast(gameFrameworkComponent);
        }
    }
}