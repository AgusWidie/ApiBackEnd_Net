using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ICustomer
    {
        Task<IEnumerable<CustomerResponse>> GetCustomer(CustomerRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomerResponse>> CreateCustomer(CustomerAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomerResponse>> UpdateCustomer(CustomerUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
