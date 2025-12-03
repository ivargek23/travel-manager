using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLUserLogin
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6)]

        public string Password { get; set; } = null!;
        public int Role { get; set; }
    }
}
