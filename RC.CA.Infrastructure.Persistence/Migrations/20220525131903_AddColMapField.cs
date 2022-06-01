using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC.CA.Infrastructure.Persistence.Migrations
{
    public partial class AddColMapField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnMap",
                table: "CsvFile",
                type: "nText",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnMap",
                table: "CsvFile");
        }
    }
}
