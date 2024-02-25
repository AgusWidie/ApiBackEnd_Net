using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IProfilUser
    {
        Task<IEnumerable<ProfilUserResponse>> GetProfilUser(ProfilUserSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilUserResponse>> CreateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilUserResponse>> UpdateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken = default);
    }
}
