using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace walkeasyfinal.Models
{
    public class Shoes
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Shoes name is required")]
        [StringLength(100, ErrorMessage = "Shoes name cannot exceed 100 characters")]
        public string ShoesName { get; set; }

        // Image path saved in database (e.g., "images/flower.jpg")
        public string? Image { get; set; }

        // Image file uploaded from form (not stored in DB)
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 100000, ErrorMessage = "Amount must be between 1 and 100000")]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }
    }
}
