using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authentication.Password.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Passwords_DeletedAt",
                schema: "AuthenticationPassword",
                table: "Passwords",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passwords_DeletedAt",
                schema: "AuthenticationPassword",
                table: "Passwords");
        }
    }
}
