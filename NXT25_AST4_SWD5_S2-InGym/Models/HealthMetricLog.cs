using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class HealthMetricLog
    {
        [Key]
        public int MetricID { get; set; }

        public double Weight { get; set; }

        public double BodyFatPercentage { get; set; }

        public DateTime LogDate { get; set; }

        public ICollection<MemberHealthMetric> MemberHealthMetrics { get; set; } = new List<MemberHealthMetric>();
    }
}