using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfermarkt.Web.Migrations
{
    public partial class AddMatchIdCornerFoul : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Fouls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Corners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fouls_MatchId",
                table: "Fouls",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Corners_MatchId",
                table: "Corners",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Corners_Matches_MatchId",
                table: "Corners",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Fouls_Matches_MatchId",
                table: "Fouls",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corners_Matches_MatchId",
                table: "Corners");

            migrationBuilder.DropForeignKey(
                name: "FK_Fouls_Matches_MatchId",
                table: "Fouls");

            migrationBuilder.DropIndex(
                name: "IX_Fouls_MatchId",
                table: "Fouls");

            migrationBuilder.DropIndex(
                name: "IX_Corners_MatchId",
                table: "Corners");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Fouls");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Corners");
        }
    }
}
