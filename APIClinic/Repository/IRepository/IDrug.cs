using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IDrug
    {
        Task<IEnumerable<DrugResponse>> GetDrug(DrugSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DrugResponse>> CreateDrug(DrugRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DrugResponse>> UpdateDrug(DrugRequest param, CancellationToken cancellationToken = default);
    }
}
