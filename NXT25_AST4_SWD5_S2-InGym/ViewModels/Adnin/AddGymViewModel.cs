using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Admin
{
    public class AddGymViewModel
    {
        [Required]
        public string GymName { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}