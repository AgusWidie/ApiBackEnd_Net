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
    public class AbsenDoctorController : ControllerBase
    {
        private readonly ILogger<AbsenDoctorController> _logger;
        private readonly IAbsenDoctorService _absenDoctorService;

        public AbsenDoctorController(ILogger<AbsenDoctorController> logger, IAbsenDoctorService absenDoctorService)
        {
            _logger = logger;
            _absenDoctorService = absenDoctorService;
        }

        [Route("GetAbsenDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<AbsenDoctorResponse>> GetAbsenDoctor([FromQuery] AbsenDoctorSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _absenDoctorService.GetAbsenDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get absen doctor."));
            }
            return Ok(ResponseHelper<AbsenDoctorResponse>.Create("Successfully get absen doctor.", param.Page, param.PageSize, result));
        }

        [Route("CreateAbsenDoctor")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<AbsenDoctorResponse>> CreateAbsenDoctor([FromBody] AbsenDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _absenDoctorService.CreateAbsenDoctor(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create absen doctor."));
            }
            return Ok(ResponseHelper<AbsenDoctorResponse>.Create("Successfully create absen doctor.", result.First()));
        }

        [Route("UpdateAbsenDoctor")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<AbsenDoctorResponse>> UpdateAbsenDoctor([FromBody] AbsenDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _absenDoctorService.UpdateAbsenDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update absen doctor."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<AbsenDoctorResponse>.Create("Data Absen Doctor Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<AbsenDoctorResponse>.Create("Successfully update branch.", result.First()));
        }
    }
}
