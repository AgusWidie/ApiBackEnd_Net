using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IProfilUserService
    {
        Task<List<ProfilUserResponse>> GetProfilUser(ProfilUserSearchRequest param, CancellationToken cancellationToken);
        Task<List<ProfilUserResponse>> CreateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken);
        Task<List<ProfilUserResponse>> UpdateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken);
    }
}
