using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Guide
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public virtual ICollection<TravelGuide> TravelGuides { get; set; } = new List<TravelGuide>();
}
