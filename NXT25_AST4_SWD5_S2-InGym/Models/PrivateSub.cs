using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class PrivateSub
    {
        [Key]
        public int PrivateSubID { get; set; }

        public decimal Price { get; set; }

        public int SessionCount { get; set; }

        public ICollection<MemberPrivateSub> MemberPrivateSubs { get; set; } = new List<MemberPrivateSub>();
    }
}