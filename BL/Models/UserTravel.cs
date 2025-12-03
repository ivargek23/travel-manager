using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class UserTravel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TravelId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Travel Travel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
