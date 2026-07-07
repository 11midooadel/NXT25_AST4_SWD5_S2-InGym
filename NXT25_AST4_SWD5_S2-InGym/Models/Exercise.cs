using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseID { get; set; }

        public string ExerciseName { get; set; } = string.Empty;

        public string MuscleGroup { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
    }
}