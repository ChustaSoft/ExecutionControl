using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.ExecutionControl.Migrations
{
    public partial class ChustaSoftExecutionControl_AddProcessDefinitionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Management",
                table: "ProcessDefinitions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Management",
                table: "ProcessDefinitions");
        }
    }
}
