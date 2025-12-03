using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using BL.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository authRepository, DatabaseContext context, IConfiguration configuration, IMapper mapper)
        {
            _authRepository = authRepository;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpPost("[action]")]
        public ActionResult<UserDto> Register([FromBody] UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<BLUser>(userDto);
                _authRepository.Register(user);
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Username already exists");
            }
        }
        [HttpPost("[action]")]
        public ActionResult<UserLoginDto> Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingUser = _context.Users.FirstOrDefault(x => x.Username == userDto.Username);
                if (existingUser == null)
                    return Unauthorized(genericLoginFail);

                var b64hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return Unauthorized(genericLoginFail);

                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, userDto.Username);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("[action]")]
        public ActionResult<UserLoginDto> ChangePassword([FromBody] UserLoginDto user)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == user.Username);
                if (existingUser == null)
                    return Unauthorized("Incorrect username");
                var blUser = _mapper.Map<BLUserLogin>(user);
                _authRepository.ChangePassword(blUser);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
