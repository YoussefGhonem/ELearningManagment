using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagment.Data.Migrations
{
    public partial class UpdateUserDateOfBirthTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirtDate",
                schema: "Security",
                table: "Users",
                newName: "DateOfBirth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                schema: "Security",
                table: "Users",
                newName: "BirtDate");
        }
    }
}
