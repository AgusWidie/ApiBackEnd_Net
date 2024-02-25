using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ISchedule
    {
        Task<IEnumerable<ScheduleDoctorResponse>> GetScheduleDoctor(ScheduleDoctorSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ScheduleDoctorResponse>> CreateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ScheduleDoctorResponse>> UpdateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken = default);
    }
}
