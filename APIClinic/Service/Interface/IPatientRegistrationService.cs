using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IPatientRegistrationService
    {
        Task<List<PatientRegistrationResponse>> GetPatientRegistration(PatientRegistrationSearchRequest param, CancellationToken cancellationToken);
        Task<List<PatientRegistrationResponse>> CreatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken);
        Task<List<PatientRegistrationResponse>> UpdatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken);
    }
}
