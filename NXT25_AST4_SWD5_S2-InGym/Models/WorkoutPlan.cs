using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class WorkoutPlan
    {
        [Key]
        public int PlanID { get; set; }

        public string Goal { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MemberWorkoutPlan> MemberWorkoutPlans { get; set; } = new List<MemberWorkoutPlan>();

        public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
    }
}