using Autofac;
using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Callback;

public class ProducerCallbackRegistry : IProducerCallbackRegistry
{
    private readonly Dictionary<string, ProducerCallbackDelegate> _callbacks = new();

    public ProducerCallbackRegistry()
    {
        _callbacks["requestA"] = RequestACallbackAsync;
        _callbacks["requestB"] = RequestBCallbackAsync;
    }

    public ProducerCallbackDelegate GetCallback(IRequest request)
    {
        return _callbacks.TryGetValue(request.Request, out var cb)
            ? cb
            : DefaultCallbackAsync;
    }

    private async Task DefaultCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<ICycleFactory>();
        using var notifier = factory.Create(request.Type);
        notifier.Send(request.Message?? "");
    }

    private async Task RequestACallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<ICycleFactory>();
        using var defaultNotification = factory.Create(CycleType.Default);
        defaultNotification.Send($"[RequestA] {request.Message}");
    }

    private async Task RequestBCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<ICycleFactory>();
        using var prompt = factory.Create(CycleType.Prompt);
        prompt.Send($"[RequestB] {request.Message}");
    }
}

