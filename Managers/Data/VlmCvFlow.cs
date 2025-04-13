using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class VlmCvFlow : IFlowService
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending prompt: {message}");
    }

    public void Dispose(){}
}

