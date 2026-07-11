using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach
{
    public class AddExerciseToPlanViewModel
    {
        public int PlanID { get; set; }

        [Required]
        public int ExerciseID { get; set; }

        [Required]
        public DayOfWeek WorkoutDay { get; set; }

        [Required]
        public int Sets { get; set; }

        [Required]
        public int Reps { get; set; }

        public List<SelectListItem> Exercises { get; set; } = new();
    }
}