namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class MemberDietaryPlan
    {
        public int MemberID { get; set; }

        public int DietID { get; set; }

        public Member Member { get; set; } = null!;

        public DietaryPlan DietaryPlan { get; set; } = null!;
    }
}