using Autofac;
using Autofac.Extensions.DependencyInjection;
using NotificationEngineWorker.Core.Interfaces;
using NotificationEngineWorker.Managers.Callback;
using NotificationEngineWorker.Managers.RequestHandling;
using NotificationEngineWorker.Managers.Data;

namespace NotificationEngineWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) // Plug in Autofac
                .UseWindowsService() // Optional: allow running as Windows Service
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>(); // Register your background worker
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    // Autofac registrations go here
                    builder.RegisterType<CycleFactory>().As<ICycleFactory>().SingleInstance();
                    builder.RegisterType<ProducerCallbackRegistry>().SingleInstance();
                    builder.RegisterType<RequestConsumer>().InstancePerLifetimeScope();
                });
    }
}