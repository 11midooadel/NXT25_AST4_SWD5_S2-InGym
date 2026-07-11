using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public ICollection<UserPhone> UserPhones { get; set; } = new List<UserPhone>();

        public Member? Member { get; set; }

        public Coach? Coach { get; set; }

        public GymManager? GymManager { get; set; }
        public Admin? Admin { get; set; }
    }
}