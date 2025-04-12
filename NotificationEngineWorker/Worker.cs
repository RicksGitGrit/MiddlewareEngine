using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

    // This method contains the background service logic
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

        // Keep the service running as long as cancellation is not requested
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Simulate receiving a message (e.g., from RabbitMQ)
                var request = await SimulateReceiveMessageAsync(stoppingToken);

                if (request != null)
                {
                    _logger.LogInformation("Received customer request: {request}", request.Message);

                    // Start processing the request within a scoped lifetime
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Resolve the consumer and handle the request
                        var consumer = scope.ServiceProvider.GetRequiredService<CustomerRequestConsumer>();
                        await consumer.HandleCustomerRequestAsync(request);
                    }
                }

                // Simulate some delay between message checks (e.g., message timeout)
                await Task.Delay(1000, stoppingToken);  // Delay 1 second before checking again
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing the message.");
            }
        }

        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
    }

    // Simulate receiving a message (replace this with RabbitMQ logic)
    private Task<CustomerRequest> SimulateReceiveMessageAsync(CancellationToken stoppingToken)
    {
        return Task.FromResult(new CustomerRequest
        {
            CustomerId = "customerA",
            Message = "This is a test message",
            Type = "sms"
        });
    }
}
