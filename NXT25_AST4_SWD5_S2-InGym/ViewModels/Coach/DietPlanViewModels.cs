using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach
{
    public class CreateDietPlanViewModel
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Calorie target is required")]
        [Range(500, 10000, ErrorMessage = "Calorie target must be between 500 and 10,000")]
        public int CalorieTarget { get; set; }

        [Required(ErrorMessage = "Protein target is required")]
        [Range(0, 500, ErrorMessage = "Protein target must be between 0 and 500g")]
        public int ProteinTarget { get; set; }

        public string Notes { get; set; } = string.Empty;
    }

    public class EditDietPlanViewModel
    {
        public int DietId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Calorie target is required")]
        [Range(500, 10000, ErrorMessage = "Calorie target must be between 500 and 10,000")]
        public int CalorieTarget { get; set; }

        [Required(ErrorMessage = "Protein target is required")]
        [Range(0, 500, ErrorMessage = "Protein target must be between 0 and 500g")]
        public int ProteinTarget { get; set; }

        public string Notes { get; set; } = string.Empty;
    }

    public class DietPlanViewModel
    {
        public int DietID { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public int CalorieTarget { get; set; }

        public int ProteinTarget { get; set; }

        public string Notes { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public string FormattedDate => CreationDate.ToString("MMM dd, yyyy");
    }
}
