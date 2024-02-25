using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ISpecialService
    {
        Task<List<SpecialistResponse>> GetSpecialist(SpecialistSearchRequest param, CancellationToken cancellationToken);
        Task<List<SpecialistResponse>> CreateSpecialist(SpecialistRequest param, CancellationToken cancellationToken);
        Task<List<SpecialistResponse>> UpdateSpecialist(SpecialistRequest param, CancellationToken cancellationToken);
    }
}
