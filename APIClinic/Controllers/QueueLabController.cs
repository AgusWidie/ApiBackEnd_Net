using APIClinic.Helper;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueueLabController : ControllerBase
    {
        private readonly ILogger<QueueLabController> _logger;
        private readonly IQueueNo _queueNoService;

        public QueueLabController(ILogger<QueueLabController> logger, IQueueNo queueNoService)
        {
            _logger = logger;
            _queueNoService = queueNoService;
        }

        [Route("GetQueueNoLab")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetQueueNoLab(long LaboratoriumId, CancellationToken cancellationToken = default)
        {
            var result = await _queueNoService.CreateQueueLab(LaboratoriumId, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get queue no lab."));
            }
            return Ok(ResponseHelper<string>.Create("Successfully get queue no lab.", result));
        }

        [Route("PrintQueueNoLab")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> PrintQueueNoLab(string QueueNo, CancellationToken cancellationToken = default)
        {
            var result = await _queueNoService.PrintQueueLab(QueueNo, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get print queue no."));
            }
            return Ok(ResponseHelper<string>.Create(result, ""));
        }
    }
}
