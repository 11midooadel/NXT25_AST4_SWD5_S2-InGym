using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels
{
    public class MemberProfileViewModel
    {
        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public double Height { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public string Goals { get; set; } = string.Empty;

        public string PhysRestrictions { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;
    }
}