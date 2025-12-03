using BL.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository _logsRepository;
        public LogsController(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        [Authorize]
        [HttpGet("[action]/{N}")]
        public ActionResult Get(int n = 10)
        {
            try
            {
                var logs = _logsRepository.GetLogs(n);
                if (logs == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No logs found");
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize]
        [HttpGet("[action]")]
        public ActionResult GetCount()
        {
            try
            {
                var count = _logsRepository.GetCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
