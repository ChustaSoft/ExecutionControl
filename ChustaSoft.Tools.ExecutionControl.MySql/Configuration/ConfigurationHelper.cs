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
            services.AddDbContext<ExecutionControlContext<Guid>>(opt => 
                    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    mso => mso.MigrationsAssembly(MIGRATIONS_ASSEMBLY).SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore)
                ));

            services.RegisterExecutionControl<TProcessEnum>(minutesToAbort);
        }

        public static void RegisterExecutionControl<TKey, TProcessEnum>(this IServiceCollection services, string connectionString, int minutesToAbort = Constants.DEFAULT_ABORT_PROCESS_TIMEOUT)
                where TKey : IComparable
                where TProcessEnum : struct, IConvertible
        {
            services.AddDbContext<ExecutionControlContext<TKey>>(opt => 
                    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    mso => mso.MigrationsAssembly(MIGRATIONS_ASSEMBLY).SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Ignore)
                ));

            services.RegisterExecutionControl<TKey, TProcessEnum>(minutesToAbort);
        }

    }
}
