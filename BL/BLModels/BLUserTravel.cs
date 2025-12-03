using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLUserTravel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TravelId { get; set; }
        public string UserUsername { get; set; } = string.Empty;
        public string TravelName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
