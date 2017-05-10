
//音乐相关
namespace LoveDance.Client.Common
{
    public enum SongMode : byte
    {
        None = 0,					//随机		

        Taiko = 1,				//经典
        Tradition = 2,	        //恋舞
        Osu = 3,		            //泡泡

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
}