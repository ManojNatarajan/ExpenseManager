using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenEagles.Logger
{
    //Only uses Library "Serilog.AspNetCore" & dependencies. This class does NOT use "Serilog.Extensions.Logging"
    public class Serilog_Logger_Alt : ILogger
    {
        public string Logger_Category_Name { get; set; }//Not used in this class
        public Microsoft.Extensions.Logging.ILogger Logger { get; set; }//Not used in this class 

        public Serilog_Logger_Alt()
        {
            InitializeLogger();
        }

        public void InitializeLogger()
        {
            //Read Configuration from appSettings & Initialize Logger    
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            //Alternatively use below code to initialize by passing in values directly
            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Debug()
            //.WriteTo.Console()
            //.WriteTo.Providers(providers)
            //.CreateLogger();
        }

        public void LogCritical(string message)
        {
            Log.Fatal(message);
        }

        public void LogCritical(Exception exception, string message)
        {
            Log.Fatal(exception, message);
        }

        public void LogDebug(string message)
        {
            Log.Debug(message);
        }

        public void LogError(string message)
        {
            Log.Error(message);
        }

        public void LogError(Exception exception)
        {
            Log.Error(exception, exception?.Message);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        public void LogTrace(string message)
        {
            Log.Verbose(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }
    }
}
