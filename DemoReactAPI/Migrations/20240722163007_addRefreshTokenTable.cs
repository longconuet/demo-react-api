using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoReactAPI.Migrations
{
    /// <inheritdoc />
    public partial class addRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("23f8cb32-d26f-437a-88ee-525a0701da52"));

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("4ce7db12-b083-402b-aa4b-8bd385dd9b53"), null, new DateTime(2024, 7, 22, 23, 30, 6, 619, DateTimeKind.Local).AddTicks(2941), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$SxqIuFg.DqtvG1s82KjuXud5IA/w0xWKfv98hD76IqtvJgnBpAoCO", "0348523140", 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4ce7db12-b083-402b-aa4b-8bd385dd9b53"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("23f8cb32-d26f-437a-88ee-525a0701da52"), null, new DateTime(2024, 7, 17, 13, 56, 47, 517, DateTimeKind.Local).AddTicks(4861), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$MJZbs.NYXrdNr7Azz0k0h.ewEY5mR7M0spAB4jkzuOloYClMtE2Pe", "0348523140", 0, 1, null, null, "admin" });
        }
    }
}
