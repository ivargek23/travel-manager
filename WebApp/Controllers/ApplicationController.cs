using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly ITravelRepository _travelRepo;
        private readonly IDestinationRepository _destinationRepo;
        private readonly IImageRepository _imageRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        public ApplicationController(ITravelRepository travelRepo, IDestinationRepository destinationRepo, IImageRepository imageRepository, IMapper mapper, IConfiguration configuration, DatabaseContext context, IApplicationRepository applicationRepository)
        {
            _travelRepo = travelRepo;
            _destinationRepo = destinationRepo;
            _imageRepository = imageRepository;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _applicationRepository = applicationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyForTravel(int id)
        {
            var travel = _travelRepo.GetTravel(id);
            var travelVm = _mapper.Map<TravelVM>(travel);

            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            var userTravel = _applicationRepository.CheckIfUserApplied(id, userId);
            if (userTravel == true)
            {
                travelVm.IsApplied = true;
            }
            else
            {
                travelVm.IsApplied = false;
            }


            return View(travelVm);
        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult ApplyForTravel(int id, TravelVM travelVm)
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            _applicationRepository.ApplyForTravel(id, userId);

            return RedirectToAction("ListUser", "Travel");
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult GetUserTravels()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            var userTravels = _applicationRepository.GetUserTravels(userId);
            var userTravelVms = _mapper.Map<List<UserTravelVM>>(userTravels);
            return View(userTravelVms);
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult CancelTravel(int id)
        {
            _applicationRepository.CancelApplication(id);
            return RedirectToAction("ListUser", "Travel");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListUserTravels()
        {
            var userTravels = _applicationRepository.GetAllUserTravels();
            var userTravelVms = _mapper.Map<List<UserTravelVM>>(userTravels);
            return View(userTravelVms);
        }
    }
}
