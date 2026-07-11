using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach
{
    public class CreateWorkoutPlanViewModel
    {
        public int MemberID { get; set; }

        [Required]
        public string Goal { get; set; } = string.Empty;

        public List<int> SelectedExercises { get; set; } = new();

        public List<SelectListItem> Exercises { get; set; } = new();
    }
}