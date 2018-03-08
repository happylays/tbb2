
using System.Diagnostics;

namespace GameFramework
{
    private static partial class Log {
        private static ILogHelper s_LogHelper = null;
        public static void SetLogHelper(ILogHelper logHelper) {
            s_LogHelper = logHelper;
        }
        [Conditional("DEBUG")]
        public static void Debug(object message) {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.log(LogLevel.Debug, message);
        }
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0) {
            if (s_LogHelper == null)
            {
                return;
            }
            s_LogHelper.log(LogLevel.Debug, string.Format(format, arg0));
        }
        public static void Info(object message) { }
        public static void Info(string format, object arg0) { }
        public static void Warning(object message) { }
        public static void Error(object message) { }
        public static void Error(string format, object arg0) { }
        public static void Fatal(string foramt, object arg0) { }
        
    }

}