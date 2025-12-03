using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class GuideRepository : IGuideRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public GuideRepository(DatabaseContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<BLGuide> GetGuides()
        {
            var guides = _context.Guides.ToList();
            return _mapper.Map<IEnumerable<BLGuide>>(guides);
        }
        public BLGuide AddGuide(BLGuide blGuide)
        {
            var guide = _mapper.Map<Guide>(blGuide);
            _context.Guides.Add(guide);
            _context.SaveChanges();
            blGuide.Id = guide.Id;
            return blGuide;
        }

        public Guide DeleteGuide(int id)
        {
            var guide = _context.Guides.Find(id);
            if (guide == null)
            {
                return null;
            }
            _context.Guides.Remove(guide);
            _context.SaveChanges();
            return guide;
        }

        public BLGuide GetGuide(int id)
        {
            var guide = _context.Guides.FirstOrDefault(g => g.Id == id);
            return _mapper.Map<BLGuide>(guide);
        }


        public BLGuide UpdateGuide(int id, BLGuide blGuide)
        {
            var guide = _context.Guides.FirstOrDefault(g => g.Id == id);
            if (guide == null)
            {
                return null;
            }
            guide.Name = blGuide.Name;
            guide.Age = blGuide.Age;
            _context.SaveChanges();
            return blGuide;
        }

        public IEnumerable<BLGuide> GetGuidesByTravelId(int travelId)
        {
            var guidesIds = _context.TravelGuides
                .Where(x => x.TravelId == travelId);
            var guides = _context.Guides
                .Where(x => guidesIds.Select(x => x.GuideId).Contains(x.Id))
                .ToList();
            var guideVms = _mapper.Map<IEnumerable<BLGuide>>(guides);
            return guideVms;
        }
    }
}
