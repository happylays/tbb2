
namespace GameFramework
{
    public static partial class Log {
        public interface ILogHelper
        {
            void log(LogLevel level, object message);
        }
    }

}