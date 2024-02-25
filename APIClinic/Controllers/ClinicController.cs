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
    public class ClinicController : ControllerBase
    {
        private readonly ILogger<ClinicController> _logger;
        private readonly IClinicService _clinicService;

        public ClinicController(ILogger<ClinicController> logger, IClinicService clinicService)
        {
            _logger = logger;
            _clinicService = clinicService;
        }

        [Route("GetClinic")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ClinicResponse>> GetClinic([FromQuery] ClinicSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _clinicService.GetClinic(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get clinic."));
            }
            return Ok(ResponseHelper<ClinicResponse>.Create("Successfully get clinic.", param.Page, param.PageSize, result));
        }

        [Route("CreateClinic")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ClinicResponse>> CreateClinic([FromBody] ClinicRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _clinicService.CreateClinic(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create clinic."));
            }
            return Ok(ResponseHelper<ClinicResponse>.Create("Successfully create clinic.", result.First()));
        }

        [Route("UpdateClinic")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ClinicResponse>> UpdateClinic([FromBody] ClinicRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _clinicService.UpdateClinic(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update clinic."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ClinicResponse>.Create("Data Clinic Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ClinicResponse>.Create("Successfully update clinic.", result.First()));
        }
    }
}
