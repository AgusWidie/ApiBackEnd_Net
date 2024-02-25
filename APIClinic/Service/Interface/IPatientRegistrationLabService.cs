using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IPatientRegistrationLabService
    {
        Task<List<PatientRegistrationLabResponse>> GetPatientLabRegistration(PatientRegistrationLabSearchRequest param, CancellationToken cancellationToken);
        Task<List<PatientRegistrationLabResponse>> CreatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken);
        Task<List<PatientRegistrationLabResponse>> UpdatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken);
    }
}
