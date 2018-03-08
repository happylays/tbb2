
namespace GameFramework
{
    public delegate TResult GameFrameworkFunc<out TResult>();
    public delegate TResult GameFrameworkFunc<out T, out TResult>(T arg);
}