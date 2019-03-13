using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Configuration
{
    public static class ConfigurationHelper
    {

        public static IServiceCollection RegisterExecutionControl<TKey>(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExecutionControlContext<TKey>>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection RegisterExecutionControl(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExecutionControlContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static void WithDefinitions<TToolConfiguration, TKey, TEnum>(this IServiceCollection services, TToolConfiguration toolConfiguration) where TToolConfiguration : ExecutionControlConfigurationBase<TKey, TEnum> where TEnum : struct, IConvertible
        {
            services.AddSingleton<ExecutionControlConfigurationBase<TKey, TEnum>>(toolConfiguration);
        }

        public static void WithDefinitions<TToolConfiguration, TKey>(this IServiceCollection services, TToolConfiguration toolConfiguration) where TToolConfiguration : ExecutionControlConfigurationBase<TKey>
        {
            services.AddSingleton<ExecutionControlConfigurationBase<TKey>>(toolConfiguration);
        }

        public static void WithDefinitions<TToolConfiguration>(this IServiceCollection services, TToolConfiguration toolConfiguration) where TToolConfiguration : ExecutionControlConfigurationBase
        {
            services.AddSingleton<ExecutionControlConfigurationBase>(toolConfiguration);
        }

    }
}
