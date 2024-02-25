using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IProfilService
    {
        Task<List<ProfilResponse>> GetProfil(ProfilRequest param, CancellationToken cancellationToken);
        Task<List<ProfilResponse>> CreateProfil(ProfilAddRequest param, CancellationToken cancellationToken);
        Task<List<ProfilResponse>> UpdateProfil(ProfilUpdateRequest param, CancellationToken cancellationToken);
    }
}
