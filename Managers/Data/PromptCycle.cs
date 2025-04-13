using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class PromptCycle : ICycleService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending prompt: {message}");
    }

    public void Dispose(){}
}

