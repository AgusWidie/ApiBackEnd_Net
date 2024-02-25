using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface ILogErrorService
    {
        Task<List<LogErrorResponse>> GetCompany(LogErrorRequest param, CancellationToken cancellationToken);
        Task<List<LogErrorResponse>> CreateLogError(LogErrorAddRequest param, CancellationToken cancellationToken);
    }
}
