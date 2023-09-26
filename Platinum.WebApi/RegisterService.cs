using Microsoft.Extensions.DependencyInjection;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;

namespace Platinum.WebApi
{
    public static class RegisterService
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            Configure(services, Data.DataRegister.GetTypes());
            Configure(services, Service.ServiceRegister.GetTypes());
             
        }

        private static void Configure(IServiceCollection services, Dictionary<Type, Type> types)
        {
            foreach (var type in types)
                services.AddScoped(type.Key, type.Value);
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
