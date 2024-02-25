using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IMessage
    {
        Task<IEnumerable<MessageResponse>> GetMessage(MessageRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MessageResponse>> CreateMessage(MessageAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MessageResponse>> UpdateMessage(MessageUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
