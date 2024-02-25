using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface ISupplierService
    {
        Task<List<SupplierResponse>> GetSupplier(SupplierRequest param, CancellationToken cancellationToken);
        Task<List<SupplierResponse>> CreateSupplier(SupplierAddRequest param, CancellationToken cancellationToken);
        Task<List<SupplierResponse>> UpdateSupplier(SupplierUpdateRequest param, CancellationToken cancellationToken);
    }
}
