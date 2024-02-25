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
    public class MonthlyStockController : ControllerBase
    {
        private readonly ILogger<MonthlyStockController> _logger;
        private readonly IMonthlyStock _monthlyStockService;

        public MonthlyStockController(ILogger<MonthlyStockController> logger, IMonthlyStock monthlyStockService)
        {
            _logger = logger;
            _monthlyStockService = monthlyStockService;
        }

        [Route("GetMonthlyStock")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<MonthlyStockResponse>> GetMonthlyStock([FromQuery] MonthlyStockRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _monthlyStockService.GetMonthlyStock(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get daily stock."));
            }
            return Ok(ResponseHelper<MonthlyStockResponse>.CreatePaging("Successfully get daily stock.", param.TotalPageSize, param.Page, param.PageSize, result));
        }
    }
}
