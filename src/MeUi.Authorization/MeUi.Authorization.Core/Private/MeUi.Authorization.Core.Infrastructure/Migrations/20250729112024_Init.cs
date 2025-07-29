using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authorization.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AuthorizationCore");

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.UniqueConstraint("AK_Actions_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.UniqueConstraint("AK_Resources_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ResourceActions",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceCode = table.Column<string>(type: "text", nullable: false),
                    ActionCode = table.Column<string>(type: "character varying(255)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceActions_Actions_ActionCode",
                        column: x => x.ActionCode,
                        principalSchema: "AuthorizationCore",
                        principalTable: "Actions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceActions_Resources_ActionCode",
                        column: x => x.ActionCode,
                        principalSchema: "AuthorizationCore",
                        principalTable: "Resources",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_Code",
                schema: "AuthorizationCore",
                table: "Actions",
                column: "Code",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_DeletedAt",
                schema: "AuthorizationCore",
                table: "Actions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ActionCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ActionCode",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_DeletedAt",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceActions_ResourceCode",
                schema: "AuthorizationCore",
                table: "ResourceActions",
                column: "ResourceCode",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Code",
                schema: "AuthorizationCore",
                table: "Resources",
                column: "Code",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_DeletedAt",
                schema: "AuthorizationCore",
                table: "Resources",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceActions",
                schema: "AuthorizationCore");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "AuthorizationCore");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "AuthorizationCore");
        }
    }
}
