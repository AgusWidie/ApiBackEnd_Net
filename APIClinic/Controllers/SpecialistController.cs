using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecialistController : ControllerBase
    {
        private readonly ILogger<ProfilController> _logger;
        private readonly ISpecialService _specialistService;

        public SpecialistController(ILogger<ProfilController> logger, ISpecialService specialistService)
        {
            _logger = logger;
            _specialistService = specialistService;
        }

        [Route("GetSpecialist")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistResponse>> GetSpecialist([FromQuery] SpecialistSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialistService.GetSpecialist(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get specialist."));
            }
            return Ok(ResponseHelper<SpecialistResponse>.Create("Successfully get specialist.", param.Page, param.PageSize, result));
        }

        [Route("CreateSpecialist")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistResponse>> CreateSpecialist([FromBody] SpecialistRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialistService.CreateSpecialist(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create specialist."));
            }
            return Ok(ResponseHelper<SpecialistResponse>.Create("Successfully create specialist.", result.First()));
        }

        [Route("UpdateSpecialist")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SpecialistResponse>> UpdateSpecialist([FromBody] SpecialistRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _specialistService.UpdateSpecialist(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update specialist."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<SpecialistResponse>.Create("Specialist Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<SpecialistResponse>.Create("Successfully update specialist.", result.First()));
        }
    }
}
