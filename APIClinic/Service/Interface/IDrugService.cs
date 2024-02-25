using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IDrugService
    {
        Task<List<DrugResponse>> GetDrug(DrugSearchRequest param, CancellationToken cancellationToken);
        Task<List<DrugResponse>> CreateDrug(DrugRequest param, CancellationToken cancellationToken);
        Task<List<DrugResponse>> UpdateDrug(DrugRequest param, CancellationToken cancellationToken);
    }
}
