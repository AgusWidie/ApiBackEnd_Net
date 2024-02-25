using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IProfilMenu
    {
        Task<IEnumerable<ProfilMenuResponse>> GetProfilMenu(ProfilMenuSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken = default);
    }
}
