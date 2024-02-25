using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IAbsenDoctorService
    {
        Task<List<AbsenDoctorResponse>> GetAbsenDoctor(AbsenDoctorSearchRequest param, CancellationToken cancellationToken);
        Task<List<AbsenDoctorResponse>> CreateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken);
        Task<List<AbsenDoctorResponse>> UpdateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken);
    }
}
