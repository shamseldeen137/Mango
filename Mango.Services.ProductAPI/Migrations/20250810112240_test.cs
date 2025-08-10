using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons");

            migrationBuilder.RenameTable(
                name: "Coupons",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Description",
                value: " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  malesuada neque. Phasellus commodo cursus pretium.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Coupons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons",
                column: "ProductId");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Description",
                value: " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.");
        }
    }
}
