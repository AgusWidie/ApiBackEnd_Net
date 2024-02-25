using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IExaminationDoctor
    {
        Task<IEnumerable<ExaminationDoctorResponse>> GetExamination(ExaminationDoctorSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExaminationDoctorResponse>> CreateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExaminationDoctorResponse>> UpdateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken = default);
    }
}
