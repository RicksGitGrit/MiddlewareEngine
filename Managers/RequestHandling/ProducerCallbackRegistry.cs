using Autofac;
using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Callback;

/// <summary>
/// Registry for callbacks based on ProducerType + RequestType
/// Acts on the scope that is passed, which is linked to the ProducerId + MachineCycleId
/// This allows for any number of Producers of the same type, as they are separated through scope and act on thread-safe root scope members.
/// </summary>
public class ProducerCallbackRegistry : IProducerCallbackRegistry
{
    private readonly Dictionary<string, ProducerCallbackDelegate> _callbacks = new();

    /// <summary>
    /// Setup up the callback routing
    /// </summary>
    public ProducerCallbackRegistry()
    {
        _callbacks["requestA"] = RequestACallbackAsync;
        _callbacks["requestB"] = RequestBCallbackAsync;
    }

    /// <summary>
    /// Exposed method for retrieving internally assigned callbacks
    /// Callbacks may create and interact with short living members within the scope
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ProducerCallbackDelegate GetCallback(IRequest request)
    {
        // check the type of the producer and request and sample the callback
        var producerId = request.ProducerId;
        var requestType = request.Type;

        return _callbacks.TryGetValue(request.Request, out var cb)
            ? cb
            : DefaultCallbackAsync;
    }


    /// <summary>
    /// TODO RKOU finish per producers request of DEFAULT/ VLMPrompt+CV request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="scope"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task DefaultCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<IFlowFactory>();
        using var notifier = factory.Create(request.Type);
        notifier.Send(request.Message?? "");
    }

    private async Task RequestACallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<IFlowFactory>();
        using var defaultNotification = factory.Create(ProducerType.DefaultFlow);
        defaultNotification.Send($"[RequestA] {request.Message}");
    }

    private async Task RequestBCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        var factory = scope.Resolve<IFlowFactory>();
        using var prompt = factory.Create(ProducerType.VlmCvFlow);
        prompt.Send($"[RequestB] {request.Message}");
    }
}

