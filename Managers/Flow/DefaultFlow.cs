using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class DefaultFlow : IFlowService
{
    public async Task SendAsync(string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending Default: {message}");
        await Task.Delay(500, cancellationToken);
        Console.WriteLine($"Finished Default: {message}");
    }

    public void Dispose() { }
}

