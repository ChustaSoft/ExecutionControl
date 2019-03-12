using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.ExecutionControl.Migrations
{
    public partial class ExecutionControlDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Management");

            migrationBuilder.CreateTable(
                name: "ProcessDefinitions",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcessDefinitionId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Result = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Server = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executions_ProcessDefinitions_ProcessDefinitionId",
                        column: x => x.ProcessDefinitionId,
                        principalSchema: "Management",
                        principalTable: "ProcessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessModuleDefinitions",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcessDefinitionId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Concurrent = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessModuleDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessModuleDefinitions_ProcessDefinitions_ProcessDefinitionId",
                        column: x => x.ProcessDefinitionId,
                        principalSchema: "Management",
                        principalTable: "ProcessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionEvents",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExecutionId = table.Column<Guid>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionEvents_Executions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalSchema: "Management",
                        principalTable: "Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionModules",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExecutionId = table.Column<Guid>(nullable: false),
                    ModuleDefinitionId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Result = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionModules_Executions_ExecutionId",
                        column: x => x.ExecutionId,
                        principalSchema: "Management",
                        principalTable: "Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionModules_ProcessModuleDefinitions_ModuleDefinitionId",
                        column: x => x.ModuleDefinitionId,
                        principalSchema: "Management",
                        principalTable: "ProcessModuleDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionModuleEvents",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExecutionModuleId = table.Column<Guid>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionModuleEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionModuleEvents_ExecutionModules_ExecutionModuleId",
                        column: x => x.ExecutionModuleId,
                        principalSchema: "Management",
                        principalTable: "ExecutionModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionEvents_ExecutionId",
                schema: "Management",
                table: "ExecutionEvents",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionModuleEvents_ExecutionModuleId",
                schema: "Management",
                table: "ExecutionModuleEvents",
                column: "ExecutionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionModules_ExecutionId",
                schema: "Management",
                table: "ExecutionModules",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionModules_ModuleDefinitionId",
                schema: "Management",
                table: "ExecutionModules",
                column: "ModuleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Executions_ProcessDefinitionId",
                schema: "Management",
                table: "Executions",
                column: "ProcessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDefinitions_Name",
                schema: "Management",
                table: "ProcessDefinitions",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessModuleDefinitions_Name",
                schema: "Management",
                table: "ProcessModuleDefinitions",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessModuleDefinitions_ProcessDefinitionId",
                schema: "Management",
                table: "ProcessModuleDefinitions",
                column: "ProcessDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionEvents",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "ExecutionModuleEvents",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "ExecutionModules",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "Executions",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "ProcessModuleDefinitions",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "ProcessDefinitions",
                schema: "Management");
        }
    }
}
