using Microsoft.AspNetCore.Mvc.Rendering;
using NXT25_AST4_SWD5_S2_InGym.Models;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach
{
    public class EditExerciseViewModel
    {
        public int PlanID { get; set; }

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