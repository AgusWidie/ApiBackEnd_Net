using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IExaminationLabService
    {
        Task<List<ExaminationLabResponse>> GetExaminationLab(ExaminationLabSearchRequest param, CancellationToken cancellationToken);
        Task<List<ExaminationLabResponse>> CreateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken);
        Task<List<ExaminationLabResponse>> UpdateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken);
    }
}
