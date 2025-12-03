using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Mapping;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly ITravelRepository _travelRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly IMapper _mapper;
        public TravelController(ITravelRepository travelRepository, ILogsRepository logsRepository, IMapper mapper)
        {
            _travelRepository = travelRepository;
            _logsRepository = logsRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<TravelDto>> Get()
        {
            try
            {
                var blTravels = _travelRepository.GetTravels();

                var travels = _mapper.Map<IEnumerable<TravelDto>>(blTravels);

                return Ok(travels);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TravelDto> Get(int id)
        {
            try
            {
                var blTravel = _travelRepository.GetTravel(id);
                if (blTravel == null)
                {
                    return NotFound($"Travel with id {id} not found");
                }

                var travel = _mapper.Map<TravelDto>(blTravel);

                return Ok(travel);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, $"Error while fetching travel with id: {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<TravelDto> Post([FromBody] TravelDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var blTravel = _mapper.Map<BLTravel>(value);
                var travel = _travelRepository.InsertTravel(blTravel);

                return Ok(travel);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, "Error while creating new travel");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<TravelDto> Put(int id, [FromBody] TravelDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var travelToUpdate = _travelRepository.GetTravel(id);
                if (travelToUpdate == null)
                {
                    return NotFound($"Travel with id {id} not found");
                }
                if (value == null)
                {
                    return BadRequest($"Value missing elements");
                }
                var blTravel = _mapper.Map<BLTravel>(value);
                _travelRepository.UpdateTravel(id, blTravel);
                value.Id = blTravel.Id;
                return Ok(value);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, $"Error while fetching travel with id: {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Travel> Delete(int id)
        {
            try
            {
                var travelToDelete = _travelRepository.GetTravel(id);
                if (travelToDelete == null)
                {
                    return NotFound($"Travel with id {id} not found");
                }
                var travel = _travelRepository.DeleteTravel(id);

                return Ok(travel);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, $"Error while deleting travel with id: {id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<TravelDto>> Search(string name, int page, int count)
        {
            try
            {
                var search = new BLSearch
                {
                    Term = name,
                    Page = page,
                    Size = count,
                    OrderBy = "Name"
                };
                var blTravels = _travelRepository.SearchTravelsByName(search);
                if (blTravels.IsNullOrEmpty())
                {
                    return NotFound($"No travels found with name {name}");
                }

                var travels = _mapper.Map<IEnumerable<TravelDto>>(blTravels);

                return Ok(travels);
            }
            catch (Exception ex)
            {
                _logsRepository.Log(3, $"Error while searching for travels with name: {name}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
