using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC.CA.Infrastructure.Persistence.Migrations
{
    public partial class ChangeTokenFieldType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "JwtJwtRefreshTokens",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nText");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "JwtJwtRefreshTokens",
                type: "nText",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
