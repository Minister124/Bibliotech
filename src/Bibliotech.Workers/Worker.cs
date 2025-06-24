using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Your background logic here
        return Task.CompletedTask;
    }
}