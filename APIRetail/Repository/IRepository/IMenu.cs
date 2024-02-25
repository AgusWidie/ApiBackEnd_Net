using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Repository.IRepository
{
    public interface IMenu
    {
        Task<IEnumerable<MenuResponse>> GetMenu(MenuRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuResponse>> CreateMenu(MenuAddRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuResponse>> UpdateMenu(MenuUpdateRequest param, CancellationToken cancellationToken = default);
    }
}
