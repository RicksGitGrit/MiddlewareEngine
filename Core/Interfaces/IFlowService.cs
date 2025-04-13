namespace NotificationEngineWorker.Core.Interfaces;

public interface IFlowService : IDisposable
{
    void Send(string message);
}

