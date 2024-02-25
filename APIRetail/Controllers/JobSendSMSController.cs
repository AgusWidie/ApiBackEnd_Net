using APIRetail.Helper;
using APIRetail.Jobs.IJobs;
using APIRetail.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSendSMSController : ControllerBase
    {
        private readonly ILogger<JobSendSMSController> _logger;
        private readonly ISendMessage _sendMessage;

        public JobSendSMSController(ILogger<JobSendSMSController> logger, ISendMessage sendMessage)
        {
            _logger = logger;
            _sendMessage = sendMessage;
        }

        [Route("RunningSendSMSJob")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> RunningSMSJob(CancellationToken cancellationToken = default)
        {
            _sendMessage.SendDataSMS();
            return Ok(ResponseHelper.Create("Successfully running job send sms."));
        }
    }
}
