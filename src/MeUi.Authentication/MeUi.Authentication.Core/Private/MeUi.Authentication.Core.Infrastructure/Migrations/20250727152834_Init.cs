using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authentication.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AuthenticationCore");

            migrationBuilder.CreateTable(
                name: "LoginMethod",
                schema: "AuthenticationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "AuthenticationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsSuspended = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "AuthenticationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationCore",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginMethods",
                schema: "AuthenticationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginMethodCode = table.Column<string>(type: "text", nullable: false),
                    LoginMethodEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginMethods_LoginMethod_LoginMethodEntityId",
                        column: x => x.LoginMethodEntityId,
                        principalSchema: "AuthenticationCore",
                        principalTable: "LoginMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLoginMethods_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "AuthenticationCore",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginMethod_Code",
                schema: "AuthenticationCore",
                table: "LoginMethod",
                column: "Code",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                schema: "AuthenticationCore",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RevokedAt",
                schema: "AuthenticationCore",
                table: "RefreshToken",
                column: "RevokedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                schema: "AuthenticationCore",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                schema: "AuthenticationCore",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginMethods_LoginMethodEntityId",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                column: "LoginMethodEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginMethods_UserId_LoginMethodCode",
                schema: "AuthenticationCore",
                table: "UserLoginMethods",
                columns: new[] { "UserId", "LoginMethodCode" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "AuthenticationCore",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "AuthenticationCore",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "AuthenticationCore");

            migrationBuilder.DropTable(
                name: "UserLoginMethods",
                schema: "AuthenticationCore");

            migrationBuilder.DropTable(
                name: "LoginMethod",
                schema: "AuthenticationCore");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "AuthenticationCore");
        }
    }
}
