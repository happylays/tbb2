
namespace GameFramework.DataTable
{
    public interface IDataTableHelper
    {
        bool LoadDataTable(object dataTableAsset, object userData);
        string[] SplitToDataRows(string text);
        void ReleaseDataTableAsset(object dataTableAsset);

    }
}