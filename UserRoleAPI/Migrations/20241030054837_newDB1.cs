using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoleAPI.Migrations
{
    /// <inheritdoc />
    public partial class newDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mst_Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "mst_Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_mst_Users_mst_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "mst_Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "mst_Roles",
                columns: new[] { "RoleId", "ActiveStatus", "CreatedBy", "CreatedOn", "RoleName" },
                values: new object[] { 1, true, "Super Admin", new DateTime(2024, 10, 30, 5, 48, 36, 641, DateTimeKind.Utc).AddTicks(806), "Super Admin" });

            migrationBuilder.InsertData(
                table: "mst_Users",
                columns: new[] { "UserId", "ActiveStatus", "CreatedBy", "CreatedOn", "Email", "MobileNumber", "Password", "RoleId", "UserName" },
                values: new object[] { 1, true, "Super Admin", new DateTime(2024, 10, 30, 5, 48, 36, 641, DateTimeKind.Utc).AddTicks(2212), "superadmin@example.com", "9876543210", "01NbeOJIZ/PIUP7ByFkbekabj4aDvmTYNQV8uf0gSqE=", 1, "superadmin" });

            migrationBuilder.CreateIndex(
                name: "IX_mst_Users_Email",
                table: "mst_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mst_Users_RoleId",
                table: "mst_Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_Users");

            migrationBuilder.DropTable(
                name: "mst_Roles");
        }
    }
}
