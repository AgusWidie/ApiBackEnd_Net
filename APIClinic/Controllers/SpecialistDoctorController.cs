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
    public class SpecialistDoctorController : ControllerBase
    {
        private readonly ILogger<SpecialistDoctorController> _logger;
        private readonly ISpecialDoctorService _specialDoctorService;

        public SpecialistDoctorController(ILogger<SpecialistDoctorController> logger, ISpecialDoctorService specialDoctorService)
        {
            _logger = logger;
            _specialDoctorService = specialDoctorService;
        }

        [Route("GetSpecialDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistDoctorResponse>> GetBranch([FromQuery] SpecialistDoctorSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialDoctorService.GetSpecialistDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get special doctor."));
            }
            return Ok(ResponseHelper<SpecialistDoctorResponse>.Create("Successfully get special doctor.", param.Page, param.PageSize, result));
        }

        [Route("CreateSpecialDoctor")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistDoctorResponse>> CreateSpecialDoctor([FromBody] SpecialistDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialDoctorService.CreateSpecialistDoctor(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create special doctor."));
            }
            return Ok(ResponseHelper<SpecialistDoctorResponse>.Create("Successfully create special doctor.", result.First()));
        }

        [Route("UpdateSpecialDoctor")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistDoctorResponse>> UpdateSpecialDoctor([FromBody] SpecialistDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialDoctorService.UpdateSpecialistDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update branch."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<SpecialistDoctorResponse>.Create("Data Special Doctor Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<SpecialistDoctorResponse>.Create("Successfully update branch.", result.First()));
        }
    }
}
