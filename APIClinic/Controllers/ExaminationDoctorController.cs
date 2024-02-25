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
    public class ExaminationDoctorController : ControllerBase
    {
        private readonly ILogger<ExaminationDoctorController> _logger;
        private readonly IExaminationDoctorService _examinationDoctorService;

        public ExaminationDoctorController(ILogger<ExaminationDoctorController> logger, IExaminationDoctorService examinationDoctorService)
        {
            _logger = logger;
            _examinationDoctorService = examinationDoctorService;
        }

        [Route("GetExaminationDoctor")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationDoctorResponse>> GetExaminationDoctor([FromQuery] ExaminationDoctorSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationDoctorService.GetExaminationDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get examination doctor."));
            }
            return Ok(ResponseHelper<ExaminationDoctorResponse>.Create("Successfully get examination doctor.", param.Page, param.PageSize, result));
        }

        [Route("CreateExaminationDoctor")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationDoctorResponse>> CreateExaminationDoctor([FromBody] ExaminationDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationDoctorService.CreateExaminationDoctor(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create examination doctor."));
            }
            return Ok(ResponseHelper<ExaminationDoctorResponse>.Create("Successfully create examination doctor.", result.First()));
        }

        [Route("UpdateExaminationDoctor")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationDoctorResponse>> UpdateExaminationDoctor([FromBody] ExaminationDoctorRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationDoctorService.UpdateExaminationDoctor(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update branch."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ExaminationDoctorResponse>.Create("Data Examination Doctor Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ExaminationDoctorResponse>.Create("Successfully update examination doctor.", result.First()));
        }
    }
}
