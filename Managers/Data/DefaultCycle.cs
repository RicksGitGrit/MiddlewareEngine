using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class DefaultCycle : ICycleService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending Default: {message}");
    }

    public void Dispose() { }
}

