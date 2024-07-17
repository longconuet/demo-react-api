using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoReactAPI.Migrations
{
    /// <inheritdoc />
    public partial class addAvatarToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("06d7fdfa-f9cc-450f-a58b-9ce24892a6b4"));

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("23f8cb32-d26f-437a-88ee-525a0701da52"), null, new DateTime(2024, 7, 17, 13, 56, 47, 517, DateTimeKind.Local).AddTicks(4861), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$MJZbs.NYXrdNr7Azz0k0h.ewEY5mR7M0spAB4jkzuOloYClMtE2Pe", "0348523140", 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("23f8cb32-d26f-437a-88ee-525a0701da52"));

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("06d7fdfa-f9cc-450f-a58b-9ce24892a6b4"), new DateTime(2024, 7, 17, 11, 3, 40, 626, DateTimeKind.Local).AddTicks(4315), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$Ppz2bresYRDzQ1DmVwOQp.mBMsOUZVivWCA6lGPy2S.pUoaU.618O", "0348523140", 0, 1, null, null, "admin" });
        }
    }
}
