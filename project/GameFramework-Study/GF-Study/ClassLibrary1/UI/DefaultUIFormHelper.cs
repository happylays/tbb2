
using System;
using GameFramework;
using GameFramework.UI;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class DefaultUIFormHelper : UiFormHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        public override object InstantiateUIForm(object uiFormAsset)
        {
            return Instantiate((Object)uiFormAsset);
        }
        public override IUIForm CreateUIForm(object uiFormInstance)
        {
            GameObject gameObject = uiFormInstance as GameObject;
            if (gameObject == null)
            {
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent((MonoBehaviour)uiGroup.Helper).transform);
            transform.localScale = Vector3.one;

            return gameObject.GetOrAddComponent<UIForm>();

        }

        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            m_ResourceComponent.UnloadAsset(uiFormAsset);
            DestroyObject((Object)uiFormInstance);
        }

        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<m_ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                return;
            }
        }
    }
}