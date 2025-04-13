using Microsoft.Extensions.Logging;

namespace MiddlewareEngineWorker.Core.Interfaces;

/// <summary>
/// Wraps the microsoft logger
/// </summary>
public interface IMiddlewareLogger : ILogger
{
}
