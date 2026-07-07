namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class MemberGymSub
    {
        public int MemberID { get; set; }

        public int GymSubID { get; set; }

        public int GymID { get; set; }

        public Member Member { get; set; } = null!;

        public GymSub GymSub { get; set; } = null!;

        public Gym Gym { get; set; } = null!;
    }
}