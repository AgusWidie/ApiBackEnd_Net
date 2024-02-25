using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IProfilService
    {
        Task<List<ProfilResponse>> GetProfil(ProfilSearchRequest param, CancellationToken cancellationToken);
        Task<List<ProfilResponse>> CreateProfil(ProfilRequest param, CancellationToken cancellationToken);
        Task<List<ProfilResponse>> UpdateProfil(ProfilRequest param, CancellationToken cancellationToken);
    }
}
