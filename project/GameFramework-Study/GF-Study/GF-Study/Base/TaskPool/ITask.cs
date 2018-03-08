
namespace GameFramework
{
    internal interface ITask
    {
        int SerialId
        {
            get;
        }

        bool Done
        {
            get;
        }
    }

}