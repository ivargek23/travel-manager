using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Image
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string PicturePath { get; set; } = null!;

    public string PictureName { get; set; } = null!;

    public virtual ICollection<Travel> Travels { get; set; } = new List<Travel>();
}
