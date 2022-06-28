using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RC.CA.Infrastructure.Persistence.Migrations
{
    public partial class AddConcurrencyToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "Members",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "JwtJwtRefreshTokens",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "Experience",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "CsvFile",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "CdnFiles",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "JwtJwtRefreshTokens");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Experience");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "CsvFile");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "CdnFiles");
        }
    }
}
