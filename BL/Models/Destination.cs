using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Destination
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Travel> Travels { get; set; } = new List<Travel>();
}
