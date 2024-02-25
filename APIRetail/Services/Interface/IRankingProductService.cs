using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IRankingProductService
    {
        Task<List<RankingProductResponse>> GetRankingProductSell(MonthlyStockRequest param, CancellationToken cancellationToken);
        Task<List<RankingProductResponse>> GetRankingProductBuy(MonthlyStockRequest param, CancellationToken cancellationToken);
    }
}
