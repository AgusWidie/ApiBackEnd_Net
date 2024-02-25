using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientRegistrationLabController : ControllerBase
    {
        private readonly ILogger<PatientRegistrationLabController> _logger;
        private readonly IPatientRegistrationLabService _patientRegistrationLabService;

        public PatientRegistrationLabController(ILogger<PatientRegistrationLabController> logger, IPatientRegistrationLabService patientRegistrationLabService)
        {
            _logger = logger;
            _patientRegistrationLabService = patientRegistrationLabService;
        }

        [Route("GetPatientRegistrationLab")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationLabResponse>> GetPatientRegistrationLab([FromQuery] PatientRegistrationLabSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationLabService.GetPatientLabRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get patient registration lab."));
            }
            return Ok(ResponseHelper<PatientRegistrationLabResponse>.Create("Successfully get registration lab.", param.Page, param.PageSize, result));
        }

        [Route("CreatePatientRegistrationLab")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationLabResponse>> CreatePatientRegistrationLab([FromBody] PatientRegistrationLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationLabService.CreatePatientRegistrationLab(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create patient registration lab."));
            }
            return Ok(ResponseHelper<PatientRegistrationLabResponse>.Create("Successfully patient registration lab.", result.First()));
        }

        [Route("UpdatePatientRegistrationLab")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<PatientRegistrationLabResponse>> UpdatePatientRegistrationLab([FromBody] PatientRegistrationLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _patientRegistrationLabService.UpdatePatientRegistrationLab(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request patient registration lab."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<PatientRegistrationLabResponse>.Create("Data Patient Lab Registration Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<PatientRegistrationLabResponse>.Create("Successfully update patient registration lab.", result.First()));
        }
    }
}
