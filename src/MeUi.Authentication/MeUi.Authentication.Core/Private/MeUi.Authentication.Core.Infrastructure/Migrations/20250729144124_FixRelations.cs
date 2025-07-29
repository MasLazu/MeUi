using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authentication.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginMethods_LoginMethod_LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginMethods_LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.DropColumn(
                name: "LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.AlterColumn<string>(
                name: "LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                type: "character varying(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LoginMethod_Code",
                schema: "AuthenticationCore",
                table: "LoginMethod",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginMethods_LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "LoginMethodCode");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginMethods_LoginMethod_LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "LoginMethodCode",
                principalSchema: "AuthenticationCore",
                principalTable: "LoginMethod",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginMethods_LoginMethod_LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginMethods_LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_LoginMethod_Code",
                schema: "AuthenticationCore",
                table: "LoginMethod");

            migrationBuilder.AlterColumn<string>(
                name: "LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)");

            migrationBuilder.AddColumn<Guid>(
                name: "LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginMethods_LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "LoginMethodEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginMethods_LoginMethod_LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "LoginMethodEntityId",
                principalSchema: "AuthenticationCore",
                principalTable: "LoginMethod",
                principalColumn: "Id");
        }
    }
}
