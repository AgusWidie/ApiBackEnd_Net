using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SendEmailController : ControllerBase
    {
        private readonly ILogger<SendEmailController> _logger;
        private readonly ISendEmail _sendEmailService;

        public SendEmailController(ILogger<SendEmailController> logger, ISendEmail sendEmailService)
        {
            _logger = logger;
            _sendEmailService = sendEmailService;
        }

        [Route("GetSendWhatsApp")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SendEmailResponse>> GetSendEmail([FromQuery] SendEmailRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendEmailService.GetSendEmail(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get send email."));
            }
            return Ok(ResponseHelper<SendEmailResponse>.CreatePaging("Successfully send email.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateSendEmail")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SendEmailResponse>> CreateSendEmail([FromBody] SendEmailAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendEmailService.CreateSendEmail(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create send email."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Sudah Ada."));
            }
            return Ok(ResponseHelper<SendEmailResponse>.Create("Successfully create send email.", result));
        }

        [Route("UpdateSendEmail")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SendEmailResponse>> UpdateSendEmail([FromBody] SendEmailUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendEmailService.UpdateSendEmail(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update send email."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Sudah Ada."));
            }
            return Ok(ResponseHelper<SendEmailResponse>.Create("Successfully update send email.", result));
        }
    }
}
