using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IStockOpname
    {
        Task<IEnumerable<StockOpnameResponse>> GetStockOpname(StockOpnameRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<StockOpnameResponse>> CreateStockOpname(StockOpnameAddRequest param, CancellationToken cancellationToken = default);
    }
}
