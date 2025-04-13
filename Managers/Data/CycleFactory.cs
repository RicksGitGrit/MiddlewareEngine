using Autofac;
using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class CycleFactory : ICycleFactory
{
    private readonly IComponentContext _context;

    public CycleFactory(IComponentContext context)
    {
        _context = context;
    }

    public ICycleService Create(CycleType type)
    {
        return type switch
        {
            CycleType.Default => _context.Resolve<DefaultCycle>(),
            CycleType.Prompt => _context.Resolve<PromptCycle>(),
            _ => throw new ArgumentException($"Unsupported notification type: {type}")
        };
    }
}

