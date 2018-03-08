using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    internal class LogHelper : Log.ILogHelper
    {
        public void Log(LogLevel level, object message)
        {
            case LogLevel.Debug:
                Debug.Log(string.Format("<color=#888888>{0}</color>", message.ToString()));
                break;
            default :
                throw;

        }
    }
}