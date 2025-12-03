using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Log
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int LogLevel { get; set; }

    public string? Message { get; set; }
}
