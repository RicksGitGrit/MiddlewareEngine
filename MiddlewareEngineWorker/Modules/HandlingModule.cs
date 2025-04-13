using Autofac;
using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Managers;
using NotificationEngineWorker.Managers.Callback;
using NotificationEngineWorker.Managers.RequestHandling;

namespace NotificationEngineWorker;

public class HandlingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register flow services
        builder.RegisterType<MachineCycleManager>().SingleInstance();
        builder.RegisterType<RequestCallbackRegistry>().As<IRequestCallbackRegistry>().InstancePerLifetimeScope();
        builder.RegisterType<RequestConsumer>().InstancePerLifetimeScope();
    }
}
