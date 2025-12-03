using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class GuideDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public int Age { get; set; }
    }
}
