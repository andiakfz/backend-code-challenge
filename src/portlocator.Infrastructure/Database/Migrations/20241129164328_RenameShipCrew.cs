using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portlocator.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameShipCrew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ship_crews");

            migrationBuilder.CreateTable(
                name: "ship_assignments",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ship_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ship_assignments", x => new { x.user_id, x.ship_id });
                    table.ForeignKey(
                        name: "fk_ship_assignments_ships_ship_id",
                        column: x => x.ship_id,
                        principalTable: "ships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ship_assignments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ship_assignments_ship_id",
                table: "ship_assignments",
                column: "ship_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ship_assignments");

            migrationBuilder.CreateTable(
                name: "ship_crews",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ship_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ship_crews", x => new { x.user_id, x.ship_id });
                    table.ForeignKey(
                        name: "fk_ship_crews_ships_ship_id",
                        column: x => x.ship_id,
                        principalTable: "ships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ship_crews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ship_crews_ship_id",
                table: "ship_crews",
                column: "ship_id");
        }
    }
}
