using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class TravelDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [Required]

        public decimal Price { get; set; }
        public int? ImageId { get; set; }
        public int DestinationId { get; set; }
        [DisplayName("Destination")]
        public string? DestinationName { get; set; }
        public List<int>? GuideIds { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
