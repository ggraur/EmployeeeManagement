using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeeManagement.Migrations
{
    public partial class NewFieldEmployeeAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Employees",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employees");
        }
    }
}
