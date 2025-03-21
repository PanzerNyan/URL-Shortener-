﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Short_URL_INFORCE.Migrations
{
    /// <inheritdoc />
    public partial class MakeShortUrlNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_URLs_ShortUrl",
                table: "URLs");

            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "URLs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FullUrl",
                table: "URLs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_URLs_FullUrl",
                table: "URLs",
                column: "FullUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_URLs_FullUrl",
                table: "URLs");

            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "URLs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullUrl",
                table: "URLs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_URLs_ShortUrl",
                table: "URLs",
                column: "ShortUrl",
                unique: true);
        }
    }
}
