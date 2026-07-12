using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name is Required")]
        [StringLength(50, ErrorMessage = "Plan Name Must be less than 51 Characters")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Plan Name is Required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Description Must be between 5 and 50 Characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "DurationDays is Required")]
        [Range(1, 365, ErrorMessage = "Duration Days between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price between 0.1 and 10000")]
        public decimal Price { get; set; }
    }
}
