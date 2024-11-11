using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoleAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 4, 5, 40, 22, 254, DateTimeKind.Utc).AddTicks(604));

            migrationBuilder.UpdateData(
                table: "mst_Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "UserName" },
                values: new object[] { new DateTime(2024, 11, 4, 5, 40, 22, 254, DateTimeKind.Utc).AddTicks(1863), "Super Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "CreatedOn", "UserName" },
                values: new object[] { new DateTime(2024, 10, 30, 11, 50, 28, 551, DateTimeKind.Utc).AddTicks(2898), "superadmin" });
        }
    }
}
