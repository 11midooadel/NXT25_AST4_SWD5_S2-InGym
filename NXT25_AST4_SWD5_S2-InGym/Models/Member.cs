using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; } = string.Empty;

        public double Height { get; set; }

        public double Weight { get; set; }

        public DateTime JoinDate { get; set; }

        public string Goals { get; set; } = string.Empty;

        public string PhysRestrictions { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Foreign Keys
        public int UserID { get; set; }

        public int CoachID { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;

        public Coach Coach { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<GymMember> GymMembers { get; set; } = new List<GymMember>();

        public ICollection<MemberHealthMetric> MemberHealthMetrics { get; set; } = new List<MemberHealthMetric>();

        public ICollection<MemberDietaryPlan> MemberDietaryPlans { get; set; } = new List<MemberDietaryPlan>();

        public ICollection<MemberWorkoutPlan> MemberWorkoutPlans { get; set; } = new List<MemberWorkoutPlan>();

        public ICollection<MemberGymSub> MemberGymSubs { get; set; } = new List<MemberGymSub>();

        public ICollection<MemberPrivateSub> MemberPrivateSubs { get; set; } = new List<MemberPrivateSub>();
    }
}