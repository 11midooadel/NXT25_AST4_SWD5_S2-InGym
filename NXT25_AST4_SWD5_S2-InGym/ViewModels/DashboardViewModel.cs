namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class DashboardViewModel
    {
        // ================= Member =================

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public double Height { get; set; }

        public double Weight { get; set; }

        public string Goals { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; }

        // ================= Gym =================

        public string GymName { get; set; } = string.Empty;

        public string GymLocation { get; set; } = string.Empty;

        // ================= Subscription =================

        public decimal SubscriptionPrice { get; set; }

        public int DurationMonths { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DaysRemaining { get; set; }

        public bool SubscriptionActive { get; set; }

        // ================= Coach =================

        public string CoachName { get; set; } = string.Empty;

        public string CoachEmail { get; set; } = string.Empty;

        public string CoachSpeciality { get; set; } = string.Empty;

        public double CoachRating { get; set; }
        // ================= Private Trainer =================

        public bool HasPrivateSubscription { get; set; }

        public string PrivateCoachName { get; set; } = string.Empty;

        public string PrivateCoachEmail { get; set; } = string.Empty;

        public string PrivateCoachSpeciality { get; set; } = string.Empty;

        public double PrivateCoachRating { get; set; }

        public decimal PrivatePackagePrice { get; set; }

        public int PrivateSessionCount { get; set; }

        public DateTime PrivateEndDate { get; set; }
    }
}