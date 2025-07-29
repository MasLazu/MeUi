using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authorization.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceActions_Resources_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                type: "character varying(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceActions_Resources_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ResourceCode",
                principalSchema: "AuthorizationCore",
                principalTable: "Resources",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceActions_Resources_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceActions_Resources_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ActionCode",
                principalSchema: "AuthorizationCore",
                principalTable: "Resources",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
