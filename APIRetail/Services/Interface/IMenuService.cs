using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IMenuService
    {
        Task<List<MenuResponse>> GetMenu(MenuRequest param, CancellationToken cancellationToken);
        Task<List<MenuResponse>> CreateMenu(MenuAddRequest param, CancellationToken cancellationToken);
        Task<List<MenuResponse>> UpdateMenu(MenuUpdateRequest param, CancellationToken cancellationToken);
    }
}
