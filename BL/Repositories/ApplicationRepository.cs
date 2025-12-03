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
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public ApplicationRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public BLUserTravel ApplyForTravel(int id, int userId)
        {
            var existingUserTravel = _context.UserTravels.FirstOrDefault(ut => ut.TravelId == id && ut.UserId == userId);
            BLUserTravel userTravelBl;

            if (existingUserTravel != null && existingUserTravel.DeletedAt != null)
            {
                existingUserTravel.DeletedAt = null;
                _context.UserTravels.Update(existingUserTravel);
                _context.SaveChanges();
                userTravelBl = _mapper.Map<BLUserTravel>(existingUserTravel);
                return userTravelBl;
            }
            else
            {
                var userTravel = new UserTravel
                {
                    TravelId = id,
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };
                _context.UserTravels.Add(userTravel);
                _context.SaveChanges();
                userTravelBl = _mapper.Map<BLUserTravel>(userTravel);
                return userTravelBl;
            }
        }

        public BLUserTravel CancelApplication(int id)
        {
            var userTravel = _context.UserTravels.FirstOrDefault(x => x.Id == id);
            userTravel.DeletedAt = DateTime.Now;

            var userTravelBl = _mapper.Map<BLUserTravel>(userTravel);
            _context.UserTravels.Update(userTravel);
            _context.SaveChanges();
            return userTravelBl;
        }

        public bool CheckIfUserApplied(int travelId, int userId)
        {
            var travel = _context.UserTravels.FirstOrDefault(ut => ut.TravelId == travelId && ut.UserId == userId && ut.DeletedAt == null);
            if (travel != null)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<BLUserTravel> GetAllUserTravels()
        {
            var userTravels = _context.UserTravels
                .Include(ut => ut.Travel)
                .Include(ut => ut.User)
                .ToList();
            var userTravelsBl = _mapper.Map<IEnumerable<BLUserTravel>>(userTravels);
            return userTravelsBl;
        }

        public IEnumerable<BLUserTravel> GetUserTravels(int userId)
        {
            var userTravels = _context.UserTravels
                .Include(ut => ut.Travel)
                .Where(ut => ut.UserId == userId && ut.DeletedAt == null)
                .ToList();
            var userTravelsBl = _mapper.Map<IEnumerable<BLUserTravel>>(userTravels);
            return userTravelsBl;
        }

    }
}
