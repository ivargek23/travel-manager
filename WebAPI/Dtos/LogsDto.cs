using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Dtos
{
    public class LogsDto
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int LogLevel { get; set; }

        public string? Message { get; set; }
    }
}
