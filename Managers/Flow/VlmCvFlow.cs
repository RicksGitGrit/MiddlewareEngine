using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class VlmCvFlow : IFlowService
{
    /// <summary>
    /// Prompt the vlm, which is chained to CV processing
    /// </summary>
    /// <param name="message"></param>
    public async Task SendAsync(string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending prompt: {message}");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine($"Receiving prompt: {message}");
        await Task.Delay(10, cancellationToken);
        Console.WriteLine($"Starting CV post-processing for prompt: {message}");
        await Task.Delay(100, cancellationToken);
        Console.WriteLine($"Finished CV post-processing for prompt: {message}");
    }

    public void Dispose(){}
}

