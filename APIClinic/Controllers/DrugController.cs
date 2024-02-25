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
    public class DrugController : ControllerBase
    {
        private readonly ILogger<DrugController> _logger;
        private readonly IDrugService _drugService;

        public DrugController(ILogger<DrugController> logger, IDrugService drugService)
        {
            _logger = logger;
            _drugService = drugService;
        }

        [Route("GetDrug")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<DrugResponse>> GetDrug([FromQuery] DrugSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _drugService.GetDrug(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get drug."));
            }
            return Ok(ResponseHelper<DrugResponse>.Create("Successfully get drug.", param.Page, param.PageSize, result));
        }

        [Route("CreateDrug")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<DrugResponse>> CreateDrug([FromBody] DrugRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _drugService.CreateDrug(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create drug."));
            }
            return Ok(ResponseHelper<DrugResponse>.Create("Successfully create drug.", result.First()));
        }

        [Route("UpdateDrug")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<DrugResponse>> UpdateDrug([FromBody] DrugRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _drugService.UpdateDrug(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update drug."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<DrugResponse>.Create("Data Drug Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<DrugResponse>.Create("Successfully update drug.", result.First()));
        }
    }
}
