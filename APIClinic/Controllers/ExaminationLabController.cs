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
    public class ExaminationLabController : ControllerBase
    {
        private readonly ILogger<ExaminationLabController> _logger;
        private readonly IExaminationLabService _examinationLabService;

        public ExaminationLabController(ILogger<ExaminationLabController> logger, IExaminationLabService examinationLabService)
        {
            _logger = logger;
            _examinationLabService = examinationLabService;
        }

        [Route("GetExaminationLab")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationLabResponse>> GetExaminationLab([FromQuery] ExaminationLabSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationLabService.GetExaminationLab(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get examination laboratorium."));
            }
            return Ok(ResponseHelper<ExaminationLabResponse>.Create("Successfully get examination laboratorium.", param.Page, param.PageSize, result));
        }


        [Route("CreateExaminationLab")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationLabResponse>> CreateExaminationLab([FromBody] ExaminationLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationLabService.CreateExaminationLab(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create examination laboratorium."));
            }
            return Ok(ResponseHelper<ExaminationLabResponse>.Create("Successfully create examination laboratorium.", result.First()));
        }

        [Route("UpdateExaminationLab")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ExaminationLabResponse>> UpdateExaminationLab([FromBody] ExaminationLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _examinationLabService.UpdateExaminationLab(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update laboratorium."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ExaminationLabResponse>.Create("Data Examination Laboratorium Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ExaminationLabResponse>.Create("Successfully update examination laboratorium.", result.First()));
        }
    }
}
