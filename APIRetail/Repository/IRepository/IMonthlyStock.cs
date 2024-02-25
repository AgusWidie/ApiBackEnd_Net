using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IMonthlyStock
    {
        Task<IEnumerable<MonthlyStockResponse>> GetMonthlyStock(MonthlyStockRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MonthlyStockResponse>> GetMonthlyStockList(MonthlyStockListRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MonthlyStockResponse>> CreateMonthlyStock(MonthlyStockAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MonthlyStockResponse>> UpdateMontlyStockBuy(MonthlyStockUpdateRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MonthlyStockResponse>> UpdateMontlyStockSell(MonthlyStockUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
