using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC.CA.Infrastructure.Persistence.Migrations
{
    public partial class RemoveRowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "JwtJwtRefreshTokens");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CsvFile");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CdnFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Members",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "JwtJwtRefreshTokens",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Experience",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "CsvFile",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "CdnFiles",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
