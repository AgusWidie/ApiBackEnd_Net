using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IUserService
    {
        Task<List<UsersResponse>> GetUser(UsersRequest param, CancellationToken cancellationToken);
        Task<List<UsersResponse>> CreateUser(UsersAddRequest param, CancellationToken cancellationToken);
        Task<List<UsersResponse>> UpdateUser(UsersUpdateRequest param, CancellationToken cancellationToken);
    }
}
