using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC.CA.Infrastructure.Persistence.Migrations
{
    public partial class AddProcessedOnToCsvFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedOn",
                table: "CsvFile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedOn",
                table: "CsvFile");
        }
    }
}
