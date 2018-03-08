
namespace GameFramework
{
    internal interface ITaskAgent<T> where T : ITask 
    {
        T Task
        {
            get;
        }

        void Initialize();
        void Update();
        void Shutdown();
        void Start(T task);
        void Reset();

    }

}