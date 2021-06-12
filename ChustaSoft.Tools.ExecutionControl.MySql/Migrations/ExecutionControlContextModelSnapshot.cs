﻿// <auto-generated />
using System;
using ChustaSoft.Tools.ExecutionControl.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChustaSoft.Tools.ExecutionControl.Migrations
{
    [DbContext(typeof(ExecutionControlContext<Guid>))]
    partial class ExecutionControlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Host")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ProcessDefinitionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProcessDefinitionId");

                    b.ToTable("Executions", "Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ExecutionEvent<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ExecutionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.ToTable("ExecutionEvents", "Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ProcessDefinition<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProcessDefinitions", "Management");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", b =>
                {
                    b.HasOne("ChustaSoft.Tools.ExecutionControl.Entities.ProcessDefinition<System.Guid>", "ProcessDefinition")
                        .WithMany("Executions")
                        .HasForeignKey("ProcessDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessDefinition");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ExecutionEvent<System.Guid>", b =>
                {
                    b.HasOne("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", "Execution")
                        .WithMany("ExecutionEvents")
                        .HasForeignKey("ExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Execution");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.Execution<System.Guid>", b =>
                {
                    b.Navigation("ExecutionEvents");
                });

            modelBuilder.Entity("ChustaSoft.Tools.ExecutionControl.Entities.ProcessDefinition<System.Guid>", b =>
                {
                    b.Navigation("Executions");
                });
#pragma warning restore 612, 618
        }
    }
}
