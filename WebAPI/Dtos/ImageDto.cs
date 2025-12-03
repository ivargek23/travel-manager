using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string PicturePath { get; set; } = null!;

        public string PictureName { get; set; } = null!;
    }
}
