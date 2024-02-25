using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IProductTypeService
    {
        Task<List<ProductTypeResponse>> GetProductType(ProductTypeRequest param, CancellationToken cancellationToken);
        Task<List<ProductTypeResponse>> CreateProductType(ProductTypeAddRequest param, CancellationToken cancellationToken);
        Task<List<ProductTypeResponse>> UpdateProductType(ProductTypeUpdateRequest param, CancellationToken cancellationToken);
    }
}
