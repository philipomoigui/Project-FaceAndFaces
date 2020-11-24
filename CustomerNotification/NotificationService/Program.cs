using EmailService;
using GreenPipes;
using MassTransit;
using Messaging.InterfacesConstant.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Messages.Consumer;
using NotificationService.Service;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NotificationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })

                .ConfigureServices((hostContext, services) =>
                {
                    var emailConfig = hostContext.Configuration.GetSection("EmailConfiguration")
                    .Get<EmailConfig>();

                    services.AddSingleton(emailConfig);

                    services.AddScoped<IEmailSender, EmailSender>();

                    services.AddMassTransit(x => {
                        x.AddConsumer<OrderProcessedEventConsumer>();
                    });

                    services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(config => {
                        //config.UseHealthCheck(provider);
                        config.Host("rabbitmq", "/", h => { });
                        config.ReceiveEndpoint(RabbitMqMassTransitConstants.NotificationServiceQueue, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                            e.Consumer<OrderProcessedEventConsumer>(provider);
                        });
                    }));

                    services.AddSingleton<IHostedService, BusService>();
                });

            return hostBuilder;
        }

    }
}
