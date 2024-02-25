using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ILogError
    {
        Task<IEnumerable<LogErrorResponse>> GetLogError(LogErrorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<LogErrorResponse>> CreateLogError(LogErrorRequest param, CancellationToken cancellationToken = default);
    }
}
