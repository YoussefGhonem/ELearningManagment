using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesManagment.Data.Migrations
{
    public partial class UpdateUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirtDate",
                schema: "Security",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirtDate",
                schema: "Security",
                table: "Users");
        }
    }
}
