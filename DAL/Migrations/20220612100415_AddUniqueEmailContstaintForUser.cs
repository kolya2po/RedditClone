using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddUniqueEmailContstaintForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "781bf12e-00d2-475f-adc1-5511a58789db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9c5281dd-1b4d-46ae-abb6-ae7a1c3c4740");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "461095e1-29c4-4f37-9cd0-0fcbc025e9b6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e2844a10-debf-464b-a638-cb5cb0084fee", "AQAAAAEAACcQAAAAEJ4+2dOdCIruv2MZPbqKPVhuiJFD7W6628yy7E4dsJt/secoVLA+M+8rtvQOK5GGKg==" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4095bc3b-396d-437d-bd6c-0fce498c209d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6eac4ad2-daab-402a-b864-691d05008f02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1d5a0c83-6a6c-4406-8880-ea3e51c22e01");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c5bc7f0d-9a4b-4d64-91b0-bee35a517361", "AQAAAAEAACcQAAAAEKewyerrw74gL9+VJ3WzuIgyJGHnoGNlFGdQQgtY6I1l0qt2TPi3uOu91Qb9dRJe/A==" });
        }
    }
}
