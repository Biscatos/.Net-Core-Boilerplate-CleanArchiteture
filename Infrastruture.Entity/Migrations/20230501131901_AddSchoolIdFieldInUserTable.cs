using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wizzzard.Identity.Migrations
{
    public partial class AddSchoolIdFieldInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                schema: "Identity",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);         

           


            migrationBuilder.CreateIndex(
                name: "IX_Users_SchoolId",
                schema: "Identity",
                table: "Users",
                column: "SchoolId");
           

            migrationBuilder.AddForeignKey(
                name: "FK_Users_School_SchoolId",
                schema: "Identity",
                table: "Users",
                column: "SchoolId",
                principalSchema: "dbo",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_School_SchoolId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SchoolId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                schema: "Identity",
                table: "Users");         
        }
    }
}
