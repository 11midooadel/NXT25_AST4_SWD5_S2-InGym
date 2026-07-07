using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2_InGym.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietaryPlans",
                columns: table => new
                {
                    DietID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalorieTarget = table.Column<int>(type: "int", nullable: false),
                    ProteinTarget = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryPlans", x => x.DietID);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseID);
                });

            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    GymID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.GymID);
                });

            migrationBuilder.CreateTable(
                name: "GymSubs",
                columns: table => new
                {
                    GymSubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymSubs", x => x.GymSubID);
                });

            migrationBuilder.CreateTable(
                name: "HealthMetricLogs",
                columns: table => new
                {
                    MetricID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    BodyFatPercentage = table.Column<double>(type: "float", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthMetricLogs", x => x.MetricID);
                });

            migrationBuilder.CreateTable(
                name: "PrivateSubs",
                columns: table => new
                {
                    PrivateSubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SessionCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateSubs", x => x.PrivateSubID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Goal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "GymLocations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GymID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymLocations", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK_GymLocations_Gyms_GymID",
                        column: x => x.GymID,
                        principalTable: "Gyms",
                        principalColumn: "GymID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    CoachID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.CoachID);
                    table.ForeignKey(
                        name: "FK_Coaches_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymManagers",
                columns: table => new
                {
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymManagers", x => x.ManagerID);
                    table.ForeignKey(
                        name: "FK_GymManagers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPhones",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhones", x => new { x.UserID, x.Phone });
                    table.ForeignKey(
                        name: "FK_UserPhones_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlanExercises",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    ExerciseID = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanPlanID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlanExercises", x => new { x.PlanID, x.ExerciseID });
                    table.ForeignKey(
                        name: "FK_WorkoutPlanExercises_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutPlanExercises_WorkoutPlans_WorkoutPlanPlanID",
                        column: x => x.WorkoutPlanPlanID,
                        principalTable: "WorkoutPlans",
                        principalColumn: "PlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymCoaches",
                columns: table => new
                {
                    GymID = table.Column<int>(type: "int", nullable: false),
                    CoachID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymCoaches", x => new { x.GymID, x.CoachID });
                    table.ForeignKey(
                        name: "FK_GymCoaches_Coaches_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coaches",
                        principalColumn: "CoachID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GymCoaches_Gyms_GymID",
                        column: x => x.GymID,
                        principalTable: "Gyms",
                        principalColumn: "GymID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Goals = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysRestrictions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChronicDiseases = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CoachID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_Coaches_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coaches",
                        principalColumn: "CoachID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendID);
                    table.ForeignKey(
                        name: "FK_Attendances_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymMembers",
                columns: table => new
                {
                    GymID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymMembers", x => new { x.GymID, x.MemberID });
                    table.ForeignKey(
                        name: "FK_GymMembers_Gyms_GymID",
                        column: x => x.GymID,
                        principalTable: "Gyms",
                        principalColumn: "GymID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GymMembers_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberDietaryPlans",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    DietID = table.Column<int>(type: "int", nullable: false),
                    DietaryPlanDietID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberDietaryPlans", x => new { x.MemberID, x.DietID });
                    table.ForeignKey(
                        name: "FK_MemberDietaryPlans_DietaryPlans_DietaryPlanDietID",
                        column: x => x.DietaryPlanDietID,
                        principalTable: "DietaryPlans",
                        principalColumn: "DietID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberDietaryPlans_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberGymSubs",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    GymSubID = table.Column<int>(type: "int", nullable: false),
                    GymID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberGymSubs", x => new { x.MemberID, x.GymSubID });
                    table.ForeignKey(
                        name: "FK_MemberGymSubs_GymSubs_GymSubID",
                        column: x => x.GymSubID,
                        principalTable: "GymSubs",
                        principalColumn: "GymSubID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberGymSubs_Gyms_GymID",
                        column: x => x.GymID,
                        principalTable: "Gyms",
                        principalColumn: "GymID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberGymSubs_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberHealthMetrics",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    MetricID = table.Column<int>(type: "int", nullable: false),
                    HealthMetricLogMetricID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberHealthMetrics", x => new { x.MemberID, x.MetricID });
                    table.ForeignKey(
                        name: "FK_MemberHealthMetrics_HealthMetricLogs_HealthMetricLogMetricID",
                        column: x => x.HealthMetricLogMetricID,
                        principalTable: "HealthMetricLogs",
                        principalColumn: "MetricID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberHealthMetrics_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberPrivateSubs",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PrivateSubID = table.Column<int>(type: "int", nullable: false),
                    CoachID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPrivateSubs", x => new { x.MemberID, x.PrivateSubID });
                    table.ForeignKey(
                        name: "FK_MemberPrivateSubs_Coaches_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coaches",
                        principalColumn: "CoachID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberPrivateSubs_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberPrivateSubs_PrivateSubs_PrivateSubID",
                        column: x => x.PrivateSubID,
                        principalTable: "PrivateSubs",
                        principalColumn: "PrivateSubID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberWorkoutPlans",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanPlanID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberWorkoutPlans", x => new { x.MemberID, x.PlanID });
                    table.ForeignKey(
                        name: "FK_MemberWorkoutPlans_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberWorkoutPlans_WorkoutPlans_WorkoutPlanPlanID",
                        column: x => x.WorkoutPlanPlanID,
                        principalTable: "WorkoutPlans",
                        principalColumn: "PlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_MemberID",
                table: "Attendances",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UserID",
                table: "Coaches",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GymCoaches_CoachID",
                table: "GymCoaches",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_GymLocations_GymID",
                table: "GymLocations",
                column: "GymID");

            migrationBuilder.CreateIndex(
                name: "IX_GymManagers_UserID",
                table: "GymManagers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GymMembers_MemberID",
                table: "GymMembers",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberDietaryPlans_DietaryPlanDietID",
                table: "MemberDietaryPlans",
                column: "DietaryPlanDietID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGymSubs_GymID",
                table: "MemberGymSubs",
                column: "GymID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGymSubs_GymSubID",
                table: "MemberGymSubs",
                column: "GymSubID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberHealthMetrics_HealthMetricLogMetricID",
                table: "MemberHealthMetrics",
                column: "HealthMetricLogMetricID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPrivateSubs_CoachID",
                table: "MemberPrivateSubs",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPrivateSubs_PrivateSubID",
                table: "MemberPrivateSubs",
                column: "PrivateSubID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CoachID",
                table: "Members",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserID",
                table: "Members",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberWorkoutPlans_WorkoutPlanPlanID",
                table: "MemberWorkoutPlans",
                column: "WorkoutPlanPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanExercises_ExerciseID",
                table: "WorkoutPlanExercises",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanExercises_WorkoutPlanPlanID",
                table: "WorkoutPlanExercises",
                column: "WorkoutPlanPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "GymCoaches");

            migrationBuilder.DropTable(
                name: "GymLocations");

            migrationBuilder.DropTable(
                name: "GymManagers");

            migrationBuilder.DropTable(
                name: "GymMembers");

            migrationBuilder.DropTable(
                name: "MemberDietaryPlans");

            migrationBuilder.DropTable(
                name: "MemberGymSubs");

            migrationBuilder.DropTable(
                name: "MemberHealthMetrics");

            migrationBuilder.DropTable(
                name: "MemberPrivateSubs");

            migrationBuilder.DropTable(
                name: "MemberWorkoutPlans");

            migrationBuilder.DropTable(
                name: "UserPhones");

            migrationBuilder.DropTable(
                name: "WorkoutPlanExercises");

            migrationBuilder.DropTable(
                name: "DietaryPlans");

            migrationBuilder.DropTable(
                name: "GymSubs");

            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "HealthMetricLogs");

            migrationBuilder.DropTable(
                name: "PrivateSubs");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
