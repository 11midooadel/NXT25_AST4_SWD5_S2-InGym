namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class DashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public double Height { get; set; }

        public double Weight { get; set; }

        public string Goals { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; }

        public string GymName { get; set; } = string.Empty;

        public string GymLocation { get; set; } = string.Empty;
    }
}