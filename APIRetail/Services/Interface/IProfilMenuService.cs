using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IProfilMenuService
    {
        Task<List<ProfilMenuResponse>> GetProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken);
        Task<List<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuAddRequest param, CancellationToken cancellationToken);
        Task<List<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuUpdateRequest param, CancellationToken cancellationToken);
    }
}
