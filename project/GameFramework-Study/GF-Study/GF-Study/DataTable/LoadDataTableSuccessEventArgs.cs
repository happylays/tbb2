namespace GameFramework.DataTable
{
    public sealed class LoadDataTableSuccessEventArgs : GameFrameworkEventArgs
    {
        public LoadDataTableSuccessEventArgs(string dataTableAssetName, float duration, object userData)
        {
            dataTableAssetName = dataTableAssetName;
            duration = duration;
            userData = userData;
        }

        public string DataTableAssetName
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }
    }

}