using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideRepository _guideRepository;
        private readonly IMapper _mapper;
        public GuideController(IGuideRepository guideRepository, IMapper mapper)
        {
            _guideRepository = guideRepository;
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            var guides = _guideRepository.GetGuides();
            var guidesVm = _mapper.Map<IEnumerable<GuideVM>>(guides);

            return View(guidesVm);
        }
        
        public ActionResult Details(int id)
        {
            var guide = _guideRepository.GetGuide(id);
            var guideVm = _mapper.Map<GuideVM>(guide);

            return View(guideVm);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GuideVM guideVm)
        {
            try
            {
                var guide = _mapper.Map<BLGuide>(guideVm);
                _guideRepository.AddGuide(guide);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var guide = _guideRepository.GetGuide(id);
            var guideVm = _mapper.Map<GuideVM>(guide);

            return View(guideVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GuideVM guideVm)
        {
            try
            {
                var guide = _mapper.Map<BLGuide>(guideVm);
                _guideRepository.UpdateGuide(id, guide);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var guide = _guideRepository.GetGuide(id);
            var guideVm = _mapper.Map<GuideVM>(guide);

            return View(guideVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GuideVM guideVm)
        {
            try
            {
                _guideRepository.DeleteGuide(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
