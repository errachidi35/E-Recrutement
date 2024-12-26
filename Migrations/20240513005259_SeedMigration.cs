using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Recrutement.Migrations
{
    /// <inheritdoc />
    public partial class SeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "Id", "Disable", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, false, "Contract Type 1", "contract-type-1" },
                    { 2, false, "Contract Type 2", "contract-type-2" }
                });

            migrationBuilder.InsertData(
                table: "Sectors",
                columns: new[] { "Id", "Description", "Disable", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, null, false, "Sector 1", "sector-1" },
                    { 2, null, false, "Sector 2", "sector-2" }
                });

            migrationBuilder.InsertData(
                table: "Profils",
                columns: new[] { "Id", "Disable", "Name", "Popular", "SectorId", "Slug" },
                values: new object[,]
                {
                    { 1, false, "Profil 1", 0, 1, "profil-1" },
                    { 2, false, "Profil 2", 0, 2, "profil-2" }
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Disable", "Name", "Popular", "SectorId", "Slug" },
                values: new object[,]
                {
                    { 1, false, "Province 1", 0, 1, "province-1" },
                    { 2, false, "Province 2", 0, 2, "province-2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profils",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Profils",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sectors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sectors",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
