namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymCoach
    {
        public int GymID { get; set; }

        public int CoachID { get; set; }

        public Gym Gym { get; set; } = null!;

        public Coach Coach { get; set; } = null!;
    }
}