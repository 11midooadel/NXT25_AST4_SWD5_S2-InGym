using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be between 5 and 100 Characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = " Phone Number Must be Egyptian Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, 1000, ErrorMessage = "Buiding Number must be between 1 and 1000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be between 2 and 30 Characters")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Street is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain only Letters and spaces")]
        public string City { get; set; } = null!;

        public string? Photo { get; set; }
    }
}
