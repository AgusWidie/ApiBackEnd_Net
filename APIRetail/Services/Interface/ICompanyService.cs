using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface ICompanyService
    {
        Task<List<CompanyResponse>> GetCompany(CompanyRequest param, CancellationToken cancellationToken);
        Task<List<CompanyResponse>> CreateCompany(CompanyAddRequest param, CancellationToken cancellationToken);
        Task<List<CompanyResponse>> UpdateCompany(CompanyUpdateRequest param, CancellationToken cancellationToken);
    }
}
