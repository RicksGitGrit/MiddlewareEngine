using NotificationEngineWorker.Managers.RequestHandling;
using NotificationEngineWorker.Core.Data;
using NotificationEngineWorker.Core.Data.Enums;

namespace NotificationEngineWorker;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Simulate receiving a message
                var request = await SimulateReceiveMessageAsync(stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<RequestConsumer>();
                    //await consumer.HandleCustomerRequestAsync(request);
                }

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in worker loop");
            }
        }

        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
    }

    private Task<ProducerRequest> SimulateReceiveMessageAsync(CancellationToken cancellationToken)
    {
        // This would be replaced with actual RabbitMQ handling
        return Task.FromResult(new ProducerRequest(request: "")
        {
            Message = "This is a test message",
            Type = CycleType.Prompt
        });
    }
}
