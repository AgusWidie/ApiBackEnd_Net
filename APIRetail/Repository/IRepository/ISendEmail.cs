using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ISendEmail
    {
        Task<IEnumerable<SendEmailResponse>> GetSendEmail(SendEmailRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SendEmailResponse>> CreateSendEmail(SendEmailAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SendEmailResponse>> UpdateSendEmail(SendEmailUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
