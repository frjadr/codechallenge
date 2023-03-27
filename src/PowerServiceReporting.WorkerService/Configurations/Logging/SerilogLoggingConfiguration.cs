using Microsoft.Extensions.Configuration;
using PowerServiceReporting.ApplicationCore.Constants;
using Serilog;

namespace PowerServiceReporting.WorkerService.Configurations.Logging
{
    /// <summary>
    /// Serilog configurator.
    /// </summary>
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
