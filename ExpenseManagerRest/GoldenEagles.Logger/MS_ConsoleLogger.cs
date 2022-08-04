using MS = Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace GoldenEagles.Logger
{  
    public class MS_ConsoleLogger : Logger_Base 
    {
        public MS_ConsoleLogger(string categoryName) : base(categoryName)
        {
        }

        public override void InitializeLogger()
        {
            var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>(Logger_Category_Name, null);
            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions }, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory, Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());

            ConsoleLoggerProvider provider = new ConsoleLoggerProvider(optionsMonitor);
            LoggerFilterOptions filterOptions = new LoggerFilterOptions { MinLevel = LogLevel.Information };

            MS.ILoggerFactory loggerFactory = new MS.LoggerFactory(new[] { provider }, filterOptions);
            Logger = loggerFactory.CreateLogger(Logger_Category_Name);
        }
    }
}
