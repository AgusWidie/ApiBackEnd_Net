using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IProfil
    {
        Task<IEnumerable<ProfilResponse>> GetProfil(ProfilRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilResponse>> CreateProfil(ProfilAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfilResponse>> UpdateProfil(ProfilUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
