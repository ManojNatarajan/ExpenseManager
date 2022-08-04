using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenEagles.Logger
{
    public abstract class Logger_Base : ILogger
    {        
        public Logger_Base(string categoryName)
        {
            Logger_Category_Name = categoryName;
            InitializeLogger();
        }

        public string Logger_Category_Name { get; set; }
        public Microsoft.Extensions.Logging.ILogger Logger { get; set; }
                
        public virtual void InitializeLogger()
        {
            //Default is Serilog implementation. 
            //This implementation uses "Serilog.Extensions.Logging" library - SerilogLoggerFactory class

            var providers = new LoggerProviderCollection();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            ILoggerFactory factory = new SerilogLoggerFactory(Log.Logger, true, providers);
            Logger = factory.CreateLogger(Logger_Category_Name);
        }

        public virtual void LogDebug(string message)
        {
            Logger.LogDebug(message);
        }

        public virtual void LogError(string message)
        {
            Logger.LogError(message);
        }

        public virtual void LogError(Exception exception)
        {
            Logger.LogError(exception, exception?.Message);
        }

        public virtual void LogInfo(string message)
        {
            Logger.LogInformation(message);
        }

        public virtual void LogTrace(string message)
        {
            Logger.LogTrace(message);
        }

        public virtual void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        public virtual void LogCritical(string message)
        {
            Logger.LogCritical(message);
        }

        public virtual void LogCritical(Exception exception, string message)
        {
            Logger.LogCritical(exception, message);
        }
    }
}
