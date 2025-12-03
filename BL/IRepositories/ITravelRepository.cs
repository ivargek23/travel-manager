using BL.BLModels;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface ITravelRepository
    {
        public IEnumerable<BLTravel> GetTravels();
        public BLTravel GetTravel(int id);
        public BLTravel InsertTravel(BLTravel blTravel);
        BLTravel UpdateTravel(int id, BLTravel blTravel);
        Travel DeleteTravel(int id);
        IEnumerable<BLTravel> SearchTravelsByName(BLSearch blSearch);
        public IEnumerable<BLUser> GetUsersForTravel(int travelId);
        public bool TravelWithNameExists(string name);
    }
}
