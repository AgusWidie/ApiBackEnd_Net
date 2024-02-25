using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IMenu
    {
        Task<IEnumerable<MenuResponse>> GetMenu(MenuSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuResponse>> CreateMenu(MenuRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuResponse>> UpdateMenu(MenuRequest param, CancellationToken cancellationToken = default);
    }
}
