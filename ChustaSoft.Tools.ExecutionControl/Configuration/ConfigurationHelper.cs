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

        #region Constants

        private const int DEFAULT_ABORT_PROCESS_TIMEOUT = 60;

        #endregion


        #region Public methods

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

            services.AddSingleton(new ExecutionControlConfiguration { MinutesToAbort = minutesToAbort });

            services.AddTransient<IExecutionRepository<TKey>, ExecutionRepository<TKey>>();
            services.AddTransient<IExecutionEventRepository<TKey>, ExecutionEventRepository<TKey>>();
            services.AddTransient<IProcessDefinitionRepository<TKey>, ProcessDefinitionRepository<TKey>>();

            services.AddTransient<IExecutionBusiness<TKey>, ExecutionBusiness<TKey>>();
            services.AddTransient<IExecutionEventBusiness<TKey>, ExecutionEventBusiness<TKey>>();
            services.AddTransient<IProcessDefinitionBusiness<TKey, TProcessEnum>, ProcessDefinitionBusiness<TKey, TProcessEnum>>();
            services.AddTransient<IProcessExecutionSummaryBusiness<TKey>, ProcessExecutionSummaryBusiness<TKey>>();

            services.AddTransient<IExecutionService<TKey, TProcessEnum>, ExecutionService<TKey, TProcessEnum>>();
            services.AddTransient<IReportingService<TKey>, ReportingService<TKey>>();
        }

        public static void ConfigureExecutionControl(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            ConfigureDatabase(serviceProvider);
        }

        public static void ConfigureExecutionControl<TProcessEnum>(this IApplicationBuilder app, IServiceProvider serviceProvider)
                where TProcessEnum : struct, IConvertible
        {
            ConfigureDatabase(serviceProvider);
            ConfigureDefinitions<TProcessEnum>(serviceProvider);
        }

        #endregion


        #region Private methods

        private static void ConfigureDefinitions<TProcessEnum>(IServiceProvider serviceProvider) where TProcessEnum : struct, IConvertible
        {
            var processDefinitionBusiness = serviceProvider.GetRequiredService<IProcessDefinitionBusiness<Guid, TProcessEnum>>();

            processDefinitionBusiness.Setup();
        }

        private static void ConfigureDatabase(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<ExecutionControlContext<Guid>>();

            databaseContext.Database.Migrate();
        }

        #endregion

    }
}
