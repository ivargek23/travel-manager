using AutoMapper;
using BL.IRepositories;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuideController : ControllerBase
    {
        private readonly IGuideRepository _guideRepository;
        private readonly IMapper _mapper;
        public GuideController(IGuideRepository guideRepository, IMapper mapper)
        {
            _guideRepository = guideRepository;
            _mapper = mapper;
        }
        // GET: api/<GuideController>
        [HttpGet]
        public ActionResult<IEnumerable<GuideDto>> Get()
        {
            try
            {
                var blGuides = _guideRepository.GetGuides();
                var guides = _mapper.Map<IEnumerable<GuideDto>>(blGuides);
                return Ok(guides);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error while processing request.");
            }
        }

        // GET api/<GuideController>/5
        [HttpGet("{id}")]
        public ActionResult<GuideDto> Get(int id)
        {
            try
            {
                var guideBl = _guideRepository.GetGuide(id);
                if (guideBl == null)
                {
                    return NotFound($"Guide with id {id} couldn't be found.");
                }
                var guide = _mapper.Map<GuideDto>(guideBl);
                return Ok(guide);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error while processing request.");
            }
        }

        // POST api/<GuideController>
        [HttpPost]
        public ActionResult<GuideDto> Post([FromBody] GuideDto value)
        {
            try
            {
                var blGuide = _mapper.Map<BL.BLModels.BLGuide>(value);
                var addedGuide = _guideRepository.AddGuide(blGuide);
                var guide = _mapper.Map<GuideDto>(addedGuide);
                return (guide);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error while processing request.");
            }
        }

        // PUT api/<GuideController>/5
        [HttpPut("{id}")]
        public ActionResult<GuideDto> Put(int id, [FromBody] GuideDto value)
        {
            try
            {
                var blGuide = _mapper.Map<BL.BLModels.BLGuide>(value);
                var updatedGuide = _guideRepository.UpdateGuide(id, blGuide);
                if (updatedGuide == null)
                {
                    return NotFound($"Guide with id {id} couldn't be found.");
                }
                var guide = _mapper.Map<GuideDto>(updatedGuide);
                guide.Id = blGuide.Id;
                return Ok(guide);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error while processing request.");
            }
        }

        // DELETE api/<GuideController>/5
        [HttpDelete("{id}")]
        public ActionResult<Guide> Delete(int id)
        {
            try
            {
                var guide = _guideRepository.DeleteGuide(id);
                if (guide == null)
                {
                    return NotFound($"Guide with id {id} couldn't be found.");
                }
                return Ok(guide);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error while processing request.");
            }
        }
    }
}
