using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ICompany
    {
        Task<IEnumerable<CompanyResponse>> GetCompany(CompanyRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<CompanyResponse>> CreateCompany(CompanyAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<CompanyResponse>> UpdateCompany(CompanyUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
