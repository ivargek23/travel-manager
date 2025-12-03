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
    public class DestinationRepository : IDestinationRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public DestinationRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<BLDestination> GetDestinations()
        {
            var dbDestinations = _context.Destinations.ToList();

            var blDestinations = _mapper.Map<IEnumerable<BLDestination>>(dbDestinations);
            return blDestinations;
        }
        public BLDestination GetDestination(int id)
        {
            var dbDestination = _context.Destinations.FirstOrDefault(x => x.Id == id);

            var blDestination = _mapper.Map<BLDestination>(dbDestination);
            return blDestination;
        }

        public BLDestination AddDestination(BLDestination blDestination)
        {
            var destination = _mapper.Map<Destination>(blDestination);
            _context.Destinations.Add(destination);
            _context.SaveChanges();

            blDestination.Id = destination.Id;
            return blDestination;
        }

        public Destination DeleteDestination(int id)
        {
            var destination = _context.Destinations.FirstOrDefault(x => x.Id == id);
            if (destination == null)
            {
                return null;
            }
            _context.Destinations.Remove(destination);
            _context.SaveChanges();
            return destination;
        }

        public BLDestination UpdateDestination(int id, BLDestination blDestination)
        {
            var destination = _context.Destinations.FirstOrDefault(x => x.Id == id);
            if (destination == null)
            {
                return null;
            }
            destination.Name = blDestination.Name;
            destination.Country = blDestination.Country;
            destination.Description = blDestination.Description;
            destination.Id = id;
            _context.SaveChanges();
            blDestination.Id = id;
            return blDestination;
        }
    }
}
