using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ILogin
    {
        Task<IEnumerable<LoginResponse>> LoginUser(LoginRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> ChangePasswordUser(ChangePasswordRequest param, CancellationToken cancellationToken = default);
    }
}
