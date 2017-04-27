
namespace LoveDance.Client.Common
{
    /// <summary>
    /// 网络环境;
    /// </summary>
	public enum NetworkType : byte
    {
        None = 0,
        _Wifi = 1,
        _2G = 2,
        _3G = 3,
        _4G = 4,
        MAX
    }

    /// <summary>
    /// 下载是否弹框的系统设置配置;
    /// </summary>
	public enum DownLoadTipsSetting : byte
    {
        OnlyWifi = 0,
        AllNetWorkType = 1,
    }

    /// <summary>
    /// 下载任务的状态;
    /// </summary>
	public enum AssetState : byte
    {
        Waitting,		//等待下载中;
        DownLoading,	//下载中;
		LocalLoading,	//本地加载中;
        Finish,			//完成下载或加载;
        HasRelease,		//已被释放;
    }

    /// <summary>
    /// 插入规则;
    /// </summary>
	public enum DownLoadOrderType : byte
    {
        Head,           //插入队首;
        AfterRunning,   //插入"下载中"之后;
    }
}