using Smart_Electric_Metering_System_BackEnd.Context;
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
        var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SmartElectricMeteringContext>();
        await Task.CompletedTask;
    }
}
