using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ISpecialDoctor
    {
        Task<IEnumerable<SpecialistDoctorResponse>> GetSpecialistDoctor(SpecialistDoctorSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SpecialistDoctorResponse>> CreateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SpecialistDoctorResponse>> UpdateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken = default);
    }
}
