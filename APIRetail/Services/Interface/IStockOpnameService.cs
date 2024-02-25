using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IStockOpnameService
    {
        Task<List<StockOpnameResponse>> GetStockOpname(StockOpnameRequest param, CancellationToken cancellationToken);
        Task<List<StockOpnameResponse>> CreateStockOpname(StockOpnameAddRequest param, CancellationToken cancellationToken);
    }
}
