using Autofac;
using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Core.Data;
using MiddlewareEngineWorker.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace NotificationEngineWorker.Managers;
public class MachineCycleManager
{
    /// <summary>
    /// Root scope, containing long living members
    /// </summary>
    private readonly ILifetimeScope _rootScope;

    /// <summary>
    /// Registry of callbacks, assigned callback depends on the producer of the request and the type of request 
    /// </summary>
    private readonly IRequestCallbackRegistry _callbackRegistry;

    /// <summary>
    /// Thread-safe dict of open cycle scopes, retrievable per producer given the machine cycle
    /// </summary>
    private readonly ConcurrentDictionary<(Guid MachineCycleId, Guid ProducerId, DateTime TimeStamp), ILifetimeScope> _cyclicalScopes;

    /// <summary>
    /// Prevent memory overflow when cyclical scopes are not disposed
    /// </summary>
    private readonly Timer _cleanupTimer;
    private readonly IMiddlewareLogger _logger;

    private const int CLEANUP_INTERVAL = 5_000; // [ms]
    private const int CLEANUP_TIMEOUT = 60_000; // [ms]

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="rootScope"></param>
    /// <param name="callbackRegistry"></param>
    public MachineCycleManager(ILifetimeScope rootScope, IRequestCallbackRegistry callbackRegistry)
    {
        _rootScope = rootScope;
        _callbackRegistry = callbackRegistry;
        _logger = rootScope.Resolve<IMiddlewareLogger>();
        _cyclicalScopes = new();
        _cleanupTimer = new Timer(new TimerCallback(_ => CleanupCyclicalScopes()),default, CLEANUP_INTERVAL, CLEANUP_INTERVAL);
    }

    /// <summary>
    /// Prevent memory overflow when cyclical scopes are not disposed
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private void CleanupCyclicalScopes()
    {
        var currentTime = DateTime.Now;
        var timedOut = _cyclicalScopes.Where(scope => (scope.Key.TimeStamp - currentTime).TotalMilliseconds >= CLEANUP_TIMEOUT);
        foreach (var scope in timedOut) 
        {
            _cyclicalScopes.TryRemove(scope.Key, out _);  
        }
    }

    /// <summary>
    /// Called on a new request, processes request, assigns callback and manages callback's lifetime
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleRequestAsync(IRequest request, CancellationToken cancellationToken)
    {
        var callback = _callbackRegistry.GetCallback(request);
        ILifetimeScope scope;

        // Start a new scope for the request if not open
        if (_cyclicalScopes.Any(entry => entry.Key.MachineCycleId == request.MachineCycleId && entry.Key.ProducerId == request.ProducerId))
        {
            scope = _rootScope.BeginLifetimeScope(builder =>
            {
                // Overwrite the default request
                builder.RegisterInstance(request).As<ProducerRequest>();
            });
            _cyclicalScopes.TryAdd((request.MachineCycleId, request.ProducerId, DateTime.Now), scope);
        }
        else
            scope = _cyclicalScopes.First(entry => entry.Key.MachineCycleId == request.MachineCycleId && entry.Key.ProducerId == request.ProducerId).Value;

        // Execute the producer-specific logic
        try
        {
            await callback(request, scope, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"[Cycle] Error for Producer {request.Id}: {ex.Message}");
        }
    }
}

