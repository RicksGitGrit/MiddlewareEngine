using Autofac;

namespace NotificationEngineWorker.Core.Interfaces;

public interface IRequestCallbackRegistry : IDisposable
{
    ProducerCallbackDelegate GetCallback(IRequest request);
}

public delegate Task ProducerCallbackDelegate(IRequest request, ILifetimeScope scope, CancellationToken cancellationToken);


