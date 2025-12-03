using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLModels
{
    public class BLSearch
    {
        public string Term { get; set; }
        public string OrderBy { get; set; } = "";
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int LastPage { get; set; }
        public int FromPager { get; set; }
        public int ToPager { get; set; }
        public string Submit { get; set; }
        public IEnumerable<BLTravel> Travels { get; set; }
    }
}
