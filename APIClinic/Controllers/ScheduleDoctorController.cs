using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleDoctorController : ControllerBase
    {
        private readonly ILogger<ScheduleDoctorController> _logger;
        private readonly IScheduleDoctorService _scheduleService;

        public ScheduleDoctorController(ILogger<ScheduleDoctorController> logger, IScheduleDoctorService scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        [Route("GetScheduleDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleDoctorResponse>> GetScheduleDoctor([FromQuery] ScheduleDoctorSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.GetScheduleDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get schedule doctor."));
            }
            return Ok(ResponseHelper<ScheduleDoctorResponse>.Create("Successfully get schedule doctor.", param.Page, param.PageSize, result));
        }

        [Route("CreateScheduleDoctor")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleDoctorResponse>> CreateScheduleDoctor([FromBody] ScheduleDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.CreateScheduleDoctor(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create schedule doctor."));
            }
            return Ok(ResponseHelper<ScheduleDoctorResponse>.Create("Successfully create schedule doctor.", result.First()));
        }

        [Route("UpdateScheduleDoctor")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ScheduleDoctorResponse>> UpdateScheduleDoctor([FromBody] ScheduleDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _scheduleService.UpdateScheduleDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update schedule doctor."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ScheduleDoctorResponse>.Create("Data Schedule Doctor Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ScheduleDoctorResponse>.Create("Successfully update schedule doctor.", result.First()));
        }
    }
}
