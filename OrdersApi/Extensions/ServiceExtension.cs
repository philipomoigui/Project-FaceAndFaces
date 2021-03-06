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
                    config.Host("rabbitmq", "/", h => { });
                    config.ReceiveEndpoint(RabbitMqMassTransitConstants.RegisterOrderCommandQueue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                        e.Consumer<RegisterOrderCommandConsumer>(provider);
                    });

                    config.ReceiveEndpoint(RabbitMqMassTransitConstants.OrderDispachedServiceQueue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                        e.Consumer<OrderDispatchedEventConsumer>(provider);
                    });

                    //config.ConfigureEndpoints(provider);
                }
            ));
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<RegisterOrderCommandConsumer>();
                x.AddConsumer<OrderDispatchedEventConsumer>();
            });
        }

        //public static void ConfigureRabbitWithMT(this IServiceCollection services)
        //{
        //    services.AddMassTransit(x =>
        //    {
        //        x.AddConsumer<RegisterOrderCommandConsumer>();
        //        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
        //        {
        //            config.UseHealthCheck(provider);
        //            config.Host("localhost", "/", h => { });
        //            config.ReceiveEndpoint(RabbitMqMassTransitConstants.RegisterOrderCommandQueue, ep =>
        //            {
        //                ep.PrefetchCount = 16;
        //                ep.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
        //                ep.ConfigureConsumer<RegisterOrderCommandConsumer>(provider);
        //            });
        //        }));
        //    });
        //}

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrdersContext>(opt => opt.UseSqlServer(configuration["DefaultConnection"], sqloptions => {
                sqloptions.EnableRetryOnFailure();
            }));
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(option => {
                option.AddPolicy("CorsPolicy", builder =>

                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
                );
            });
        }

        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = null;
            });
        }
    }
}
