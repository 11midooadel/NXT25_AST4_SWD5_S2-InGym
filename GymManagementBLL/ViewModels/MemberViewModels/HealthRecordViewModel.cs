using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is Required")]
        [Range(0.1, 300, ErrorMessage = "Height Must be between 0.1 and 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is Required")]
        [Range(0.1, 500, ErrorMessage = "Weight Must be between 0.1 and 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is Required")]
        [StringLength(3, ErrorMessage = "Blood Type Must be 3 Characters or less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
