using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IUser
    {
        Task<IEnumerable<UsersResponse>> GetUser(UsersRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> CreateUser(UsersAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> UpdateUser(UsersUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
