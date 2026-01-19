using System;

namespace PrettyLogging
{
    public class SimpleLogger<T> : SimpleLogger
    {
        public SimpleLogger() : base(typeof(T)) { }
    }

    public class SimpleLogger : ILogger
    {
        public string Scope { get; }

        public SimpleLogger(Type type)
            : this (type.Name)
        {
        }

        public SimpleLogger(string section)
        {
            Scope = section;
        }

        public void Log(string message, UnityEngine.LogType logType = UnityEngine.LogType.Log, UnityEngine.Object context = null)
        {
            Utility.PrettyLog(Scope, message, logType, context);
        }
    }
}
