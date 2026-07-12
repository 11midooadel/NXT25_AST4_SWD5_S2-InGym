using System.ComponentModel.DataAnnotations;

namespace InGym.Models
{
    public class Equipment
    {
        [Key]
        [StringLength(10)]
        public string Shelf_Code { get; set; }

        [Required(ErrorMessage = "Equipment name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0, 10000, ErrorMessage = "Quantity must be between 0 and 10000")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
