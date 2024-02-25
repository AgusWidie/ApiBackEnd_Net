using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly ISchedule _scheduleService;

        public ScheduleController(ILogger<ScheduleController> logger, ISchedule scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        [Route("GetSchedule")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleResponse>> GetSchedule([FromQuery] ScheduleRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.GetSchedule(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get schedule."));
            }
            return Ok(ResponseHelper<ScheduleResponse>.CreatePaging("Successfully get schedule.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateSchedule")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleResponse>> CreateSchedule([FromBody] ScheduleAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.CreateSchedule(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create schedule."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Sudah Ada."));
            }
            return Ok(ResponseHelper<ScheduleResponse>.Create("Successfully create schedule.", result));
        }

        [Route("UpdateSchedule")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleResponse>> UpdateSchedule([FromBody] ScheduleUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.UpdateSchedule(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update schedule."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.CreateError(200, "Data Sudah Ada."));
            }

            return Ok(ResponseHelper<ScheduleResponse>.Create("Successfully update schedule.", result));
        }
    }
}
