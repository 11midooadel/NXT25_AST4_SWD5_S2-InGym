namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class MemberWorkoutPlan
    {
        public int MemberID { get; set; }

        public int PlanID { get; set; }

        public Member Member { get; set; } = null!;

        public WorkoutPlan WorkoutPlan { get; set; } = null!;
    }
}