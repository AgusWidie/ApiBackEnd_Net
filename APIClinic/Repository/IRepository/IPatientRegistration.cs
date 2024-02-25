using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IPatientRegistration
    {
        Task<IEnumerable<PatientRegistrationResponse>> GetPatientRegistration(PatientRegistrationSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PatientRegistrationResponse>> CreatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PatientRegistrationResponse>> UpdatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken = default);
    }
}
