using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class GymLocation
    {
        [Key]
        public int LocationID { get; set; }

        public string Location { get; set; } = string.Empty;

        public int GymID { get; set; }

        public Gym Gym { get; set; } = null!;
    }
}