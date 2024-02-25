using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompany _companyService;

        public CompanyController(ILogger<CompanyController> logger, ICompany companyService)
        {
            _logger = logger;
            _companyService = companyService;
        }

        [Route("GetCompany")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<CompanyResponse>> GetCompany([FromQuery] CompanyRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _companyService.GetCompany(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get company."));
            }
            return Ok(ResponseHelper<CompanyResponse>.CreatePaging("Successfully get company.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateCompany")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<CompanyResponse>> CreateCompany([FromBody] CompanyAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _companyService.CreateCompany(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create company."));
            }
            return Ok(ResponseHelper<CompanyResponse>.Create("Successfully create company.", result));
        }

        [Route("UpdateCompany")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<CompanyResponse>> UpdateCompany([FromBody] CompanyUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _companyService.UpdateCompany(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update company."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<CompanyResponse>.Create("Data Company Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<CompanyResponse>.Create("Successfully update company.", result));
        }
    }
}
