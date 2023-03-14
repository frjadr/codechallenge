using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerServiceReporting.ApplicationCore.Constants;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.Infrastructure.ServiceImplementations;
using PowerServiceReporting.WorkerService.Configurations;
using PowerServiceReporting.WorkerService.WorkerServices;
using System;

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
        var tradesReportingWorkerServiceSettings = new TradesReportingWorkerServiceSettings();
        hostingContext.Configuration.GetSection(nameof(TradesReportingWorkerServiceSettings)).Bind(tradesReportingWorkerServiceSettings);

        var clientLocalTime = WorkerServiceConfiguration.LocalClientTime(tradesReportingWorkerServiceSettings.TimeZoneId);
        Console.WriteLine($"Local time: {DateTime.Now}");
        Console.WriteLine($"Client local time: {clientLocalTime}");

        services.AddTransient<ITradesReportingService, TradesReportingService>(trs =>
            new TradesReportingService(new TradesService(), new ReportExportingService()));

        services.AddCronScheduledHostedWorkerService<TradesReportingWorkerService>(csws =>
        {
            //csws.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(tradesReportingWorkerServiceSettings.TimeZoneId);
            csws.TimeZoneInfo = TimeZoneInfo.Local;
            csws.CronExpression = CronExpression.Parse(tradesReportingWorkerServiceSettings.CronExpression, CronFormat.IncludeSeconds);
        });

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



