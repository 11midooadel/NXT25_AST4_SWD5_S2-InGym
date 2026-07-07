namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class MemberHealthMetric
    {
        public int MemberID { get; set; }

        public int MetricID { get; set; }

        public Member Member { get; set; } = null!;

        public HealthMetricLog HealthMetricLog { get; set; } = null!;
    }
}