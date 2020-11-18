﻿using GreenPipes;
using MassTransit;
using Messaging.InterfacesConstant.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Messages.Consumer;
using OrdersApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureRabbitWithMT(this IServiceCollection services)
        {
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                config =>
                {
                    config.Host("localhost", "/", h => { });
                    config.ReceiveEndpoint(RabbitMqMassTransitConstants.RegisterOrderCommandQueue, e => {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                        e.Consumer<RegisterOrderCommandConsumer>(provider);
                    });

                    config.ConfigureEndpoints((IBusRegistrationContext)provider);
                }
            ));
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrdersContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqloptions => {
                sqloptions.EnableRetryOnFailure();
            }));
        }
    }
}
