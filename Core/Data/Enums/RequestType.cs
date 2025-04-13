namespace NotificationEngineWorker.Core.Data.Enums;

/// <summary>
/// Standardized request type that the producer is limited to
/// </summary>
public enum RequestType : byte
{
    /// <summary>
    /// Start a machine cycle
    /// </summary>
    Execute,

    /// <summary>
    /// Retrieve results from open machine cycle once finished
    /// </summary>
    Retrieve,

    /// <summary>
    /// Return execution results and close machine cycle
    /// </summary>
    Return
}

