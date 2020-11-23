using FacesApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",

                    builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials()
                 );
            });

        public static void ConfigureAzureCrednetials(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new AzureFaceConfiguration();
            configuration.Bind("AzureFaceConfiguration", config);
            services.AddSingleton(config);
        };

        public static void ConfigureImageSharpSynchronousCalls(this IServiceCollection services)
        {
            //if using Kestrel
            services.Configure<KestrelServerOptions>(opt =>
            {
                opt.AllowSynchronousIO = true;
            });

            //if using IIS
            services.Configure<IISServerOptions>(opt =>
            {
                opt.AllowSynchronousIO = true;
            });
        }


    }
}
