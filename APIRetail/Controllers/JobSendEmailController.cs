using APIRetail.Helper;
using APIRetail.Jobs.IJobs;
using APIRetail.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSendEmailController : ControllerBase
    {
        private readonly ILogger<JobSendEmailController> _logger;
        private readonly ISendMessage _sendMessage;

        public JobSendEmailController(ILogger<JobSendEmailController> logger, ISendMessage sendMessage)
        {
            _logger = logger;
            _sendMessage = sendMessage;
        }

        [Route("RunningSendEmailJob")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> RunningSendEmailJob(CancellationToken cancellationToken = default)
        {
            _sendMessage.SendDataWhatsApp();
            return Ok(ResponseHelper.Create("Successfully running job send whatsapp."));
        }
    }
}
