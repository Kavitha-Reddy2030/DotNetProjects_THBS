using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoleAPI.Migrations
{
    /// <inheritdoc />
    public partial class uservalidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 10, 30, 11, 50, 28, 551, DateTimeKind.Utc).AddTicks(2140));

            migrationBuilder.UpdateData(
                table: "mst_Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 10, 30, 11, 50, 28, 551, DateTimeKind.Utc).AddTicks(2898));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 10, 30, 11, 43, 22, 827, DateTimeKind.Utc).AddTicks(3059));

            migrationBuilder.UpdateData(
                table: "mst_Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 10, 30, 11, 43, 22, 827, DateTimeKind.Utc).AddTicks(3736));
        }
    }
}
