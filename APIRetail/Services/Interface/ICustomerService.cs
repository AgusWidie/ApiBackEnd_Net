using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface ICustomerService
    {
        Task<List<CustomerResponse>> GetCustomer(CustomerRequest param, CancellationToken cancellationToken);
        Task<List<CustomerResponse>> CreateCustomer(CustomerAddRequest param, CancellationToken cancellationToken);
        Task<List<CustomerResponse>> UpdateCustomer(CustomerUpdateRequest param, CancellationToken cancellationToken);
    }
}
