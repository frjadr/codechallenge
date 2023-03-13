using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.Infrastructure.ServiceImplementations;
using PowerServiceReporting.WorkerService.Configurations;
using PowerServiceReporting.WorkerService.WorkerServices;


try
{
    // create service host
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(configuration => { })
        .ConfigureServices(services => {

            services.AddTransient<ITradesReportingService, TradesReportingService>(trs => 
                new TradesReportingService(new TradesService(), new ReportExportingService()));

            services.AddCronScheduledHostedWorkerService<TradesReportingWorkerService>(csws => 
                { 
                    csws.TimeZone = TimeZoneInfo.Local; 
                    csws.CronExpression = CronExpression.Parse("", CronFormat.IncludeSeconds); 
                });

        })
        .Build().Run();
}
catch (Exception ex)
{

}
finally
{

}



