using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NXT25_AST4_SWD5_S2_InGym.ViewModels.Admin
{
    public class AddCoachViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string Speciality { get; set; } = string.Empty;

        [Required]
        public double Rating { get; set; }

        [Required]
        public DateTime HireDate { get; set; } = DateTime.Today;

        [Required]
        public int GymID { get; set; }

        public List<SelectListItem> Gyms { get; set; } = new();
    }
}