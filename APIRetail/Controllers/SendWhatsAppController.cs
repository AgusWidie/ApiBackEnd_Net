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
    public class SendWhatsAppController : ControllerBase
    {
        private readonly ILogger<SendWhatsAppController> _logger;
        private readonly ISendWhatsApp _sendWhatsAppService;

        public SendWhatsAppController(ILogger<SendWhatsAppController> logger, ISendWhatsApp sendWhatsAppService)
        {
            _logger = logger;
            _sendWhatsAppService = sendWhatsAppService;
        }

        [Route("GetSendWhatsApp")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SendWhatsAppResponse>> GetSendWhatsApp([FromQuery] SendWhatsAppRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendWhatsAppService.GetSendWhatsApp(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get send whatsapp."));
            }
            return Ok(ResponseHelper<SendWhatsAppResponse>.CreatePaging("Successfully send whatsapp.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateSendWhatsApp")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SendWhatsAppResponse>> CreateSendWhatsApp([FromBody] SendWhatsAppAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendWhatsAppService.CreateSendWhatsApp(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create send whatsapp."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Already Exist."));
            }
            return Ok(ResponseHelper<SendWhatsAppResponse>.Create("Successfully create send whatsapp.", result));
        }

        [Route("UpdateSendWhatsApp")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SendWhatsAppResponse>> UpdateSendWhatsApp([FromBody] SendWhatsAppUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _sendWhatsAppService.UpdateSendWhatsApp(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update send whatsapp."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Already Exist."));
            }
            return Ok(ResponseHelper<SendWhatsAppResponse>.Create("Successfully update send whatsapp.", result));
        }
    }
}
