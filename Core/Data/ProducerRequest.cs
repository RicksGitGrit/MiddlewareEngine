using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Core.Data;

/// <summary>
/// Default request that a produces may publish to the CycleManager
/// </summary>
public class ProducerRequest : IRequest
{
    /// <inheritdoc/>
    public Guid Id { get; set; }

    /// <inheritdoc/>
    public Guid ProducerId { get; set; }

    /// <inheritdoc/>
    public Guid MachineCycleId { get; set; }

    /// <inheritdoc/>
    public string? Message { get; set; }

    /// <inheritdoc/>
    public string Request { get; set; }

    /// <inheritdoc/>
    public ProducerType ProducerType { get; set; }

    /// <inheritdoc/>
    public RequestType RequestType { get; set; }

    /// <summary>
    /// Creates a producer request
    /// </summary>
    /// <param name="request"></param>
    public ProducerRequest(string request)
    {
        Request = request;
    }
}

