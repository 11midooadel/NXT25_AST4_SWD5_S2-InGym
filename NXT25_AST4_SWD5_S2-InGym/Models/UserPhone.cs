namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class UserPhone
    {
        public int UserID { get; set; }

        public string Phone { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}