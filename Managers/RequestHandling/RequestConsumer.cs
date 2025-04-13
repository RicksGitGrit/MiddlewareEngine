using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Core.Data;
using NotificationEngineWorker.Core.Data.Enums;
using System.Runtime.CompilerServices;

namespace NotificationEngineWorker.Managers.RequestHandling;

public class RequestConsumer
{
    private readonly NotificationCycleManager _cycleManager;

    public RequestConsumer(NotificationCycleManager cycleManager)
    {
        _cycleManager = cycleManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await foreach (var request in ReceiveRequestsAsync(cancellationToken))
        {
            // Process the request
            _ = Task.Run(() => _cycleManager.HandleRequestAsync(request, cancellationToken));
        }
    }

    // Simulate receiving customer requests asynchronously (from RabbitMQ)
    private async IAsyncEnumerable<IRequest> ReceiveRequestsAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            yield return await ReceiveAsync(); // Simulating async receive from RabbitMQ
        }
    }

    private Task<ProducerRequest> ReceiveAsync()
    {
        // Simulate receiving a request from RabbitMQ
        return Task.FromResult(new ProducerRequest(request: "RequestA")
        {
            // This would be dynamic in a real-world case
            Message = "Test message",
            Type = CycleType.Default
        }) ;
    }
}
