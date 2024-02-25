using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IPurchaseOrder
    {
        Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderHeader(PurchaseOrderRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PurchaseOrderDetailResponse>> GetPurchaseOrderDetail(PurchaseOrderRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PurchaseOrderResponse>> CreatePurchaseOrder(PurchaseOrderHeaderAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PurchaseOrderResponse>> UpdatePuchaseOrder(PurchaseOrderHeaderUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
