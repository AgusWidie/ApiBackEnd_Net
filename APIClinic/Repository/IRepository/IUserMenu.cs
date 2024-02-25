using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface IUserMenu
    {
        Task<IEnumerable<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMenuResponse>> GetUserMenu(UserMenuRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<CheckUserMenuResponse>> GetCheckUserMenu(CheckUserMenuRequest param, CancellationToken cancellationToken = default);
    }
}
