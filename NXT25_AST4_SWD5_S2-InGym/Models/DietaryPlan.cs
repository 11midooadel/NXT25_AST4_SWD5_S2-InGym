using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class DietaryPlan
    {
        [Key]
        public int DietID { get; set; }

        public string Goal { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Calories { get; set; }

        public int Protein { get; set; }

        public int Carbs { get; set; }

        public int Fats { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int CalorieTarget { get; set; }

        public int ProteinTarget { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ICollection<MemberDietaryPlan> MemberDietaryPlans { get; set; } = new List<MemberDietaryPlan>();
    }
}