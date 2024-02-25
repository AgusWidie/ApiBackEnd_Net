using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<DoctorResponse>> GetDoctor(DoctorSearchRequest param, CancellationToken cancellationToken);
        Task<List<DoctorResponse>> CreateDoctor(DoctorRequest param, CancellationToken cancellationToken);
        Task<List<DoctorResponse>> UpdateDoctor(DoctorRequest param, CancellationToken cancellationToken);
    }
}
