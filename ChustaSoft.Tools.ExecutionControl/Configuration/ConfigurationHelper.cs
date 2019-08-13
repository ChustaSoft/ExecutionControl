using ChustaSoft.Tools.ExecutionControl.Context;
using ChustaSoft.Tools.ExecutionControl.Domain;
using ChustaSoft.Tools.ExecutionControl.Repositories;
using ChustaSoft.Tools.ExecutionControl.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Configuration
{
    public static class ConfigurationHelper
    {

        private const int DEFAULT_ABORT_PROCESS_TIMEOUT = 60;


        public static void RegisterExecutionControl<TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TProcessEnum : struct, IConvertible
        {
            services.RegisterExecutionControl<Guid, TProcessEnum>(connectionString, minutesToAbort);
        }

        public static void RegisterExecutionControl<TKey, TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TKey : IComparable
                where TProcessEnum : struct, IConvertible
        {
            services.AddDbContext<ExecutionControlContext<TKey>>(options => options.UseSqlServer(connectionString));

            services.AddSingleton(new ExecutionControlConfiguration { });

            services.AddTransient<IExecutionRepository<TKey>, ExecutionRepository<TKey>>();
            services.AddTransient<IExecutionEventRepository<TKey>, ExecutionEventRepository<TKey>>();
            services.AddTransient<IProcessDefinitionRepository<TKey>, ProcessDefinitionRepository<TKey>>();

            services.AddTransient<IExecutionBusiness<TKey>, ExecutionBusiness<TKey>>();
            services.AddTransient<IExecutionEventBusiness<TKey>, ExecutionEventBusiness<TKey>>();

            services.AddTransient<IExecutionService<TKey, TProcessEnum>, ExecutionService<TKey, TProcessEnum>>();
        }

        public static void ConfigureExecutionControl(this IApplicationBuilder app, ExecutionControlContext<Guid> executionControlContext)
        {
            executionControlContext.Database.Migrate();
        }

    }
}
