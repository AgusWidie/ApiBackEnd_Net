using APIRetail.Helper;
using APIRetail.Jobs.IJobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class JobSendWhatsAppController : ControllerBase
    {
        private readonly ILogger<JobSendWhatsAppController> _logger;
        private readonly ISendMessage _sendMessage;

        public JobSendWhatsAppController(ILogger<JobSendWhatsAppController> logger, ISendMessage sendMessage)
        {
            _logger = logger;
            _sendMessage = sendMessage;
        }

        [Route("RunningSendWhatsAppJob")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> RunningSendWhatsAppJob(CancellationToken cancellationToken = default)
        {
            _sendMessage.SendDataWhatsApp();
            return Ok(ResponseHelper.Create("Successfully running job send whatsapp."));
        }
    }
}
