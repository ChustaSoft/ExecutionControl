using ChustaSoft.Tools.ExecutionControl.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ChustaSoft.Tools.ExecutionControl.Migrations
{
    [DbContext(typeof(ExecutionControlContext<Guid>))]
    [Migration("20190923102022_ChustaSoft-ExecutionControl_InitialDbCreation")]
    partial class ChustaSoftExecutionControl_InitialDbCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BeginDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("Host");

                    b.Property<Guid>("ProcessDefinitionId");

                    b.Property<string>("Result")
                        .IsRequired();

                    b.Property<string>("Status")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProcessDefinitionId");

                    b.ToTable("Executions","Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ExecutionEvent<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("ExecutionId");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<string>("Summary");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.ToTable("ExecutionEvents","Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ProcessDefinition<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("ProcessDefinitions","Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", b =>
                {
                    b.HasOne("ChustaSoft.Tools.ExecutionControl.Entities.ProcessDefinition<System.Guid>", "ProcessDefinition")
                        .WithMany("Executions")
                        .HasForeignKey("ProcessDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ExecutionEvent<System.Guid>", b =>
                {
                    b.HasOne("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", "Execution")
                        .WithMany("ExecutionEvents")
                        .HasForeignKey("ExecutionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
