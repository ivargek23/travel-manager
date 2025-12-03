using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class TravelRepository : ITravelRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ILogsRepository _logsRepository;
        public TravelRepository(DatabaseContext context, IMapper mapper, ILogsRepository logsRepository)
        {
            _context = context;
            _mapper = mapper;
            _logsRepository = logsRepository;
        }
        public IEnumerable<BLTravel> GetTravels()
        {
            var dbTravels = _context
                .Travels
                .Include(x => x.Destination)
                .Include(x => x.Image)
                .Include(x => x.TravelGuides)
                .Include(x => x.UserTravels)
                .ToList();
            _logsRepository.Log(1, $"All travels retrieved");

            var blTravels = _mapper.Map<IEnumerable<BLTravel>>(dbTravels);
            foreach (var travel in blTravels)
            {
                if (travel.ImageId != null)
                {
                    travel.ImagePath = _context.Images.FirstOrDefault(x => x.Id == travel.ImageId)?.PicturePath;
                }
            }
            return blTravels;
        }

        public BLTravel GetTravel(int id)
        {
            var dbTravel = _context.Travels
                .Include(x => x.Destination)
                .Include(x => x.Image)
                .Include(x => x.TravelGuides)
                .Include(x => x.UserTravels)
                .FirstOrDefault(x => x.Id == id);

            _logsRepository.Log(1, $"Travel with id {id} retrieved");

            var blTravel = _mapper.Map<BLTravel>(dbTravel);
            if (blTravel.ImageId != null)
            {
                blTravel.ImagePath = _context.Images.FirstOrDefault(x => x.Id == blTravel.ImageId)?.PicturePath;
            }

            return blTravel;
        }

        public BLTravel InsertTravel(BLTravel blTravel)
        {
            var travel = _mapper.Map<Travel>(blTravel);

            _context.Travels.Add(travel);
            _logsRepository.Log(1, $"Travel with id {travel.Id} inserted");
            _context.SaveChanges();

            blTravel.Id = travel.Id;

            return blTravel;
        }

        public Travel DeleteTravel(int id)
        {
            var travel = _context.Travels.FirstOrDefault(x => x.Id == id);

            _context.RemoveRange(travel.TravelGuides);
            _context.RemoveRange(travel.UserTravels);
            _context.Travels.Remove(travel);
            _logsRepository.Log(1, $"Travel with id {id} deleted");
            _context.SaveChanges();
            return travel;
        }

        public BLTravel UpdateTravel(int id, BLTravel blTravel)
        {
            var travel = _context.Travels
                .Include(x => x.TravelGuides)
                .Include(x => x.UserTravels)
                .FirstOrDefault(x => x.Id == id);
            
            travel.Name = blTravel.Name;
            travel.Description = blTravel.Description;
            travel.StartDate = blTravel.StartDate;
            travel.EndDate = blTravel.EndDate;
            travel.Price = blTravel.Price;
            travel.ImageId = blTravel.ImageId;
            travel.DestinationId = blTravel.DestinationId;

            _context.RemoveRange(travel.TravelGuides);
            var travelGuides = blTravel.GuideIds?.Select(x => new TravelGuide { TravelId = id, GuideId = x });
            if (travelGuides != null)
            {
                _context.AddRange(travelGuides);
            }

            _logsRepository.Log(1, $"Travel with id {id} updated");
            _context.SaveChanges();

            return blTravel;
        }

        public IEnumerable<BLTravel> SearchTravelsByName(BLSearch blSearch)
        {
            var travels = _context.Travels
                .Include(x => x.Destination)
                .Include(x => x.Image)
                .Include(x => x.TravelGuides)
                .Include(x => x.UserTravels)
                .Where(x => x.Name.Contains(blSearch.Term));
            if (!string.IsNullOrEmpty(blSearch.OrderBy))
            {
                switch (blSearch.OrderBy.ToLower())
                {
                    case "name":
                        travels = travels.OrderBy(x => x.Name);
                        break;
                    case "price":
                        travels = travels.OrderBy(x => x.Price);
                        break;
                    case "start date":
                        travels = travels.OrderBy(x => x.StartDate);
                        break;
                    case "end date":
                        travels = travels.OrderBy(x => x.EndDate);
                        break;
                    case "destination name":
                        travels = travels.OrderBy(x => x.Destination.Name);
                        break;
                }
            }
            travels = travels.Skip((blSearch.Page - 1) * blSearch.Size).Take(blSearch.Size);
            _logsRepository.Log(1, $"Travels with name [{blSearch.Term}] retrieved");
            var blTravels = _mapper.Map<IEnumerable<BLTravel>>(travels);
            return blTravels;
        }

        public IEnumerable<BLUser> GetUsersForTravel(int travelId)
        {
            var listOfUsersForTravel = _context.UserTravels
                .Where(x => x.TravelId == travelId)
                .Select(x => x.UserId)
                .ToList();
            var dbUsers = _context.Users
                .Where(x => listOfUsersForTravel.Contains(x.Id))
                .ToList();
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);
            return blUsers;
        }

        public bool TravelWithNameExists(string name)
        {
            var travel = _context.Travels.FirstOrDefault(x => x.Name == name);
            if (travel == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
