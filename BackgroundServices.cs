using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
namespace Smart_Electric_Metering_System_BackEnd;
public class BackgroundServices : BackgroundService
{
    IServiceScopeFactory _serviceScopeFactory;
    public BackgroundServices(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested){
            var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SmartElectricMeteringContext>();
            await Task.CompletedTask;
            var _dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
            await _dataService.CheckConnection();
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }
}
