using Faces.WebMVC.Sevices;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMVC.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureRabbitMqWithMT(this IServiceCollection services)
        {
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                config =>
                {
                    config.Host("localhost", "/", h => { });
                    services.AddSingleton(provider => provider.GetRequiredService<IBusControl>());
                    services.AddSingleton<IHostedService, BusService>();
                }
             ));
        }
    }
}
