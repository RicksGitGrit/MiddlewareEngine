namespace NotificationEngineWorker.Core.Interfaces;

public interface IFlowService : IDisposable
{
    Task SendAsync(string message, CancellationToken cancellationToken);
}

