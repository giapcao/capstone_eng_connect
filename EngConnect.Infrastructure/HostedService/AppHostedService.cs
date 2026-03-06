using EngConnect.Application.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace EngConnect.Infrastructure.HostedService;

public class AppHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IScheduler _scheduler;

    public AppHostedService(IServiceScopeFactory scopeFactory, IScheduler scheduler)
    {
        _scopeFactory = scopeFactory;
        _scheduler = scheduler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _scopeFactory.CreateScope();

        var outboxEventScheduler = scope.ServiceProvider.GetRequiredService<IOutboxEventScheduler>();

        // Initialize tasks that need to run on startup here
        await outboxEventScheduler.ScheduleOutboxEventAsync(cancellationToken);
        
        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        // Stop the scheduler
        await _scheduler.PauseAll(cancellationToken);
        await _scheduler.Shutdown(cancellationToken);
    }
}