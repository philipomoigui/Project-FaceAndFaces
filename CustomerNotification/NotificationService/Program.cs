using GreenPipes;
using MassTransit;
using Messaging.InterfacesConstant.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Messages.Consumer;
using NotificationService.Service;
using System;
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x => {
                        x.AddConsumer<OrderProcessedEventConsumer>();
                    });

                    services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(config => {
                        //config.UseHealthCheck(provider);
                        config.Host("localhost", "/", h => { });
                        config.ReceiveEndpoint(RabbitMqMassTransitConstants.NotificationServiceQueue, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                            e.Consumer<OrderProcessedEventConsumer>();
                        });
                    }));

                    services.AddSingleton<IHostedService, BusService>();
                });

            return hostBuilder;
        }

    }
}
