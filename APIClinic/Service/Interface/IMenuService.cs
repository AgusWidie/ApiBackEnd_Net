using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IMenuService
    {
        Task<List<MenuResponse>> GetMenu(MenuSearchRequest param, CancellationToken cancellationToken);
        Task<List<MenuResponse>> CreateMenu(MenuRequest param, CancellationToken cancellationToken);
        Task<List<MenuResponse>> UpdateMenu(MenuRequest param, CancellationToken cancellationToken);
    }
}
