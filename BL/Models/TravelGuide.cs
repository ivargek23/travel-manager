using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class TravelGuide
{
    public int Id { get; set; }

    public int TravelId { get; set; }

    public int GuideId { get; set; }

    public virtual Guide Guide { get; set; } = null!;

    public virtual Travel Travel { get; set; } = null!;
}
