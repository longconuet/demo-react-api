using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoReactAPI.Migrations
{
    /// <inheritdoc />
    public partial class initUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("06d7fdfa-f9cc-450f-a58b-9ce24892a6b4"), new DateTime(2024, 7, 17, 11, 3, 40, 626, DateTimeKind.Local).AddTicks(4315), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$Ppz2bresYRDzQ1DmVwOQp.mBMsOUZVivWCA6lGPy2S.pUoaU.618O", "0348523140", 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
