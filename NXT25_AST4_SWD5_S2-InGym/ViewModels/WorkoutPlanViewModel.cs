using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class WorkoutPlanViewModel
    {
        public int PlanID { get; set; }

        [Required]
        public string Goal { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}