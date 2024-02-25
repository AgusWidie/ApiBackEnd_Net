using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ISendWhatsApp
    {
        Task<IEnumerable<SendWhatsAppResponse>> GetSendWhatsApp(SendWhatsAppRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SendWhatsAppResponse>> CreateSendWhatsApp(SendWhatsAppAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<SendWhatsAppResponse>> UpdateSendWhatsApp(SendWhatsAppUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
