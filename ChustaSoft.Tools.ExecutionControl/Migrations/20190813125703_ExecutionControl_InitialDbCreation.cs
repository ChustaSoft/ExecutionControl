using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.ExecutionControl.Migrations
{
    public partial class ExecutionControl_InitialDbCreation : Migration
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
                    Status = table.Column<string>(nullable: false),
                    Result = table.Column<string>(nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Host = table.Column<string>(nullable: true)
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
                name: "ExecutionEvents",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExecutionId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionEvents_ExecutionId",
                schema: "Management",
                table: "ExecutionEvents",
                column: "ExecutionId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionEvents",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "Executions",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "ProcessDefinitions",
                schema: "Management");
        }
    }
}
