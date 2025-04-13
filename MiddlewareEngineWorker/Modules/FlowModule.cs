using Autofac;
using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Managers.Data;

namespace NotificationEngineWorker;

public class FlowModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register flow services
        builder.RegisterType<FlowFactory>()
               .As<IFlowFactory>()
               .SingleInstance();

        builder.RegisterType<DefaultFlow>().InstancePerLifetimeScope();
        builder.RegisterType<VlmCvFlow>().InstancePerLifetimeScope();
    }
}
