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
    public class DailyStockController : ControllerBase
    {
        private readonly ILogger<DailyStockController> _logger;
        private readonly IDailyStock _dailyStockService;

        public DailyStockController(ILogger<DailyStockController> logger, IDailyStock dailyStockService)
        {
            _logger = logger;
            _dailyStockService = dailyStockService;
        }

        [Route("GetDailyStock")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<DailyStockResponse>> GetDailyStock([FromQuery] DailyStockRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _dailyStockService.GetDailyStock(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get daily stock."));
            }
            return Ok(ResponseHelper<DailyStockResponse>.CreatePaging("Successfully get daily stock.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateDailyStock")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<DailyStockResponse>> CreateDailyStock([FromBody] DailyStockAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _dailyStockService.CreateDailyStock(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create daily stock."));
            }
            return Ok(ResponseHelper<DailyStockResponse>.Create("Successfully create daily stock.", result));
        }

        [Route("UpdateDailyStockBuy")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<DailyStockResponse>> UpdateDailyStockBuy([FromBody] DailyStockUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _dailyStockService.UpdateDailyStockBuy(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update daily stock buy."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper.Create("Data Daily Stock Id (Buy) : " + param.Id.ToString() + " Not Found."));
            }
            return Ok(ResponseHelper<DailyStockResponse>.Create("Successfully update daily stock buy.", result));
        }

        [Route("UpdateDailyStockSell")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<DailyStockResponse>> UpdateDailyStockSell([FromBody] DailyStockUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _dailyStockService.UpdateDailyStockSell(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update daily stock sell."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<DailyStockResponse>.Create("Data Daily Stock Id (Sell) : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<DailyStockResponse>.Create("Successfully update daily stock sell.", result));
        }
    }
}
