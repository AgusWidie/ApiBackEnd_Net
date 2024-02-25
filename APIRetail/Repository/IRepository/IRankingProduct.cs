using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IRankingProduct
    {
        Task<IEnumerable<RankingProductResponse>> GetRangkingProductSell(MonthlyStockRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<RankingProductResponse>> GetRangkingProductBuy(MonthlyStockRequest param, CancellationToken cancellationToken = default);
    }
}
