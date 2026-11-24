using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInquiryCompletionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCompletedAtUtc",
                table: "LogInquiryForUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LastCompletionHasQuote",
                table: "LogInquiryForUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCompletedAtUtc",
                table: "LogInquiryForUsers");

            migrationBuilder.DropColumn(
                name: "LastCompletionHasQuote",
                table: "LogInquiryForUsers");
        }
    }
}
