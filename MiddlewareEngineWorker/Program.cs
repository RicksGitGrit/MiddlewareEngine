using Autofac;
using Autofac.Extensions.DependencyInjection;

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
                    builder.RegisterModule<FlowModule>();
                    builder.RegisterModule<HandlingModule>();
                });
    }
}