using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessage _messageService;

        public MessageController(ILogger<MessageController> logger, IMessage messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [Route("GetMessage")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<MessageResponse>> GetBranch([FromQuery] MessageRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _messageService.GetMessage(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get message."));
            }
            return Ok(ResponseHelper<MessageResponse>.CreatePaging("Successfully get message.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateMessage")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<MessageResponse>> CreateMessage([FromBody] MessageAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _messageService.CreateMessage(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create message."));
            }
            return Ok(ResponseHelper<MessageResponse>.Create("Successfully create message.", result));
        }

        [Route("UpdateMessage")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<MessageResponse>> UpdateMessage([FromBody] MessageUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _messageService.UpdateMessage(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update message."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<MessageResponse>.Create("Message Id: " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<MessageResponse>.Create("Successfully update message.", result));
        }
    }
}
