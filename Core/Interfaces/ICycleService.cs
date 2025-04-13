namespace NotificationEngineWorker.Core.Interfaces;

public interface ICycleService : IDisposable
{
    void Send(string message);
}

