using NotificationEngineWorker.Core.Data.Enums;

namespace NotificationEngineWorker.Core.Interfaces;

public interface IFlowFactory
{
    IFlowService Create(ProducerType type);
}

