using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateUserAndRuleEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "56abe46d-7897-4b9c-a467-1fd0e26db1fd", "Registered", "REGISTERED" },
                    { 2, "c09e0c46-b774-42fd-ab3f-e8ddc63c0dcf", "Moderator", "MODERATOR" },
                    { 3, "b328830a-1394-435e-a8f3-969b775bbec8", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedCommunityId", "Email", "EmailConfirmed", "Karma", "LockoutEnabled", "LockoutEnd", "ModeratedCommunityId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "15f09a08-f38d-42e3-82f4-dc43cb824213", null, "admin@email.com", true, 0, false, null, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEI/95hZKYAf7ssbIGgDnvNwwqIDosOrfPXnDmGHrQFi3ogpktKVmTHDAgoWV5OjhvA==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { 1, 3 });
        }
    }
}
