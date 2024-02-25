using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IProfilUserService
    {
        Task<List<ProfilUserResponse>> GetProfilUser(ProfilUserRequest param, CancellationToken cancellationToken);
        Task<List<ProfilUserResponse>> CreateProfilUser(ProfilUserAddRequest param, CancellationToken cancellationToken);
        Task<List<ProfilUserResponse>> UpdateProfilUser(ProfilUserUpdateRequest param, CancellationToken cancellationToken);
    }
}
