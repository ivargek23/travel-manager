using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class DestinationVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter destination name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Country name is required to create new destination.")]

        public string Country { get; set; } = null!;
        [StringLength(500)]
        [Required]
        public string? Description { get; set; }
    }
}
