using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IPatientRegistrationLab
    {
        Task<IEnumerable<PatientRegistrationLabResponse>> GetPatientRegistrationLab(PatientRegistrationLabSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PatientRegistrationLabResponse>> CreatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<PatientRegistrationLabResponse>> UpdatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken = default);
    }
}
