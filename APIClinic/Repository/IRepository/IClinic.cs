using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IClinic
    {
        Task<IEnumerable<ClinicResponse>> GetClinic(ClinicSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ClinicResponse>> CreateClinic(ClinicRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ClinicResponse>> UpdateClinic(ClinicRequest param, CancellationToken cancellationToken = default);
    }
}
