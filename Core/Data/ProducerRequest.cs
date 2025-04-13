using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Core.Data;

public class ProducerRequest : IRequest
{
    /// <inheritdoc/>
    public Guid Id { get; set; }

    /// <inheritdoc/>
    public string? Message { get; set; }

    /// <inheritdoc/>
    public CycleType Type { get; set; }

    /// <inheritdoc/>
    public string Request { get; set; }

    /// <summary>
    /// Required constructor
    /// </summary>
    /// <param name="request"></param>
    public ProducerRequest(string request)
    {
        Request = request;
    }
}

