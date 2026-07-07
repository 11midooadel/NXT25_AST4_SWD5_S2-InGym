using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymManager
    {
        [Key]
        public int ManagerID { get; set; }

        public decimal Salary { get; set; }

        public DateTime HireDate { get; set; }

        public int UserID { get; set; }

        public User User { get; set; } = null!;
    }
}