using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueueDoctorController : ControllerBase
    {
        private readonly ILogger<QueueLabController> _logger;
        private readonly IQueueNo _queueNoService;

        public QueueDoctorController(ILogger<QueueLabController> logger, IQueueNo queueNoService)
        {
            _logger = logger;
            _queueNoService = queueNoService;
        }

        [Route("GetQueueNoDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetQueueNoDoctor(long SpecialistDoctorId, CancellationToken cancellationToken = default)
        {
            var result = await _queueNoService.CreateQueueDoctor(SpecialistDoctorId, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get queue no."));
            }
            return Ok(ResponseHelper<string>.Create("Successfully get queue no.", result));
        }

        [Route("PrintQueueNoDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> PrintQueueNoDoctor(string QueueNo, CancellationToken cancellationToken = default)
        {
            var result = await _queueNoService.PrintQueueDoctor(QueueNo, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get print queue no."));
            }
            return Ok(ResponseHelper<string>.Create(result, ""));
        }
    }
}
