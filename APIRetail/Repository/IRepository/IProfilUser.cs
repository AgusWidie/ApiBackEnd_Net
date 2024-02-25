using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IProfilUser
    {
        Task<IEnumerable<ProfilUserResponse>> GetProfilUser(ProfilUserRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilUserResponse>> CreateProfilUser(ProfilUserAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilUserResponse>> UpdateProfilUser(ProfilUserUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
