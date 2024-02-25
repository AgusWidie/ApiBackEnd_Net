using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IScheduleService
    {
        Task<List<ScheduleResponse>> GetSchedule(ScheduleRequest param, CancellationToken cancellationToken);
        Task<List<ScheduleResponse>> CreateSchedule(ScheduleAddRequest param, CancellationToken cancellationToken);
        Task<List<ScheduleResponse>> UpdateSchedule(ScheduleUpdateRequest param, CancellationToken cancellationToken);
    }
}
