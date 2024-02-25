using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IProfil
    {
        Task<IEnumerable<ProfilResponse>> GetProfil(ProfilSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilResponse>> CreateProfil(ProfilRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilResponse>> UpdateProfil(ProfilRequest param, CancellationToken cancellationToken = default);
    }
}
