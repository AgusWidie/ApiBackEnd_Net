using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ISupplier
    {
        Task<IEnumerable<SupplierResponse>> GetSupplier(SupplierRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SupplierResponse>> CreateSupplier(SupplierAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SupplierResponse>> UpdateSupplier(SupplierUpdateRequest param, CancellationToken cancellationToken = default);

    }
}
