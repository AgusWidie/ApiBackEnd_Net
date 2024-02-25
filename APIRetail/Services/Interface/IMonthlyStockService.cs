using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IMonthlyStockService
    {
        Task<List<MonthlyStockResponse>> GetMonthlyStock(MonthlyStockRequest param, CancellationToken cancellationToken);
        Task<List<MonthlyStockResponse>> CreateMonthlyStock(MonthlyStockAddRequest param, CancellationToken cancellationToken);
        Task<List<MonthlyStockResponse>> UpdateMonthlyStockBuy(MonthlyStockUpdateRequest param, CancellationToken cancellationToken);
    }
}
