using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "GameTransactions",
                columns: table => new
                {
                    GameTransactionsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fkFromUserId = table.Column<long>(type: "bigint", nullable: false),
                    fkToUserId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTransactions", x => x.GameTransactionsId);
                    table.ForeignKey(
                        name: "FK_GameTransactions_User_fkFromUserId",
                        column: x => x.fkFromUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTransactions_User_fkToUserId",
                        column: x => x.fkToUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchHistory",
                columns: table => new
                {
                    MatchHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fkPlayer1Id = table.Column<long>(type: "bigint", nullable: false),
                    fkPlayer2Id = table.Column<long>(type: "bigint", nullable: true),
                    Stake = table.Column<decimal>(type: "numeric", nullable: false),
                    fkWinnerId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchHistory", x => x.MatchHistoryId);
                    table.ForeignKey(
                        name: "FK_MatchHistory_User_fkPlayer1Id",
                        column: x => x.fkPlayer1Id,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchHistory_User_fkPlayer2Id",
                        column: x => x.fkPlayer2Id,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_MatchHistory_User_fkWinnerId",
                        column: x => x.fkWinnerId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTransactions_fkFromUserId",
                table: "GameTransactions",
                column: "fkFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTransactions_fkToUserId",
                table: "GameTransactions",
                column: "fkToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistory_fkPlayer1Id",
                table: "MatchHistory",
                column: "fkPlayer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistory_fkPlayer2Id",
                table: "MatchHistory",
                column: "fkPlayer2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistory_fkWinnerId",
                table: "MatchHistory",
                column: "fkWinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTransactions");

            migrationBuilder.DropTable(
                name: "MatchHistory");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
