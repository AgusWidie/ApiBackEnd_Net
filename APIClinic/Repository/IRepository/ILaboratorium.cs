using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ILaboratorium
    {
        Task<IEnumerable<LaboratoriumResponse>> GetLaboratorium(LaboratoriumSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<LaboratoriumResponse>> CreateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<LaboratoriumResponse>> UpdateLaboratorium(LaboratoriumRequest param, CancellationToken cancellationToken = default);
    }
}
