using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IDailyStock
    {
        Task<IEnumerable<DailyStockResponse>> GetDailyStock(DailyStockRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DailyStockResponse>> GetDailyStockList(DailyStockListRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DailyStockResponse>> CreateDailyStock(DailyStockAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DailyStockResponse>> UpdateDailyStockBuy(DailyStockUpdateRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DailyStockResponse>> UpdateDailyStockSell(DailyStockUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
