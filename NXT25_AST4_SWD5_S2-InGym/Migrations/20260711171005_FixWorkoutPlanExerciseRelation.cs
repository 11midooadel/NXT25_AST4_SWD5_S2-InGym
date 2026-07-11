using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2InGym.Migrations
{
    /// <inheritdoc />
    public partial class FixWorkoutPlanExerciseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanPlanID",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlanExercises_WorkoutPlanPlanID",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanPlanID",
                table: "WorkoutPlanExercises");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_PlanID",
                table: "WorkoutPlanExercises",
                column: "PlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "PlanID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_PlanID",
                table: "WorkoutPlanExercises");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanPlanID",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanExercises_WorkoutPlanPlanID",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanPlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanPlanID",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanPlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "PlanID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
