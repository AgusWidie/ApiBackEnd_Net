using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface ILogin
    {
        Task<IEnumerable<LoginResponse>> LoginUser(LoginRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsersResponse>> ChangePasswordUser(ChangePasswordRequest param, CancellationToken cancellationToken = default);
    }
}
