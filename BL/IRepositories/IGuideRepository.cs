using BL.BLModels;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface IGuideRepository
    {
        public IEnumerable<BLGuide> GetGuides();
        public BLGuide GetGuide(int id);
        public BLGuide AddGuide(BLGuide blGuide);
        public BLGuide UpdateGuide(int id, BLGuide blGuide);
        public Guide DeleteGuide(int id);
        public IEnumerable<BLGuide> GetGuidesByTravelId(int travelId);
    }
}
