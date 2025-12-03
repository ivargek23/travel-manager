using BL.BLModels;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface IDestinationRepository
    {
        public IEnumerable<BLDestination> GetDestinations();
        public BLDestination GetDestination(int id);
        public BLDestination AddDestination(BLDestination blDestination);
        public BLDestination UpdateDestination(int id, BLDestination blDestination);
        public Destination DeleteDestination(int id);
    }
}
