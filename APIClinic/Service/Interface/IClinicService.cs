using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IClinicService
    {
        Task<List<ClinicResponse>> GetClinic(ClinicSearchRequest param, CancellationToken cancellationToken);
        Task<List<ClinicResponse>> CreateClinic(ClinicRequest param, CancellationToken cancellationToken);
        Task<List<ClinicResponse>> UpdateClinic(ClinicRequest param, CancellationToken cancellationToken);
    }
}
