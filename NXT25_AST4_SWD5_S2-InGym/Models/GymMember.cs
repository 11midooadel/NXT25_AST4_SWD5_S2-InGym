namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymMember
    {
        public int GymID { get; set; }

        public int MemberID { get; set; }

        public Gym Gym { get; set; } = null!;

        public Member Member { get; set; } = null!;
    }
}