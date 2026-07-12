using System.ComponentModel.DataAnnotations;

namespace InGym.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }

        public DateTime DateJoined { get; set; }
    }
}
