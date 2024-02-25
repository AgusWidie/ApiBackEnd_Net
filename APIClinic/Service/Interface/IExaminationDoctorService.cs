using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IExaminationDoctorService
    {
        Task<List<ExaminationDoctorResponse>> GetExaminationDoctor(ExaminationDoctorSearchRequest param, CancellationToken cancellationToken);
        Task<List<ExaminationDoctorResponse>> CreateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken);
        Task<List<ExaminationDoctorResponse>> UpdateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken);
    }
}
