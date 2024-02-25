using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IMessageService
    {
        Task<List<MessageResponse>> GetMessage(MessageRequest param, CancellationToken cancellationToken);
        Task<List<MessageResponse>> CreateMessage(MessageAddRequest param, CancellationToken cancellationToken);
        Task<List<MessageResponse>> UpdateMessage(MessageUpdateRequest param, CancellationToken cancellationToken);
    }
}
