using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trains_FSD.Models.Migrations
{
    public partial class LinesAddedToTrajectLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrajectLine_LineId",
                table: "TrajectLine",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrajectLine_Line_LineId",
                table: "TrajectLine",
                column: "LineId",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrajectLine_Line_LineId",
                table: "TrajectLine");

            migrationBuilder.DropIndex(
                name: "IX_TrajectLine_LineId",
                table: "TrajectLine");
        }
    }
}
