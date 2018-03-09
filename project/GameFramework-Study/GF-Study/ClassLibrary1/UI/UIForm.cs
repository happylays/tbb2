
using GameFramework;
using GameFramework.UI;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed class UIForm : MonoBehaviour, IUIForm
    {
        private int m_SerialId;
        private string m_UIFormAssetName;
        private IUIGroup m_UIGroup;
        private UIFormLogic m_UIFormLogic;

        public object Handle { get { return gameObject; } }
        public UIFormLogic Logic
        {
            get { return m_UIFormLogic; }
        }

        public void OnInit(int serialId, string uiFormAssetName)
        {
            m_SerialId = m_SerialId;
            m_UIFormAssetName = name;

            m_UIFormLogic = GetComponent<UIFormLogic>();

            m_UIFormLogic.OnInit(userData);
        }

        public void OnRecycle()
        {
            m_SerialId = 0;
        }
        public void OnOpen(object userData)
        {
            m_UIFormLogic.Open(userData);
        }
        public void OnClose(object userData)
        {
            m_UIFormLogic.OnClose(userData);
        }
        public void OnUpdate()
        {
            m_UIFormLogic.OnUpdate();s
        }
    }

}