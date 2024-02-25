using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ISpecialDoctorService
    {
        Task<List<SpecialistDoctorResponse>> GetSpecialistDoctor(SpecialistDoctorSearchRequest param, CancellationToken cancellationToken);
        Task<List<SpecialistDoctorResponse>> CreateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken);
        Task<List<SpecialistDoctorResponse>> UpdateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken);
    }
}
