using AutoMapper;
using BL.IRepositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;
using BL.BLModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogsRepository _logsRepo;
        private readonly IMapper _mapper;
        public UserController(IAuthRepository authRepository, IMapper mapper, ILogsRepository logsRepository)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _logsRepo = logsRepository;
        }
        public IActionResult Login(string returnUrl)
        {
            var userLoginVM = new UserLoginVM
            {
                ReturnUrl = returnUrl
            };
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginVM userLoginVM)
        {
            var genericLoginFail = "Incorrect username or password";

            var blUser = _mapper.Map<BLUserLogin>(userLoginVM);
            var existingUser = _authRepository.CheckIfUserExists(blUser);
            if (existingUser == null)
            {
                ModelState.AddModelError("", genericLoginFail);
                return View();
            }

            var user = _authRepository.CheckUserCredentials(blUser);
            if (user == null)
            {
                ModelState.AddModelError("", genericLoginFail);
                return View();
            }

            var userRole = _authRepository.CheckUserRole(user);
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (userLoginVM.ReturnUrl != null)
                return LocalRedirect(userLoginVM.ReturnUrl);
            else if (userRole == "Admin")
                return RedirectToAction("List", "Travel");
            else if (userRole == "User")
                return RedirectToAction("ListUser", "Travel");
            else
                return View();
        }
        public IActionResult Register(string returnUrl)
        {
            UserVM userVM = new UserVM { ReturnUrl = returnUrl };

            return View(userVM);
        }

        [HttpPost]
        public IActionResult Register(UserVM userVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userVmLogin = new UserLoginVM
                {
                    Username = userVm.Username,
                    Password = userVm.Password
                };
                var existingUser = _authRepository.CheckIfUserExists(_mapper.Map<BLUserLogin>(userVmLogin));
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User already exists");
                    return View(userVm);
                }

                var blUser = _mapper.Map<BLUser>(userVm);
                var user = _authRepository.Register(blUser);
                userVm = _mapper.Map<UserVM>(user);

                if (string.IsNullOrEmpty(userVm.ReturnUrl))
                    return RedirectToAction("RegisterConfirmation", userVm);
                else
                    return LocalRedirect(userVm.ReturnUrl);
            }
            catch (Exception ex)
            {
                _logsRepo.Log((int)LogLevel.Error, "Error while registering user");
                return StatusCode(500, ex.Message);
            }
        }
        public IActionResult RegisterConfirmation(UserVM userVm)
        {
            return View(userVm);
        }

        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();
            this.HttpContext.User = null;

            return View();
        }

        [Authorize(Roles = "Admin, User")]
        public IActionResult ProfileDetails()
        {
            var username = HttpContext.User.Identity.Name;

            var blUser = _authRepository.GetUserByUsername(username);
            var userVm = _mapper.Map<UserVM>(blUser);

            return View(userVm);
        }

        [Authorize(Roles = "Admin, User")]
        public IActionResult ProfileEdit(int id)
        {
            var blUser = _authRepository.GetUserById(id);
            var userVm = _mapper.Map<UserVM>(blUser);

            return View(userVm);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public IActionResult ProfileEdit(int id, UserVM userVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var blUser = _authRepository.GetUserById(id);
                var userDb = _mapper.Map<BLUser>(userVm);
                _authRepository.UpdateUser(userDb);

                return RedirectToAction("ProfileDetails");
            }
            catch (Exception ex)
            {
                _logsRepo.Log((int)LogLevel.Error, "Error while updating user profile");
                return StatusCode(500, ex.Message);
            }
        }

        public JsonResult GetProfileData(int id)
        {
            var blUser = _authRepository.GetUserById(id);
            var userVm = _mapper.Map<UserVM>(blUser);
            return Json(new
            {
                userVm.FirstName,
                userVm.LastName,
                userVm.Email,
                userVm.Phone
            });
        }

        [HttpPut]
        public ActionResult SetProfileData(int id, [FromBody] UserVM userVm)
        {
            var userDb = _authRepository.GetUserById(id);

            userDb.FirstName = userVm.FirstName;
            userDb.LastName = userVm.LastName;
            userDb.Email = userVm.Email;
            userDb.Phone = userVm.Phone;

            _authRepository.UpdateUser(userDb);

            return Ok();
        }
    }
}
