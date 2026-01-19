using UnityEngine;

namespace PrettyLogging
{
    public static class Utility
    {
        [HideInCallstack]
        public static void PrettyLog(string scope, object message, LogType logType = LogType.Log, Object context = null)
        {
            var resultMessage = $"[{scope}]: {message}";
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(resultMessage, context);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(resultMessage, context);
                    break;
                case LogType.Error:
                    Debug.LogError(resultMessage, context);
                    break;
                default:
                    throw new System.NotImplementedException($"{logType} log is not supported");
            }
        }

        [HideInCallstack]
        public static void Log(this Object @object, object message, LogType logType = LogType.Log)
        {
            Log(@object, message, logType, @object);
        }

        [HideInCallstack]
        public static void Log(this object @object, object message, LogType logType = LogType.Log, Object context = null)
        {
            PrettyLog(@object.GetType().Name, message, logType, context);
        }
    }
}
