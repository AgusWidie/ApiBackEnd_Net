using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IAbsenDoctor
    {
        Task<IEnumerable<AbsenDoctorResponse>> GetAbsenDoctor(AbsenDoctorSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<AbsenDoctorResponse>> CreateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<AbsenDoctorResponse>> UpdateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken = default);
    }
}
