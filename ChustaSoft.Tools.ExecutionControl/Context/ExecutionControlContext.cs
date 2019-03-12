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

        #endregion


        #region Properties

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

                entity.HasOne(e => e.ProcessDefinition).WithMany(n => n.Executions).HasForeignKey(e => e.ProcessDefinitionId);
            });

            modelBuilder.Entity<ExecutionEvent<TKey>>(entity =>
            {
                entity.ToTable(EXECUTIONEVENT_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Execution).WithMany(n => n.ExecutionEvents).HasForeignKey(e => e.ExecutionId);
            });

            modelBuilder.Entity<ExecutionModule<TKey>>(entity =>
            {
                entity.ToTable(EXECUTIONMODULE_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Execution).WithMany(n => n.ExecutionModules).HasForeignKey(e => e.ExecutionId);
                entity.HasOne(e => e.ModuleDefinition).WithMany(n => n.ExecutionModules).HasForeignKey(e => e.ModuleDefinitionId);
            });

            modelBuilder.Entity<ExecutionModuleEvent<TKey>>(entity =>
            {
                entity.ToTable(EXECUTIONMODULEEVENT_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.ExecutionModule).WithMany(n => n.ExecutionModuleEvents).HasForeignKey(e => e.ExecutionModuleId);
            });

            modelBuilder.Entity<ProcessDefinition<TKey>>(entity =>
            {
                entity.ToTable(PROCESSDEFINITION_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<ProcessModuleDefinition<TKey>>(entity =>
            {
                entity.ToTable(PROCESSMODULEDEFINITION_TABLENAME, SCHEMA_NAME);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsUnique();

                entity.HasOne(e => e.ProcessDefinition).WithMany(pd => pd.ModuleDefinitions).HasForeignKey(e => e.ProcessDefinitionId);
            });

        }

        #endregion

    }


    public class ExecutionControlContext : ExecutionControlContext<Guid>
    {

        public ExecutionControlContext(DbContextOptions<ExecutionControlContext<Guid>> options) : base(options) { }

    }

}
