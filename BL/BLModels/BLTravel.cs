using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLTravel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [Required]

        public decimal Price { get; set; }
        public int? ImageId { get; set; }
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }
        public int DestinationId { get; set; }
        [DisplayName("Destination")]
        public string? DestinationName { get; set; }
        [DisplayName("Image")]
        public string? ImageName { get; set; }
        public List<int>? GuideIds { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
