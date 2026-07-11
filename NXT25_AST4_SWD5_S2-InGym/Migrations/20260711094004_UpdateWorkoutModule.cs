using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2InGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkoutModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "WorkoutPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkoutPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "WorkoutPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Reps",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutDay",
                table: "WorkoutPlanExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "Reps",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "WorkoutPlanExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutDay",
                table: "WorkoutPlanExercises");
        }
    }
}
