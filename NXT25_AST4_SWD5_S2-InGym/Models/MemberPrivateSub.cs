namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class MemberPrivateSub
    {
        public int MemberID { get; set; }

        public int PrivateSubID { get; set; }

        public int CoachID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public Member Member { get; set; } = null!;

        public PrivateSub PrivateSub { get; set; } = null!;

        public Coach Coach { get; set; } = null!;
    }
}