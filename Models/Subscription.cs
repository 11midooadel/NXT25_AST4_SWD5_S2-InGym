using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InGym.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string PlanName { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
