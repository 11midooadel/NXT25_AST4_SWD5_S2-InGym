namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymMember
    {
        //in class GymMember, we have two properties: GymID and MemberID, which are both of type int. These properties represent the foreign keys that link the GymMember entity to the Gym and Member entities, respectively.
        public int GymID { get; set; }

        public int MemberID { get; set; }

        public Gym Gym { get; set; } = null!;

        public Member Member { get; set; } = null!;
    }
}