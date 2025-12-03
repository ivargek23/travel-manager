using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Travel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public int? ImageId { get; set; }

    public int DestinationId { get; set; }

    public virtual Destination Destination { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual ICollection<TravelGuide> TravelGuides { get; set; } = new List<TravelGuide>();

    public virtual ICollection<UserTravel> UserTravels { get; set; } = new List<UserTravel>();
}
