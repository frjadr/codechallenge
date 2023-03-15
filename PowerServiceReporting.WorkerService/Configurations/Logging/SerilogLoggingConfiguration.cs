using Microsoft.Extensions.Configuration;
using PowerServiceReporting.ApplicationCore.Constants;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.WorkerService.Configurations.Logging
{
    public class SerilogLoggingConfiguration
    {
        public static void ConfigureSerilogLogging(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(AppContext.BaseDirectory + @configuration[ConfigurationConstants.AppName] + ".log", rollingInterval: RollingInterval.Day)
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
        }
    }
}
