using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MehrShopping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToInvoiceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_ProductId",
                table: "InvoiceItems");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ProductId_InvoiceId",
                table: "InvoiceItems",
                columns: new[] { "ProductId", "InvoiceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_ProductId_InvoiceId",
                table: "InvoiceItems");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ProductId",
                table: "InvoiceItems",
                column: "ProductId");
        }
    }
}
