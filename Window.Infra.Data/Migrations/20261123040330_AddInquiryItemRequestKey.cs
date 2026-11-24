using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInquiryItemRequestKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientRequestId",
                table: "logInquiryForUserDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_logInquiryForUserDetails_LogInquiryForUserId_ClientRequestId",
                table: "logInquiryForUserDetails",
                columns: new[] { "LogInquiryForUserId", "ClientRequestId" },
                unique: true,
                filter: "[ClientRequestId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_logInquiryForUserDetails_LogInquiryForUserId_ClientRequestId",
                table: "logInquiryForUserDetails");

            migrationBuilder.DropColumn(
                name: "ClientRequestId",
                table: "logInquiryForUserDetails");
        }
    }
}
