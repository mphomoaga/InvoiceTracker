using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InvoiceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.EnsureSchema(
                name: "setting");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "security",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                schema: "setting",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "security",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security",
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "RoleId", "CreatedBy", "CreatedDate", "Description", "IsActive", "ModifiedBy", "ModifiedDate", "RoleName" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(4615), "System Administrator", true, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(4993), "Admin" },
                    { 2, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5503), "Regular user", true, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5504), "User" },
                    { 3, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5507), "Invoice Manager", true, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5508), "InvoiceManager" },
                    { 4, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5510), "Invoice Agent", true, "System", new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5511), "Invoice Agent" }
                });

            migrationBuilder.InsertData(
                schema: "setting",
                table: "setting",
                columns: new[] { "SettingId", "CreatedBy", "CreatedDate", "IsActive", "Key", "ModifiedBy", "ModifiedDate", "Value" },
                values: new object[] { 1, "System", new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc).AddTicks(531), true, "EcryptionKey", null, new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc).AddTicks(532), "X9#mP7@vL2!qR8$zN5&wK1*Tj6^Ys3%Hu9+Cg4=Db7?Fm2!Qr8@Wx5#Lp1$Zn6^Vk3&Te0" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "security",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "setting",
                schema: "setting");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "security");
        }
    }
}
