using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ISalesOrder
    {
        Task<IEnumerable<SalesOrderResponse>> GetSalesOrderHeader(SalesOrderRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SalesOrderDetailResponse>> GetSalesOrderDetail(SalesOrderRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SalesOrderResponse>> CreateSalesOrder(SalesOrderHeaderAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SalesOrderResponse>> UpdateSalesOrder(SalesOrderHeaderUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
