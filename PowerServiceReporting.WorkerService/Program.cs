using Cronos;
using Serilog;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerServiceReporting.ApplicationCore.Constants;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.Infrastructure.ServiceImplementations;
using PowerServiceReporting.WorkerService.Configurations;
using PowerServiceReporting.WorkerService.WorkerServices;
using PowerServiceReporting.ApplicationCore.Mappers;
using System.Reflection;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.WorkerService.Configurations.Logging;

try
{
    // create service host
    var hostBuilder = Host.CreateDefaultBuilder(args);
    hostBuilder.ConfigureAppConfiguration(configuration =>
    {
        configuration.AddEnvironmentVariables();
        configuration.AddJsonFile(ConfigurationConstants.AppSettingsJson, optional: false, reloadOnChange: false);
        configuration.AddJsonFile($"{ConfigurationConstants.AppSettingsDot}{Environment.GetEnvironmentVariable(ConfigurationConstants.EnvironmentVariable)}{ConfigurationConstants.DotJson}", optional: false, reloadOnChange: false);
    });

    hostBuilder.ConfigureServices((hostingContext, services) =>
    {
        // Serilog configuration
        SerilogLoggingConfiguration.ConfigureSerilogLogging(hostingContext.Configuration);

        #region config settings
        var tradesReportingWorkerServiceSettings = new TradesReportingWorkerServiceSettings();
        hostingContext.Configuration.GetSection(nameof(TradesReportingWorkerServiceSettings)).Bind(tradesReportingWorkerServiceSettings);
        #endregion

        // Client local time
        var clientLocalTime = WorkerServiceConfiguration.LocalClientTime(tradesReportingWorkerServiceSettings.TimeZoneId);

        #region AutoMapper
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        IMapper autoMapper = mappingConfig.CreateMapper();
        services.AddSingleton(autoMapper);
        #endregion

        #region services DI
        services.AddTransient<ITradesReportingService, TradesReportingService>(trs =>
            new TradesReportingService(new TradesService(autoMapper, clientLocalTime), new ReportExportingService(tradesReportingWorkerServiceSettings.ExportFilePath, tradesReportingWorkerServiceSettings.ExportFileNamePrefix, clientLocalTime), clientLocalTime));
        #endregion
       
        // Time Zone Info for scheduling depends on environment
        var environment = Environment.GetEnvironmentVariable(ConfigurationConstants.EnvironmentVariable);
        var timeZoneInfo = environment == "prod" || environment == "release" ? TimeZoneInfo.FindSystemTimeZoneById(tradesReportingWorkerServiceSettings.TimeZoneId) : TimeZoneInfo.Local;

        #region schueduled Worker Service registration
        services.AddCronScheduledHostedWorkerService<TradesReportingWorkerService>(csws =>
        {
            csws.TimeZoneInfo = timeZoneInfo;
            csws.CronExpression = CronExpression.Parse(tradesReportingWorkerServiceSettings.CronExpression, CronFormat.IncludeSeconds);
            csws.ClientLocalTime = clientLocalTime;
        });
        #endregion
    });
    hostBuilder.UseWindowsService();
    hostBuilder.Build().Run();
}
catch (Exception ex)
{
    Log.Fatal($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(Program).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
        $" - with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");

}
finally
{
    Log.CloseAndFlush();
}



