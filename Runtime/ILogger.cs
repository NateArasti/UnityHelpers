using UnityEngine;

namespace PrettyLogging
{
    public interface ILogger
    {
        [HideInCallstack]
        void Log(string message, LogType logType = LogType.Log, Object context = null);
    }
}
