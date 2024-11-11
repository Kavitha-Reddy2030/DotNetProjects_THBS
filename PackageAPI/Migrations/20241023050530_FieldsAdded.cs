using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageAPI.Migrations
{
    /// <inheritdoc />
    public partial class FieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Packages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Status" },
                values: new object[] { new DateTime(2024, 10, 23, 10, 35, 29, 732, DateTimeKind.Local).AddTicks(7228), true });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Status" },
                values: new object[] { new DateTime(2024, 10, 23, 10, 35, 29, 732, DateTimeKind.Local).AddTicks(7237), true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Packages");
        }
    }
}
