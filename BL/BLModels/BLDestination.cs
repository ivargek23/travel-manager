using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLDestination
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public string Country { get; set; } = null!;
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
