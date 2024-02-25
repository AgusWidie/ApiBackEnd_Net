using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IProfilMenu
    {
        Task<IEnumerable<ProfilMenuResponse>> GetProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
