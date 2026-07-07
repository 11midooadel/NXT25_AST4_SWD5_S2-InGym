using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class DietaryPlan
    {
        [Key]
        public int DietID { get; set; }

        public int CalorieTarget { get; set; }

        public int ProteinTarget { get; set; }

        public DateTime CreationDate { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ICollection<MemberDietaryPlan> MemberDietaryPlans { get; set; } = new List<MemberDietaryPlan>();
    }
}