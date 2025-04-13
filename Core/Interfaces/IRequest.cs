using NotificationEngineWorker.Core.Data.Enums;

namespace NotificationEngineWorker.Core.Interfaces;

public interface IRequest
{
    /// <summary>
    /// unique identifier used to discern between clients and servers
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Message, may be prompt or other instruction or left empty for Types with default instruction
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// Type of notification request
    /// </summary>
    CycleType Type { get; set; }

    /// <summary>
    /// Request message
    /// </summary>
    string Request { get; set; }
}
