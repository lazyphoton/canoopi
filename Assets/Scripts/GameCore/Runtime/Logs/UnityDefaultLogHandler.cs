using UnityEngine;

namespace GameCore
{
    public class UnityDefaultLogHandler : ILogHandler
    {
        public void Log(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Debug:
                default:
                    Debug.Log(message);
                    break;
            }
        }
    }
}