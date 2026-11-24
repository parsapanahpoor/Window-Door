using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInquirySellerExposure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InquirySellerExposures",
                columns: table => new
                {
                    InquiryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tariff = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InquirySellerExposures", x => new { x.InquiryId, x.SellerId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InquirySellerExposures");
        }
    }
}
