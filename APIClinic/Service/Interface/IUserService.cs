using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IUserService
    {
        Task<List<UsersResponse>> GetUser(UserSearchRequest param, CancellationToken cancellationToken);
        Task<List<UsersResponse>> CreateUser(UserRequest param, CancellationToken cancellationToken);
        Task<List<UsersResponse>> UpdateUser(UserRequest param, CancellationToken cancellationToken);
    }
}
