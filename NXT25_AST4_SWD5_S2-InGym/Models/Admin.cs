using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserID { get; set; }

        public User User { get; set; } = null!;
    }
}