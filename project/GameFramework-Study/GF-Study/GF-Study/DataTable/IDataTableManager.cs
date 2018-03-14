

using GameFramework.Resource;
using System;

namespace GameFramework.DataTable
{
    public interface IDataTableManager
    {
        int Count
        {
            get;
        }

        event EventHandler<LoadDataTableSuccessEventArgs> LoadDataTableSuccess;
        void SetResourceManager(IResourceManager resourceManager);
        void SetDataTableHelper(IDataTableHelper dataTableHelper);
        void LoadDataTable(string dataTableAssetName);
        bool HasDataTable<T>() where T : IDataRow;
        IDataTable<T> GetDataTable<T>() where T : IDataRow;
        IDataTable<T> CreateDataTable<T>(string text) where T : class, IDataRow, new();
        bool DestroyDataTable<T>() where T : IDataRow;
    }
}