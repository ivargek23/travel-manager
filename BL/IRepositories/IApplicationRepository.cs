using BL.BLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface IApplicationRepository
    {
        public BLUserTravel ApplyForTravel(int id, int userId);
        public BLUserTravel CancelApplication(int id);
        public IEnumerable<BLUserTravel> GetUserTravels(int userId);
        public IEnumerable<BLUserTravel> GetAllUserTravels();
        public bool CheckIfUserApplied(int travelId, int userId);
    }
}
