using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Attendance
    {
        [Key]
        public int AttendID { get; set; }

        public DateTime CheckInTime { get; set; }

        public int MemberID { get; set; }

        public Member Member { get; set; } = null!;
    }
}