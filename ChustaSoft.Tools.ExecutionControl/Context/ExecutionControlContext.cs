using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.ExecutionControl.Enums;
using ChustaSoft.Tools.ExecutionControl.Model;
using Microsoft.EntityFrameworkCore;
using System;


namespace ChustaSoft.Tools.ExecutionControl.Context
{
    public class ExecutionControlContext<TKey> : DbContext
    {

        #region Constants

        private const string SCHEMA_NAME = "Management";

        private const string EXECUTION_TABLENAME = "Executions";
        private const string EXECUTIONEVENT_TABLENAME = "ExecutionEvents";
        private const string EXECUTIONMODULE_TABLENAME = "ExecutionModules";
        private const string EXECUTIONMODULEEVENT_TABLENAME = "ExecutionModuleEvents";
        private const string PROCESSDEFINITION_TABLENAME = "ProcessDefinitions";
        private const string PROCESSMODULEDEFINITION_TABLENAME = "ProcessModuleDefinitions";

        private const int MAX_VARCHAR_LENGTH = 500;

        #endregion


        #region Properties

        public DbSet<Execution<TKey>> Executions { get; set; }

        public DbSet<ExecutionEvent<TKey>> ExecutionEvents { get; set; }

        public DbSet<ProcessDefinition<TKey>> ProcessDefinitions { get; set; }

        #endregion


        #region Constructors

        public ExecutionControlContext(DbContextOptions<ExecutionControlContext<TKey>> options) : base(options) { }

        #endregion


        #region Protected methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Execution<TKey>>(entity =>
            {
                entity.ToTable(EXECUTION_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e._BeginContext).HasColumnName("BeginContext").HasMaxLength(MAX_VARCHAR_LENGTH);
                entity.Property(e => e._EndContext).HasColumnName("EndContext").HasMaxLength(MAX_VARCHAR_LENGTH);

                entity.Property(e => e.Status).HasConversion(
                    dtoValue => dtoValue.ToString(),
                    entityValue => EnumsHelper.GetByString<ExecutionStatus>(entityValue)
                );
                entity.Property(e => e.Result).HasConversion(
                    dtoValue => dtoValue.ToString(),
                    entityValue => EnumsHelper.GetByString<ExecutionResult>(entityValue)
                );

                entity.HasOne(e => e.ProcessDefinition).WithMany(n => n.Executions).HasForeignKey(e => e.ProcessDefinitionId);
            });

            modelBuilder.Entity<ExecutionEvent<TKey>>(entity =>
            {
                entity.ToTable(EXECUTIONEVENT_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Status).HasConversion(
                    dtoValue => dtoValue.ToString(),
                    entityValue => EnumsHelper.GetByString<ExecutionStatus>(entityValue)
                );

                entity.HasOne(e => e.Execution).WithMany(n => n.ExecutionEvents).HasForeignKey(e => e.ExecutionId);
            });
          
            modelBuilder.Entity<ProcessDefinition<TKey>>(entity =>
            {
                entity.ToTable(PROCESSDEFINITION_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsUnique();
            });

        }

        #endregion

    }


    public class ExecutionControlContext : ExecutionControlContext<Guid>
    {

        public ExecutionControlContext(DbContextOptions<ExecutionControlContext<Guid>> options) : base(options) { }

    }

}
