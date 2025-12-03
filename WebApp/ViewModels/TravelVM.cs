using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class TravelVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        [Display(Name="Start date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name="End date")]
        public DateTime EndDate { get; set; }
        [Required]

        public decimal Price { get; set; }
        [Display(Name = "Image")]
        public int? ImageId { get; set; }
        
        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }
        [Display(Name = "Picture")]
        public string? ImageName { get; set; }
        [Display(Name = "Image")]
        public string? ImagePath { get; set; }
        [DisplayName("Destination")]
        public int DestinationId { get; set; }
        [DisplayName("Destination")]
        public string? DestinationName { get; set; }
        [DisplayName("Guides")]
        public List<int> GuideIds { get; set; }
        public List<int> UserIds { get; set; }
        public bool IsApplied { get; set; }

    }
}
