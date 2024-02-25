using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IDoctor
    {
        Task<IEnumerable<DoctorResponse>> GetDoctor(DoctorSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorResponse>> CreateDoctor(DoctorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorResponse>> UpdateDoctor(DoctorRequest param, CancellationToken cancellationToken = default);
    }
}
