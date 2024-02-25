using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IUsers
    {
        Task<IEnumerable<UsersResponse>> GetUser(UserSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> CreateUser(UserRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> UpdateUser(UserRequest param, CancellationToken cancellationToken = default);
    }
}
