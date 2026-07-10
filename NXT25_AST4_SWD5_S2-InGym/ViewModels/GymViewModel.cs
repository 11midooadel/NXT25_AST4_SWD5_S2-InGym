using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class GymViewModel
    {
        [Required]
        public string GymName { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}