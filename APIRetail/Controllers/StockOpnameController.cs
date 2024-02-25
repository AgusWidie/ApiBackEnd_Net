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
    public class StockOpnameController : ControllerBase
    {
        private readonly ILogger<StockOpnameController> _logger;
        private readonly IStockOpname _stockOpnameService;

        public StockOpnameController(ILogger<StockOpnameController> logger, IStockOpname stockOpnameService)
        {
            _logger = logger;
            _stockOpnameService = stockOpnameService;
        }

        [Route("GetStockOpname")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<StockOpnameResponse>> GetStockOpname([FromQuery] StockOpnameRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _stockOpnameService.GetStockOpname(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get stock opname."));
            }
            return Ok(ResponseHelper<StockOpnameResponse>.CreatePaging("Successfully get stock opname.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateStockOpname")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<StockOpnameResponse>> CreateStockOpname([FromBody] StockOpnameAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _stockOpnameService.CreateStockOpname(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create stock opname."));
            }
            return Ok(ResponseHelper<StockOpnameResponse>.Create("Successfully create stock opname.", result));
        }
    }
}
