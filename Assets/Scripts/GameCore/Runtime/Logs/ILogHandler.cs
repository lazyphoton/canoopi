namespace GameCore
{
    public enum LogLevel
    {
        Debug,
        Warning,
        Error
    }

    public interface ILogHandler
    {
        public void Log(string message, LogLevel logLevel);
    }
}