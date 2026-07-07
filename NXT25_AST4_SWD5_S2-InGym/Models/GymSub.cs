using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymSub
    {
        [Key]
        public int GymSubID { get; set; }

        public decimal Price { get; set; }

        public int DurationMonths { get; set; }

        public ICollection<MemberGymSub> MemberGymSubs { get; set; } = new List<MemberGymSub>();
    }
}