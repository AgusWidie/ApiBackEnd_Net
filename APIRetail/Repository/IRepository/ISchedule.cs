using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ISchedule
    {
        Task<IEnumerable<ScheduleResponse>> GetSchedule(ScheduleRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ScheduleResponse>> CreateSchedule(ScheduleAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ScheduleResponse>> UpdateSchedule(ScheduleUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
