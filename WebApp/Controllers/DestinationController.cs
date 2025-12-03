using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class DestinationController : Controller
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;
        public DestinationController(IDestinationRepository destinationRepository, IMapper mapper)
        {
            _destinationRepository = destinationRepository;
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            var blDestinations = _destinationRepository.GetDestinations();
            var destinationVms = _mapper.Map<List<DestinationVM>>(blDestinations);
            return View(destinationVms);
        }
        public ActionResult Details(int id)
        {
            var blDestination = _destinationRepository.GetDestination(id);
            var destinationVm = _mapper.Map<DestinationVM>(blDestination);
            return View(destinationVm);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DestinationVM destinationVm)
        {
            try
            {
                var blDestination = _mapper.Map<BLDestination>(destinationVm);
                _destinationRepository.AddDestination(blDestination);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            var blDestination = _destinationRepository.GetDestination(id);
            var destinationVm = _mapper.Map<DestinationVM>(blDestination);

            return View(destinationVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DestinationVM destinationVm)
        {
            try
            {
                var blDestination = _mapper.Map<BLDestination>(destinationVm);
                _destinationRepository.UpdateDestination(id, blDestination);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var blDestination = _destinationRepository.GetDestination(id);
            var destinationVm = _mapper.Map<DestinationVM>(blDestination);

            return View(destinationVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DestinationVM destinationVm)
        {
            try
            {
                var deletedDestination = _destinationRepository.DeleteDestination(id);

                if (deletedDestination != null)
                {
                    return RedirectToAction("Index", "Destination");
                }
                else
                {
                    return Json(new { success = false, message = "Destination not found or could not be deleted." });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
