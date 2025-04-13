using Autofac;
using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Callback;

/// <summary>
/// Registry for callbacks based on ProducerType + RequestType
/// Acts on the scope that is passed, which is linked to the request's ProducerId + MachineCycleId
/// Producers work concurrently, as they are separated through scope and act on thread-safe root scope members.
/// </summary>
public class RequestCallbackRegistry : IRequestCallbackRegistry
{
    private readonly Dictionary<(ProducerType ProducerType, RequestType RequestType), ProducerCallbackDelegate> _callbacks = new();

    /// <summary>
    /// Setup up the callback routing
    /// </summary>
    public RequestCallbackRegistry()
    {
        // Producer default
        _callbacks[(ProducerType.DefaultFlow,RequestType.Retrieve)] = DefaultRetrieveCallbackAsync;
        _callbacks[(ProducerType.DefaultFlow, RequestType.Execute)] = DefaultExecuteCallbackAsync;
        _callbacks[(ProducerType.DefaultFlow, RequestType.Return)] = DefaultReturnCallbackAsync;

        // producer VLM + CV
        _callbacks[(ProducerType.VlmCvFlow, RequestType.Retrieve)] = VlmCvRetrieveCallbackAsync;
        _callbacks[(ProducerType.VlmCvFlow, RequestType.Execute)] = VlmCvExecuteCallbackAsync;
        _callbacks[(ProducerType.VlmCvFlow, RequestType.Return)] = VlmCvReturnCallbackAsync;
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

        return _callbacks.TryGetValue((request.ProducerType, request.RequestType), out var cb)
            ? cb
            : UnsupportedCallbackAsync;
    }

    /// <summary>
    /// Case not supported request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="scope"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task UnsupportedCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #region Default callbacks
    private async Task DefaultRetrieveCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        // prior to execultion the flow has to be resolved as this takes time
        var factory = scope.Resolve<IFlowFactory>();
        var flow = factory.Create(ProducerType.DefaultFlow);
    }

    private async Task DefaultExecuteCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken) 
    {
        // flow has been loaded, now execute
        var factory = scope.Resolve<IFlowFactory>();
        var flow = factory.Create(ProducerType.DefaultFlow);
        await flow.SendAsync(request.Message ?? "", cancellationToken);
    }
    
    private async Task DefaultReturnCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken) { throw new NotImplementedException(); }
    #endregion

    #region VLM + CV callbacks
    private async Task VlmCvRetrieveCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {
        // prior to execultion the flow has to be resolved as this takes time
        var factory = scope.Resolve<IFlowFactory>();
        var flow = factory.Create(ProducerType.VlmCvFlow);
    }

    private async Task VlmCvExecuteCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken)
    {         
        // flow has been loaded, now execute
        var factory = scope.Resolve<IFlowFactory>();
        var flow = factory.Create(ProducerType.VlmCvFlow);
        await flow.SendAsync(request.Message ?? "", cancellationToken);
    }
   
    private async Task VlmCvReturnCallbackAsync(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken) { throw new NotImplementedException(); }
    #endregion

    public void Dispose() { }
}

