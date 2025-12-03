using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using BL.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthRepository(DatabaseContext context, IConfiguration configuration, IMapper mapper) {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
        public BLUserLogin ChangePassword(BLUserLogin blUserLogin)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username.Equals(blUserLogin.Username));
            if (user == null)
                throw new Exception($"User {blUserLogin.Username} not found");
            user.PwdSalt = PasswordHashProvider.GetSalt();
            user.PwdHash = PasswordHashProvider.GetHash(blUserLogin.Password, user.PwdSalt);
            _context.Users.Update(user);
            _context.SaveChanges();
            return blUserLogin;
        }

        public BLUserLogin CheckIfUserExists(BLUserLogin blUserLogin)
        {
            var username = blUserLogin.Username;
            var user = _context.Users.FirstOrDefault(x => x.Username.Equals(username));
            if (user == null)
            {
                return null;
            }
            blUserLogin.Id = user.Id;
            return blUserLogin;
        }

        public BLUserLogin CheckUserCredentials(BLUserLogin blUserLogin)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Username == blUserLogin.Username);
            var b64hash = PasswordHashProvider.GetHash(blUserLogin.Password, existingUser.PwdSalt);
            if (b64hash != existingUser.PwdHash)
                return null;
            blUserLogin.Id = existingUser.Id;

            return blUserLogin;
        }

        public string CheckUserRole(BLUserLogin blUserLogin)
        {
            var user = _context.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Username == blUserLogin.Username);
            
            if (user.Role.Name == "Admin")
            {
                return "Admin";
            }
            else if (user.Role.Name == "User")
            {
                return "User";
            }
            return "User";
        }

        public BLUser GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            var blUser = _mapper.Map<BLUser>(user);
            return blUser;
        }

        public BLUser GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == username);
            var blUser = _mapper.Map<BLUser>(user);
            blUser.Id = user.Id;
            return blUser;
        }

        public string Login(BLUserLogin userDto)
        {
            var secureKey = _configuration["JWT:SecureKey"];
            var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, userDto.Username);
            return serializedToken;
        }

        public BLUser Register(BLUser blUser)
        {
            var trimmedUsername = blUser.Username.Trim();
            if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                throw new Exception($"Username {trimmedUsername} already exists");

            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(blUser.Password, b64salt);

            var user = new User
            {
                Username = blUser.Username,
                PwdHash = b64hash,
                PwdSalt = b64salt,
                FirstName = blUser.FirstName,
                LastName = blUser.LastName,
                Email = blUser.Email,
                RoleId = 2,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            blUser.Id = user.Id;

            return blUser;
        }

        public BLUser UpdateUser(BLUser blUser)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == blUser.Id);
            if (user == null)
                throw new Exception($"User {blUser.Id} not found");
            user.Username = blUser.Username;
            user.FirstName = blUser.FirstName;
            user.LastName = blUser.LastName;
            user.Email = blUser.Email;
            user.Phone = blUser.Phone;
            _context.Users.Update(user);
            _context.SaveChanges();
            blUser.Id = user.Id;
            return blUser;
        }

    }
}
