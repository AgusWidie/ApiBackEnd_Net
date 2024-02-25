using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IProduct
    {
        Task<IEnumerable<ProductResponse>> GetProduct(ProductRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductResponse>> CreateProduct(ProductAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductResponse>> UpdateProduct(ProductUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
