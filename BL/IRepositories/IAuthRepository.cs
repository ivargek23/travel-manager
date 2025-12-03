using BL.BLModels;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepositories
{
    public interface IAuthRepository
    {
        public BLUser Register(BLUser blUser);
        public string Login(BLUserLogin blUserLogin);
        public BLUserLogin ChangePassword(BLUserLogin blUserLogin);
        public BLUser UpdateUser(BLUser blUser);
        public BLUserLogin CheckIfUserExists(BLUserLogin blUserLogin);
        public BLUserLogin CheckUserCredentials(BLUserLogin blUserLogin);
        public string CheckUserRole(BLUserLogin blUserLogin);
        public BLUser GetUserByUsername(string username);
        public BLUser GetUserById(int id);
    }
}
