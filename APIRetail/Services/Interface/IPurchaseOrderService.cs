using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrderResponse>> GetPurchaseOrder(PurchaseOrderRequest param, CancellationToken cancellationToken);
        Task<List<PurchaseOrderResponse>> CreatePurchaseOrder(PurchaseOrderHeaderAddRequest param, CancellationToken cancellationToken);
        Task<List<PurchaseOrderResponse>> UpdatePurchaseOrder(PurchaseOrderHeaderUpdateRequest param, CancellationToken cancellationToken);
    }
}
