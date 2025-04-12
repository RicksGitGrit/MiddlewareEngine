using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NotificationEngineWorker;

public class Program
{
    public static void Main(string[] args)
    {
        // Build and run the host
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Register services like consumer, notification factory, etc.
                services.AddHostedService<Worker>(); // This registers the Worker as a hosted service

                // Optionally, register Autofac and other services here
                // services.AddAutofac();
            })
            .UseWindowsService() // Ensures it runs as a Windows Service
            .UseDefaultServiceProvider(options =>
                options.ValidateScopes = false); // Default validation for DI scopes
}
