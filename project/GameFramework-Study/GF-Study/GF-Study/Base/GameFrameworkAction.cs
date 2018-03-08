
namespace GameFramework
{
    public delegate void GameFrameworkAction();
    public delegate void GameFrameworkAction<in T>(T obj);
    public delegate void GameFrameworkAction<in T1, in T2>(T1 arg1, T2 arg2);
}