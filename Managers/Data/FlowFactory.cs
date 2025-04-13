using Autofac;
using NotificationEngineWorker.Core.Data.Enums;
using NotificationEngineWorker.Core.Interfaces;

namespace NotificationEngineWorker.Managers.Data;

public class FlowFactory : IFlowFactory
{
    private readonly IComponentContext _context;

    public FlowFactory(IComponentContext context)
    {
        _context = context;
    }

    public IFlowService Create(ProducerType type)
    {
        return type switch
        {
            ProducerType.DefaultFlow => _context.Resolve<DefaultFlow>(),
            ProducerType.VlmCvFlow => _context.Resolve<VlmCvFlow>(),
            _ => throw new ArgumentException($"Unsupported notification type: {type}")
        };
    }
}

