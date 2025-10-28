using System.Collections.Generic;

namespace GameCore
{
    public static class Log
    {
        public static readonly string UnityDefault = nameof(UnityDefault);

        private static Dictionary<string, ILogHandler> _logHandlers;

        public static void Initialize()
        {
            _logHandlers = new Dictionary<string, ILogHandler>();
            _logHandlers[UnityDefault] = new UnityDefaultLogHandler();
        }

        public static void Debug(string message)
        {
            LogMessage(message, LogLevel.Debug);
        }

        public static void Warning(string message)
        {
            LogMessage(message, LogLevel.Warning);
        }

        public static void Error(string message)
        {
            LogMessage(message, LogLevel.Error);
        }

        private static void LogMessage(string message, LogLevel logLevel)
        {
            foreach(var handler in _logHandlers.Values)
            {
                handler.Log(message, logLevel);
            }
        }
    }
}