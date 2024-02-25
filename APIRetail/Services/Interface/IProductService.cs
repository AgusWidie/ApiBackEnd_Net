using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetProduct(ProductRequest param, CancellationToken cancellationToken);
        Task<List<ProductResponse>> CreateProduct(ProductAddRequest param, CancellationToken cancellationToken);
        Task<List<ProductResponse>> UpdateProduct(ProductUpdateRequest param, CancellationToken cancellationToken);
    }
}
