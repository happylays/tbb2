
//音乐相关
namespace LoveDance.Client.Common
{
    public enum SongMode : byte
    {
        None = 0,					//随机		

        Taiko = 1,				//经典
        Tradition = 2,	        //恋舞
        Osu = 3,		            //泡泡
        Audition = 4,	            //劲舞

        BasicMax = 10,            //一般模式与超级模式分界线

        SuperTaiko = 11,		//超级经典
        SuperTradition = 12,	//超级恋舞
        SuperOsu = 13,		    //超级泡泡
        SuperAudition = 14,	    //超级劲舞

        Super_Max = 20,           //超级分界线

        HeartBeat = 21,	        //心动（也成为情侣）

        Max,
    }


    public enum BeatResultRank : byte
    {
        None,

        Miss,
        Bad,
        Good,
        Cool,
        Perfect,

        Max,
    }

    /// <summary>
    ///舞蹈模式大类型
    /// </summary>
    public enum SongModeType : byte
    {
        All = 0,    //所有模式
        Base = 1,   //一般模式
        Super = 2, //超级模式
    }
}