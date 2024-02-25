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
    public class LaboratoriumController : ControllerBase
    {
        private readonly ILogger<LaboratoriumController> _logger;
        private readonly ILaboratoriumService _laboratoriumService;

        public LaboratoriumController(ILogger<LaboratoriumController> logger, ILaboratoriumService laboratoriumService)
        {
            _logger = logger;
            _laboratoriumService = laboratoriumService;
        }

        [Route("GetLaboratorium")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<LaboratoriumResponse>> GetBranch([FromQuery] LaboratoriumSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _laboratoriumService.GetLaboratorium(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get laboratorium."));
            }
            return Ok(ResponseHelper<LaboratoriumResponse>.Create("Successfully get laboratorium.", param.Page, param.PageSize, result));
        }


        [Route("CreateLaboratorium")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<LaboratoriumResponse>> CreateLaboratorium([FromBody] LaboratoriumRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _laboratoriumService.CreateLaboratium(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create laboratorium."));
            }
            return Ok(ResponseHelper<LaboratoriumResponse>.Create("Successfully create laboratorium.", result.First()));
        }

        [Route("UpdateLaboratorium")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<LaboratoriumResponse>> UpdateBranch([FromBody] LaboratoriumRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _laboratoriumService.UpdateLaboratium(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update laboratorium."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<LaboratoriumResponse>.Create("Data Laboratorium Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<LaboratoriumResponse>.Create("Successfully update laboratorium.", result.First()));
        }
    }
}
