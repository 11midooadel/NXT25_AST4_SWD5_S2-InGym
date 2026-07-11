using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2InGym.Migrations
{
    /// <inheritdoc />
    public partial class AddGymManagerGymRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GymID",
                table: "GymManagers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GymManagers_GymID",
                table: "GymManagers",
                column: "GymID");

            migrationBuilder.AddForeignKey(
                name: "FK_GymManagers_Gyms_GymID",
                table: "GymManagers",
                column: "GymID",
                principalTable: "Gyms",
                principalColumn: "GymID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymManagers_Gyms_GymID",
                table: "GymManagers");

            migrationBuilder.DropIndex(
                name: "IX_GymManagers_GymID",
                table: "GymManagers");

            migrationBuilder.DropColumn(
                name: "GymID",
                table: "GymManagers");
        }
    }
}
