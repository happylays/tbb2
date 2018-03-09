
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.UI
{
    internal sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private readonly Dictionary<string, UIGroup> m_UIGroup;
        private readonly List<int> m_UIFormsBeingLoaded;
        private readonly List<string> m_UIFormAssetNamesBeingLoaded;
        private readonly HashSet<int> m_UIFormToReleaseOnLoad;
        private readonly LinkedList<IUIForm> m_RecycleQueue;
        private readonly LoadAssetCallback m_LoadAssetCallback;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IObjectPool<UIFormInstanceObject> m_InstancePool;
        private int m_Serial;
        private EventHandler<OpenUIFormSuccessEventArgs> m_OpenUIFormSuccessEventHandler;

        public UIManager()
        {
            m_UIGroup = new Dictionary<string, UIGroup>();
            m_UIFormsBeingLoaded = new List<int>();
            m_UIFormsToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new LinkedList<IUIForm>();
            m_LoadAssetCallback = new LoadAssetCallback(LoadUIFormSuccessCallback,..);
            m_ObjectPoolManager = null;
            m_ResourceManager = null;
            m_InstancePool = null;
            m_UIFormHelper = null;
            m_Serial = 0;
            m_OpenUIFormSuccessEventHandler = null;
        }

        public float InstanceAutoReleaseInterval
        {
            get
            {
                return m_InstancePool.AutoReleaseInterval;
            }
            set
            {
                m_InstancePool.AutoReleaseInterval = value;
            }
        }
        public int InstanceCapacity
        {
            get
            {
                return m_InstancePool.Capacity;
            }
            set
            {
                m_InstancePool.Capacity = value;
            }
        }

        public float InstanceExpireTime
        {
            get
            {
                return m_InstancePool.ExpireTime;
            }
            set
            {
                m_InstancePool.ExpireTime = value;
            }
        }

        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add
            {
                m_OpenUIFormSuccessEventHandler += value;
            }
            remove
            {
                m_OpenUIFormSuccessEventHandler -= value;
            }
        }
        internal override void Update()
        {
            while (m_RecycleQueue.Count > 0)
            {
                IUIForm uiForm = m_RecycleQueue.First.Value;
                m_RecycleQueue.RemoveFirst();
                uiForm.OnRecycle();
                m_InstancePool.Unspawn(uiForm.Handle);
            }

            foreach (KeyValuePair<string,UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update();
            }
        }

        internal override void Shutdown()
        {
            CloseAllLoadedUIForms();
            m_UIGroups.Clear();
            m_UIFormsBeingLoaded.Clear();
            m_UIFormToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw;
            }
            m_ObjectPoolManager = objectPoolManager;
            m_InstancePool = m_ObjectPoolManager.CreateSingleSpawnObjectPool<UIFormInstanceObject>("UI Instance Pool");
        }

        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {

        }

        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw;
            }
            UIGroup uiGroup = null;
            if (m_UIGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }
            return null;
        }

        public IUIForm GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(ui))
            {
                throw;
            }
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }
        }
        public bool IsLoadingUIForm(int serialId)
        {
            return m_UIFormsBeingLoaded.Contains(serialId);
        }
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            return m_UIFormAssetNamesBeingLoaded.Contains(uiFormAssetName);
        }
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            UIGroup uig = (UIGroup)GetUIGroup(name);

            int serialId = m_Serial++;
            UIFormInstanceObject uiFormInstanceObject = m_InstancePool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                m_UIFormsBeingLoaded.Add(serialId);
                m_UIFormAssetNamesBeingLoaded.Add(uiFormAssetName);
                m_ResourceManager.LoadAsset(uiFormAssetName, m_LoadAssetCallback, new OpenUIFormInfo(serialId, uiGroupName, userData));
            }
            else
            {
                InternalOpenUIForm(serialId, uiFormAssetName, uiGroupName, uiFormInstanceObject.Target);
            }

            return serialId;
        }

        private void InternalOpenUIForm(int serialId, string uiFormAssetName, object uiFormInstance, bool isNewInstance, float duration)
        {
            try
            {
                IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, GetUIGroup, userData);
                if (uiForm == null)
                {

                }

                uiForm.OnInit(serialId, uiFormAssetName, GetUIGroup, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData);
                GetUIGroup.Refresh();

                if (m_OpenUIFormSuccessEventHandler != null)
                {
                    m_OpenUIFormSuccessEventHandler(this, new OpenUIFormSuccessEventArgs(uiForm, duration, userData));
                }
            }
            catch (Exception exception)
            {
                if (m_OpenUIFormFailureEventHandler != null)
                {
                    m_OpenUIFormSuccessEventHandler(this, new OpenUIFormSuccessEventArgs(serialId, uiUIFormAssetName);
                    return;
                }
                throw;
            }
        }

        public void CloseUIForm(int serialId, object userData)
        {
            if (IsLoadingUIForm(serialId))
            {
                m_UIFormToReleaseOnLoad.Add(serialId);
                return;
            }

            IUIForm uiForm = GetUIForm(serialId);
            if (uiForm == null)
            {

            }
            CloseUIForm(uiForm, userData);
        }

        public void CloseUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw;
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;

            uiGroup.RemoveUIForm(uiForm);
            uiGroup.OnClose(userData);
            uiGroup.Refresh();

            m_CloseUIFormCompleteEventHandler(this, new());

            m_RecycleQueue.AddLast(uiForm);
        }

        private void LoadUIFormSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
        {
            OpenUIForm openUIFormInfo = (OpenUIFormInfo)userData;

            m_UIFormBeingLoaded.Remove(openUIFormInfo.SerialId);
            m_UIFormAssetNamesBeingLoaded.Remove(uiFormAssetName);
            if (m_UIFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                Log.Debug();
                m_UIFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                m_UIFormHelper.ReleaseUIForm(uiFormAsset, null);
                return;
            }

            UIFormInstanceObject uiFormInstanceObject = new UIFormInstanceObject(uiFormAssetName, uiFormAsset, m_UIFormHelper.InstantiateUIForm(uiFormAsset), m_UiFormHelper);
            m_InstancePool.Register(uiFormInstanceObject, true);

            InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, uiFormInstanceObject.Target);
        }
    }
}