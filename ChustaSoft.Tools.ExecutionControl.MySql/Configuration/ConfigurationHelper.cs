using ChustaSoft.Tools.ExecutionControl.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Configuration
{
    public static class ConfigurationHelper
    {

        private const string MIGRATIONS_ASSEMBLY = "ChustaSoft.Tools.ExecutionControl.MySql";


        public static void RegisterExecutionControl<TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = Constants.DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TProcessEnum : struct, IConvertible
        {
#if NET5_0
            services.AddDbContext<ExecutionControlContext<Guid>>(opt => 
                    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    mso => mso.MigrationsAssembly(MIGRATIONS_ASSEMBLY).SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore)
                ));
#else
            services.AddDbContext<ExecutionControlContext<Guid>>(options => options.UseMySql(connectionString));
#endif

            services.RegisterExecutionControl<TProcessEnum>(minutesToAbort);
        }

        public static void RegisterExecutionControl<TKey, TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = Constants.DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TKey : IComparable
                where TProcessEnum : struct, IConvertible
        {
#if NET5_0
            services.AddDbContext<ExecutionControlContext<TKey>>(opt => 
                    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    mso => mso.MigrationsAssembly(MIGRATIONS_ASSEMBLY).SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore)
                ));
#else
            services.AddDbContext<ExecutionControlContext<TKey>>(options => options.UseMySql(connectionString));
#endif
            services.RegisterExecutionControl<TKey, TProcessEnum>(minutesToAbort);
        }

    }
}
