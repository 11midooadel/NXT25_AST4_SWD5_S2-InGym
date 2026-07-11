using NXT25_AST4_SWD5_S2_InGym.Models;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class AttendanceViewModel
    {
        public bool CheckedIn { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public List<Attendance> AttendanceHistory { get; set; } = new();
    }
}