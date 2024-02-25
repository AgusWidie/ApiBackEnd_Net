using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IScheduleDoctorService
    {
        Task<List<ScheduleDoctorResponse>> GetScheduleDoctor(ScheduleDoctorSearchRequest param, CancellationToken cancellationToken);
        Task<List<ScheduleDoctorResponse>> CreateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken);
        Task<List<ScheduleDoctorResponse>> UpdateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken);
    }
}
