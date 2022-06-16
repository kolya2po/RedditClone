using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateTopicEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Communities",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostingDate",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "56abe46d-7897-4b9c-a467-1fd0e26db1fd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c09e0c46-b774-42fd-ab3f-e8ddc63c0dcf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b328830a-1394-435e-a8f3-969b775bbec8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "15f09a08-f38d-42e3-82f4-dc43cb824213", "AQAAAAEAACcQAAAAEI/95hZKYAf7ssbIGgDnvNwwqIDosOrfPXnDmGHrQFi3ogpktKVmTHDAgoWV5OjhvA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Communities_Title",
                table: "Communities",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Communities_Title",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "PostingDate",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
        }
    }
}
