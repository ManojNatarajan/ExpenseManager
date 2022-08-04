namespace GoldenEagles.Logger
{
    public interface ILogger
    {
        string Logger_Category_Name { get; set; }
        Microsoft.Extensions.Logging.ILogger Logger { get; set; }
        void InitializeLogger();
        void LogTrace(string message); 
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception);
        void LogCritical(string message);
        void LogCritical(Exception exception, string message);

    }
}