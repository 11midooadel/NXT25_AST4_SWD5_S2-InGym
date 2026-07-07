namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class WorkoutPlanExercise
    {
        public int PlanID { get; set; }

        public int ExerciseID { get; set; }

        public WorkoutPlan WorkoutPlan { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
    }
}