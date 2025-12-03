using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class TravelController : Controller
    {
        private readonly ITravelRepository _travelRepo;
        private readonly IDestinationRepository _destinationRepo;
        private readonly IImageRepository _imageRepository;
        private readonly IGuideRepository _guideRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        public TravelController(ITravelRepository travelRepo, IMapper mapper, IDestinationRepository destinationRepo, IImageRepository imageRepository, IConfiguration configuration, DatabaseContext context, IGuideRepository guideRepository)
        {
            _travelRepo = travelRepo;
            _mapper = mapper;
            _destinationRepo = destinationRepo;
            _imageRepository = imageRepository;
            _configuration = configuration;
            _context = context;
            _guideRepository = guideRepository;
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index()
        {
            try
            {
                var travels = _travelRepo.GetTravels();
                var travelVms = _mapper.Map<List<TravelVM>>(travels);

                return View(travelVms);
            }
            catch
            {
                throw;
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var travel = _travelRepo.GetTravel(id);
            var travelVm = _mapper.Map<TravelVM>(travel);
            
            var destinations = _destinationRepo.GetDestinations().ToList();
            var destinationVms = _mapper.Map<IEnumerable<DestinationVM>>(destinations);
            ViewBag.DestinationDdlItems = destinationVms.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            var guides = _guideRepository.GetGuides().ToList();
            var guideVms = _mapper.Map<IEnumerable<GuideVM>>(guides);
            ViewBag.GuideDdlItems = guideVms.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            var dbUsers = _travelRepo.GetUsersForTravel(id);
            if (!dbUsers.IsNullOrEmpty())
            {
                var users = _mapper.Map<IEnumerable<UserVM>>(dbUsers);
                ViewBag.Users = users;
            }
            return View(travelVm);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult UserDetails(int id)
        {
            var travel = _travelRepo.GetTravel(id);
            var travelVm = _mapper.Map<TravelVM>(travel);

            var dbGuides = _guideRepository.GetGuidesByTravelId(id);
            if (!dbGuides.IsNullOrEmpty())
            {
                var guides = _mapper.Map<IEnumerable<GuideVM>>(dbGuides);
                ViewBag.GuideDdlItems = guides.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            }

            return View(travelVm);
        }

        

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var destinations = _destinationRepo.GetDestinations().ToList();
            var destinationVms = _mapper.Map<IEnumerable<DestinationVM>>(destinations);
            ViewBag.DestinationDdlItems = destinationVms.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TravelVM travelVm)
        {
            try
            {
                var existingTravel = _travelRepo.TravelWithNameExists(travelVm.Name);
                if (existingTravel)
                {
                    ModelState.AddModelError("", $"Travel with name {travelVm.Name} already exists");
                    return View(travelVm);
                }
                int? imageId = null;

                if (travelVm.Image != null)
                {
                    var formFile = travelVm.Image;
                    var fileName = Guid.NewGuid().ToString() + "_" + travelVm.ImageName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        travelVm.Image.CopyTo(fileStream);
                    }

                    var blImage = new BLImage
                    {
                        PicturePath = Path.Combine("images", fileName),
                        PictureName = travelVm.Image.FileName,
                        Content = travelVm.Image.ContentType
                    };

                    var savedImage = _imageRepository.SaveImage(blImage);
                    imageId = savedImage.Id;
                }


                var travelBl = _mapper.Map<BL.BLModels.BLTravel>(travelVm);
                travelBl.ImageId = imageId;
                var travel = _travelRepo.InsertTravel(travelBl);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {

            var destinations = _destinationRepo.GetDestinations().ToList();
            var destinationVms = _mapper.Map<IEnumerable<DestinationVM>>(destinations);
            ViewBag.DestinationDdlItems = destinationVms.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            var guides = _guideRepository.GetGuides().ToList();
            var guideVms = _mapper.Map<IEnumerable<GuideVM>>(guides);
            ViewBag.GuideDdlItems = guideVms.Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            var travel = _travelRepo.GetTravel(id);
            var travelVm = _mapper.Map<TravelVM>(travel);
            if (travelVm.ImageId != null)
            {
                var image = _imageRepository.GetImage(travelVm.ImageId.Value);
                travelVm.ImageName = image.PictureName;
                travelVm.ImagePath = image.PicturePath;
            }
            else
            {
                travelVm.ImagePath = null;
            }

            return View(travelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TravelVM travelVm)
        {
            try
            {
                int? imageId = null;
                if (travelVm.Image != null)
                {
                    var formFile = travelVm.Image;
                    var fileName = Guid.NewGuid().ToString() + "_" + travelVm.Image.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        travelVm.Image.CopyTo(fileStream);
                    }

                    var blImage = new BLImage
                    {
                        PicturePath = Path.Combine("images", fileName),
                        PictureName = travelVm.Image.FileName,
                        Content = travelVm.Image.ContentType
                    };

                    var savedImage = _imageRepository.SaveImage(blImage);
                    imageId = savedImage.Id;
                }


                var travelBl = _mapper.Map<BL.BLModels.BLTravel>(travelVm);
                travelBl.ImageId = imageId;
                var travel = _travelRepo.UpdateTravel(id, travelBl);

                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var travel = _travelRepo.GetTravel(id);
                var travelVm = _mapper.Map<TravelVM>(travel);

                return View(travelVm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TravelVM travelVm)
        {
            try
            {
                var travel = _travelRepo.DeleteTravel(id);

                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult List(SearchVM searchVm)
        {
            try
            {
                PrepareSearchViewModel(searchVm);

                return View(searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult ListUser(SearchVM searchVm)
        {
            try
            {
                PrepareSearchViewModel(searchVm);

                return View(searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult ListUserPartial(SearchVM searchVm)
        {
            try
            {
                PrepareSearchViewModel(searchVm);

                return PartialView("_ListUserPartial", searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListPartial(SearchVM searchVm)
        {
            try
            {
                PrepareSearchViewModel(searchVm);

                return PartialView("_ListPartial", searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrepareSearchViewModel(SearchVM searchVm)
        {
            IQueryable<Travel> travels = _context.Travels
                .Include(x => x.Destination)
                .Include(x => x.Image)
                .Include(x => x.TravelGuides)
                .Include(x => x.UserTravels);

            if (!string.IsNullOrEmpty(searchVm.Term))
            {
                travels = travels.Where(x => x.Name.Contains(searchVm.Term));
            }

            var filteredCount = travels.Count();
            switch (searchVm.OrderBy.ToLower())
            {
                case "id":
                    travels = travels.OrderBy(x => x.Id);
                    break;
                case "name":
                    travels = travels.OrderBy(x => x.Name);
                    break;
                case "start date":
                    travels = travels.OrderBy(x => x.StartDate);
                    break;
                case "end date":
                    travels = travels.OrderBy(x => x.EndDate);
                    break;
                case "price":
                    travels = travels.OrderBy(x => x.Price);
                    break;
                case "destination name":
                    travels = travels.OrderBy(x => x.Destination.Name);
                    break;
            }

            travels = travels.Skip((searchVm.Page - 1) * searchVm.Size).Take(searchVm.Size);

            searchVm.Travels = travels.Select(x => new TravelVM
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Price = x.Price,
                DestinationId = x.DestinationId,
                DestinationName = x.Destination.Name,
                ImageId = x.ImageId
            }).ToList();
            foreach (var travel in searchVm.Travels)
            {
                if (travel.ImageId != null)
                {
                    var image = _imageRepository.GetImage(travel.ImageId.Value);
                    travel.ImagePath = image.PicturePath;
                }
                else
                {
                    travel.ImagePath = null;
                }
            }
            // BEGIN PAGER
            var expandPages = _configuration.GetValue<int>("Paging:ExpandPages");
            searchVm.LastPage = (int)Math.Ceiling(1.0 * filteredCount / searchVm.Size);
            searchVm.FromPager = searchVm.Page > expandPages ?
                searchVm.Page - expandPages :
                1;
            searchVm.ToPager = (searchVm.Page + expandPages) < searchVm.LastPage ?
                searchVm.Page + expandPages :
                searchVm.LastPage;
            // END PAGER
        }
    }
}
