using Cronos;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerServiceReporting.ApplicationCore.Constants;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.Infrastructure.ServiceImplementations;
using PowerServiceReporting.WorkerService.Configurations;
using PowerServiceReporting.WorkerService.WorkerServices;
using System;
using PowerServiceReporting.ApplicationCore.Mappers;

try
{
    // create service host
    var hostBuilder = Host.CreateDefaultBuilder(args);
    hostBuilder.ConfigureAppConfiguration(configuration => {
        configuration.AddEnvironmentVariables();
        configuration.AddJsonFile(ConfigurationConstants.AppSettingsJson, optional: false, reloadOnChange: false);
        configuration.AddJsonFile($"{ConfigurationConstants.AppSettingsDot}{Environment.GetEnvironmentVariable(ConfigurationConstants.EnvironmentVariable)}{ConfigurationConstants.DotJson}", optional: false, reloadOnChange: false);
    });

    hostBuilder.ConfigureServices((hostingContext, services) =>
    {
        #region config settings
        var tradesReportingWorkerServiceSettings = new TradesReportingWorkerServiceSettings();
        hostingContext.Configuration.GetSection(nameof(TradesReportingWorkerServiceSettings)).Bind(tradesReportingWorkerServiceSettings);
        #endregion

        #region Client local time
        var clientLocalTime = WorkerServiceConfiguration.LocalClientTime(tradesReportingWorkerServiceSettings.TimeZoneId);
        Console.WriteLine($"Local time: {DateTime.Now}");
        Console.WriteLine($"Client local time: {clientLocalTime}");
        #endregion

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
            new TradesReportingService(new TradesService(autoMapper, clientLocalTime), new ReportExportingService()));
        #endregion

        #region schueduled Worker Service registration
        services.AddCronScheduledHostedWorkerService<TradesReportingWorkerService>(csws =>
        {
            //csws.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(tradesReportingWorkerServiceSettings.TimeZoneId);
            csws.TimeZoneInfo = TimeZoneInfo.Local;
            csws.CronExpression = CronExpression.Parse(tradesReportingWorkerServiceSettings.CronExpression, CronFormat.IncludeSeconds);
        });
        #endregion
    });

    hostBuilder.Build().Run();
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message} + {ex.StackTrace}");
}
finally
{

}



