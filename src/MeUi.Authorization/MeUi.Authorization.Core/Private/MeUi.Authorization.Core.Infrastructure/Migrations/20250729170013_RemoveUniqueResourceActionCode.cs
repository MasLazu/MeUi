using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authorization.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueResourceActionCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResourceActions_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.DropIndex(
                name: "IX_ResourceActions_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ActionCode",
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ResourceCode",
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResourceActions_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.DropIndex(
                name: "IX_ResourceActions_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ActionCode",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ResourceCode",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }
    }
}
