
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    internal sealed partial class DataTableManager : GameFrameworkModule, IDataTableManager
    {
        private readonly Dictionary<string, DataTableBase> m_DataTables;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private IDataTableManager m_DataTableHelper;
        private EventHandler<LoadDataTableSuccessEventArgs> m_LoadDataTableSuccessEventHandler;

        public DataTableManager()
        {
            m_DataTables = new Dictionary<string, DataTableBase>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadDataTableSuccessCallback,);
            m_ResourceManager = null;
            m_DataTableHelper = null;
            m_LoadDataTableSuccessEventHandler = null;
        }

        public event EventHandler<LoadDataTableSuccessEventArgs> LoadDataTableSuccess
        {
            add
            {
                m_LoadDataTableSuccessEventHandler += value;
            }
            remove
            {
                m_LoadDataTableSuccessEventHandler -= value;
            }
        }

        internal override void Update()
        {
            
        }
        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, DataTableBase> dataTable in m_DataTables)
            {
                dataTable.Value.Shutdown();
            }
            m_DataTables.Clear();
        }

        public void LoadDataTable(string dataTableAssetName, object userData)
        {
            m_ResourceManager.LoadAsset(dataTableAssetName, m_LoadAssetCallbacks, userData);
        }

        public bool DestroyDataTable<T>(string name) where T : IDataRow
        {
            return InternalDestroyDataTable(Utility.Text.GetFullName<T>(name));
        }
        private bool InternalHasDataTable(string fullName)
        {
            return m_DataTables.ContainsKey(fullName);
        }
        private DataTableBase InternalGetDataTable(string fullName)
        {
            DataTableBase dataTable = null;
            if (m_DataTables.TryGetValue(fullName, out dataTable))
            {
                return dataTable;
            }
            return null;
        }
        private bool InternalDestroyDataTable(string fullName)
        {
            DataTableBase dataTable = null;
            if (m_DataTables.TryGetValue(fullName, out dataTable))
            {
                dataTable.Shutdown();
                return m_DataTables.Remove();
            }
            return false;
        }

        public IDataTable<T> CreateDataTable<T>(string name, string text) where T : class, IDataRow, new()
        {
            if (HasDataTable<T>(name))
            {

            }

            DataTable<T> dataTable = new DataTable<T>(name);
            string[] dataRowTexts = m_DataTableHelper.SplitToDataRows(text);
            foreach (string dataRowText in dataRowTexts)
            {
                dataTable.AddDataRow(dataRowText);
            }

            m_DataTables.Add(Utility.Text.GetFullName<T>(name), dataTable);
            return dataTable;
        }

        private void LoadDataTableSuccessCallback(string dataTableAssetName, object dataTableAsset, float duration, object userData)
        {
            try
            {
                if (!m_DataTableHelper.LoadDataTable(dataTableAsset, userData))
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                if (m_LoadDataTableFailureEventHandler != null)
                {
                    m_LoadDataTableFailureEventHandler(this, new LoadDataTableFailureEventArgs());
                    return;
                }
                throw;
            }
            finally
            {
                m_DataTableHelper.ReleaseDataTableAsset(dataTableAsset);
            }

            if (m_LoadDataTableSuccessEventHandler != null)
            {
                m_LoadDataTableSuccessEventHandler(this, new LoadDataTableSuccessEventArgs());
            }

        }
    }
}