using NotificationEngineWorker.Core.Data.Enums;

namespace NotificationEngineWorker.Core.Interfaces;

public interface ICycleFactory
{
    ICycleService Create(CycleType type);
}

