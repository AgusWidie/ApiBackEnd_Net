using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RankingProductController : ControllerBase
    {
        private readonly ILogger<RankingProductController> _logger;
        private readonly IRankingProduct _rankingProductService;

        public RankingProductController(ILogger<RankingProductController> logger, IRankingProduct rankingProductService)
        {
            _logger = logger;
            _rankingProductService = rankingProductService;
        }

        [Route("GetRankingProductSell")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<RankingProductResponse>> GetRankingProductSell([FromQuery] MonthlyStockRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _rankingProductService.GetRangkingProductSell(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get ranking product sell."));
            }
            return Ok(ResponseHelper<RankingProductResponse>.CreatePaging("Successfully get ranking product sell.", param.TotalPageSize, param.Page, param.PageSize, result));

        }

        [Route("GetRankingProductBuy")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<RankingProductResponse>> GetRankingProductBuy([FromQuery] MonthlyStockRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _rankingProductService.GetRangkingProductBuy(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get ranking product buy."));
            }
            return Ok(ResponseHelper<RankingProductResponse>.CreatePaging("Successfully get ranking product buy.", param.TotalPageSize, param.Page, param.PageSize, result));
        }
    }
}
