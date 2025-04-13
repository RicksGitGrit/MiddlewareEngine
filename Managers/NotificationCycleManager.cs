using Autofac;
using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Core.Data;

namespace NotificationEngineWorker.Managers;
public class NotificationCycleManager
{
    private readonly ILifetimeScope _rootScope;
    private readonly IProducerCallbackRegistry _callbackRegistry;

    public NotificationCycleManager(ILifetimeScope rootScope, IProducerCallbackRegistry callbackRegistry)
    {
        _rootScope = rootScope;
        _callbackRegistry = callbackRegistry;
    }

    public async Task HandleRequestAsync(IRequest request, CancellationToken cancellationToken)
    {
        var callback = _callbackRegistry.GetCallback(request);

        // Start a new scope for the request
        using var scope = _rootScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterInstance(request).As<ProducerRequest>();
        });

        try
        {
            // execute the Producer-specific logic
            await callback(request, scope, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Cycle] Error for Producer {request.Id}: {ex.Message}");
        }
    }
}

