using Microsoft.Extensions.Configuration;
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
    
    public class Serilog_Logger<T> : Logger_Base where T : class
    {
        public Serilog_Logger(string categoryName) : base(categoryName)
        {
        }
        public override void InitializeLogger()
        {
            var providers = new LoggerProviderCollection();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            ILoggerFactory factory = new SerilogLoggerFactory(Log.Logger, true, providers);
            Logger = factory.CreateLogger<T>();
        }
    }

    
}
