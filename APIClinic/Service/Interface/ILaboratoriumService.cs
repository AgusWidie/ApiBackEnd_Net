using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ILaboratoriumService
    {
        Task<List<LaboratoriumResponse>> GetLaboratorium(LaboratoriumSearchRequest param, CancellationToken cancellationToken);
        Task<List<LaboratoriumResponse>> CreateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken);
        Task<List<LaboratoriumResponse>> UpdateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken);
    }
}
