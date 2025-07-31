using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeUi.Authorization.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageGroups",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PageGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_PageGroups_PageGroupId",
                        column: x => x.PageGroupId,
                        principalSchema: "AuthorizationCore",
                        principalTable: "PageGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageResourceActions",
                schema: "AuthorizationCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResourceActionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageResourceActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageResourceActions_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "AuthorizationCore",
                        principalTable: "Pages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageResourceActions_ResourceActions_ResourceActionId",
                        column: x => x.ResourceActionId,
                        principalSchema: "AuthorizationCore",
                        principalTable: "ResourceActions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageGroups_Code",
                schema: "AuthorizationCore",
                table: "PageGroups",
                column: "Code",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PageGroups_DeletedAt",
                schema: "AuthorizationCore",
                table: "PageGroups",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PageResourceActions_DeletedAt",
                schema: "AuthorizationCore",
                table: "PageResourceActions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PageResourceActions_PageId",
                schema: "AuthorizationCore",
                table: "PageResourceActions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageResourceActions_ResourceActionId",
                schema: "AuthorizationCore",
                table: "PageResourceActions",
                column: "ResourceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Code",
                schema: "AuthorizationCore",
                table: "Pages",
                column: "Code",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_DeletedAt",
                schema: "AuthorizationCore",
                table: "Pages",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_PageGroupId",
                schema: "AuthorizationCore",
                table: "Pages",
                column: "PageGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageResourceActions",
                schema: "AuthorizationCore");

            migrationBuilder.DropTable(
                name: "Pages",
                schema: "AuthorizationCore");

            migrationBuilder.DropTable(
                name: "PageGroups",
                schema: "AuthorizationCore");
        }
    }
}
