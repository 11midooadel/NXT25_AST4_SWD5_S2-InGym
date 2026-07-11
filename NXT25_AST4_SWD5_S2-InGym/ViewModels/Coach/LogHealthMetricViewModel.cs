using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Coach
{
    public class LogHealthMetricViewModel
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Weight is required")]
        [Range(0.1, 500, ErrorMessage = "Weight must be between 0.1 and 500 kg")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Body Fat Percentage is required")]
        [Range(0, 100, ErrorMessage = "Body Fat Percentage must be between 0 and 100")]
        public double BodyFatPercentage { get; set; }
    }

    public class HealthMetricViewModel
    {
        public int MetricID { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public double Weight { get; set; }

        public double BodyFatPercentage { get; set; }

        public DateTime LogDate { get; set; }

        public string FormattedDate => LogDate.ToString("MMM dd, yyyy HH:mm");

        public string FormattedWeight => Weight.ToString("F2") + " kg";

        public string FormattedBodyFatPercentage => BodyFatPercentage.ToString("F2") + "%";
    }
}
