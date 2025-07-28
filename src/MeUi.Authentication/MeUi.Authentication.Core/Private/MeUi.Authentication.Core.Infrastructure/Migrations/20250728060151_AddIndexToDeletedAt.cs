using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authentication.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedAt",
                schema: "AuthenticationCore",
                table: "Users",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginMethods_DeletedAt",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_DeletedAt",
                schema: "AuthenticationCore",
                table: "RefreshToken",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LoginMethod_DeletedAt",
                schema: "AuthenticationCore",
                table: "LoginMethod",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_DeletedAt",
                schema: "AuthenticationCore",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginMethods_DeletedAt",
                schema: "AuthenticationCore",
                table: "UserLoginMethods");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_DeletedAt",
                schema: "AuthenticationCore",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_LoginMethod_DeletedAt",
                schema: "AuthenticationCore",
                table: "LoginMethod");
        }
    }
}
