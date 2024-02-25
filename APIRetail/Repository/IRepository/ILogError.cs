using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ILogError
    {
        Task<IEnumerable<LogErrorResponse>> GetLogError(LogErrorRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<LogErrorResponse>> CreateLogError(LogErrorAddRequest param, CancellationToken cancellationToken = default);
    }
}
