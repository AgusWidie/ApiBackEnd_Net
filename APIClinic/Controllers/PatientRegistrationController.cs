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
    public class PatientRegistrationController : ControllerBase
    {
        private readonly ILogger<PatientRegistrationController> _logger;
        private readonly IPatientRegistrationService _patientRegistrationService;

        public PatientRegistrationController(ILogger<PatientRegistrationController> logger, IPatientRegistrationService patientRegistrationService)
        {
            _logger = logger;
            _patientRegistrationService = patientRegistrationService;
        }

        [Route("GetPatientRegistration")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationResponse>> GetPatientRegistration([FromQuery] PatientRegistrationSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationService.GetPatientRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get patient registration."));
            }
            return Ok(ResponseHelper<PatientRegistrationResponse>.Create("Successfully get registration.", param.Page, param.PageSize, result));
        }

        [Route("CreatePatientRegistration")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationResponse>> CreatePatientRegistration([FromBody] PatientRegistrationRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationService.CreatePatientRegistration(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create patient registration."));
            }
            return Ok(ResponseHelper<PatientRegistrationResponse>.Create("Successfully patient registration.", result.First()));
        }

        [Route("UpdatePatientRegistration")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationResponse>> UpdatePatientRegistration([FromBody] PatientRegistrationRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationService.UpdatePatientRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request patient registration."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<PatientRegistrationResponse>.Create("Data Patient Registration Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<PatientRegistrationResponse>.Create("Successfully update patient registration.", result.First()));
        }
    }
}
