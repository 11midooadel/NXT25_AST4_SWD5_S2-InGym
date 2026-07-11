using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class HealthMetricLog
    {
        [Key]
        public int MetricID { get; set; }

        [ForeignKey("Member")]
        public int MemberID { get; set; }

        public double Weight { get; set; }

        public double BodyFat { get; set; }

        public double BMI { get; set; }

        public double MuscleMass { get; set; }

        public double BodyFatPercentage { get; set; }

        public DateTime LogDate { get; set; }

        public string Notes { get; set; } = string.Empty;

        public Member? Member { get; set; }

        public ICollection<MemberHealthMetric> MemberHealthMetrics { get; set; } = new List<MemberHealthMetric>();
    }
}