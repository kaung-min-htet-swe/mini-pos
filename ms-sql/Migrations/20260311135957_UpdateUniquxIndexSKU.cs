using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms_sql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniquxIndexSKU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ__Products__CA1ECF0DDF49614F",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "UQ__Products__CA1ECF0DDF49614F",
                table: "Products",
                column: "SKU",
                unique: true,
                filter: "([DeletedAt] IS NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Products__CA1ECF0DDF49614F",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "UQ__Products__CA1ECF0DDF49614F",
                table: "Products",
                column: "SKU",
                unique: true);
        }
    }
}
