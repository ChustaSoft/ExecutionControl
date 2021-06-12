using ChustaSoft.Tools.ExecutionControl.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Configuration
{
    public static class ConfigurationHelper
    {

        private const string MIGRATIONS_ASSEMBLY = "ChustaSoft.Tools.ExecutionControl.SqlServer";


        /// <summary>
        /// Automatically register ExecutionControl dependencies in SqlServer
        /// </summary>
        /// <typeparam name="TProcessEnum">Processes defined</typeparam>
        /// <param name="services">Service Collection from DI Container</param>
        /// <param name="connectionString">SqlServer connection string</param>
        /// <param name="minutesToAbort">Minutes configured to wait until abort a process. By default, 60</param>
        public static void RegisterExecutionControl<TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = Constants.DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TProcessEnum : struct, IConvertible
        {
            services.AddDbContext<ExecutionControlContext<Guid>>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly(MIGRATIONS_ASSEMBLY)));

            services.RegisterExecutionControl<TProcessEnum>(minutesToAbort);
        }

        /// <summary>
        /// Automatically register ExecutionControl dependencies in SqlServer
        /// </summary>
        /// <typeparam name="TKey">Primary key configured for required tables</typeparam>
        /// <typeparam name="TProcessEnum">Processes defined</typeparam>
        /// <param name="services">Service Collection from DI Container</param>
        /// <param name="connectionString">SqlServer connection string</param>
        /// <param name="minutesToAbort">Minutes configured to wait until abort a process. By default, 60</param>
        public static void RegisterExecutionControl<TKey, TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = Constants.DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TKey : IComparable
                where TProcessEnum : struct, IConvertible
        {
            services.AddDbContext<ExecutionControlContext<TKey>>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly(MIGRATIONS_ASSEMBLY)));

            services.RegisterExecutionControl<TKey, TProcessEnum>(minutesToAbort);
        }

    }
}
