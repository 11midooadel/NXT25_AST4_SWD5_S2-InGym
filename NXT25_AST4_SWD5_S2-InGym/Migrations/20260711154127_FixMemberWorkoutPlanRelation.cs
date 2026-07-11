using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2InGym.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberWorkoutPlanRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberWorkoutPlans_WorkoutPlans_WorkoutPlanPlanID",
                table: "MemberWorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_MemberWorkoutPlans_WorkoutPlanPlanID",
                table: "MemberWorkoutPlans");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanPlanID",
                table: "MemberWorkoutPlans");

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkoutPlans_PlanID",
                table: "MemberWorkoutPlans",
                column: "PlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberWorkoutPlans_WorkoutPlans_PlanID",
                table: "MemberWorkoutPlans",
                column: "PlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "PlanID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberWorkoutPlans_WorkoutPlans_PlanID",
                table: "MemberWorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_MemberWorkoutPlans_PlanID",
                table: "MemberWorkoutPlans");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanPlanID",
                table: "MemberWorkoutPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkoutPlans_WorkoutPlanPlanID",
                table: "MemberWorkoutPlans",
                column: "WorkoutPlanPlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberWorkoutPlans_WorkoutPlans_WorkoutPlanPlanID",
                table: "MemberWorkoutPlans",
                column: "WorkoutPlanPlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "PlanID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
