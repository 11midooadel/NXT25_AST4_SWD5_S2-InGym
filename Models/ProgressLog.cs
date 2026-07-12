using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InGym.Models
{
    public class ProgressLog
    {
        public int Id { get; set; }

        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public User Trainee { get; set; }

        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public User Trainer { get; set; }

        [Required]
        public DateTime DateLogged { get; set; }

        [Range(0.1, 500, ErrorMessage = "Weight must be between 0.1 and 500 kg")]
        public decimal? Weight { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
