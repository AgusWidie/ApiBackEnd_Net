using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ISpecialist
    {
        Task<IEnumerable<SpecialistResponse>> GetSpecialist(SpecialistSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SpecialistResponse>> CreateSpecialist(SpecialistRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SpecialistResponse>> UpdateSpecialist(SpecialistRequest param, CancellationToken cancellationToken = default);
    }
}
