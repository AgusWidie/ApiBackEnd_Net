using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IProfilMenuService
    {
        Task<List<ProfilMenuResponse>> GetProfilMenu(ProfilMenuSearchRequest param, CancellationToken cancellationToken);
        Task<List<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken);
        Task<List<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken);
    }
}
