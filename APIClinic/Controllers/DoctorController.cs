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
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorService _doctorService;

        public DoctorController(ILogger<DoctorController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        [Route("GetDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<DoctorResponse>> GetDoctor([FromQuery] DoctorSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _doctorService.GetDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get doctor."));
            }
            return Ok(ResponseHelper<DoctorResponse>.Create("Successfully get doctor.", param.Page, param.PageSize, result));
        }

        [Route("CreateDoctor")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<DoctorResponse>> CreateBranch([FromBody] DoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _doctorService.CreateDoctor(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create doctor."));
            }
            return Ok(ResponseHelper<DoctorResponse>.Create("Successfully create doctor.", result.First()));
        }

        [Route("UpdateDoctor")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<DoctorResponse>> UpdateBranch([FromBody] DoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _doctorService.UpdateDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update branch."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<DoctorResponse>.Create("Data Doctor Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<DoctorResponse>.Create("Successfully update doctor.", result.First()));
        }
    }
}
