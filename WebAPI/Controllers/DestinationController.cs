using AutoMapper;
using BL.BLModels;
using BL.IRepositories;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;
        public DestinationController(IDestinationRepository destinationRepository, IMapper mapper)
        {
            _destinationRepository = destinationRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<DestinationDto>> Get()
        {
            try
            {
                var blDestinations = _destinationRepository.GetDestinations();
                var destinations = _mapper.Map<IEnumerable<DestinationDto>>(blDestinations);
                return Ok(destinations);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DestinationDto> Get(int id)
        {
            try
            {
                var blDestination = _destinationRepository.GetDestination(id);
                if (blDestination == null)
                {
                    return NotFound($"Destination with id {id} not found");
                }
                var destination = _mapper.Map<DestinationDto>(blDestination);
                return Ok(destination);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        public ActionResult<DestinationDto> Post([FromBody] DestinationDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var blDestination = _mapper.Map<BLDestination>(value);
                var destination = _destinationRepository.AddDestination(blDestination);
                value.Id = destination.Id;
                return Ok(value);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        // PUT api/<DestinationController>/5
        [HttpPut("{id}")]
        public ActionResult<DestinationDto> Put(int id, [FromBody] DestinationDto value)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            try
            {
                var destinationToUpdate = _destinationRepository.GetDestination(id);
                if (destinationToUpdate == null)
                {
                    return NotFound($"Destination with id {id} not found");
                }
                var blDestination = _mapper.Map<BLDestination>(value);
                var destination = _destinationRepository.UpdateDestination(id, blDestination);
                var destinationDto = _mapper.Map<DestinationDto>(destination);
                return Ok(destinationDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while processing your request");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Destination> Delete(int id)
        {
            try
            {
                var destination = _destinationRepository.DeleteDestination(id);
                if (destination == null)
                {
                    return NotFound($"Destination with id {id} not found");
                }
                return Ok(destination);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while processing your request.");
            }
        }
    }
}
