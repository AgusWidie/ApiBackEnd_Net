using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IExaminationLab
    {
        Task<IEnumerable<ExaminationLabResponse>> GetExaminationLab(ExaminationLabSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExaminationLabResponse>> CreateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExaminationLabResponse>> UpdateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken = default);
    }
}
