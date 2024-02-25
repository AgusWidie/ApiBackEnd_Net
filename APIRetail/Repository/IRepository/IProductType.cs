using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IProductType
    {
        Task<IEnumerable<ProductTypeResponse>> GetProductType(ProductTypeRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductTypeResponse>> CreateProductType(ProductTypeAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductTypeResponse>> UpdateProductType(ProductTypeUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
